using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour
{
    //memo
    //最初にセルを各セクター内にm_maxCell個作ってスタートからゴールまで一本ずつ道を作る
    //これをm_maxCell回繰り返す

    /// <summary>セクター数</summary>
    private int m_sector = 7;
    /// <summary>親セクター</summary>
    [SerializeField] Transform m_parentSector;
    /// <summary>セクターｐレハブ</summary>
    [SerializeField] GameObject m_sectorPrefab;
    /// <summary>とりあえずセル</summary>
    [SerializeField] GameObject m_cellPrefab;
    [Header("下は今は使わない")]
    /// <summary>開始セル(最大１つ)</summary>
    [SerializeField] GameObject m_startCellPrefab;
    /// <summary>最終セル(最大１つ)</summary>
    [SerializeField] GameObject m_bossCellPrefab;
    /// <summary>戦闘セル(たくさん)</summary>
    [SerializeField] GameObject m_battleCellPrefab;
    /// <summary>強敵セル(最大３つ)</summary>
    [SerializeField] GameObject m_eliteCellPrefab;
    /// <summary>イベントセル(たくさん)</summary>
    [SerializeField] GameObject m_eventCellPrefab;
    /// <summary>休憩マス(最大２つ)</summary>
    [SerializeField] GameObject m_restCellPrefab;

    /// <summary>所属セクター保存用</summary>
    private GameObject[] m_cellLocation;
    /// <summary>線描画用</summary>
    private LineRenderer m_lineRenderer;

    void Start()
    {
        m_cellLocation = new GameObject[m_sector];
        CreateCell();
    }

    /// <summary>
    /// セルの生成と配置
    /// </summary>
    private void CreateCell()
    {
        GameObject obj = gameObject;
        for (int i = 0; i < m_sector; i++)
        {
            if (i == 0 || i == m_sector - 1)
            {
                obj = Instantiate(m_cellPrefab);
            }
            else
            {
                obj = Instantiate(m_sectorPrefab);
            }
            m_cellLocation[i] = obj;
            obj.transform.SetParent(m_parentSector, false);
        }
        CreateMap(null);
    }

    private void CreateMap(GameObject cell, int num = 0)
    {
        //捜索セルを保存する→次のセクターの中からランダムで一つ決める→numをインクリメント
        GameObject nowCell = cell;
        if (num >= m_sector) return; //最後まで行ったら終了
        if (!nowCell)
        {
            nowCell = m_cellLocation[num].transform.GetChild(0).gameObject; //最初の一回はStartがほしいので特別
            m_lineRenderer = nowCell.GetComponent<LineRenderer>(); //線描画前の初期設定
            //m_lineRenderer.startWidth = 0.1f;
            //m_lineRenderer.endWidth = 0.1f;
            m_lineRenderer.positionCount = m_sector;
        }
        m_lineRenderer.SetPosition(num, nowCell.transform.position);
        int r = Random.Range(0, m_cellLocation[num].transform.childCount); //次セクターからランダムで一つセルを選択
        CreateMap(m_cellLocation[num].transform.GetChild(r).gameObject, num + 1);
    }
}
