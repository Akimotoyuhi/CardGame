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
        foreach (int e in m_stateArray)
        {
            Debug.Log(e);
        }
        SetCondisionTurn(m_stateArray);
        int damage = m_stateArray[(int)BuffDebuff.Damage];
        m_hp -= damage;
        m_hpSlider.value = m_hp;
        if (m_hp < 0)
        {
            Destroy(this.gameObject);
        }
        SetText();
    }

    private int[] SetAttack(int[] state)
    {
        int[] nums = state;
        nums[(int)BuffDebuff.Damage] = m_condition.AtAttack(nums[(int)BuffDebuff.Damage]);
        return nums;
    }

    public void Action(int turn)
    {
        while (true)
        {
            if (turn < m_enemyActionData.m_enemyDatas.Length)
            {
                m_player.GetAcceptDamage(SetAttack(m_enemyActionData.m_enemyDatas[turn].Action()));
                return;
            }
            else
            {
                turn -= m_enemyActionData.m_enemyDatas.Length - 1;
            }
        }
    }
}
