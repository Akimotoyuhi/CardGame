using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// カードの実体
/// </summary>
public class BlankCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    private string m_tooltip;
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
                ret = m_player.Cost;
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

    public UseType GetCardType { get => m_useType; }

    public BlankCard OnCast()
    {
        m_player.Cost -= Cost; //プレイヤーのコストを減らす
        transform.SetParent(m_discard, false); //捨て札に移動
        return this;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (m_player.Cost < Cost) //コスト足りなかったら使えない
        {
            Debug.Log($"コストが足りない!\nカードのコスト:{Cost} プレイヤーのコスト:{m_player.Cost}");
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