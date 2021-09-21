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
    [SerializeField] private Slider m_hpSlider;
    [SerializeField] private Slider m_blkSlider;
    [SerializeField] private Text m_text;
    private IBuffCard m_buffCard;
    private int[] m_stateArray = new int[(int)BuffDebuff.end];

    void Start()
    {
        m_hp = m_maxHp;
        m_hpSlider.maxValue = m_maxHp;
        m_hpSlider.value = m_hp;
        m_blkSlider.value = m_block;
        SetText();
    }

    public void Damage(int damage)
    {
        damage = m_block -= damage;
        if (m_block < 0) { m_block = 0; }
        m_blkSlider.value = m_block;
        if (damage > 0)
        {
            SetText();
            return;
        }
        m_hp += damage;
        m_hpSlider.value = m_hp;
        SetText();
    }

    private void EndTurn()
    {

    }

    private void SetText()
    {
        if (m_block > 0) { m_text.text = $"{m_block}"; }
        else { m_text.text = $"{m_hp} : {m_maxHp}"; }
    }

    public void OnDrop(PointerEventData pointerEvent)
    {
        m_buffCard = pointerEvent.pointerDrag.GetComponent<IBuffCard>();
        if (m_buffCard == null) { return; }
        m_block += m_buffCard.GetBlock();
        m_blkSlider.value = m_block;
        SetText();
    }
}
