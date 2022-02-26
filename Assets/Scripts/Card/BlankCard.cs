using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using DG.Tweening;

public enum ReplaceType
{
    Attack,
    Block
}
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
    [SerializeField] Text m_viewCost;
    [SerializeField] Text m_viewName;
    [SerializeField] Image m_viewImage;
    [SerializeField] Text m_viewTooltip;
    [SerializeField, Tooltip("レア度に応じたカードの色。\nそれぞれ\nCommon\nRare\nElite\nSpecial\nCurse\nBadEffect\nの順")]
    private List<Color> m_cardColor = default;
    /// <summary>ドラッグ中フラグ</summary>
    private bool m_isDrag = false;
    /// <summary>アニメーション中フラグ</summary>
    private bool m_isAnim = false;
    /// <summary>廃棄カードフラグ</summary>
    private bool m_isDiscarding = false;
    /// <summary>カードの状態</summary>
    private CardState m_cardState = default;
    private string m_tooltip;
    private string m_cost;
    /// <summary>強化されたかどうか</summary>
    private int m_upgrade;
    /// <summary>DataManagerが管理しているこのカードのindex</summary>
    private int m_index;
    private CardID m_cardID;
    private UseType m_useType;
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
    public int Power { get; private set; }
    public int AttackNum { get; private set; }
    public int Block { get; private set; }
    public int BlockNum { get; private set; }
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
    public List<Condition> Conditions { get; private set; }
    public UseType GetCardType { get => m_useType; }
    public string Name => m_viewName.text;

    private void Setup()
    {
        //m_viewTooltip.text = m_tooltip;
        m_viewCost.text = m_cost;
        m_rectTransform = gameObject.GetComponent<RectTransform>();
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
        Power = carddata.Attack;
        AttackNum = carddata.AttackNum;
        Block = carddata.Block;
        BlockNum = carddata.BlockNum;
        m_cost = carddata.Cost;
        Conditions = carddata.Conditions;
        GetComponent<Image>().color = m_cardColor[(int)carddata.Rarity];
        m_cardID = carddata.CardId;
        m_useType = carddata.UseType;
        m_isDiscarding = carddata.IsDiscarding;
        GetPlayerEffect();
        Setup();
    }

    /// <summary>
    /// プレイヤーの状態を参照してカードのパラメーターを変える
    /// 基本ドローした時に呼ばれる
    /// </summary>
    /// <returns></returns>
    public void GetPlayerEffect()
    {
        string text = m_tooltip;
        MatchCollection matches = Regex.Matches(m_tooltip, "{%atk([0-9]*)}");
        foreach (Match m in matches)
        {
            int index = int.Parse(m.Groups[1].Value);
            if (m_cardState == CardState.Play) { Power = m_player.ConditionEffect(EventTiming.Attacked, ParametorType.Attack, int.Parse(m.Groups[1].Value)); }
            else Power = int.Parse(m.Groups[1].Value);
            text = text.Replace(m.Value, Power.ToString());
        }
        matches = Regex.Matches(m_tooltip, "{%def([0-9]*)}");
        foreach (Match m in matches)
        {
            int index = int.Parse(m.Groups[1].Value);
            if (m_cardState == CardState.Play) { Block = m_player.ConditionEffect(EventTiming.Attacked, ParametorType.Block, int.Parse(m.Groups[1].Value)); }
            else Block = int.Parse(m.Groups[1].Value);
            text = text.Replace(m.Value, Block.ToString());
        }
        SetText(text);
    }

    public void SetText(string text)
    {
        m_viewTooltip.text = text;
    }

    public void OnCast()
    {
        m_player.CurrrentCost -= Cost; //プレイヤーのコストを減らす
        m_player.PlayerAnim();
        BattleManager.Instance.SetCostText(m_player.MaxCost.ToString(), m_player.CurrrentCost.ToString());
        m_isDrag = false;
        //BattleManager.Instance.SetHandUI();
        BattleManager.Instance.CardCast();
        m_cardState = CardState.None;
        if (m_isDiscarding) Destroy(gameObject);
        else transform.SetParent(m_discard.CardParent, false); //捨て札に移動
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!m_isDrag && !m_isAnim && m_cardState == CardState.Play)
        {
            m_isAnim = true;
            m_defPos = m_rectTransform.anchoredPosition;
            m_rectTransform.DOAnchorPosY(m_defPos.y + 10f, 0.05f).OnComplete(() => m_isAnim = false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!m_isDrag && m_cardState == CardState.Play)
        {
            m_isAnim = true;
            m_rectTransform.DOAnchorPos3DY(m_defPos.y, 0.05f).OnComplete(() => m_isAnim = false);
            //transform.DOMoveY(m_defPos.y, 0.05f).OnComplete(() => m_isAnim = false);
        }
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
            m_reward.OnClick(m_cardID, m_upgrade);
        }
        else if (m_cardState == CardState.Upgrade)
        {
            if (!m_rest) return;
            m_rest.OnUpgrade(m_index);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (m_cardState != CardState.Play) return;
        if (m_player.CurrrentCost < Cost) //コスト足りなかったら使えない
        {
            Debug.Log($"コストが足りない!\nカードのコスト:{Cost} プレイヤーのコスト:{m_player.CurrrentCost}");
            EffectManager.Instance.SetBattleUIText("コストが足りない！", Color.red, 1f);
            return;
        }
        //ドロップ時に自分の座標にRayを飛ばす
        var result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);
        foreach (var hit in result)
        {
            //ドロップされたオブジェクトがドロップを受け付けるインターフェースが実装されていれば自分の情報を渡す
            IDrop item = hit.gameObject.GetComponent<IDrop>();
            if (item == null) continue;
            List<Condition> conditions = Conditions;
            item.GetDrop(Power, Block, conditions, m_useType, OnCast);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (m_cardState == CardState.Play)
        {
            m_isDrag = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (m_cardState == CardState.Play)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_canvasRect, eventData.position, m_camera, out localPoint);
            localPoint.y += 230;
            m_rectTransform.localPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_cardState == CardState.Play)
        {
            m_rectTransform.DOAnchorPos(m_defPos, 0.1f)
                .OnComplete(() => m_isDrag = false);
        }
    }
}