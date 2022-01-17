using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Linq;

public class EnemyBase : CharactorBase, IDrop
{
    private Player m_player;
    private EnemyManager m_enemyManager;
    //private EnemyCommand[] m_command;
    //private List<EnemyCommand> m_enemyCommands = new List<EnemyCommand>();
    private EnemyDataBase m_enemyDataBase = new EnemyDataBase();

    void Start()
    {
        gameObject.name = m_name;
        m_enemyManager = transform.parent.gameObject.GetComponent<EnemyManager>();
        SetUp();
    }

    protected override void SetUp()
    {
        base.SetUp();
    }

    public void SetParam(EnemyDataBase data)
    {
        m_name = data.Name;
        m_maxLife = data.Life;
        m_image = data.Image;
        m_enemyDataBase = data;
    }

    public void GetDrop(BlankCard card)
    {
        if (card == null || card.GetCardType != UseType.ToEnemy) return;
        AddEffect(card.Conditions);
        int damage = ConditionEffect(EventTiming.Attacked, ParametorType.Attack, card.Power);
        damage = m_block -= damage;
        if (m_block < 0) { m_block = 0; }
        damage *= -1;
        if (damage < 0) { }
        else
        {
            //StartCoroutine(ContinuousReaction(GetCardType.Damage, damage, card.AttackNum));
            m_life -= damage;

            if (m_life <= 0)
            {
                m_isDead = true;
                m_enemyManager.Removed();
                Destroy(this.gameObject);
            }
        }
        SetUI();
        card.OnCast();
    }

    //private int SetAttack(int power)
    //{
    //    //EnemyCommand ret = new EnemyCommand();
    //    int ret = ConditionEffect(EventTiming.Attacked, ParametorType.Attack, power);
    //    return ret;
    //}

    /// <summary>
    /// ActionDataに基づいた敵の行動
    /// </summary>
    /// <param name="turn">現在ターン数</param>
    public void Action(int turn)
    {
        if (!m_player) m_player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (m_enemyDataBase.CommandSelect(this, turn) == null) return;
        Debug.Log($"{m_enemyDataBase.CommandSelect(this, turn).Power}ダメージ");
        m_player.GetAcceptDamage(m_enemyDataBase.CommandSelect(this, turn));
        AttackAnim(false);
    }
}