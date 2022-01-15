using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mastar;
using System;
using UniRx;
using UnityEngine.UI;

enum Timing { Start, End }

public class BattleManager : MonoBehaviour
{
    #region メンバ変数
    #region Player関連のメンバ
    [Header("プレイヤー関連")]
    /// <summary>プレイヤー初期データ</summary>
    [SerializeField] PlayerStatsData m_playerStatsData;
    /// <summary>プレイヤーのプレハブ</summary>
    [SerializeField] GameObject m_playerPrefab;
    /// <summary>プレイヤーの配置場所</summary>
    [SerializeField] RectTransform m_playerPos;
    /// <summary>プレイヤークラス</summary>
    private Player m_player;
    /// <summary>プレイヤーのコスト表示用テキスト</summary>
    [SerializeField] Text m_costViewText;
    #endregion
    #region Enemy関連のメンバ
    [Header("敵関連")]
    /// <summary>敵グループ</summary>
    [SerializeField] GameObject m_enemies;
    /// <summary>敵グループのデータ</summary>
    [SerializeField] EncountData m_encountData;
    private EncountDataBase m_encountDatabase;
    /// <summary>敵グループの管理クラス</summary>
    private EnemyManager m_enemyManager;
    [Header("バトル中のパラメーター管理")]
    /// <summary>経過ターン数</summary>
    private int m_progressTurn = 0;
    #endregion
    #region その他のメンバ
    [Space]
    /// <summary>デッキ</summary>
    [SerializeField] Deck m_deck;
    /// <summary>捨て札</summary>
    [SerializeField] Discard m_discard;
    /// <summary>手札</summary>
    [SerializeField] Hand m_hand;
    /// <summary>報酬画面</summary>
    [SerializeField] Reward m_reward;
    /// <summary>カードデータ</summary>
    [SerializeField] NewCardData m_cardData;
    [SerializeField] GameObject m_cardPrefab;
    /// <summary>報酬枚数</summary>
    [SerializeField] int m_rewardNum = 3;
    /// <summary>ボタンの受付拒否</summary>
    private bool m_isPress = true;
    /// <summary>バトル中かどうかのフラグ</summary>
    private bool m_isGame = false;
    #endregion
    #endregion
    #region プロパティ
    public static BattleManager Instance { get; private set; }
    private Subject<int> m_turnBegin = new Subject<int>();
    private Subject<int> m_turnEnd = new Subject<int>();
    public IObservable<int> TurnBegin => m_turnBegin;
    public IObservable<int> TurnEnd2 => m_turnEnd;
    public int GetDrowNum => m_player.DrowNum;
    #endregion

    private void Awake()
    {
        Instance = this;
        m_cardData.Setup();
    }

    void Start()
    {
        Setup();
        m_reward.RewardDisabled();
    }

    /// <summary>
    /// セットアップ<br/>
    /// 最初に呼ぶ
    /// </summary>
    public void Setup()
    {
        if (m_isGame) GetComponent<Canvas>().enabled = true;
        else GetComponent<Canvas>().enabled = false;
    }

    public void SetCostText(string maxCost, string currentCost)
    {
        m_costViewText.text = currentCost + "/" + maxCost;
    }
    /// <summary>
    /// 戦闘開始
    /// </summary>
    /// <param name="enemyid">エンカウントした敵のID</param>
    public void BattleStart(int enemyid)
    {
        m_progressTurn = 0;
        m_isGame = true;
        Setup();
        CreateField(enemyid);
        FirstTurn();
    }
    /// <summary>
    /// 戦闘終了
    /// </summary>
    public void BatlteEnd()
    {
        m_discard.CardDelete();
        m_deck.CardDelete();
        m_hand.CardDelete();
        for (int i = 0; i < m_rewardNum; i++)
        {
            m_reward.RewardView(m_cardData.GetCardRarityRandom());
        }
    }

    /// <summary>
    /// 報酬画面終了
    /// </summary>
    /// <param name="getCardId"></param>
    public void RewardEnd(CardID getCardId)
    {
        DataManager.Instance.Cards.Add(getCardId);
        m_reward.RewardDisabled();
        GameManager.Instance.FloorFinished(m_player);
    }

    /// <summary>
    /// プレイヤーや敵の生成を行う
    /// </summary>
    /// <param name="enemyid"></param>
    private void CreateField(int enemyid)
    {
        //デッキとプレイヤー構築
        if (DataManager.Instance.IsSaveData())
        {
            //データが存在する場合は保存されているManagerから取ってくる
            Debug.Log("保存されたデータが見つかった");
            m_player = Instantiate(m_playerPrefab, m_playerPos).gameObject.GetComponent<Player>();
            m_player.SetParam(DataManager.Instance.Name, DataManager.Instance.Sprite, DataManager.Instance.MaxLife, DataManager.Instance.CurrentLife);
            for (int i = 0; i < DataManager.Instance.Cards.Count; i++)
            {
                CreateCard((int)DataManager.Instance.Cards[i]);
            }
        }
        else
        {
            //データが無い場合は初期値のデータから取ってくる
            Debug.Log("初回起動");
            m_player = Instantiate(m_playerPrefab, m_playerPos).gameObject.GetComponent<Player>();
            m_player.SetParam(m_playerStatsData.Name, m_playerStatsData.Image, m_playerStatsData.HP, m_playerStatsData.HP);
            for (int i = 0; i < m_playerStatsData.GetCardLength; i++)
            {
                CreateCard(m_playerStatsData.GetCard(i));
                DataManager.Instance.Cards.Add((CardID)m_playerStatsData.GetCard(i));
            }
        }
        //敵グループ生成
        m_encountDatabase = m_encountData.m_data[enemyid];
        m_enemyManager = m_enemies.GetComponent<EnemyManager>();
        for (int i = 0; i < m_encountDatabase.GetLength; i++)
        {
            //Debug.Log(m_encountDatabase.GetID(i));
            m_enemyManager.CreateEnemies(m_encountDatabase.GetID(i));
        }
        m_enemyManager.EnemyCount();
    }

    /// <summary>
    /// 最初のターンの特別処理<br/>
    /// いらんかも
    /// </summary>
    private void FirstTurn()
    {
        Debug.Log(m_progressTurn + "ターン目");
        //m_turnBegin.OnNext(m_progressTurn);
        m_progressTurn++;
        TurnStart();
    }

    /// <summary>ターン終了</summary>
    public void TurnEnd()
    {
        if (m_isPress) return;
        m_isPress = true;
        m_hand.AllCast();
        m_turnEnd.OnNext(m_progressTurn);
        //m_enemyManager.EnemyTrun(m_progressTurn);
        m_player.TurnEnd();
        m_progressTurn++;
        Invoke("TurnStart", 0.5f);
    }

    /// <summary>
    /// ターン開始
    /// </summary>
    private void TurnStart()
    {
        m_isPress = false;
        m_turnBegin.OnNext(m_progressTurn);
        //m_deck.Draw(m_drowNum);
        Debug.Log(m_progressTurn + "ターン目");
        m_player.TurnStart();
        SetCostText(m_player.MaxCost.ToString(), m_player.CurrrentCost.ToString());
    }

    /// <summary>
    /// カードの作成
    /// </summary>
    public void CreateCard(int id)
    {
        GameObject obj = Instantiate(m_cardPrefab);
        BlankCard card = obj.GetComponent<BlankCard>();
        NewCardDataBase cardData = m_cardData.CardDatas[id];
        card.SetInfo(cardData, m_player);
        obj.transform.SetParent(m_deck.transform, false);
        card.GetPlayerEffect();
    }
}