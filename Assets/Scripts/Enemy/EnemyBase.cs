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
        transform.localScale = transform.localScale * data.ScaleMagnification;
    }

    public bool CanDrop(UseType useType)
    {
        if (useType == UseType.ToEnemy) return true;
        else return false;
    }

    public void GetDrop(List<int[]> cardCommand)
    {
        BattleManager.Instance.DropManager.CardExecute(cardCommand, this);
        m_enemyManager.EnemyDamaged();
    }

    public override void GetDamage(int[] cardParam)
    {
        CommandParam command = (CommandParam)cardParam[0];
        switch (command)
        {
            case CommandParam.Attack:
                Damage(cardParam[2], 0, null, false, () => Dead());
                break;
            case CommandParam.Block:
                Damage(0, cardParam[2], null, false, () => Dead());
                break;
            case CommandParam.Conditon:
                ConditionSelection cs = new ConditionSelection();
                Damage(0, 0, cs.SetCondition((ConditionID)cardParam[2], cardParam[3]), false, () => Dead());
                break;
            default:
                Debug.LogError("例外");
                break;
        }
    }

    /// <summary>
    /// プレイヤー以外からのダメージ受け付け用
    /// </summary>
    /// <param name="damagem"></param>
    /// <param name="block"></param>
    /// <param name="condition"></param>
    public void GetDamage(int damage, int block, Condition condition)
    {
        Damage(damage, block, condition, false, () => Dead());
        m_enemyManager.EnemyDamaged();
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
        foreach (var com in commands)
        {
            int power = ConditionEffect(EventTiming.Attacked, ParametorType.Attack, com.Power);
            int block = ConditionEffect(EventTiming.Attacked, ParametorType.Block, com.Block);
            List<Condition> conditions = com.Conditions;
            if (com.Target == TargetType.ToEnemy)
            {
                Damage(com.Power, com.Block, null, false, () => Dead());
                foreach (var condition in com.Conditions)
                {
                    Damage(0, 0, condition, false, () => Dead());
                }
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

    /// <summary>
    /// やられた時
    /// </summary>
    public void Dead()
    {
        m_isDead = true;
        m_image.DOColor(Color.clear, 0.5f)
            .OnComplete(() =>
            {
                m_enemyManager.Removed();
                m_image.enabled = false;
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            });
    }
}