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
    private int m_block;
    [SerializeField] private Slider m_hpSlider;
    [SerializeField] private Slider m_blkSlider;
    [SerializeField] private Text m_text;
    private Player m_player;
    private int[] m_stateArray;
    [SerializeField] private EnemyActionData m_enemyActionData;

    void Start()
    {
        m_player = GameObject.FindWithTag("Player").GetComponent<Player>();
        m_stateArray = new int[(int)BuffDebuff.end];
        m_hp = m_maxHp;
        m_hpSlider.maxValue = m_maxHp;
        m_hpSlider.value = m_hp;
        m_blkSlider.value = m_block;
        SetText();
    }

    public void OnDrop(PointerEventData eventData)
    {
        BlankCard atkCard = eventData.pointerDrag.GetComponent<BlankCard>();
        if (atkCard == null || atkCard.GetCardType() != CardType.ToPlayer) return;
        m_stateArray = atkCard.GetEffect();
        int damage = m_stateArray[(int)BuffDebuff.Damage];
        m_hp -= damage;
        m_hpSlider.value = m_hp;
        if (m_hp < 0)
        {
            Destroy(this.gameObject);
        }
        m_stateArray[(int)BuffDebuff.Damage] = 0;
        SetText();
    }

    private int[] SetAttack(int[] state)
    {
        int[] nums = state;
        if (m_stateArray[(int)BuffDebuff.Weakness] > 0)
        {
            nums[(int)BuffDebuff.Damage] = Parsent(nums[(int)BuffDebuff.Damage], 0.25f);
        }
        return nums;
    }

    private int Parsent(int num, float parsent)
    {
        float total = num * (1 - parsent);
        return (int)total;
    }

    private void SetText()
    {
        if (m_block > 0)
        {
            m_blkSlider.value = m_block;
            m_text.text = $"{m_block}";
        }
        else { m_text.text = $"{m_hp} : {m_maxHp}"; }
    }

    public void Action(int turn)
    {
        int num = turn;
        while (true)
        {
            if (num < m_enemyActionData.m_enemyDatas.Length)
            {

                m_player.GetAcceptDamage(SetAttack(m_enemyActionData.m_enemyDatas[num].Action()));
                m_stateArray[(int)BuffDebuff.Damage] = 0;
                m_stateArray[(int)BuffDebuff.Block] = 0;
                return;
            }
            else
            {
                num -= m_enemyActionData.m_enemyDatas.Length - 1;
            }
        }
    }
}
