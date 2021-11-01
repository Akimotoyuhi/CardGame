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
    [SerializeField] int m_maxCell = 3;
    /// <summary>親セクター</summary>
    [SerializeField] Transform m_parentSector;
    /// <summary>セクターｐレハブ</summary>
    [SerializeField] GameObject m_sectorPrefab;
    /// <summary>とりあえずセル</summary>
    [SerializeField] GameObject m_cellPrefab;
    /// <summary>所属セクター保存用</summary>
    private GameObject[] m_cellLocation;
    /// <summary>線の描画用</summary>
    private Vector3[] m_pos;
    private LineRenderer m_lineRenderer;
    private Canvas m_canvas;

    private void Start()
    {
        m_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        m_cellLocation = new GameObject[m_sector];
        CreateCell();
    }
    /// <summary>
    /// セルの生成と配置
    /// </summary>
    private void CreateCell()
    {
        GameObject sector = gameObject;
        GameObject cell;
        for (int i = 0; i < m_sector; i++)
        {
            sector = Instantiate(m_sectorPrefab);
            sector.transform.SetParent(this.transform, false);
            sector.name = $"Sector{i}";

            if (i == 0 || i == m_sector - 1)
            {
                //最初と最後はセル一つ
                cell = Instantiate(m_cellPrefab);
                cell.transform.SetParent(sector.transform, false);
            }
            else
            {
                for (int n = 0; n < m_maxCell; n++)
                {
                    cell = Instantiate(m_cellPrefab);
                    cell.transform.SetParent(sector.transform, false);
                }
            }
            m_cellLocation[i] = sector;
            sector.transform.SetParent(m_parentSector, false);
        }
        CreateMap();
        //m_cellLocation[0].transform.GetChild(0).gameObject.GetComponent<Cell>().ListChecker();
        //m_cellLocation[0].transform.GetChild(0).gameObject.GetComponent<Cell>().LineCaster();
    }

    /// <summary>
    /// マップ生成
    /// </summary>
    private void CreateMap()
    {
        GameObject cell;
        GameObject beforeCell = null;
        for (int i = 0; i < m_sector; i++)
        {
            //セルのランダム抽選
            cell = m_cellLocation[i].transform.GetChild(Random.Range(0, m_cellLocation[i].transform.childCount)).gameObject;
            cell.GetComponent<Image>().color = Color.red;
            if (i == 0) //一回目は前セルが存在しないので前セルに自分を入れてターンエンド
            {
                beforeCell = cell;
                continue;
            }
            beforeCell.GetComponent<Cell>().m_objList.Add(cell); //前のセルと現在のセルをつなげる
            beforeCell = cell;
        }
    }
}
