using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 全てのカードのベースとなるクラス
/// </summary>
public class CardBase : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] protected CardDataBase m_cardData;
    [SerializeField] private Text m_cost;
    [SerializeField] private Text m_name;
    [SerializeField] protected Text m_tooltip;
    /// <summary>移動前の場所保存用</summary>
    private Vector2 m_defPos;
    /// <summary>捨て札</summary>
    protected GameObject m_discard;
    protected Player m_player;

    public void SetUp()
    {
        m_cost.text = $"{m_cardData.m_cardData.Cost}";
        m_name.text = m_cardData.m_cardData.Name;
        m_discard = GameObject.Find("Discard");
        m_player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    protected int Parsent(int num, float parsent)
    {
        float t = num * (1 - parsent);
        return (int)t;
    }

    /// <summary>カード使用後</summary>
    public void OnCast()
    {
        transform.parent = m_discard.transform;
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
