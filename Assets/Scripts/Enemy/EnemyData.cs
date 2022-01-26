using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    [SerializeField] int m_useData = 3;
    [SerializeField] List<EnemyDataBase> m_enemyDataBases = new List<EnemyDataBase>();
    private List<EnemyDataBase> m_act1Enemies = new List<EnemyDataBase>();
    private List<EnemyDataBase> m_act1Elites = new List<EnemyDataBase>();
    private List<EnemyDataBase> m_act1Boss = new List<EnemyDataBase>();
    public EnemyDataBase EnemyDataBase(int index) => m_enemyDataBases[index];
    public List<EnemyDataBase> Act1Enemy => m_act1Enemies;
    public List<EnemyDataBase> Act1Elite => m_act1Elites;
    public List<EnemyDataBase> Act1Boss => m_act1Boss;
    public void Assignment()
    {
        m_act1Enemies.Clear();
        m_act1Elites.Clear();
        m_act1Boss.Clear();
        for (int i = 0; i < m_enemyDataBases.Count; i++)
        {
            EnemyDataBase data = m_enemyDataBases[i];
            switch (data.EnemyAppearanceEria)
            {
                case EnemyAppearanceEria.Act1Enemy:
                    m_act1Enemies.Add(data);
                    break;
                case EnemyAppearanceEria.Act1Elite:
                    m_act1Elites.Add(data);
                    break;
                case EnemyAppearanceEria.Act1Boss:
                    m_act1Boss.Add(data);
                    break;
                default:
                    Debug.LogWarning("与えられたパラメーターに対する値がありません");
                    break;
            }
        }
    }
}

/// <summary>
/// 敵ID
/// </summary>
public enum EnemyID
{
    /// <summary>オリジムシ</summary>
    origimusi,
    /// <summary>レユニオン兵</summary>
    Soldier,
    /// <summary>見棄てられた者</summary>
    ForsakenOne,
    endLength,
}
/// <summary>
/// 敵出現場所
/// </summary>
public enum EnemyAppearanceEria
{
    Act1Enemy,
    Act1Elite,
    Act1Boss,
    Act2Enemy,
    Act2Elite,
    Act2Boss,
    Act3Enemy,
    Act3Elite,
    Act3Boss,
}
public enum ActionPlan
{
    Attack,
    Block,
    Buff,
    Debuff,
    Unknown,
}
[System.Serializable]
public class EnemyDataBase
{
    [SerializeField, Tooltip("名前")] string m_name;
    [SerializeField, Tooltip("最大体力")] int m_life;
    [SerializeField, Tooltip("画像")] Sprite m_image;
    [SerializeField, Tooltip("出現場所")] EnemyAppearanceEria m_enemyAppearanceEria;
    public string Name => m_name;
    public int Life => m_life;
    public Sprite Image => m_image;
    public EnemyAppearanceEria EnemyAppearanceEria => m_enemyAppearanceEria;
    public enum NodeType { Selector, Sequence }
    private NodeType m_NodeType = NodeType.Sequence;
    public List<EnemyBaseState> m_enemyBaseState;
    public EnemyActionCommnad3 CommandSelect(EnemyBase enemy, int turn)
    {
        switch (m_NodeType)
        {
            case NodeType.Sequence: //一つでも失敗したらそこで終了
                for (int i = 0; i < m_enemyBaseState.Count; i++)
                {
                    bool flag = false;
                    //if (m_enemyBaseState[i].m_conditionalCommand.Count <= 0)
                    //{
                    //    return m_enemyBaseState[i].m_actionCommnad; //こいつが悪い
                    //}
                    for (int n = 0; n < m_enemyBaseState[i].m_conditionalCommand.Count; n++)
                    {
                        if (!m_enemyBaseState[i].m_conditionalCommand[n].Conditional(enemy, turn))
                        {
                            flag = false;
                            break;
                        }
                        flag = true;
                    }
                    if (flag) return m_enemyBaseState[i].m_actionCommnad;
                }
                return null;
            default:
                Debug.LogError("無効なケース");
                return null;
        }
    }
}

[System.Serializable]
public class EnemyBaseState
{
    public List<EnemyConditionalCommand3> m_conditionalCommand;
    public EnemyActionCommnad3 m_actionCommnad;
}

[System.Serializable]
public class EnemyActionCommnad3
{
    [Header("行動データ")]
    [SerializeField] int m_power = 0;
    [SerializeField] int m_block = 0;
    [SerializeField] List<ConditionSelection> m_condition;
    [SerializeField] TargetType m_target;
    [SerializeField] List<ActionPlan> m_plan;

    public int Power { get => m_power; set => m_power = value; }
    public int Block { get => m_block; set => m_block = value; }
    public List<Condition> Conditions
    {
        get
        {
            List<Condition> ret = new List<Condition>();
            for (int i = 0; i < m_condition.Count; i++)
            {
                if (m_condition[i].GetCondition != null)
                {
                    ret.Add(m_condition[i].GetCondition);
                }
            }
            return ret;
        }
    }
    public TargetType Target => m_target;
    public List<ActionPlan> Plan => m_plan;
}

[System.Serializable]
public class EnemyConditionalCommand3
{
    [Header("条件データ")]
    [SerializeField, Tooltip("条件")] WhereType m_type;
    [SerializeField, Tooltip("評価する値")] int m_value;
    [SerializeField, Tooltip("確率にするか否か")] bool m_isProbability;
    /// <summary>
    /// 条件式
    /// </summary>
    /// <returns>成功可否</returns>
    public bool Conditional(EnemyBase enemy, int turn)
    {
        if (turn == 0)
        {
            if (m_type == WhereType.BattleBegin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        switch (m_type)
        {
            case WhereType.Turn:
                if (turn == m_value)
                {
                    return true;
                }
                return false;
            case WhereType.RowTurn:
                if (turn <= m_value)
                {
                    return true;
                }
                return false;
            case WhereType.HighTurn:
                if (turn >= m_value)
                {
                    return true;
                }
                return false;
            case WhereType.MultipleTurn:
                if (turn % m_value == 0)
                {
                    return true;
                }
                return false;
            case WhereType.RowLife:
                if (enemy.CurrentLife <= m_value)
                {
                    return true;
                }
                return false;
            case WhereType.HighLife:
                if (enemy.CurrentLife >= m_value)
                {
                    return true;
                }
                return false;
            default:
                Debug.LogError("無効な条件ケース");
                return false;
        }
    }
}

public enum TargetType
{
    ToPlayer,
    ToEnemy,
}

public enum WhereType
{
    Turn,
    RowTurn,
    HighTurn,
    MultipleTurn,
    RowLife,
    HighLife,
    BattleBegin,
}