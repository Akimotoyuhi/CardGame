using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using DG.Tweening;

/// <summary>カードの状態<br/>何に使用するカードかを設定するため</summary>
public enum CardState
{
    None,
    Play,
    Upgrade,
    Reward,
}

/// <summary>
/// カードの実体
/// </summary>
public class BlankCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    #region field
    [SerializeField] Text m_viewCost;
    [SerializeField] Text m_viewName;
    [SerializeField] Image m_viewImage;
    [SerializeField] Text m_viewTooltip;
    [SerializeField] CardEffectHelp m_cardEffectHelp;
    [SerializeField, Tooltip("レア度に応じたカードの色。\nそれぞれ\nCommon\nRare\nElite\nSpecial\nCurse\nBadEffect\nの順")]
    private List<Color> m_cardColor = default;
    /// <summary>カード効果<br/>効果の種類(CommandParam), 発動対象(UseType), 効果(int)</summary>
    private List<int[]> m_cardCommand = new List<int[]>();
    /// <summary>このカードのデータ</summary>
    private CardInfomationData m_carddata;
    /// <summary>ドラッグ中フラグ</summary>
    private bool m_isDrag = false;
    /// <summary>アニメーション中フラグ</summary>
    private bool m_isAnim = false;
    /// <summary>廃棄カードフラグ</summary>
    private bool m_isDiscarding = false;
    /// <summary>エセリアル</summary>
    private bool m_ethereal = false;
    /// <summary>カードの状態</summary>
    private CardState m_cardState = default;
    private string m_tooltip;
    private string m_cost;
    /// <summary>強化されたかどうか</summary>
    private int m_upgrade;
    /// <summary>DataManagerが管理しているこのカードのindex</summary>
    private int m_index;
    private CardID m_cardID;
    private UseTiming m_useType;
    private CardConditional m_conditional;
    private Reward m_reward;
    private Rest m_rest;
    private Player m_player;
    /// <summary>移動前の場所保存用</summary>
    private Vector2 m_defPos;
    /// <summary>捨て札</summary>
    private Discard m_discard;
    /// <summary>自分のRectTransform</summary>
    private RectTransform m_rectTransform;
    /// <summary>カメラ</summary>
    private Camera m_camera;
    /// <summary>CanvasのRectTransform</summary>
    private RectTransform m_canvasRect;
    #endregion
    public CardState CardState { set => m_cardState = value; }
    public int Cost
    {
        get
        {
            int ret = default;
            if (int.TryParse(m_cost, out ret))
            {
                ret = int.Parse(m_cost);
            }
            else
            {
                ret = m_player.CurrrentCost;
            }
            return ret;
        }
    }
    public UseTiming UseType { get => m_useType; }
    public string Name => m_viewName.text;

    private void Setup()
    {
        //m_viewTooltip.text = m_tooltip;
        m_viewCost.text = m_cost;
        m_rectTransform = gameObject.GetComponent<RectTransform>();
        List<Condition> c = new List<Condition>();
        ConditionSelection cs = new ConditionSelection();
        bool trueDmg = false;
        foreach (var com in m_cardCommand)
        {
            CommandParam cp = (CommandParam)com[(int)CommonCmdEnum.CommandParam];
            if (cp == CommandParam.Conditon)
                c.Add(cs.SetCondition((ConditionID)com[(int)ConditionCmdEnum.ConditionID], com[(int)ConditionCmdEnum.Turn]));
            if (cp == CommandParam.Attack && !trueDmg)
                trueDmg = com[(int)AttackCmdEnum.TrueDmg] == 1 ? true : false;
        }
        m_cardEffectHelp.SetText(m_isDiscarding, m_ethereal, trueDmg, c);
        m_cardEffectHelp.SetActive(false);
    }
    private void Init()
    {
        m_reward = null;
        m_player = null;
    }
    public void SetInfo(CardInfomationData carddata, RectTransform canvasRect, Player player, Camera camera, Discard discard)
    {
        Init();
        SetCardData(carddata);
        m_player = player;
        m_camera = camera;
        m_canvasRect = canvasRect;
        m_discard = discard;
        Setup();
    }
    public void SetInfo(CardInfomationData carddata, int upgrade, Reward reward)
    {
        Init();
        SetCardData(carddata);
        m_upgrade = upgrade;
        m_reward = reward;
    }
    public void SetInfo(CardInfomationData carddata, int index, Rest rest)
    {
        Init();
        SetCardData(carddata);
        m_index = index;
        m_rest = rest;
    }
    public void SetInfo(CardInfomationData carddata)
    {
        Init();
        SetCardData(carddata);
    }
    /// <summary>
    /// カードデータの反映
    /// </summary>
    /// <param name="carddata"></param>
    private void SetCardData(CardInfomationData carddata)
    {
        m_viewName.text = carddata.Name;
        m_viewImage.sprite = carddata.Sprite;
        m_tooltip = carddata.Tooltip;
        m_carddata = carddata;
        m_cost = carddata.Cost;
        GetComponent<Image>().color = m_cardColor[(int)carddata.Rarity];
        m_cardID = carddata.CardId;
        m_useType = carddata.UseType;
        m_conditional = carddata.CardConditional;
        m_isDiscarding = carddata.IsDiscarding;
        m_ethereal = carddata.Ethereal;
        GetPlayerEffect();
        Setup();
    }

    /// <summary>
    /// プレイヤーの状態を参照してカード内のパラメーターを変える<br/>
    /// 基本ドローした時に呼ばれる
    /// </summary>
    /// <returns></returns>
    public void GetPlayerEffect()
    {
        string text = m_tooltip;
        List<int[]> n = new List<int[]>();
        foreach (var c in m_carddata.Commands)
            n.Add(c.CardCommand);
        m_cardCommand = n;
        //m_cardCommand = m_carddata.Commands;
        if (m_cardState == CardState.Play)
        {
            foreach (var cc in m_cardCommand)
            {
                CommandParam cp = (CommandParam)cc[(int)CommonCmdEnum.CommandParam];
                bool b;
                switch (cp)//プレイヤーのバフを評価して数値を増減させる
                {
                    case CommandParam.Attack:
                        b = cc[(int)AttackCmdEnum.TrueDmg] == 1 ? true : false;
                        if (!b)
                        {
                            cc[(int)AttackCmdEnum.Power] = GameManager.Instance.CustomEvaluation(CustomEntityType.PlayerAndCard, CustomParamType.Power, cc[(int)AttackCmdEnum.Power]);
                            cc[(int)AttackCmdEnum.Power] = m_player.OnBattleEffect(EventTiming.Attacked, ParametorType.Attack, cc[(int)AttackCmdEnum.Power]);
                        }
                        break;
                    case CommandParam.Block:
                        b = cc[(int)BlockCmdEnum.TrueBlk] == 1 ? true : false;
                        if (!b)
                        {
                            cc[(int)BlockCmdEnum.Block] = GameManager.Instance.CustomEvaluation(CustomEntityType.PlayerAndCard, CustomParamType.Difence, cc[(int)BlockCmdEnum.Block]);
                            cc[(int)BlockCmdEnum.Block] = m_player.OnBattleEffect(EventTiming.Attacked, ParametorType.Block, cc[(int)BlockCmdEnum.Block]);
                        }
                        break;
                    default:
                        continue;
                }
                if (cc[(int)AttackCmdEnum.Power] <= 1) cc[(int)AttackCmdEnum.Power] = 1;
            }
        }
        MatchCollection match = Regex.Matches(text, "{leg([0-9]*)}");
        foreach (Match m in match)
        {
            //{leg}と書かれた部分を対応した変数に置き換える
            int index = int.Parse(m.Groups[1].Value);
            if (m_cardCommand.Count < index || (CommandParam)m_cardCommand[index][0] == CommandParam.Conditon)
                continue;
            text = text.Replace(m.Value, m_cardCommand[index][3].ToString());
        }
        SetText(text);
        UpdateCostText();
    }

    public void SetText(string text)
    {
        m_viewTooltip.text = text;
    }

    /// <summary>
    /// カードを捨て札に移動する
    /// </summary>
    public void OnCast(bool isUsed)
    {
        if (isUsed)
        {
            m_player.CurrrentCost -= Cost;
            BattleManager.Instance.SetCostText(m_player.MaxCost.ToString(), m_player.CurrrentCost.ToString());
        }
        else
        {
            List<int[]> vs = new List<int[]>();
            for (int i = 0; i < m_carddata.Commands.Count; i++)
            {
                if (m_carddata.Commands[i].Conditional.Count > 0)
                {
                    if (m_carddata.Commands[i].Conditional.Evaluation(m_player, null, BattleManager.Instance.DeckChildCount, BattleManager.Instance.HandChildCount, BattleManager.Instance.DiscardChildCount, isUsed))
                    {
                        vs.Add(m_cardCommand[i]);
                    }
                }
            }
            if (vs.Count > 0)
                BattleManager.Instance.CommandManager.CommandExecute(vs, true);
            if (m_ethereal)
                Destroy(gameObject);
        }
        BattleManager.Instance.OnCardDrag(null);
        m_isDrag = false;
        BattleManager.Instance.CardCast();
        m_cardState = CardState.None;
        UpdateCostText();
        List<int[]> n = new List<int[]>();
        foreach (var c in m_carddata.Commands)
            n.Add(c.CardCommand);
        m_cardCommand = n;
        //m_cardCommand = m_carddata.Command;
        if (isUsed && m_isDiscarding) Destroy(gameObject);
        else transform.SetParent(m_discard.CardParent, false); //捨て札に移動
    }

    /// <summary>
    /// プレイヤーのコストによってカード左上のコストの色を変化させる
    /// </summary>
    private void UpdateCostText()
    {
        if (!m_player) return;
        if (Cost <= m_player.CurrrentCost)
            m_viewCost.color = Color.black;
        else if (m_cardState != CardState.Play)
            m_viewCost.color = Color.black;
        else
            m_viewCost.color = Color.red;
    }

    //以下インターフェース

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!m_isDrag && !m_isAnim && m_cardState == CardState.Play)
        {
            m_isAnim = true;
            m_defPos = m_rectTransform.anchoredPosition;
            m_rectTransform.DOAnchorPosY(m_defPos.y + 30f, 0.05f).OnComplete(() => m_isAnim = false);
        }
        m_cardEffectHelp.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!m_isDrag && m_cardState == CardState.Play)
        {
            m_isAnim = true;
            m_rectTransform.DOAnchorPos3DY(m_defPos.y, 0.05f).OnComplete(() => m_isAnim = false);
        }
        m_cardEffectHelp.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //これが無いとOnPointerUpが呼ばれない
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (m_cardState == CardState.Reward)
        {
            if (!m_reward) return;
            m_reward.OnClickCard(m_cardID, m_upgrade);
        }
        else if (m_cardState == CardState.Upgrade)
        {
            if (!m_rest) return;
            m_rest.OnUpgrade(m_index);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (m_cardState != CardState.Play)
            return;
        if (m_player.CurrrentCost < Cost) //コスト足りなかったら使えない
        {
            EffectManager.Instance.SetUIText(PanelType.Battle, "コストが足りない！", Color.red, 1f);
            return;
        }
        //ドロップ時に自分の座標にRayを飛ばす
        var result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);
        foreach (var hit in result)
        {
            //ドロップされたオブジェクトがドロップを受け付けるインターフェースが実装されていれば自分の情報を渡す
            IDrop dropObj = hit.gameObject.GetComponent<IDrop>();
            if (dropObj == null || !dropObj.CanDrop(m_useType)) continue;
            EnemyBase enemy = dropObj.IsEnemy();
            //使用条件調べる
            if (!m_conditional.Evaluation(m_player, enemy, BattleManager.Instance.DeckChildCount, BattleManager.Instance.HandChildCount, BattleManager.Instance.DiscardChildCount, true))
            {
                EffectManager.Instance.SetUIText(PanelType.Battle, "使用条件を満たしていない！", Color.red, 1f);
                continue;
            }
            List<int[]> vs = new List<int[]>();
            for (int i = 0; i < m_carddata.Commands.Count; i++)
            {
                if (m_carddata.Commands[i].Conditional.Evaluation(m_player, enemy, BattleManager.Instance.DeckChildCount, BattleManager.Instance.HandChildCount, BattleManager.Instance.DiscardChildCount, true))
                {
                    vs.Add(m_cardCommand[i]);
                }
            }
            dropObj.GetDrop(vs);
            OnCast(true);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (m_cardState == CardState.Play)
        {
            m_isDrag = true;
            BattleManager.Instance.OnCardDrag(m_useType);
        }
        m_cardEffectHelp.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (m_cardState == CardState.Play)
        {
            //カードの座標をマウスの座標と合わせる
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_canvasRect, eventData.position, m_camera, out localPoint);
            localPoint.y += 230; //原因は分からないけど座標変換後ちょっとズレるのでそれの修正用
            m_rectTransform.localPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_cardState == CardState.Play)
        {
            m_rectTransform.DOAnchorPos(m_defPos, 0.1f)
                .OnComplete(() => m_isDrag = false);
            BattleManager.Instance.OnCardDrag(null);
        }
    }
}