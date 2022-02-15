using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBase : CharactorBase, IDrop
{
    [SerializeField] GameObject m_planImage;
    [SerializeField] Transform m_planImageParent;
    private Player m_player;
    private EnemyManager m_enemyManager;
    private EnemyDataBase m_enemyDataBase = new EnemyDataBase();

    void Start()
    {
        SetUp();
    }

    protected override void SetUp()
    {
        gameObject.name = m_name;
        base.SetUp();
        m_isEnemy = true;
    }

    public void SetParam(EnemyDataBase data, EnemyManager enemyManager)
    {
        m_name = data.Name;
        m_maxLife = data.Life;
        m_sprite = data.Image;
        m_enemyDataBase = data;
        m_enemyManager = enemyManager;
    }

    public void GetDrop(int power, int block, List<Condition> conditions, UseType useType, System.Action onCast)
    {
        if (useType != UseType.ToEnemy) return;
        onCast();
        //card.OnCast();
        Damage(power, block, conditions);
    }

    public void GetDamage(BlankCard card)
    {
        Damage(card.Power, card.Block, card.Conditions);
    }

    public override void Damage(int damage, int block, List<Condition> conditions)
    {
        AddEffect(conditions);
        int dmg = ConditionEffect(EventTiming.Damaged, ParametorType.Attack, damage);
        dmg = m_block -= dmg;
        if (m_block < 0) { m_block = 0; }
        dmg *= -1;
        if (dmg < 0) { }
        else
        {
            m_life -= dmg;
            //EffectManager.Instance.DamageText(dmg.ToString(), Color.red, Vector2.zero, transform, true);
            if (m_life <= 0)
            {
                m_life = 0;
                m_isDead = true;
                Dead();
            }
            else
            {
                EffectManager.Instance.DamageText(dmg.ToString(), Color.red, Vector2.zero, transform, true);
            }
        }
        SetUI();
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
            if (m_block <= 0)
            {
                m_block = 0;
            }
            else
            {
                EffectManager.Instance.DamageText(damage.ToString(), Color.blue, Vector2.zero, transform, true);
            }
            damage *= -1;
            if (damage <= 0) { }
            else
            {
                m_life -= damage;
                //EffectManager.Instance.DamageText(damage.ToString(), Color.red, Vector2.zero, transform, true);
                if (m_life <= 0)
                {
                    m_life = 0;
                    m_isDead = true;
                    Dead();
                }
            }
            AddEffect(command.Conditions);
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
            g.transform.SetParent(m_planImageParent, false);
            g.transform.localScale = new Vector2(-1, 1);
            g.GetComponent<PlanController>().SetImage(m_enemyDataBase.CommandSelect(this, turn).Plan[i],
                ConditionEffect(EventTiming.Attacked, ParametorType.Attack, m_enemyDataBase.CommandSelect(this, turn).Power));
        }
    }

    public void Dead()
    {
        m_image.DOColor(Color.clear, 0.5f)
            .OnComplete(() =>
            {
                m_enemyManager.Removed();
                m_image.enabled = false;
                for (int i = 0; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
            });
    }
}