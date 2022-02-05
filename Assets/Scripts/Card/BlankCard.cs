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

/// <summary>
/// カードの実体
/// </summary>
public class BlankCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] Text m_viewCost;
    [SerializeField] Text m_viewName;
    [SerializeField] Image m_viewImage;
    [SerializeField] Text m_viewTooltip;
    [SerializeField, Tooltip("レア度に応じたカードの色。\nそれぞれ\nCommon\nRare\nElite\nSpecial\nCurse\nBadEffect\nの順")]
    List<Color> m_cardColor = new List<Color>();
    private bool m_isDrag = false;
    private bool m_isAnim = false;
    private bool m_isDiscarding = false;
    private string m_tooltip;
    private string m_cost;
    private UseType m_useType;
    private Player m_player;
    /// <summary>移動前の場所保存用</summary>
    private Vector2 m_defPos;
    /// <summary>捨て札</summary>
    private Transform m_discard;
    private Camera m_camera;
    public int Power { get; private set; }
    public int AttackNum { get; private set; }
    public int Block { get; private set; }
    public int BlockNum { get; private set; }
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

    private void Setup()
    {
        m_discard = GameObject.Find("Discard").transform;
        m_viewTooltip.text = m_tooltip;
        m_viewCost.text = m_cost;
    }
    public void SetInfo(NewCardDataBase carddata, Player player, Camera camera)
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
        m_useType = carddata.UseType;
        m_isDiscarding = carddata.IsDiscarding;
        m_player = player;
        GetComponent<Image>().color = m_cardColor[(int)carddata.Rarity];
        m_camera = camera;
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
            Power = m_player.ConditionEffect(EventTiming.Attacked, ParametorType.Attack, int.Parse(m.Groups[1].Value));
            text = text.Replace(m.Value, Power.ToString());
        }
        matches = Regex.Matches(m_tooltip, "{%def([0-9]*)}");
        foreach (Match m in matches)
        {
            int index = int.Parse(m.Groups[1].Value);
            Block = m_player.ConditionEffect(EventTiming.Attacked, ParametorType.Block, int.Parse(m.Groups[1].Value));
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
        //BattleManager.Instance.SetHandUI();
        BattleManager.Instance.CardCast();
        if (m_isDiscarding) Destroy(gameObject);
        else transform.SetParent(m_discard, false); //捨て札に移動
    }
    
    public void PointerEntor()
    {
        if (!m_isDrag && !m_isAnim)
        {
            m_isAnim = true;
            m_defPos = transform.position;
            transform.DOMoveY(m_defPos.y + 30, 0.05f).OnComplete(() => m_isAnim = false);
        }
    }
    public void PointerExit()
    {
        if (!m_isDrag)
        {
            m_isAnim = true;
            transform.DOMoveY(m_defPos.y, 0.05f).OnComplete(() => m_isAnim = false);
        }
    }
    //public void SetDefpos()
    //{
    //    m_defPos = transform.position;
    //}
    public void OnDrop(PointerEventData eventData)
    {
        if (m_player.CurrrentCost < Cost) //コスト足りなかったら使えない
        {
            Debug.Log($"コストが足りない!\nカードのコスト:{Cost} プレイヤーのコスト:{m_player.CurrrentCost}");
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
        //m_defPos = transform.position;
        m_isDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = eventData.position;
        //Debug.Log(eventData.position);
        //参考サイト https://shibuya24.info/entry/screen2world
        // マウスのスクリーン座標  
        Vector3 screenPos = eventData.position;
        // 例としてカメラから5m離れた場所を指定  
        screenPos.z = 5;
        // スクリーン座標をワールド座標に変換  
        var worldPos = m_camera.ScreenToWorldPoint(screenPos);
        transform.position = worldPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.DOMove(m_defPos, 0.1f);
        //transform.position = m_defPos;
        m_isDrag = false;
    }
}