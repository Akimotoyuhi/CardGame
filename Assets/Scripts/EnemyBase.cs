using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class EnemyBase : MonoBehaviour, IDropHandler
{
    [SerializeField] private string m_name = "name";
    [SerializeField] private int m_maxHp = 1;
    private int m_hp;
    private Slider m_slider;
    private IAttackCard m_atkCard;
    protected Player m_player;
    private int[] m_stateArray = new int[(int)BuffDebuff.end];

    void Start()
    {
        m_player = GameObject.FindWithTag("Player").GetComponent<Player>();
        m_slider = transform.GetChild(0).GetComponent<Slider>();
        m_hp = m_maxHp;
        m_slider.maxValue = m_maxHp;
        m_slider.value = m_hp;
    }

    public void OnDrop(PointerEventData eventData)
    {
        m_atkCard = eventData.pointerDrag.GetComponent<IAttackCard>();
        if (m_atkCard == null) { return; }
        m_stateArray = m_atkCard.GetDamage();
        int damage = m_stateArray[(int)BuffDebuff.Damage];
        m_hp -= damage;
        m_slider.value = m_hp;
        if (m_hp < 0)
        {
            Destroy(this.gameObject);
        }
        m_stateArray[(int)BuffDebuff.Damage] = 0;
    }

    /// <summary>
    /// デバフの効果込みの攻撃力を返す
    /// </summary>
    /// <param name="atk">元の攻撃力</param>
    /// <returns>最終的な攻撃力</returns>
    protected int SetAttack(int atk)
    {
        if (m_stateArray[(int)BuffDebuff.Weakness] > 0)
        {
            atk = Parsent(atk, 0.25f);
        }
        return atk;
    }

    protected int Parsent(int num, float parsent)
    {
        float total = num * (1 - parsent);
        return (int)total;
    }

    public abstract void Action();
}
