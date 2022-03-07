using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField, Header("�G�X�e�[�^�X�̃f�[�^")]
    List<EnemyDataBase> m_enemyDataBases = new List<EnemyDataBase>();
    [Header("�G���J�E���g�f�[�^")]
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
                Debug.Log("�ςȒl������");
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
/// <summary>�GID</summary>
public enum EnemyID
{
    /// <summary>�I���W���V</summary>
    origimusi,
    /// <summary>�����j�I����</summary>
    Soldier,
    /// <summary>�u�����Ă��ҁv</summary>
    ForsakenOne,
    /// <summary>�Ή��r�\�k</summary>
    CocktailThrower,
    /// <summary>�d����</summary>
    HeavyDefender,
}
/// <summary>�G�o���ꏊ</summary>
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
/// <summary>�s���\��</summary>
public enum ActionPlan
{
    Attack,
    Block,
    Buff,
    Debuff,
    Unknown,
}
/// <summary>�U���̕W�I</summary>
public enum TargetType
{
    ToPlayer,
    ToEnemy,
}
/// <summary>�G�s������</summary>
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
#region �G�f�[�^�ݒ�֌W
[Serializable]
public class EnemyDataBase
{
    [SerializeField, Tooltip("���O")] string m_name;
    [SerializeField, Tooltip("�GID")] EnemyID m_id;
    [SerializeField, Tooltip("�ő�̗�")] int m_life;
    [SerializeField, Tooltip("�摜")] Sprite m_sprite;
    [SerializeField, Tooltip("�摜�̃f�J���{��")] float m_spriteScaleMagnification = 1;
    [SerializeField, Tooltip("�o���ꏊ")] EnemyAppearanceEria m_enemyAppearanceEria;
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
            case NodeType.Sequence: //��ł����s�����炻���ŏI��
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
                Debug.LogError("�����ȃP�[�X");
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
    [Header("�s���f�[�^")]
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
    [Header("�����f�[�^")]
    [SerializeField, Tooltip("����")] WhereType m_type;
    [SerializeField, Tooltip("�]������l")] int m_value;
    [SerializeField, Tooltip("���̍s��������m��"), Range(0, 100)] int m_probability;
    [SerializeField, Tooltip("���̍s������x����ɂ��邩�ǂ���")] bool m_isOnlyOnce;
    private bool m_isOnlyFlag;
    public bool SetOnlyFlag
    {
        set
        {
            if (m_isOnlyOnce) //��x����ɂ���t���O��true�o�Ȃ���Ώ��������͂��Ȃ�
            {
                m_isOnlyFlag = value;
            }
        }
    }
    /// <summary>
    /// ������
    /// </summary>
    /// <returns>������</returns>
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
                Debug.LogError("�����ȏ����P�[�X");
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
    /// �GID�̎擾
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int GetID(int index) { return (int)m_enemyID[index]; }
    public int GetLength { get => m_enemyID.Length; }
}