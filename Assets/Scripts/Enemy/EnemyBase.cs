using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Linq;

public class EnemyBase : CharactorBase, IDrop
{
    [SerializeField] GameObject m_planImage;
    [SerializeField] Transform m_planImageParent;
    private Player m_player;
    private EnemyManager m_enemyManager;
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
            m_life -= damage;
            if (m_life <= 0)
            {
                m_isDead = true;
                m_enemyManager.Removed();
                Destroy(gameObject);
            }
            else
            {
                EffectManager.Instance.DamageText(damage.ToString(), Color.red, Vector2.zero, transform, true);
            }
        }
        SetUI();
        card.OnCast();
    }

    /// <summary>
    /// ActionDataに基づいた敵の行動
    /// </summary>
    /// <param name="turn">現在ターン数</param>
    public void Action(int turn)
    {
        if (!m_player) m_player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (m_enemyDataBase.CommandSelect(this, turn) == null)
        {
            Debug.Log("何もしない");
            return;
        }
        EnemyActionCommnad3 command = m_enemyDataBase.CommandSelect(this, turn);
        if (command.Target == TargetType.ToEnemy)
        {
            EffectChecker(EventTiming.Damaged, ParametorType.Any);
            m_block += command.Block;
            int damage = m_block - command.Power;
            if (m_block < 0)
            {
                m_block = 0;
            }
            else
            {
                EffectManager.Instance.DamageText(damage.ToString(), Color.blue, Vector2.zero, transform, true);
            }
            damage *= -1; //ブロック値計算の後ダメージの符号が反転するので戻す
            if (damage < 0) { }
            else
            {
                m_life -= damage;
                if (m_life <= 0)
                {
                    Debug.Log("がめおべｒ");
                }
                else
                {
                    EffectManager.Instance.DamageText(damage.ToString(), Color.red, Vector2.zero, transform, true);
                }
                AddEffect(command.Conditions);
            }
            SetUI();
        }
        else
        {
            int atk = ConditionEffect(EventTiming.Attacked, ParametorType.Attack, command.Power);
            int blk = command.Block;
            List<Condition> conditions = command.Conditions;
            m_player.GetAcceptDamage(atk, blk, conditions);
        }
        AttackAnim(false);
    }

    /// <summary>
    /// 行動予定の表示
    /// </summary>
    /// <param name="turn"></param>
    public void ActionPlan(int turn)
    {
        for (int i = 0; i < m_planImageParent.childCount; i++)
        {
            Destroy(m_planImageParent.GetChild(i).gameObject);
        }
        if (m_enemyDataBase.CommandSelect(this, turn) == null) return;
        for (int i = 0; i < m_enemyDataBase.CommandSelect(this, turn).Plan.Count; i++)
        {
            GameObject g = Instantiate(m_planImage);
            g.transform.SetParent(m_planImageParent);
            g.GetComponent<PlanController>().SetImage(m_enemyDataBase.CommandSelect(this, turn).Plan[i],
                ConditionEffect(EventTiming.Attacked, ParametorType.Attack, m_enemyDataBase.CommandSelect(this, turn).Power));
        }
    }
}