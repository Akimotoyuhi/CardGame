using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAttackCard : IEffect
{
    [SerializeField] int m_power;

    public int[] GetParam()
    {
        int[] state = new int[(int)BuffDebuff.end];
        state[(int)BuffDebuff.Damage] = m_power;
        return state;
    }

    public string GetTooltip()
    {
        return $"{m_power}ダメージを与える";
    }
}

public class BlockCard : IEffect
{
    [SerializeField] int m_block;

    public int[] GetParam()
    {
        int[] state = new int[(int)BuffDebuff.end];
        state[(int)BuffDebuff.Block] = m_block;
        return state;
    }

    public string GetTooltip()
    {
        return $"{m_block}ブロックを得る";
    }
}

public class SetBuffDebuff : IEffect
{
    [SerializeField] BuffDebuff[] m_buffDebuffs;
    [SerializeField] int m_turn;

    public int[] GetParam()
    {
        int[] state = new int[(int)BuffDebuff.end];
        for (int i = 0; i < m_buffDebuffs.Length; i++)
        {
            state[(int)m_buffDebuffs[i]] += m_turn;
        }
        return state;
    }

    public string GetTooltip()
    {
        return $"{m_buffDebuffs}を{m_turn}ターン与える";
    }
}