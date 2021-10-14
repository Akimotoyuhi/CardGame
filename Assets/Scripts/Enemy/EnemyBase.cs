using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyBase : CharactorBase, IDropHandler
{
    private Player m_player;
    [SerializeField] EnemyActionData m_enemyActionData;

    void Start()
    {
        m_player = GameObject.FindWithTag("Player").GetComponent<Player>();
        SetUp();
    }

    public void OnDrop(PointerEventData eventData)
    {
        BlankCard card = eventData.pointerDrag.GetComponent<BlankCard>();
        if (card == null || card.GetCardType() != CardType.ToEnemy) return;
        m_stateArray = card.GetEffect();
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
            Debug.Log("脱力中");
            nums[(int)BuffDebuff.Damage] = Parsent(nums[(int)BuffDebuff.Damage], 25);
        }
        return nums;
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
