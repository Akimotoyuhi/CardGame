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
        m_stateArray = card.GetEffect().conditions;
        SetCondisionTurn(m_stateArray);
        int damage = card.GetEffect().attack;
        m_hp -= damage;
        m_hpSlider.value = m_hp;
        if (m_hp < 0)
        {
            Destroy(this.gameObject);
        }
        SetText();
    }

    private EnemyCommand SetAttack(EnemyCommand command)
    {
        EnemyCommand ret = new EnemyCommand();
        ret.m_attack = m_condition.AtAttack(command.m_attack);
        return ret;
    }

    /// <summary>
    /// ActionDataに基づいた敵の行動
    /// </summary>
    /// <param name="turn">現在ターン数</param>
    public void Action(int turn)
    {
        while (true)
        {
            if (turn < m_enemyActionData.m_enemyDatas.Count)
            {
                m_player.GetAcceptDamage(SetAttack(m_enemyActionData.m_enemyDatas[turn].Action()));
                return;
            }
            else
            {
                turn -= m_enemyActionData.m_enemyDatas.Count - 1;
            }
        }
    }
}
