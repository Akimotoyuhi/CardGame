using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mastar;
using System;
using UniRx;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

enum Timing { Start, End }

public class BattleManager : MonoBehaviour
{
    #region Player関連のメンバ
    [Header("プレイヤー関連")]
    /// <summary>プレイヤー初期データ</summary>
    [SerializeField] PlayerStatsData m_playerStatsData;
    /// <summary>プレイヤーのプレハブ</summary>
    [SerializeField] Player m_playerPrefab;
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
    //[SerializeField] EnemyData m_enemyData;
    /// <summary>敵グループの管理クラス</summary>
    [SerializeField] EnemyManager m_enemyManager;
    /// <summary>敵グループのDrop対象</summary>
    [SerializeField] EnemiesTarget m_enemiesTarget;
    /// <summary>経過ターン数</summary>
    private int m_currentTurn = 0;
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
    /// <summary>全てのドロップを受け取るクラス</summary>
    [SerializeField] AllDropTarget m_allDropTarget;
    /// <summary>報酬画面</summary>
    [SerializeField] Reward m_reward;
    /// <summary>戦闘時のUI管理クラス</summary>
    [SerializeField] BattleUIController m_battleUIController;
    /// <summary>カードの報酬枚数</summary>
    [SerializeField] int m_cardRewardNum = 3;
    /// <summary>レリックの報酬個数</summary>
    [SerializeField] int m_relicRewardNum = 3;
    /// <summary>カード追加コマンドが実行されたときに画面中央で止める秒数</summary>
    [SerializeField] float m_cardAddedShowDuration = 0.5f;
    [SerializeField] CommandManager m_commandManager;
    /// <summary>カメラ</summary>
    [SerializeField] Camera m_camera;
    /// <summary>カードデータ</summary>
    private CardData m_cardData;
    /// <summary>レリックデータ</summary>
    private RelicData m_relicData;
    /// <summary>カードのプレハブ</summary>
    private BlankCard m_cardPrefab;
    /// <summary>ドラッグ中のカードのUseType保存用</summary>
    private UseTiming? m_dragCardUseType = null;
    private EnemyType m_encountEnemyType;
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
    public IObservable<int> TurnBeginNotice => m_turnBegin;
    public IObservable<int> TurnEndNotice => m_turnEnd;
    public int GetDrowNum => m_player.DrowNum;
    public CommandManager CommandManager => m_commandManager;
    public bool IsGame { get => m_isGame; set => m_isGame = value; }
    public int CurrentTurn => m_currentTurn;
    public int DeckChildCount => m_deck.CardParent.childCount;
    public int HandChildCount => m_hand.CardParent.childCount;
    public int DiscardChildCount => m_discard.CardParent.childCount;
    public Player Player => m_player;
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    public void Setup()
    {
        m_cardData = GameManager.Instance.CardData;
        m_cardPrefab = GameManager.Instance.CardPrefab;
        m_cardData.Setup();
        m_relicData = GameManager.Instance.RelicData;
        Vector2 size = m_cardPrefab.gameObject.GetRectTransform().sizeDelta;
        m_deck.GridLayoutGroupSetting(size);
        m_discard.GridLayoutGroupSetting(size);
        SetCanvas();
        m_reward.RewardDisabled();
        m_reward.CanvasRectTransform = m_battleUICanvas.GetComponent<RectTransform>();
        //プレイヤーデータの保存
        List<int> cards = new List<int>();
        List<int> isUpgrade = new List<int>();
        for (int i = 0; i < m_playerStatsData.GetCardLength; i++)
        {
            cards.Add(m_playerStatsData.GetCardData(i));
            isUpgrade.Add(m_playerStatsData.IsUpgrade(i));
        }
        int playerMaxLife = GameManager.Instance.CustomEvaluation(CustomEntityType.PlayerAndCard, CustomParamType.Life, m_playerStatsData.HP);
        GameManager.Instance.PlayerDataSave(m_playerStatsData.Name, m_playerStatsData.IdleSprite, m_playerStatsData.AttackedSprite, m_playerStatsData.GameoverSprite, playerMaxLife, playerMaxLife, cards.ToArray(), isUpgrade.ToArray());
        foreach (var p in m_playerStatsData.GetRelicData)
            GameManager.Instance.SaveRelicData(p);
    }

    /// <summary>
    /// バトル中フラグによってCanvasの表示を切り替える
    /// </summary>
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

    /// <summary>
    /// コストテキストの更新
    /// </summary>
    public void SetCostText(string maxCost, string currentCost)
    {
        m_costViewText.text = currentCost + "/" + maxCost;
    }

