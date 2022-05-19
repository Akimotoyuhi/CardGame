using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    //最初にセルを各セクター内にm_maxCell個作ってスタートからゴールまで一本ずつ道を作る
    //これをm_maxCell回繰り返す

    /// <summary>セクター数</summary>
    [SerializeField] int m_sector = 10;
    /// <summary>各セクターに生成するセルの最大数</summary>
    [SerializeField] int m_maxCell = 3;
    /// <summary>ここまで進んだらクリアとするAct数</summary>
    [SerializeField] int m_crearAct = 1;
    /// <summary>マップデータ</summary>
    [SerializeField] MapData m_mapData;
    /// <summary>親セクター</summary>
    [SerializeField] Transform m_parentSector;
    /// <summary>セクタープレハブ</summary>
    [SerializeField] GameObject m_sectorPrefab;
    /// <summary>セルプレハブ</summary>
    [SerializeField] Cell m_cellPrefab;
    /// <summary>線描画用</summary>
    [SerializeField] GameObject m_linePrefab;
    /// <summary>線となるオブジェクトの親</summary>
    [SerializeField] Transform m_lineParent;
    /// <summary>背景</summary>
    [SerializeField] Image m_background;
    /// <summary>マップのContent</summary>
    [SerializeField] RectTransform m_scrollViewContentTra;
    /// <summary>現在act保存用</summary>
    private int m_act = 1;
    /// <summary>デバッグ用マップ固定フラグ</summary>
    private bool m_isFixedMap;
    /// <summary>現在のMapID保存用</summary>
    private MapID m_mapID;
    /// <summary>セクター保存用</summary>
    private GameObject[] m_sectorLocation;
    /// <summary>現在のマップデータ</summary>
    private MapDataBase m_nowMapData;
    private List<Cell> m_cells = new List<Cell>();
    public int Sector => m_sector;
    /// <summary>セルのクリック可否フラグ</summary>
    public bool CanClick { get; set; }
    private void Setup()
    {
        if (m_isFixedMap)
            m_nowMapData = m_mapData.GetMapData(m_mapID);
        else
            m_nowMapData = m_mapData.GetMapData((Act)m_act);
        m_mapID = m_nowMapData.MapID;
        m_background.sprite = m_nowMapData.Background;
        AudioManager.Instance.Play(m_nowMapData.MapBgm);
    }
    /// <summary>
    /// セルの生成と配置
    /// </summary>
    public void CreateMap()
    {
        Setup();
        m_sectorLocation = new GameObject[m_sector];
        for (int i = 0; i < m_sector; i++)
        {
            GameObject sector = Instantiate(m_sectorPrefab);
            sector.transform.SetParent(transform, false);
            sector.name = $"Sector{i}";
            Cell cell;
            if (i == 0 || i == m_sector - 1)
            {
                //最初と最後はセル一つ
                cell = Instantiate(m_cellPrefab);
                cell.transform.SetParent(sector.transform, false);
                cell.SectorIndex = i;
                m_cells.Add(cell);
                cell.Map = this;
                cell.MapID = m_mapID;
            }
            else
            {
                for (int n = 0; n < m_maxCell; n++)
                {
                    cell = Instantiate(m_cellPrefab);
                    cell.transform.SetParent(sector.transform, false);
                    cell.SectorIndex = i;
                    m_cells.Add(cell);
                    cell.Map = this;
                    cell.MapID = m_mapID;
                }
            }
            m_sectorLocation[i] = sector;
            sector.transform.SetParent(m_parentSector, false);
        }
        for (int i = 0; i < m_maxCell; i++)
        {
            AddPath();
        }
        DeleteCell();
    }
    /// <summary>
    /// 道を作る
    /// </summary>
    /// <param name="sectorIndex"></param>
    private void AddPath(int sectorIndex = 0, int cellIndex = 0, Cell beforeCell = null)
    {
        //次のセクターから進むセルを一つ抽選する
        Cell c = m_sectorLocation[sectorIndex].transform.GetChild(cellIndex).GetComponent<Cell>();
        c.Floor = sectorIndex;
        if (m_nowMapData.GetDetailSetting.RestIndex(sectorIndex))
        {
            c.SetCellState = CellState.Rest;
        }
        else if (m_nowMapData.GetDetailSetting.EliteIndex(sectorIndex))
        {
            c.SetCellState = CellState.Elite;
        }
        else if (sectorIndex == m_sector - 1)
        {
            c.SetCellState = CellState.Boss;
        }
        else
        {
            c.SetCellState = CellState.Enemy;
        }
        c.CreatedFlag = true;
        if (sectorIndex + 1 >= m_sector) return;
        #region 未完成の線引く処理
        //線引く
        //if (beforeCell)
        //{
        //    GameObject line = Instantiate(m_linePrefab);
        //    line.transform.SetParent(c.transform, false);
        //    //line.transform.SetParent(m_lineParent, false);
        //    float x = beforeCell.GetChildPosition(CellChildType.Begin).x - c.GetChildPosition(CellChildType.End).x;
        //    float y = beforeCell.GetChildPosition(CellChildType.Begin).y - c.GetChildPosition(CellChildType.End).y;
        //    float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        //    line.transform.rotation = Quaternion.Euler(0, 0, angle);
        //    float mX = x / 2;
        //    float mY = y / 2;
        //    line.GetRectTransform().anchoredPosition = new Vector2(mX, mY);
        //    float distance = Vector2.Distance(beforeCell.GetChildPosition(CellChildType.Begin), c.GetChildPosition(CellChildType.End));
        //    RectTransform rect = line.GetRectTransform();
        //    rect.localScale = new Vector2(distance, rect.rect.height);
        //}
        #endregion
        //今のセルに次のインデックスを教えてあげる
        int r = Random.Range(0, m_sectorLocation[sectorIndex + 1].transform.childCount);
        c.AddNextCell(r);
        AddPath(sectorIndex + 1, r, c);
    }

    /// <summary>
    /// マップ上全てのセルの色を再設定する
    /// </summary>
    public void AllColorChange()
    {
        CanClick = false;
        for (int i = 0; i < m_sectorLocation.Length; i++)
        {
            for (int n = 0; n < m_sectorLocation[i].transform.childCount; n++)
            {
                m_sectorLocation[i].transform.GetChild(n).GetComponent<Cell>().ColorChange();
            }
        }
    }

    /// <summary>
    /// マップを踏破したかの判定<br/>
    /// クリアしたらtrue
    /// </summary>
    public bool ClearCheck(int floor)
    {
        if (floor == 0) return false;
        if (floor % m_sector == 0)
        {
            if (m_crearAct == m_act)//最後まで到達したらクリア
            {
                GameManager.Instance.Gameover(true);
                return true;
            }
            int i = 1;
            while (true)
            {
                int n = floor - m_sector * i;
                if (n < 0)
                {
                    break;
                }
                i++;
            }
            Debug.Log($"現在Act{i}");
            m_act = i;
            foreach (var s in m_sectorLocation)
            {
                Destroy(s);
            }
            m_scrollViewContentTra.anchoredPosition = new Vector2(0, m_scrollViewContentTra.anchoredPosition.y);
            CreateMap();
        }
        return false;
    }

    /// <summary>
    /// マップ生成時に使ってないセルを消す
    /// </summary>
    private void DeleteCell()
    {
        for (int i = 0; i < m_sectorLocation.Length; i++)
        {
            for (int n = 0; n < m_sectorLocation[i].transform.childCount; n++)
            {
                if (!m_sectorLocation[i].transform.GetChild(n).gameObject.GetComponent<Cell>().CreatedFlag)
                {
                    Destroy(m_sectorLocation[i].transform.GetChild(n).gameObject);
                }
            }
        }
    }

    /// <summary>デバッグ用のマップ固定処理</summary>
    public void SetFixedMapDebug(MapID mapID)
    {
        m_isFixedMap = true;
        m_mapID = mapID;
    }
}
