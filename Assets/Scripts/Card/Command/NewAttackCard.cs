using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAttackCard : ICommand
{
    [SerializeField] int m_power;

    public int[] SetParam()
    {
        int[] state = new int[(int)BuffDebuff.end];
        state[(int)BuffDebuff.Damage] = m_power;
        return state;
    }
}

public class BlockCard : ICommand
{
    [SerializeField] int m_block;

    public int[] SetParam()
    {
        Debug.Log("b");
        int[] state = new int[(int)BuffDebuff.end];
        state[(int)BuffDebuff.Block] = m_block;
        return state;
    }
}

public class SetBuffDebuff : ICommand
{
    [SerializeField] BuffDebuff[] m_buffDebuffs;
    [SerializeField] int m_turn;

    public int[] SetParam()
    {
        Debug.Log("c");
        int[] state = new int[(int)BuffDebuff.end];
        for (int i = 0; i < m_buffDebuffs.Length; i++)
        {
            state[(int)m_buffDebuffs[i]] += m_turn;
        }
        return state;
    }
}