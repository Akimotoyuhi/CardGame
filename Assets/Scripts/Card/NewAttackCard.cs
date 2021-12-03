using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAttackCard : IEffect
{
    [SerializeField] int m_power;

    //public int[] GetParam()
    //{
    //    int[] state = new int[(int)BuffDebuff.end];
    //    state[(int)BuffDebuff.Damage] = m_power;
    //    return state;
    //}
    public CardBase GetParam()
    {
        CardBase cardBase = new CardBase();
        cardBase.m_attack = m_power;
        return cardBase;
    }

    public string GetTooltip()
    {
        return $"{m_power}ダメージを";
    }
}

public class BlockCard : IEffect
{
    [SerializeField] int m_block;

    //public int[] GetParam()
    //{
    //    int[] state = new int[(int)BuffDebuff.end];
    //    state[(int)BuffDebuff.Block] = m_block;
    //    return state;
    //}
    public CardBase GetParam()
    {
        CardBase cardBase = new CardBase();
        cardBase.m_block = m_block;
        return cardBase;
    }

    public string GetTooltip()
    {
        return $"{m_block}ブロックを";
    }
}

public class ConditionCard : IEffect
{
    //[SerializeField] BuffDebuff m_buffDebuffs;
    //[SerializeField] int m_turn;
    //[SerializeField] Condition m_condition;
    //[SerializeReference] SetCondition m_condition;
    //[System.Serializable]
    //public class SetCondition
    //{
    //    [SerializeReference, SubclassSelector]
    //    public ICondition m_condition;
    //}
    //public List<SetCondition> m_setConditions = new List<SetCondition>();

    public CardBase GetParam()
    {
        CardBase cardBase = new CardBase();
        cardBase.conditions = new int[(int)ConditionID.end];
        //cardBase.conditions[(int)m_buffDebuffs] += m_turn;
        return cardBase;
    }

    public string GetTooltip()
    {
        //return $"{m_buffDebuffs}を{m_turn}ターン";
        return "a";
    }
}