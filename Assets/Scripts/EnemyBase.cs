using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour, IDropHandler
{
    [SerializeField] private string m_name = "name";
    [SerializeField] private int m_maxHp = 1;
    private int m_hp;
    [SerializeField] private Slider m_slider;
    private AttackCard m_atkCard;

    void Start()
    {
        m_hp = m_maxHp;
        m_slider.maxValue = m_maxHp;
        m_slider.value = m_hp;
    }

    public void OnDrop(PointerEventData eventData)
    {
        m_atkCard = eventData.pointerDrag.GetComponent<AttackCard>();
        m_hp -= m_atkCard.GetDamage();
        m_slider.value = m_hp;
        Debug.Log($"{m_name}は{m_atkCard.GetDamage()}ダメージ受けた");
        if (m_hp < 0)
        {
            Destroy(this.gameObject);
            Debug.Log("オリジム死！W");
        }
    }
}