    /// <summary>
    /// 戦闘開始
    /// </summary>
    public void BattleStart(EnemyType eria, MapID mapID)
    {
        m_currentTurn = 0;
        m_isGame = true;
        m_encountEnemyType = eria;
        SetCanvas();
        GameManager.Instance.RelicSetup();
        CreateField(eria, mapID);
        StartCoroutine(OnBattle());
    }

    /// <summary>
    /// 戦闘終了
    /// </summary>
    public void BattleEnd()
    {
        m_discard.CardDelete();
        m_deck.CardDelete();
        m_hand.CardDelete();
        for (int i = 0; i < m_cardRewardNum; i++)
            m_reward.CardData.Add(m_cardData.GetCardRarityRandom(0, m_encountEnemyType));
        for (int i = 0; i < m_relicRewardNum; i++)
            m_reward.RelicData.Add(m_relicData.GetRelic(m_encountEnemyType));
        m_reward.ShowRewardPanel(() => GameManager.Instance.FloorFinished(m_player));
    }

    /// <summary>
    /// 新規カードの保存
    /// </summary>
    public void GetCard(CardID cardId, int isUpgrade)
    {
        DataManager.Instance.AddCards(cardId, isUpgrade);
    }

    /// <summary>
    /// 新規レリックの保存
    /// </summary>
    public void GetRelic(RelicID relic)
    {
        GameManager.Instance.SaveRelicData(relic);
    }

    /// <summary>
    /// プレイヤーや敵の生成を行う
    /// </summary>
    /// <param name="enemyid"></param>
    private void CreateField(EnemyType eria, MapID mapID)
    {
        //プレイヤー生成
        if (!DataManager.Instance.IsPlayerData) //データが無かったら今持ってるデッキを保存
        {
            List<int> cards = new List<int>();
            List<int> isUpgrade = new List<int>();
            for (int i = 0; i < m_playerStatsData.GetCardLength; i++)
            {
                cards.Add(m_playerStatsData.GetCardData(i));
                isUpgrade.Add(m_playerStatsData.IsUpgrade(i));
            }
            int playerMaxLife = GameManager.Instance.CustomEvaluation(CustomEntityType.PlayerAndCard, CustomParamType.Life, m_playerStatsData.HP);
            GameManager.Instance.PlayerDataSave(m_playerStatsData.Name, m_playerStatsData.IdleSprite, m_playerStatsData.AttackedSprite, m_playerStatsData.GameoverSprite, playerMaxLife, playerMaxLife, cards.ToArray(), isUpgrade.ToArray());
        }
        m_player = Instantiate(m_playerPrefab, m_playerPos);
        m_player.Canvas = m_battleCanvas.transform;
        m_player.SetParam(DataManager.Instance.Name, DataManager.Instance.IdleSprite, DataManager.Instance.AttackedSprite, DataManager.Instance.GameoverSprite, DataManager.Instance.MaxLife, DataManager.Instance.CurrentLife);
        //敵グループ生成
        m_enemyManager.Setup(m_enemiesTarget);
        m_enemyManager.CreateEnemies(eria, mapID);
        m_enemyManager.EnemyCount();
        m_commandManager.Setup(m_enemyManager, m_player, m_hand, m_discard, m_deck);
        //カード生成
        for (int i = 0; i < DataManager.Instance.Cards.Count; i++)
        {
            int[] nms = DataManager.Instance.Cards[i];
            CreateCard(nms[0], nms[1]);
        }
    }

    /// <summary>
    /// バトル中の流れ
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnBattle()
    {
        m_isPress = true;
        if (m_currentTurn == 0)
        {
            m_enemyManager.ActionPlan(m_currentTurn);
            m_battleUIController.Play(BattleUIType.BattleStart, () => m_battleFlag = true);
            while (!m_battleFlag)
                yield return null;
            m_battleFlag = false;

            FirstTurn();
        }
        else
        {
            m_battleUIController.Play(BattleUIType.EnemyTurn, () => m_battleFlag = true);
            while (!m_battleFlag)
                yield return null;
            m_battleFlag = false;
            TurnEnd();
        }
        m_enemyManager.EnemyTrun(m_currentTurn, () => m_battleFlag = true);
        while (!m_battleFlag)
            yield return null;
        m_battleFlag = false;

        m_battleUIController.Play(BattleUIType.PlayerTurn, () => m_battleFlag = true);
        while (!m_battleFlag)
            yield return null;
        m_battleFlag = false;
        TurnStart();
        m_isPress = false;
    }

