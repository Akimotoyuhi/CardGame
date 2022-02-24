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
        Damage(power, block, conditions, false, () =>
        {
            //DOTween.KillAll();
            m_isDead = true;
            Dead();
        });
    }

    /// <summary>
    /// プレイヤー以外からのダメージ受け付け用
    /// </summary>
    /// <param name="damagem"></param>
    /// <param name="block"></param>
    /// <param name="conditions"></param>
    public void GetDamage(int damage, int block, List<Condition> conditions)
    {
        Damage(damage, block, conditions, false, () =>
        {
            //DOTween.KillAll();
            m_isDead = true;
            Dead();
        });
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
        List<EnemyActionCommnad3> commands = m_enemyDataBase.CommandSelect(this, turn);
        foreach (var item in commands)
        {
            int power = ConditionEffect(EventTiming.Attacked, ParametorType.Attack, item.Power);
            int block = ConditionEffect(EventTiming.Attacked, ParametorType.Block, item.Block);
            List<Condition> conditions = item.Conditions;
            if (item.Target == TargetType.ToEnemy)
            {
                Damage(item.Power, item.Block, item.Conditions, false, () =>
                {
                    //DOTween.KillAll();
                    m_isDead = true;
                    Dead();
                });
            }
            else
            {
                m_player.GetAcceptDamage(power, block, conditions);
            }
        }
        //AttackAnim(false);
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
        for (int i = 0; i < m_enemyDataBase.CommandSelect(this, turn).Count; i++)
        {
            for (int n = 0; n < m_enemyDataBase.CommandSelect(this, turn)[i].Plan.Count; n++)
            {
                GameObject g = Instantiate(m_planImage);
                g.transform.SetParent(m_planImageParent, false);
                g.transform.localScale = new Vector2(-1, 1);
                g.GetComponent<PlanController>().SetImage(m_enemyDataBase.CommandSelect(this, turn)[i].Plan[n],
                    ConditionEffect(EventTiming.Attacked, ParametorType.Attack, m_enemyDataBase.CommandSelect(this, turn)[i].Power));
            }
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