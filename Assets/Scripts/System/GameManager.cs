using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mastar;

public class GameManager : MonoBehaviour
{
    /*
    [Header("プレイヤー関連")]
    /// <summary>プレイヤー初期データ</summary>
    [SerializeField] PlayerStatsData m_playerStatsData;
    /// <summary>プレイヤーのプレハブ</summary>
    [SerializeField] GameObject m_playerPrefab;
    /// <summary>プレイヤーの配置場所</summary>
    [SerializeField] RectTransform m_playerPos;
    /// <summary>プレイヤークラス</summary>
    private Player m_player;
    [Header("ゲーム関連")]
    /// <summary>ゲーム画面</summary>
    //[SerializeField] GameObject m_game;
    /// <summary>デッキ</summary>
    [SerializeField] Deck m_deck;
    /// <summary>捨て札</summary>
    [SerializeField] Discard m_discard;
    /// <summary>手札</summary>
    [SerializeField] Hand m_hand;
    /// <summary>カードのデータベース</summary>
    [SerializeField] NewCardData m_cardData;
    [Header("敵関連")]
    /// <summary>敵グループ</summary>
    [SerializeField] GameObject m_enemies;
    /// <summary>敵プレハブ</summary>
    [SerializeField] GameObject m_enemyPrefab;
    /// <summary>敵グループのデータ</summary>
    [SerializeField] EncountData m_encountData;
    private EncountDataBase m_encountDatabase;
    /// <summary>敵のデータ</summary>
    [SerializeField] EnemyData m_enemydata;
    private EnemyDataBase m_enemyDatabase;
    /// <summary>敵グループの管理クラス</summary>
    private EnemyManager m_enemyManager;
    */
    [Header("その他")]
    [SerializeField] GameObject m_map;
    /// <summary>バトルマネージャー</summary>
    [SerializeField] BattleManager m_battleManager;
    /// <summary>経過ターン数</summary>
    //private int m_progressTurn = 0;
    /// <summary>ボタンの受付拒否</summary>
    //private bool m_isPress = true;
    
    //いらんかも
    //delegate void Dele(int i);
    //Dele d = default;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //m_progressTurn = 0;
        m_battleManager.Setup();
    }

    public void Battle(int id)
    {
        m_map.GetComponent<Canvas>().enabled = false;
        m_battleManager.Battle(id);
    }

    public void SceneReload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
