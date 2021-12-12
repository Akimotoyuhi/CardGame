using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData3 : ScriptableObject
{
    public List<EnemyDataBase3> m_enemyDataBase3s;
}

[System.Serializable]
public class EnemyDataBase3
{
    [SerializeField] string m_name;
    [SerializeField] int m_life;
    [SerializeField] Sprite m_image;
    public string Name => m_name;
    public int Life => m_life;
    public Sprite Image => m_image;
    public enum NodeType { Selector, Sequence }
    public NodeType m_NodeType;
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
                Debug.LogError("��������v");
                return null;
            case NodeType.Selector: //�ǂꂩ��ł�����������true
                for (int i = 0; i < m_enemyBaseState.Count; i++)
                {
                    for (int n = 0; n < m_enemyBaseState[i].m_conditionalCommand.Count; n++)
                    {
                        if (m_enemyBaseState[i].m_conditionalCommand[n].Conditional(enemy, turn))
                        {

                        }
                    }
                }
                return null;
        }
        return null;
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
    //�s���f�[�^
    [SerializeField] string m_power;
    [SerializeField] string m_block;
    [SerializeField] ConditionSelection m_condition;
    [SerializeField] TargetType m_target;
    public int Power => int.Parse(m_power);
    public int Block => int.Parse(m_block);
    public Condition Condition => m_condition.GetCondition;
    public TargetType Target => m_target;
}

[System.Serializable]
public class EnemyConditionalCommand3
{
    //�����f�[�^
    [SerializeField] WhereType m_type;
    [SerializeField] int m_value;
    /// <summary>
    /// ������
    /// </summary>
    /// <returns>������</returns>
    public bool Conditional(EnemyBase enemy, int turn)
    {
        switch (m_type)
        {
            case WhereType.Turn:
                if (turn == m_value)
                {
                    return true;
                }
                return false;
            case WhereType.BerrowLife:
                if (enemy.Life <= m_value)
                {
                    return true;
                }
                return false;
            default:
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
    BerrowLife
}