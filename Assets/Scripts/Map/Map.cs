using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    //memo
    //最初にセルを各セクター内にm_maxCell個作ってスタートからゴールまで一本ずつ道を作る
    //これをm_maxCell回繰り返す

    /// <summary>セクター数</summary>
    private int m_sector = 7;
    /// <summary>各セクターに生成するセルの最大数</summary>
    [SerializeField] int m_maxCell = 3;
    /// <summary>親セクター</summary>
    [SerializeField] Transform m_parentSector;
    /// <summary>セクターｐレハブ</summary>
    [SerializeField] GameObject m_sectorPrefab;
    /// <summary>セルプレハブ</summary>
    [SerializeField] GameObject m_cellPrefab;
    /// <summary>線描画用</summary>
    [SerializeField] GameObject m_linePrefab;
    /// <summary>セクター保存用</summary>
    private GameObject[] m_sectorLocation;
    private List<Cell> m_cells = new List<Cell>();

    /// <summary>
    /// セルの生成と配置
    /// </summary>
    public void CreateMap()
    {
        m_sectorLocation = new GameObject[m_sector];
        for (int i = 0; i < m_sector; i++)
        {
            GameObject sector = Instantiate(m_sectorPrefab);
            sector.transform.SetParent(this.transform, false);
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
        AddPath();
        AddPath();
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
        c.CellState = CellState.Enemy;
        c.m_encountId = Random.Range(0, (int)EnemyID.endLength);
        c.Step = sectorIndex;
        c.CreatedFlag = true;
        if (sectorIndex + 1 >= m_sector) return;
        #region 未完成の線引く処理
        //線引く
        //if (beforeCell)
        //{
        //    GameObject line = Instantiate(m_linePrefab);
        //    line.transform.SetParent(c.transform, false);
        //    float x = beforeCell.GetChildPosition(CellChildType.Begin).x - c.GetChildPosition(CellChildType.End).x;
        //    float y = beforeCell.GetChildPosition(CellChildType.Begin).y - c.GetChildPosition(CellChildType.End).y;
        //    float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        //    line.transform.rotation = Quaternion.Euler(0, 0, angle);
        //    float mX = x / 2;
        //    float mY = y / 2;
        //    line.transform.position = new Vector2(mX, mY);
        //    float distance = Vector2.Distance(beforeCell.GetChildPosition(CellChildType.Begin), c.GetChildPosition(CellChildType.End));
        //    RectTransform rect = line.GetComponent<RectTransform>();
        //    rect.sizeDelta = new Vector2(distance, rect.rect.height);
        //}
        #endregion
        //今のセルに次のインデックスを教えてあげる
        int r = Random.Range(0, m_sectorLocation[sectorIndex + 1].transform.childCount);
        c.AddNextCell(r);
        AddPath(sectorIndex + 1, r, c);
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

    /*
    // 対象となる Canvas
    [SerializeField]
    Canvas _targetCanvas;
    // 始点となる Image
    [SerializeField]
    Image _startImage;
    // 終点となる Image
    [SerializeField]
    Image _endImage;
    // 描画するためのラインとなる Image
    [SerializeField]
    Image _lineImage;

    RectTransform _line;
    GameObject _lineObj;

    Vector2 _startPos = Vector2.zero;
    Vector2 _endPos = Vector2.zero;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) SetStartPoint();
        if (Input.GetMouseButton(0)) Drow();
        if (Input.GetMouseButtonUp(0)) SetEndPoint();
    }

    void SetStartPoint()
    {
        _startPos = Input.mousePosition;
        GameObject image = Instantiate(_startImage, _startPos, Quaternion.identity).gameObject;
        image.transform.SetParent(_targetCanvas.transform);

        _lineObj = Instantiate(_lineImage, _startPos, Quaternion.identity).gameObject;
        _line = _lineObj.GetComponent<RectTransform>();
        _lineObj.transform.SetParent(_targetCanvas.transform);
    }

    void Drow()
    {
        Vector2 mouse = Input.mousePosition;

        float diffX = mouse.x - _startPos.x;
        float diffY = mouse.y - _startPos.y;

        // 終点となるImageの方向に向かせる
        float angle = Mathf.Atan2(diffY, diffX) * Mathf.Rad2Deg;
        _lineObj.transform.rotation = Quaternion.Euler(0, 0, angle);

        // 中点の計算
        float mX = (_startPos.x + mouse.x) / 2;
        float mY = (_startPos.y + mouse.y) / 2;
        _lineObj.transform.position = new Vector2(mX, mY);
        float distance = Vector2.Distance(_startPos, mouse);
        _line.sizeDelta = new Vector2(distance, _line.rect.height);
    }

    void SetEndPoint()
    {
        _endPos = Input.mousePosition;
        GameObject image = Instantiate(_endImage, _endPos, Quaternion.identity).gameObject;
        image.transform.SetParent(_targetCanvas.transform);
    }
     */
}
