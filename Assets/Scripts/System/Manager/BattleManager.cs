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
    /// <summary>敵データ管理クラス</summary>
    [SerializeField] EnemyData m_enemyData;
    /// <summary>敵グループの管理クラス</summary>
    private EnemyManager m_enemyManager;
    [Header("バトル中のパラメーター管理")]
    /// <summary>経過ターン数</summary>
    private int m_progressTurn = 0;
    #endregion
    #region その他のメンバ
    /// <summary>バトル画面表示用キャンバス</summary>
    [Space, SerializeField] Canvas m_battleCanvas;
    /// <summary>バトル画面のUI表示用キャンバス</summary>
    [SerializeField] Canvas m_battleUICanvas;
    /// <summary>デッキ</summary>
    [SerializeField] Deck m_deck;
    /// <summary>捨て札</summary>
    [SerializeField] Discard m_discard;
    /// <summary>手札</summary>
    [SerializeField] Hand m_hand;
    /// <summary>報酬画面</summary>
    [SerializeField] Reward m_reward;
    /// <summary>戦闘時のUI管理クラス</summary>
    [SerializeField] BattleUIController m_battleUIController;
    /// <summary>カードデータ</summary>
    [SerializeField] NewCardData m_cardData;
    /// <summary>カードのプレハブ</summary>
    [SerializeField] GameObject m_cardPrefab;
    /// <summary>ダメージ表示用のテキスト</summary>
    [SerializeField] GameObject m_damageTextPrefab;
    /// <summary>報酬枚数</summary>
    [SerializeField] int m_rewardNum = 3;
    /// <summary>カメラ</summary>
    [SerializeField] Camera m_camera;
    /// <summary>ボタンの受付</summary>
    private bool m_isPress = true;
    /// <summary>バトル中かどうかのフラグ</summary>
    private bool m_isGame = false;
    /// <summary>戦闘中のコルーチンに使うフラグ</summary>
    private bool m_battleFlag = false;
    /// <summary>ターン開始を通知する</summary>
    private Subject<int> m_turnBegin = new Subject<int>();
    /// <summary>ターン終了を通知する</summary>
    private Subject<int> m_turnEnd = new Subject<int>();
    #endregion
    #region プロパティ
    public static BattleManager Instance { get; private set; }
    public IObservable<int> TurnBegin => m_turnBegin;
    public IObservable<int> TurnEnd2 => m_turnEnd;
    public int GetDrowNum => m_player.DrowNum;
    public bool IsGame { get => m_isGame; set => m_isGame = value; }
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        m_cardData.Setup();
        SetCanvas();
        m_reward.RewardDisabled();
        m_reward.CanvasRectTransform = m_battleUICanvas.GetComponent<RectTransform>();
    }
    public void SetCanvas()
    {
        if (m_isGame)
        {
            m_battleCanvas.enabled = true;
            m_battleUICanvas.enabled = true;
        }
        else
        {
            m_battleCanvas.enabled = false;
            m_battleUICanvas.enabled = false;
        }
    }

    public void SetCostText(string maxCost, string currentCost)
    {
        m_costViewText.text = currentCost + "/" + maxCost;
    }
    /// <summary>
    /// 戦闘開始
    /// </summary>
    /// <param name="enemyid">エンカウントした敵のID</param>
    public void BattleStart(EnemyAppearanceEria eria)
    {
        m_progressTurn = 0;
        m_isGame = true;
        SetCanvas();
        CreateField(eria);
        StartCoroutine(OnBattle());
        //m_battleUIController.Play(BattleUIType.BattleStart, FirstTurn);
        //FirstTurn();
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
    public void RewardEnd(int getCardId, int isUpgrade)
    {
        DataManager.Instance.AddCards(getCardId, isUpgrade);
        m_reward.RewardDisabled();
        GameManager.Instance.FloorFinished(m_player);
    }

    /// <summary>
    /// プレイヤーや敵の生成を行う
    /// </summary>
    /// <param name="enemyid"></param>
    private void CreateField(EnemyAppearanceEria eria)
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
                int[] nms = DataManager.Instance.Cards[i];
                CreateCard(nms[0], nms[1]);
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
                CreateCard(m_playerStatsData.GetCardData(i), m_playerStatsData.IsUpgrade(i));
                DataManager.Instance.AddCards(m_playerStatsData.GetCardData(i), m_playerStatsData.IsUpgrade(i));
            }
        }
        //敵グループ生成
        m_enemyManager = m_enemies.GetComponent<EnemyManager>();
        m_enemyManager.CreateEnemies(eria);
        m_enemyManager.EnemyCount();
    }
    /// <summary>
    /// バトル中の流れ
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnBattle()
    {
        if (m_progressTurn == 0)
        {
            m_battleUIController.Play(BattleUIType.BattleStart, () => m_battleFlag = true);
            while (!m_battleFlag)
            {
                yield return null;
            }
            FirstTurn();
            m_battleFlag = false;
        }
        else
        {
            m_battleUIController.Play(BattleUIType.EnemyTurn, () => m_battleFlag = true);
            while (!m_battleFlag)
            {
                yield return null;
            }
            TurnEnd();
            m_battleFlag = false;
        }
        m_battleUIController.Play(BattleUIType.PlayerTurn, () => m_battleFlag = true);
        while (!m_battleFlag)
        {
            yield return null;
        }
        TurnStart();
        m_battleFlag = false;
    }

    /// <summary>
    /// 最初のターンの特別処理<br/>
    /// いらんかも
    /// </summary>
    private void FirstTurn()
    {
        Debug.Log(m_progressTurn + "ターン目");
        m_enemyManager.EnemyTrun(m_progressTurn);
        m_progressTurn++;
    }

    public void OnClick()
    {
        if (m_isPress) return;
        StartCoroutine(OnBattle());
    }

    /// <summary>
    /// ターン終了
    /// </summary>
    private void TurnEnd()
    {
        m_isPress = true;
        m_hand.AllCast();
        m_player.TurnEnd();
        m_turnEnd.OnNext(m_progressTurn);
        m_progressTurn++;
        //m_battleUIController.Play(BattleUIType.PlayerTurn, TurnStart);
    }

    /// <summary>
    /// ターン開始
    /// </summary>
    private void TurnStart()
    {
        m_isPress = false;
        Debug.Log(m_progressTurn + "ターン目");
        m_player.TurnStart();
        m_turnBegin.OnNext(m_progressTurn);
        SetCostText(m_player.MaxCost.ToString(), m_player.CurrrentCost.ToString());
    }

    /// <summary>
    /// カードの作成
    /// </summary>
    public void CreateCard(int id, int isUpgrade)
    {
        GameObject obj = Instantiate(m_cardPrefab);
        BlankCard card = obj.GetComponent<BlankCard>();
        NewCardDataBase cardData;
        if (isUpgrade == 1) { cardData = m_cardData.CardDatas[id].UpgradeData; }
        else { cardData = m_cardData.CardDatas[id]; }
        card.SetInfo(cardData, m_battleUICanvas.GetComponent<RectTransform>(), m_player, m_camera, m_discard);
        //m_deck.SetParent(obj.transform);
        obj.transform.SetParent(m_deck.CardParent, false);
        card.GetPlayerEffect();
    }
    public void ViewText(string str, RectTransform tra, ColorType colorType)
    {
        DamageText text = Instantiate(m_damageTextPrefab).GetComponent<DamageText>();
        text.transform.SetParent(tra);
        text.transform.position = tra.anchoredPosition;
        text.Color(colorType);
        text.ChangeText(str);
        text.RandomMove();
    }
    /// <summary>
    /// カード使用時に呼ばれて手札の全カードのテキストを更新させる関数
    /// </summary>
    public void CardCast()
    {
        m_hand.UpdateTooltip();
    }
}