using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBase : CharactorBase, IDrop
{
    [SerializeField] PlanController m_planImage;
    [SerializeField] Transform m_planImageParent;
    private Player m_player;
    private EnemyManager m_enemyManager;
    private EnemyDataBase m_enemyDataBase;
    private EnemyActionCommnad3 m_actionCommnad;
    private List<int[]> m_commands = new List<int[]>();

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
        //transform.localScale = transform.localScale * data.ScaleMagnification;
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

    public void OnCard(UseType? useType)
    {
        if (m_isDead) return;
        if (useType == UseType.ToEnemy) m_flame.SetActive(true);
        else m_flame.SetActive(false);
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
        if (m_enemyDataBase.CommandSelect(this, turn) == null)//行動データが無かったら何もしない
        {
            Debug.Log("何もしない");
            return;
        }
        //List<EnemyActionCommnad3> commands = m_enemyDataBase.CommandSelect(this, turn);
        //foreach (var com in commands)
        //{
        //    int power = ConditionEffect(EventTiming.Attacked, ParametorType.Attack, com.Power);
        //    int block = ConditionEffect(EventTiming.Attacked, ParametorType.Block, com.Block);
        //    List<Condition> conditions = com.Conditions;
        //    if (com.Target == TargetType.ToEnemy)
        //    {
        //        Damage(com.Power, com.Block, null, false, () => Dead());
        //        foreach (var condition in com.Conditions)
        //        {
        //            Damage(0, 0, condition, false, () => Dead());
        //        }
        //    }
        //    else
        //    {
        //        m_player.GetAcceptDamage(power, block, conditions);
        //    }
        //}
    }

    /// <summary>
    /// 自身のバフデバフを評価して行動データの値を増減させる
    /// </summary>
    private void Effect()
    {
        m_commands = m_actionCommnad.Command;
        foreach (var c in m_commands)
        {
            CommandParam cp = (CommandParam)c[0];
            switch (cp)//自身のバフを評価して数値を増減させる
            {
                case CommandParam.Attack:
                    c[2] = ConditionEffect(EventTiming.Attacked, ParametorType.Attack, c[2]);
                    break;
                case CommandParam.Block:
                    c[2] = ConditionEffect(EventTiming.Attacked, ParametorType.Block, c[2]);
                    break;
                default:
                    continue;
            }
            if (c[2] <= 1) c[2] = 1;
        }
    }

    /// <summary>
    /// このターン行う行動を決める
    /// </summary>
    public void ActionCommand(int turn)
    {
        m_actionCommnad = m_enemyDataBase.CommandSelect(this, turn);
    }

    /// <summary>
    /// 行動予定の表示
    /// </summary>
    public void ActionPlan()
    {
        for (int i = 0; i < m_planImageParent.childCount; i++)
        {
            Destroy(m_planImageParent.GetChild(i).gameObject);
        }
        if (m_actionCommnad == null) return;
        for (int i = 0; i < m_actionCommnad.ActionPlan.Count; i++)
        {
            if (m_actionCommnad.ActionPlan[i].NumIndex < 0) continue;
            //m_commands[m_actionCommnad.ActionPlan[i].NumIndex]
        }
        //if (m_enemyDataBase.CommandSelect(this, turn) == null) return;
        //for (int i = 0; i < m_enemyDataBase.CommandSelect(this, turn).Count; i++)
        //{
        //    for (int n = 0; n < m_enemyDataBase.CommandSelect(this, turn)[i].ActionPlan.Count; n++)
        //    {
        //        PlanController p = Instantiate(m_planImage);
        //        p.transform.SetParent(m_planImageParent, false);
        //        p.SetImage(m_enemyDataBase.CommandSelect(this, turn)[i].ActionPlan[n].ActionPlan,
        //            ConditionEffect(EventTiming.Attacked, ParametorType.Attack, m_enemyDataBase.CommandSelect(this, turn)[i].Power));
        //    }
        //}
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