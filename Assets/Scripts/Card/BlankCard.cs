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

    void Start()
    {
        m_discard = GameObject.Find("Discard").transform;
    }

    public Player Player { set => m_player = value; }

    public void SetInfo(string name, Sprite image, string tooltip, int power, int attackNum, int block, int blockNum, string cost, List<Condition> conditions, UseType useType, Player player)
    {
        transform.Find("Name").GetComponent<Text>().text = name;
        transform.Find("Icon").GetComponent<Image>().sprite = image;
        m_tooltip = tooltip;
        Power = power;
        AttackNum = attackNum;
        Block = block;
        BlockNum = blockNum;
        m_cost = cost;
        Conditions = conditions;
        m_useType = useType;
        m_player = player;
    }

    public UseType GetCardType { get => m_useType; }

    public BlankCard OnCast()
    {

        CostCal();
        transform.SetParent(m_discard, false);
        return this;
    }

    /// <summary>
    /// プレイヤーのコストを減らす
    /// </summary>
    /// <returns></returns>
    private void CostCal()
    {
        m_player.Cost -= Cost;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (m_player.Cost < Cost) return; //コスト足りなかったら使えない
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