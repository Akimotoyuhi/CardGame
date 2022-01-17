using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public List<EnemyDataBase> m_enemyDataBases;
}

/// <summary>
/// �GID
/// </summary>
public enum EnemyID
{
    origimusi,
    Soldier,
    endLength
}

[System.Serializable]
public class EnemyDataBase
{
    [SerializeField] string m_name;
    [SerializeField] int m_life;
    [SerializeField] Sprite m_image;
    public string Name => m_name;
    public int Life => m_life;
    public Sprite Image => m_image;
    public enum NodeType { Selector, Sequence }
    public NodeType m_NodeType = NodeType.Sequence;
    public List<EnemyBaseState> m_enemyBaseState;
    public EnemyActionCommnad3 CommandSelect(EnemyBase enemy, int turn)
    {
        switch (m_NodeType)
        {
            case NodeType.Sequence: //��ł����s�����炻���ŏI��
                for (int i = 0; i < m_enemyBaseState.Count; i++)
                {
                    bool flag = false;
                    if (m_enemyBaseState[i].m_conditionalCommand.Count <= 0)
                    {
                        return m_enemyBaseState[i].m_actionCommnad;
                    }
                    for (int n = 0; n < m_enemyBaseState[i].m_conditionalCommand.Count; n++)
                    {
                        if (!m_enemyBaseState[i].m_conditionalCommand[n].Conditional(enemy, turn))
                        {
                            Debug.Log($"����{n}���� false");
                            flag = false;
                            break;
                        }
                        Debug.Log($"����{n}���� true");
                        flag = true;
                    }
                    if (flag) return m_enemyBaseState[i].m_actionCommnad;
                }
                Debug.Log("��������v");
                return null;
            default:
                Debug.LogError("�����ȃP�[�X");
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
    [Header("�s���f�[�^")]
    [SerializeField] string m_power = "0";
    [SerializeField] string m_block = "0";
    [SerializeField] List<ConditionSelection> m_condition;
    [SerializeField] TargetType m_target;
    public int Power => int.Parse(m_power);
    public int Block => int.Parse(m_block);
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
}

[System.Serializable]
public class EnemyConditionalCommand3
{
    [Header("�����f�[�^")]
    [SerializeField, Tooltip("����")] WhereType m_type;
    [SerializeField, Tooltip("�]������l")] int m_value;
    [SerializeField, Tooltip("�m���ɂ��邩�ۂ�")] bool m_isProbability;
    /// <summary>
    /// ������
    /// </summary>
    /// <returns>������</returns>
    public bool Conditional(EnemyBase enemy, int turn)
    {
        //if (turn == 0)
        //{
        //    if (m_type != WhereType.Turn && m_value != 0)
        //    {
        //        //0�^�[���ڂ͓��ʂȏ������������蓮���Ȃ��悤�ɂ���
        //        return false;
        //    }
        //}
        switch (m_type)
        {
            case WhereType.Turn:
                if (turn == m_value)
                {
                    return true;
                }
                return false;
            case WhereType.NotTurn:
                if (turn == m_value)
                {
                    return false;
                }
                return true;
            case WhereType.RowTurn:
                if (turn <= m_value)
                {
                    return true;
                }
                return false;
            case WhereType.HighLife:
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
            default:
                Debug.Log("�����ȏ����P�[�X");
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
    NotTurn,
    RowTurn,
    HighTurn,
    MultipleTurn,
    RowLife,
    HighLife,
}