using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField, Header("敵ステータスのデータ")]
    List<EnemyDataBase> m_enemyDataBases = new List<EnemyDataBase>();
    [Header("エンカウントデータ")]
    [SerializeField] List<EncountDataBase> m_act1EnemyEncountData = new List<EncountDataBase>();
    [SerializeField] List<EncountDataBase> m_act1EliteEncountData = new List<EncountDataBase>();
    [SerializeField] List<EncountDataBase> m_act1BossEncountData = new List<EncountDataBase>();
    public List<EnemyDataBase> Encount(EnemyAppearanceEria eria)
    {
        List<EnemyDataBase> ret = new List<EnemyDataBase>();
        List<EncountDataBase> encountData;
        switch (eria)
        {
            case EnemyAppearanceEria.Act1Enemy:
                encountData = m_act1EnemyEncountData;
                break;
            case EnemyAppearanceEria.Act1Elite:
                encountData = m_act1EliteEncountData;
                break;
            case EnemyAppearanceEria.Act1Boss:
                encountData = m_act1BossEncountData;
                break;
            default:
                Debug.Log("変な値入れんな");
                return null;
        }
        int r = UnityEngine.Random.Range(0, encountData.Count);
        for (int i = 0; i < encountData[r].GetLength; i++)
        {
            ret.Add(m_enemyDataBases[encountData[r].GetID(i)]);
        }
        return ret;
    }
}
#region Enums
/// <summary>敵ID</summary>
public enum EnemyID
{
    /// <summary>オリジムシ</summary>
    origimusi,
    /// <summary>レユニオン兵</summary>
    Soldier,
    /// <summary>「見棄てし者」</summary>
    ForsakenOne,
    /// <summary>火炎瓶暴徒</summary>
    CocktailThrower,
    /// <summary>重装兵</summary>
    HeavyDefender,
}
/// <summary>敵出現場所</summary>
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
/// <summary>行動予定</summary>
public enum ActionPlan
{
    Attack,
    Block,
    Buff,
    Debuff,
    Unknown,
}
/// <summary>攻撃の標的</summary>
public enum TargetType
{
    ToPlayer,
    ToEnemy,
}
/// <summary>敵行動条件</summary>
public enum WhereType
{
    Any,
    Turn,
    RowTurn,
    HighTurn,
    MultipleTurn,
    RowLife,
    HighLife,
    BattleBegin,
}
#endregion
#region 敵データ設定関係
[Serializable]
public class EnemyDataBase
{
    [SerializeField, Tooltip("名前")] string m_name;
    [SerializeField, Tooltip("敵ID")] EnemyID m_id;
    [SerializeField, Tooltip("最大体力")] int m_life;
    [SerializeField, Tooltip("画像")] Sprite m_sprite;
    [SerializeField, Tooltip("画像のデカさ倍率")] float m_spriteScaleMagnification = 1;
    [SerializeField, Tooltip("出現場所")] EnemyAppearanceEria m_enemyAppearanceEria;
    public string Name => m_name;
    public EnemyID ID => m_id;
    public int Life => m_life;
    public Sprite Image => m_sprite;
    public float ScaleMagnification => m_spriteScaleMagnification;
    public EnemyAppearanceEria EnemyAppearanceEria => m_enemyAppearanceEria;
    public enum NodeType { Selector, Sequence }
    private NodeType m_NodeType = NodeType.Sequence;
    public List<EnemyBaseState> m_enemyBaseState;
    public List<EnemyActionCommnad3> CommandSelect(EnemyBase enemy, int turn)
    {
        switch (m_NodeType)
        {
            case NodeType.Sequence: //一つでも失敗したらそこで終了
                for (int i = 0; i < m_enemyBaseState.Count; i++)
                {
                    bool flag = false;
                    for (int n = 0; n < m_enemyBaseState[i].m_conditionalCommand.Count; n++)
                    {
                        if (!m_enemyBaseState[i].m_conditionalCommand[n].Conditional(enemy, turn))
                        {
                            flag = false;
                            break;
                        }
                        flag = true;
                    }
                    if (flag)
                    {
                        //m_enemyBaseState[i].m_conditionalCommand
                        return m_enemyBaseState[i].m_actionCommnad;
                    }
                }
                return null;
            default:
                Debug.LogError("無効なケース");
                return null;
        }
    }
}

[Serializable]
public class EnemyBaseState
{
    public List<EnemyConditionalCommand3> m_conditionalCommand;
    public List<EnemyActionCommnad3> m_actionCommnad;
}

[Serializable]
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

[Serializable]
public class EnemyConditionalCommand3
{
    [Header("条件データ")]
    [SerializeField, Tooltip("条件")] WhereType m_type;
    [SerializeField, Tooltip("評価する値")] int m_value;
    [SerializeField, Tooltip("この行動をする確率"), Range(0, 100)] int m_probability;
    [SerializeField, Tooltip("この行動を一度きりにするかどうか")] bool m_isOnlyOnce;
    private bool m_isOnlyFlag;
    public bool SetOnlyFlag
    {
        set
        {
            if (m_isOnlyOnce) //一度きりにするフラグがtrue出なければ書き換えはしない
            {
                m_isOnlyFlag = value;
            }
        }
    }
    /// <summary>
    /// 条件式
    /// </summary>
    /// <returns>成功可否</returns>
    public bool Conditional(EnemyBase enemy, int turn)
    {
        if (m_isOnlyFlag) return false;
        if (turn == 0)
        {
            if (m_type == WhereType.BattleBegin) { return true; }
            else { return false; }
        }

        switch (m_type)
        {
            case WhereType.Any:
                return true;
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
            case WhereType.BattleBegin:
                return false;
            default:
                Debug.LogError("無効な条件ケース");
                return false;
        }
    }
}
#endregion
[Serializable]
public class EncountDataBase
{
    [SerializeField] EnemyID[] m_enemyID = new EnemyID[Enum.GetNames(typeof(EnemyID)).Length];
    /// <summary>
    /// 敵IDの取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int GetID(int index) { return (int)m_enemyID[index]; }
    public int GetLength { get => m_enemyID.Length; }
}