    /// <summary>
    /// 最初のターンの特別処理<br/>
    /// いらんかも
    /// </summary>
    private void FirstTurn()
    {
        Debug.Log(m_currentTurn + "ターン目");
        //m_enemyManager.EnemyTrun(m_currentTurn);
        GameManager.Instance.RelicExecute(RelicTriggerTiming.BattleBegin, ParametorType.Other, 0);
        m_currentTurn++;
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
        m_hand.AllCast();
        GameManager.Instance.RelicExecute(RelicTriggerTiming.TurnEnd, ParametorType.Other, 0);
        m_player.TurnEnd();
        m_currentTurn++;
    }

    /// <summary>
    /// ターン開始
    /// </summary>
    private void TurnStart()
    {
        Debug.Log(m_currentTurn + "ターン目");
        GameManager.Instance.RelicExecute(RelicTriggerTiming.TurnBegin, ParametorType.Other, 0);
        m_player.TurnStart();
        m_turnBegin.OnNext(m_currentTurn);
        SetCostText(m_player.MaxCost.ToString(), m_player.CurrrentCost.ToString());
    }

    /// <summary>
    /// カードを引く<br/>
    /// 今後カードデータと一緒に改善予定
    /// </summary>
    public void CardDraw(int drawNum)
    {
        m_deck.Draw(drawNum);
    }

    /// <summary>
    /// カードを捨てる<br/>
    /// 今後カードデータと一緒に改善予定
    /// </summary>
    public void CardDispose(int disposeNum)
    {
        Debug.LogWarning("未実装");
        return;
    }

    /// <summary>
    /// カードの作成
    /// </summary>
    public void CreateCard(int id, int upgradeNum)
    {
        //GameObject obj = Instantiate(m_cardPrefab);
        BlankCard card = Instantiate(m_cardPrefab);
        CardInfomationData cardData;
        cardData = m_cardData.CardDatas(id, upgradeNum);
        card.SetInfo(cardData, m_battleUICanvas.GetComponent<RectTransform>(), m_player, m_camera, m_discard);
        card.CardState = CardState.None;
        card.transform.SetParent(m_deck.CardParent, false);
        card.GetPlayerEffect();
    }

    /// <summary>
    /// 手札の全カードのテキストを更新させる
    /// </summary>
    public void CardCast()
    {
        m_hand.UpdateTooltip();
    }

    /// <summary>
    /// 場にカードを追加する
    /// </summary>
    public async void AddCard(CardAddDestination addDestination, CardID cardID, int num, int isUpgrade)
    {
        for (int i = 0; i < num; i++)
        {
            Vector2 v = default;
            CardInfomationData c = m_cardData.CardDatas((int)cardID, isUpgrade);
            BlankCard b = Instantiate(m_cardPrefab);
            b.SetInfo(c, m_battleUICanvas.GetComponent<RectTransform>(), m_player, m_camera, m_discard);
            switch (addDestination)
            {
                case CardAddDestination.ToDeck:
                    b.transform.SetParent(m_deck.CardParent, false);
                    v = m_deck.AddCardMoveingAtPos;
                    break;
                case CardAddDestination.ToHand:
                    b.transform.SetParent(m_hand.CardParent, false);
                    b.CardState = CardState.Play;
                    v = m_hand.AddCardMoveingAtPos;
                    break;
                case CardAddDestination.ToDiscard:
                    b.transform.SetParent(m_discard.CardParent, false);
                    v = m_discard.AddCardMoveingAtPos;
                    break;
                default:
                    Debug.LogError("存在しないの追加先");
                    break;
            }
            await MoveingCard(v, cardID, isUpgrade);
        }
    }

    /// <summary>カードが追加されたときに追加されたカードを表示させる</summary>
    public async UniTask MoveingCard(Vector2 moveingTo, CardID cardId, int upgrade)
    {
        BlankCard card = Instantiate(m_cardPrefab);
        card.SetInfo(m_cardData.CardDatas((int)cardId, upgrade));
        card.CardState = CardState.None;
        card.transform.SetParent(m_battleUICanvas.gameObject.GetRectTransform(), false);
        RectTransform rt = card.gameObject.GetRectTransform();
        Sequence s = DOTween.Sequence();
        await s.AppendInterval(m_cardAddedShowDuration)
            .Append(rt.DOAnchorPos(moveingTo, 0.1f))
            .OnComplete(() => Destroy(card.gameObject))
            .AsyncWaitForCompletion();
    }

    /// <summary>カードのドラッグ中に各ドロップ対象に枠を表示させる</summary>
    /// <param name="useType">使用対象</param>
    public void OnCardDrag(UseTiming? useType)
    {
        m_enemiesTarget.OnCard(useType);
        m_player.OnCard(useType);
        m_allDropTarget.OnCard(useType);
        m_enemyManager.OnCardDrag(useType);
    }
}