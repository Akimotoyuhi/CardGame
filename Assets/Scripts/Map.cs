using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    //memo
    //最初にセルを各セクター内にm_maxCell個作ってスタートからゴールまで一本ずつ道を作る
    //これをm_maxCell回繰り返す

    /// <summary>セクター数</summary>
    [SerializeField] int m_sector = 10;
    /// <summary>１セクター内の最大セル数</summary>
    [SerializeField] int m_maxCell = 3;
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

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void CreateMap()
    {

    }
}
