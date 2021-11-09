using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mastar;

public class GameManager : MonoBehaviour
{
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
    [SerializeField] GameObject m_game;
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
    [Header("その他")]
    [SerializeField] GameObject m_map;
    /// <summary>経過ターン数</summary>
    private int m_progressTurn = 0;
    /// <summary>ボタンの受付拒否</summary>
    private bool m_isPress = true;
    
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
        //CreateFirld();
        m_game.GetComponent<Canvas>().enabled = false;
        //m_player = GameObject.Find("Player").GetComponent<Player>();
        //d += m_hand.AllCast;
        //d += m_enemies.EnemyTrun;
        //d += m_player.TurnEnd;
        //Invoke("FirstTurn", 0.1f);
    }

    public void Battle(int id)
    {
        m_map.GetComponent<Canvas>().enabled = false;
        m_game.GetComponent<Canvas>().enabled = true;
        CreateFirld(id);
        FirstTurn();
    }

    /// <summary>
    /// プレイヤー、敵、カードを生成してゲーム画面を作る
    /// </summary>
    private void CreateFirld(int id)
    {
        //初期デッキとプレイヤー構築
        if (GodGameManager.Instance().StartCheck())
        {
            m_player = Instantiate(m_playerPrefab, m_playerPos).gameObject.GetComponent<Player>();
            GodGameManager inst = GodGameManager.Instance();
            m_player.SetParam(inst.Name, inst.Image, inst.Hp, this);
            for (int i = 0; i < GodGameManager.Instance().Cards.Length; i++)
            {
                CreateCard(GodGameManager.Instance().GetHaveCardID(i));
            }
        }
        else
        {
            m_player = Instantiate(m_playerPrefab, m_playerPos).gameObject.GetComponent<Player>();
            m_player.SetParam(m_playerStatsData.Name, m_playerStatsData.Image, m_playerStatsData.HP, this);
            for (int i = 0; i < m_playerStatsData.GetCardLength; i++)
            {
                CreateCard(m_playerStatsData.GetCard(i));
            }
        }
        //敵グループ生成
        m_encountDatabase = m_encountData.m_data[id];
        for (int i = 0; i < m_encountDatabase.GetLength; i++)
        {
            m_enemyDatabase = m_enemydata.m_enemyDatas[m_encountDatabase.GetID(i)];
            Transform tra = Instantiate(m_enemyPrefab, m_enemies.transform).transform;
            tra.SetParent(m_enemies.transform, false);
            EnemyBase e = tra.GetComponent<EnemyBase>();
            e.SetParam(m_enemyDatabase.Name, m_enemyDatabase.Image, m_enemyDatabase.HP, m_enemyDatabase.SetAction(), this);
        }
        m_enemyManager = m_enemies.GetComponent<EnemyManager>();
        m_enemyManager.EnemyCount();
    }

    /// <summary>
    /// 最初のターンの特別処理
    /// いらんかも
    /// </summary>
    private void FirstTurn()
    {
        Debug.Log(m_progressTurn + "ターン目");
        //m_deck.Draw();
        m_enemyManager.EnemyTrun(m_progressTurn);
        m_progressTurn++;
        m_isPress = false;
        //Invoke("TurnStart", 1f);
        TurnStart();
    }

    /// <summary>ターン終了</summary>
    public void TurnEnd()
    {
        if (m_isPress) return;
        m_isPress = true;
        m_hand.AllCast();
        m_enemyManager.EnemyTrun(m_progressTurn);
        m_player.TurnEnd();
        //d(m_progressTurn);
        m_progressTurn++;
        Invoke("TurnStart", 1f);
    }

    /// <summary>
    /// ターン開始
    /// </summary>
    private void TurnStart()
    {
        m_isPress = false;
        m_deck.Draw();
        Debug.Log(m_progressTurn + "ターン目");
        m_player.TurnStart();
    }

    /// <summary>
    /// カードの作成
    /// </summary>
    public void CreateCard(int id)
    {
        GameObject obj = Instantiate((GameObject)Resources.Load("BlankCard"));
        BlankCard card = obj.GetComponent<BlankCard>();
        NewCardDataBase cardData = m_cardData.m_cardData[id];
        card.SetInfo(cardData.m_image, cardData.m_name, cardData.m_cost, cardData.GetTooltip(), cardData.GetParam(), cardData.m_cardType);
        //obj.transform.parent = m_deck.transform;
        obj.transform.SetParent(m_deck.transform, false);
    }

    public void SceneReload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
