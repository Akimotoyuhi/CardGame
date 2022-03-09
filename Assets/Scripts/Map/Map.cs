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
    /// <summary>親セクター</summary>
    [SerializeField] Transform m_parentSector;
    /// <summary>セクタープレハブ</summary>
    [SerializeField] GameObject m_sectorPrefab;
    /// <summary>セルプレハブ</summary>
    [SerializeField] GameObject m_cellPrefab;
    /// <summary>線描画用</summary>
    [SerializeField] GameObject m_linePrefab;
    /// <summary>線となるオブジェクトの親</summary>
    [SerializeField] Transform m_lineParent;
    /// <summary>詳細設定</summary>
    [SerializeField] DetailSettings m_detailSettings;
    /// <summary>セクター保存用</summary>
    private GameObject[] m_sectorLocation;
    private List<Cell> m_cells = new List<Cell>();
    /// <summary>マップの詳細な設定</summary>
    [System.Serializable]
    public class DetailSettings
    {
        [SerializeField, Tooltip("休憩マスを生成する最小位置")] int m_restMinIndex;
        [SerializeField, Tooltip("休憩マスを生成する最大位置")] int m_restMaxIndex;
        [SerializeField, Tooltip("絶対に休憩マスを生成する位置")] int m_restAbsolutelyIndex;
        [SerializeField, Tooltip("エリートマスを生成する最小位置")] int m_eliteMinIndex;
        [SerializeField, Tooltip("エリートマスを生成する最大位置")] int m_eliteMaxIndex;
        [SerializeField, Tooltip("絶対にエリートマスを生成する位置")] int m_eliteAbsolutelyIndex;
        public bool RestIndex(int sector)
        {
            if (m_restAbsolutelyIndex == sector) return true;
            if (m_restMinIndex <= sector && m_restMaxIndex >= sector)
                if (Random.Range(0, 5) == 0)
                    return true;
            return false;
        }
        public bool EliteIndex(int sector)
        {
            if (m_eliteAbsolutelyIndex == sector) return true;
            if (m_eliteMinIndex <= sector && m_eliteMaxIndex >= sector)
                if (Random.Range(0, 5) == 0)
                    return true;
            return false;
        }
    }

    /// <summary>
    /// セルの生成と配置
    /// </summary>
    public void CreateMap()
    {
        //DontDestroyOnLoad(gameObject);
        m_sectorLocation = new GameObject[m_sector];
        for (int i = 0; i < m_sector; i++)
        {
            GameObject sector = Instantiate(m_sectorPrefab);
            sector.transform.SetParent(transform, false);
            sector.name = $"Sector{i}";
            Cell cell = default;
            //ここ同じ処理が二度出てるので修正するように
            if (i == 0 || i == m_sector - 1)
            {
                //最初と最後はセル一つ
                cell = Instantiate(m_cellPrefab).GetComponent<Cell>();
                cell.transform.SetParent(sector.transform, false);
                cell.SectorIndex = i;
                m_cells.Add(cell);
                //cell.m_encountId = Random.Range(0, (int)EnemyID.endLength);
            }
            else
            {
                for (int n = 0; n < m_maxCell; n++)
                {
                    cell = Instantiate(m_cellPrefab).GetComponent<Cell>();
                    cell.transform.SetParent(sector.transform, false);
                    cell.SectorIndex = i;
                    m_cells.Add(cell);
                    //cell.m_encountId = Random.Range(0, (int)EnemyID.endLength);
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
        c.Step = sectorIndex;
        if (m_detailSettings.RestIndex(sectorIndex))
        {
            c.SetCellState = CellState.Rest;
        }
        else if (m_detailSettings.EliteIndex(sectorIndex))
        {
            c.SetCellState = CellState.Elite;
        }
        else if (sectorIndex == m_sector - 1)
        {
            c.SetCellState = CellState.Boss;
            //c.m_encountId = Random.Range(0, System.Enum.GetNames(typeof(EnemyID)));
        }
        else
        {
            c.SetCellState = CellState.Enemy;
            //c.m_encountId = Random.Range(0, (int)EnemyID.endLength);
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
    /// 後に変える
    /// </summary>
    /// <param name="floor"></param>
    /// <returns></returns>
    public bool ClearCheck(int floor)
    {
        if (floor >= m_sector) return true;
        else return false;  
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
                    //int r = Random.Range(0, 2);
                    //if (r == 0)
                    //{
                    //    Destroy(m_sectorLocation[i].transform.GetChild(n).gameObject);
                    //}
                    //else
                    //{
                    //    m_sectorLocation[i].transform.GetChild(n).GetComponent<
                    //    >().interactable = false;
                    //    m_sectorLocation[i].transform.GetChild(n).GetComponent<Image>().color = new Color(0, 0, 0, 1);
                    //}
                }
            }
        }
    }
}
