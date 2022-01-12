using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;

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
    private string m_tooltip;
    private string m_viewText;
    //private int m_power;
    public int Power { get; private set; }
    public int AttackNum { get; private set; }
    public int Block { get; private set; }
    public int BlockNum { get; private set; }
    private string m_cost;
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
    private UseType m_useType;
    private Player m_player;
    /// <summary>移動前の場所保存用</summary>
    private Vector2 m_defPos;
    /// <summary>捨て札</summary>
    private Transform m_discard;

    private void Setup()
    {
        m_discard = GameObject.Find("Discard").transform;
    }

    public Player Player { set => m_player = value; }

    public void SetInfo(NewCardDataBase carddata, Player player)
    {
        transform.Find("Name").GetComponent<Text>().text = carddata.CardName;
        transform.Find("Icon").GetComponent<Image>().sprite = carddata.Image;
        m_tooltip = carddata.Tooltip;
        transform.Find("Tooltip").GetComponent<Text>().text = m_tooltip;
        Power = carddata.Attack;
        AttackNum = carddata.AttackNum;
        Block = carddata.Block;
        BlockNum = carddata.BlockNum;
        m_cost = carddata.Cost;
        Conditions = carddata.Conditions;
        m_useType = carddata.UseType;
        m_player = player;
        Setup();
    }

    /// <summary>
    /// プレイヤーの状態を参照してカードのパラメーターを変える
    /// 基本ドローした時に呼ばれる
    /// </summary>
    /// <returns></returns>
    public void GetPlayerEffect()
    {
        string ret = m_tooltip;
        MatchCollection matches = Regex.Matches(m_tooltip, "{%power([0-9]*)}");
        foreach (Match m in matches)
        {
            int index = int.Parse(m.Groups[1].Value);
            Power = m_player.ConditionEffect(EventTiming.Attacked, ParametorType.Attack, int.Parse(m.Groups[1].Value));
            ret = ret.Replace(m.Value, Power.ToString());
        }
        matches = Regex.Matches(m_tooltip, "{%block([0-9]*)}");
        foreach (Match m in matches)
        {
            int index = int.Parse(m.Groups[1].Value);
            Block = m_player.ConditionEffect(EventTiming.Attacked, ParametorType.Block, int.Parse(m.Groups[1].Value));
            ret = ret.Replace(m.Value, Block.ToString());
        }
        m_viewText = ret;
        SetText();
    }

    public void SetText()
    {
        transform.Find("Tooltip").GetComponent<Text>().text = m_viewText;
    }

    public UseType GetCardType { get => m_useType; }

    public BlankCard OnCast()
    {
        m_player.CurrrentCost -= Cost; //プレイヤーのコストを減らす
        BattleManager.Instance.SetCostText(m_player.MaxCost.ToString(), m_player.CurrrentCost.ToString());
        transform.SetParent(m_discard, false); //捨て札に移動
        return this;
    }

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
            item.GetDrop(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_defPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = m_defPos;
    }
}