using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    EnemyCommand GetParam();
}

public class EnemyNanmosinai : ICommand
{
    public EnemyCommand GetParam()
    {
        EnemyCommand command = new EnemyCommand();
        command.m_ = 0;
        return command;
    }
}

public class EnemyAttack : ICommand
{
    [SerializeField] int m_power;
    public EnemyCommand GetParam()
    {
        EnemyCommand command = new EnemyCommand();
        command.m_attack = m_power;
        return command;
    }
}

public class EnemyBlock : ICommand
{
    [SerializeField] int m_block;
    public EnemyCommand GetParam()
    {
        EnemyCommand command = new EnemyCommand();
        command.m_block = m_block;
        return command;
    }
}

public class EnemyCondition : ICommand
{
    [SerializeField] ConditionID m_buffDebuffs;
    [SerializeField] int m_turn;
    public EnemyCommand GetParam()
    {
        EnemyCommand command = new EnemyCommand();
        command.m_conditions = new int[(int)ConditionID.end];
        command.m_conditions[(int)m_buffDebuffs] += m_turn;
        return command;
    }
}

public class EnemyCommand
{
    public int m_attack;
    public int m_block;
    //public List<ICondition> conditions = new List<ICondition>();
    public int[] m_conditions;
    public byte m_;

    //public int Attack => m_attack;
    //public int Block => m_block;
    //public List<Condition> conditions
}