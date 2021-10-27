using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    EnemyCommand GetParam();
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
    [SerializeField] BuffDebuff m_buffDebuffs;
    [SerializeField] int m_turn;
    public EnemyCommand GetParam()
    {
        EnemyCommand command = new EnemyCommand();
        command.m_conditions = new int[(int)BuffDebuff.end];
        command.m_conditions[(int)m_buffDebuffs] += m_turn;
        return command;
    }
}

public class EnemyCommand
{
    public int m_attack;
    public int m_block;
    public int[] m_conditions;
}