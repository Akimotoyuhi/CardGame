using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �E��<br/>
/// �^����_���[�W��25%��������
/// </summary>
public class Weakness : Condition
{
    public override int Effect(EventTiming eventTiming, ParametorType parametorType, int power)
    {
        if (Turn <= 0 && parametorType != ParametorType.Attack) return power;
        float ret = default;
        switch (eventTiming)
        {
            case EventTiming.TurnBegin:
                ret = power * (1 - 0.25f);
                return (int)ret;
            case EventTiming.Attacked:
                ret = power * (1 - 0.25f);
                return (int)ret;
            case EventTiming.TurnEnd:
                if (Turn > 0) Turn--;
                break;
            default:
                return power;
        }
        return power;
    }
    public override int IsBuff() { return 1; }
    public override ConditionID GetConditionID() { return ConditionID.Weakness; }
}
/// <summary>
/// �Ǝ㉻<br/>
/// ����u���b�N��25%����
/// </summary>
public class Vulnerable : Condition
{
    public override int Effect(EventTiming eventTiming, ParametorType parametorType, int block)
    {
        if (Turn <= 0 && parametorType != ParametorType.Block) return block;
        float ret = default;
        switch (eventTiming)
        {
            case EventTiming.TurnBegin:
                ret = block * (1 - 0.25f);
                return (int)ret;
            case EventTiming.Attacked:
                ret = block * (1 - 0.25f);
                return (int)ret;
            case EventTiming.TurnEnd:
                if (Turn > 0) Turn--;
                break;
        }
        return block;
    }
    public override int IsBuff() { return 1; }
    public override ConditionID GetConditionID() { return ConditionID.Vulnerable; }
}