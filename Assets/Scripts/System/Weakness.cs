using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakness : Condition
{
    public override int Effect(EventTiming eventTiming, ParametorType parametorType, int power = 0)
    {
        if (Turn <= 0 && parametorType != ParametorType.Attack) return power;
        float ret = default;
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                ret = power * (1 - 0.25f);
                return (int)ret;
            case EventTiming.Drow:
                ret = power * (1 - 0.25f);
                return (int)ret;
            case EventTiming.TurnEnd:
                if (Turn > 0) Turn--;
                break;
        }
        return power;
    }
    public override int IsBuff() => 1;
    public override ConditionID ConditionID() => global::ConditionID.Strength;
}
public class Vulnerable : Condition
{
    public override int Effect(EventTiming eventTiming, ParametorType parametorType, int block = 0)
    {
        if (Turn <= 0 && parametorType != ParametorType.Block) return block;
        float ret = default;
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                ret = block * (1 - 0.25f);
                return (int)ret;
            case EventTiming.Drow:
                ret = block * (1 - 0.25f);
                return (int)ret;
            case EventTiming.TurnEnd:
                if (Turn > 0) Turn--;
                break;
        }
        return block;
    }
    public override int IsBuff() => 1;
    public override ConditionID ConditionID() => global::ConditionID.Vulnerable;
}
public class Strength : Condition
{
    public override int Effect(EventTiming eventTiming, ParametorType parametorType, int power = 0)
    {
        if (Turn <= 0 && parametorType != ParametorType.Attack) return power;
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                return power + Turn;
            default:
                return power;
        }
    }
    public override ConditionID ConditionID() => global::ConditionID.Strength;
    public override int IsBuff() => 0;
}
public class Agile : Condition
{
    public override int Effect(EventTiming eventTiming, ParametorType parametorType, int block = 0)
    {
        if (Turn <= 0 && parametorType != ParametorType.Block) return block;
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                return block + Turn;
            default:
                return block;
        }
    }
    public override ConditionID ConditionID() => global::ConditionID.Agile;
    public override int IsBuff() => 0;
}
public class PlateArmor : Condition
{
    public override int Effect(EventTiming eventTiming, ParametorType parametorType, int block = 0)
    {
        if (Turn <= 0 && parametorType != ParametorType.Block) return block;
        switch (eventTiming)
        {
            case EventTiming.TurnEnd:
                return Turn;
            case EventTiming.Damaged:
                Turn--;
                return 0;
            default:
                return block;
        }
    }
    public override ConditionID ConditionID() => global::ConditionID.PlateArmor;
    public override int IsBuff() => 0;
}