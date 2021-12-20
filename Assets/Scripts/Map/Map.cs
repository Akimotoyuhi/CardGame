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
    /// <summary>線描画用プレハブ</summary>
    [SerializeField] GameObject m_linePrefab;
    /// <summary>セクター保存用</summary>
    private GameObject[] m_cellLocation;
    /// <summary>線の描画用</summary>
    private Vector3[] m_pos;

    private void Start()
    {
        CreateMap();
    }
    /// <summary>
    /// セルの生成と配置
    /// </summary>
    private void CreateMap()
    {
        m_cellLocation = new GameObject[m_sector];
        for (int i = 0; i < m_sector; i++)
        {
            GameObject sector = Instantiate(m_sectorPrefab);
            sector.transform.SetParent(this.transform, false);
            sector.name = $"Sector{i}";
            Cell cell = default;

            if (i == 0 || i == m_sector - 1)
            {
                //最初と最後はセル一つ
                cell = Instantiate(m_cellPrefab).GetComponent<Cell>();
                cell.transform.SetParent(sector.transform, false);
                cell.SectorIndex = i;
                cell.m_encountId = Random.Range(0, (int)EnemyID.endLength);
            }
            else
            {
                for (int n = 0; n < m_maxCell; n++)
                {
                    cell = Instantiate(m_cellPrefab).GetComponent<Cell>();
                    cell.transform.SetParent(sector.transform, false);
                    cell.SectorIndex = i;
                    cell.m_encountId = Random.Range(0, (int)EnemyID.endLength);
                }
            }
            m_cellLocation[i] = sector;
            sector.transform.SetParent(m_parentSector, false);
        }
        AddPath(0, 0);
    }
    /// <summary>
    /// 道を作る
    /// </summary>
    /// <param name="sectorIndex"></param>
    private void AddPath(int sectorIndex, int cellIndex)
    {
        if (sectorIndex > m_sector) return;
        //次のセクターから進むセルを一つ抽選する
        int r = Random.Range(0, m_cellLocation[sectorIndex + 1].transform.childCount);
        //今のセルに次のインデックスを教えてあげる
        m_cellLocation[sectorIndex].transform.GetChild(cellIndex).GetComponent<Cell>().NextCell.Add(r);
        AddPath(sectorIndex + 1, r);
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
