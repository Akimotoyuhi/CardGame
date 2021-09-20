using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IDropHandler
{
    [SerializeField] private string m_name = "name";
    [SerializeField] private int m_maxHp = 1;
    private int m_hp;
    private int m_block = 0;
    private Slider m_slider;
    private IBuffCard m_buffCard;

    void Start()
    {
        m_slider = this.transform.GetChild(0).GetComponent<Slider>();
        m_hp = m_maxHp;
        m_slider.maxValue = m_maxHp;
        m_slider.value = m_hp;
    }

    private void EndTurn()
    {

    }

    public void OnDrop(PointerEventData pointerEvent)
    {
        m_buffCard = pointerEvent.pointerDrag.GetComponent<IBuffCard>();
        if (m_buffCard == null) { return; }
        m_block += m_buffCard.GetBlock();
        Debug.Log($"Playerの現在ブロック数{m_block}");
    }
}
