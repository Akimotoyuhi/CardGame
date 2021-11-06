using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class EnemyBase : CharactorBase, IDropHandler
{
    private Player m_player;
    public int m_id;
    private EnemyManager m_enemyManager;
    [SerializeField] EnemyData m_enemyData;
    private EnemyDataBase m_data;
    private EnemyCommand[] m_command;

    void Start()
    {
        SetUp();
    }

    protected override void SetUp()
    {
        m_data = m_enemyData.m_enemyDatas[m_id];
        m_name = m_data.Name;
        m_maxHp = m_data.HP;
        m_command = m_data.SetAction();
        //m_player = GameObject.FindWithTag("Player").GetComponent<Player>();
        m_enemyManager = transform.parent.gameObject.GetComponent<EnemyManager>();
        base.SetUp();
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
        if (m_hp <= 0)
        {
            m_isDead = true;
            m_enemyManager.Removed();
            Destroy(this.gameObject);
        }
        SetText();
    }

    /// <summary>
    /// 敵攻撃力計算
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
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
        while (turn >= m_command.Length)
        {
            turn -= m_command.Length - 1;
        }

        if (!m_player)
        {
            m_player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }
        m_player.GetAcceptDamage(SetAttack(m_command[turn]));
    }
}
