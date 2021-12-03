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
    private EnemyCommand[] m_command;

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

    public void SetParam(string name, Sprite image, int hp, EnemyCommand[] command)
    {
        m_name = name;
        m_image = image;
        m_maxHp = hp;
        m_command = command;
    }

    public void GetDrop(BlankCard card)
    {
        if (card == null || card.GetCardType != UseType.ToEnemy) return;
        foreach (var item in card.Conditions)
        {
            m_conditions.Add(item);
        }
        int damage = card.Power;
        damage = m_block -= damage;
        if (m_block < 0) { m_block = 0; }
        m_blkSlider.value = m_block;
        if (damage > 0) { }
        else
        {
            //StartCoroutine(ContinuousReaction(GetCardType.Damage, damage, card.AttackNum));
            if (m_hp <= 0)
            {
                m_isDead = true;
                m_enemyManager.Removed();
                Destroy(this.gameObject);
            }
        }
        SetUI();
    }
    
    /// <summary>
    /// 敵攻撃力計算
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    private EnemyCommand SetAttack(EnemyCommand command)
    {
        EnemyCommand ret = new EnemyCommand();
        ret.m_attack = ConditionEffect(EventTiming.Attacked, command.m_attack);
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
        //Debug.Log($"damage:{SetAttack(m_command[turn]).m_attack}, turn:{turn}");
        m_player.GetAcceptDamage(SetAttack(m_command[turn]));
    }
}
