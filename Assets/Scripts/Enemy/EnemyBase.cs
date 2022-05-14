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
        m_maxLife = GameManager.Instance.CustomEvaluation(CustomEntityType.AllEnemies, CustomParamType.Life, data.Life);
        m_life = m_maxLife;
        m_sprite = data.Image;
        m_enemyDataBase = data;
        m_enemyManager = enemyManager;
        transform.localScale = transform.localScale * data.ScaleMagnification;
    }

    public bool CanDrop(UseTiming useType)
    {
        if (useType == UseTiming.ToEnemy) return true;
        else return false;
    }

    public void GetDrop(List<int[]> cardCommand)
    {
        BattleManager.Instance.CommandManager.CommandExecute(cardCommand, true, this);
        Effect();
        ActionPlan();
        //m_enemyManager.EnemyDamaged();
    }

    public void OnCard(UseTiming? useType)
    {
        if (m_isDead) return;
        if (useType == UseTiming.ToEnemy) m_flame.SetActive(true);
        else m_flame.SetActive(false);
    }

    public EnemyBase IsEnemy()
    {
        return this;
    }

    public override void GetDamage(int[] cardParam, ParticleID particleID)
    {
        CommandParam command = (CommandParam)cardParam[0];
        bool b;
        switch (command)
        {
            case CommandParam.Attack:
                b = cardParam[4] == 1 ? true : false;
                Damage(cardParam[3], 0, null, false, particleID, b, () => Dead());
                break;
            case CommandParam.Block:
                Damage(0, cardParam[3], null, false, particleID, false, () => Dead());
                break;
            case CommandParam.Conditon:
                ConditionSelection cs = new ConditionSelection();
                Damage(0, 0, cs.SetCondition((ConditionID)cardParam[3], cardParam[4]), false, particleID, false, () => Dead());
                break;
            default:
                Debug.LogError("例外");
                break;
        }
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
        BattleManager.Instance.CommandManager.CommandExecute(m_commands, false, this);
    }

    /// <summary>
    /// 自身のバフデバフを評価して行動データの値を増減させる
    /// </summary>
    public void Effect()
    {
        if (m_actionCommnad == null)
            return;
        m_commands = m_actionCommnad.Command;
        foreach (var c in m_commands)
        {
            CommandParam cp = (CommandParam)c[0];
            switch (cp)//自身のバフを評価して数値を増減させる
            {
                case CommandParam.Attack:
                    c[3] = GameManager.Instance.CustomEvaluation(CustomEntityType.AllEnemies, CustomParamType.Power, c[3]);
                    c[3] = ConditionEffect(EventTiming.Attacked, ParametorType.Attack, c[3]);
                    break;
                case CommandParam.Block:
                    c[3] = GameManager.Instance.CustomEvaluation(CustomEntityType.AllEnemies, CustomParamType.Difence, c[3]);
                    c[3] = ConditionEffect(EventTiming.Attacked, ParametorType.Block, c[3]);
                    break;
                default:
                    continue;
            }
            if (c[3] <= 1) c[3] = 1;
        }
    }

    /// <summary>
    /// このターン行う行動を決める
    /// </summary>
    public void SelectActionCommand(int turn)
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
        for (int i = 0; i < m_actionCommnad.ActionPlans.Count; i++)
        {
            int index = default;
            for (int n = 0; n < m_commands.Count; n++)
            {
                if ((CommandParam)m_commands[n][0] == CommandParam.Attack)
                {
                    index = i;
                    break;
                }
            }
            PlanController p = Instantiate(m_planImage);
            p.transform.SetParent(m_planImageParent, false);
            p.SetImage(m_actionCommnad.ActionPlans[i].ActionPlan, m_commands[index][3]);
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