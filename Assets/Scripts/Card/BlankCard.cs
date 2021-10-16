using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// GameManagerで動的に作られたカードのUIをセットしたりする受け皿
/// </summary>
public class BlankCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private int[] m_effect;
    private CardType m_cardType;
    /// <summary>移動前の場所保存用</summary>
    private Vector2 m_defPos;
    /// <summary>捨て札</summary>
    private Transform m_discard;

    void Start()
    {
        m_discard = GameObject.Find("Discard").transform;
    }

    public void SetInfo(Sprite image, string name, int cost, string tooltip, int[] effect, CardType type)
    {
        transform.Find("Icon").GetComponent<Image>().sprite = image;
        transform.Find("Name").GetComponent<Text>().text = name;
        transform.Find("Cost").GetComponent<Text>().text = $"{cost}";
        transform.Find("Tooltip").GetComponent<Text>().text = tooltip;
        m_effect = new int[(int)BuffDebuff.end];
        for (int i = 0; i < effect.Length; i++)
        {
            m_effect[i] = effect[i];
        }
        m_cardType = type;
    }

    public CardType GetCardType()
    {
        return m_cardType;
    }

    public int[] GetEffect()
    {
        //バグについて
        //使用せずにターンを終了(捨て札に移動)した場合m_effectに入っている値は消えないが
        //使用した状態でターンを終了させるとm_effectが消える
        OnCast();
        Debug.Log($"GetEffect.攻撃:{m_effect[(int)BuffDebuff.Damage]}");
        Debug.Log($"GetEffect.脱力:{m_effect[(int)BuffDebuff.Weakness]}");
        return m_effect;
    }

    public void OnCast()
    {
        //transform.parent = m_discard.transform;
        transform.SetParent(m_discard, false);
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
