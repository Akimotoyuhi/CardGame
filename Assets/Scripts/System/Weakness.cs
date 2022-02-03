using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakness : Condition
{
    public override int Effect(EventTiming eventTiming, ParametorType parametorType, int power = 0)
    {
        if (parametorType != ParametorType.Any)
        {
            if (Turn <= 0 || parametorType != GetParametorType()) return power;
        }
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
    public override ConditionID GetConditionID() => ConditionID.Weakness;
    public override ParametorType GetParametorType() => ParametorType.Attack;
}
public class Frail : Condition
{
    public override int Effect(EventTiming eventTiming, ParametorType parametorType, int block = 0)
    {
        if (parametorType != ParametorType.Any)
        {
            if (Turn <= 0 || parametorType != GetParametorType()) return block;
        }
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
    public override ConditionID GetConditionID() => ConditionID.Frail;
    public override ParametorType GetParametorType() => ParametorType.Block;
}
public class Strength : Condition
{
    public override int Effect(EventTiming eventTiming, ParametorType parametorType, int power = 0)
    {
        if (parametorType != ParametorType.Any)
        {
            if (Turn <= 0 || parametorType != ParametorType.Attack) return power;
        }
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                return power + Turn;
            default:
                return power;
        }
    }
    public override ConditionID GetConditionID() => ConditionID.Strength;
    public override int IsBuff() => 0;
    public override ParametorType GetParametorType() => ParametorType.Attack;
}
public class Agile : Condition
{
    public override int Effect(EventTiming eventTiming, ParametorType parametorType, int block = 0)
    {
        if (parametorType != ParametorType.Any)
        {
            if (Turn <= 0 || parametorType != ParametorType.Block) return block;
        }
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                return block + Turn;
            default:
                return block;
        }
    }
    public override ConditionID GetConditionID() => ConditionID.Agile;
    public override int IsBuff() => 0;
    public override ParametorType GetParametorType() => ParametorType.Block;
}
public class PlateArmor : Condition
{
    public override int Effect(EventTiming eventTiming, ParametorType parametorType, int block = 0)
    {
        if (Turn <= 0 || parametorType != ParametorType.Any) return block;
        switch (eventTiming)
        {
            case EventTiming.TurnEnd:
                Debug.Log("プレートアーマー" + Turn);
                return Turn;
            case EventTiming.Damaged:
                Turn--;
                return 0;
            default:
                return block;
        }
    }
    public override ConditionID GetConditionID() => global::ConditionID.PlateArmor;
    public override int IsBuff() => 0;
    public override ParametorType GetParametorType() => ParametorType.Any;
}
public class StrengthDown : Condition
{
    public override int Effect(EventTiming eventTiming, ParametorType parametorType, int value = 0)
    {
        Debug.Log($"渡された値 {eventTiming}:{parametorType}:{value}({(ConditionID)value})");
        if (parametorType != ParametorType.Any)
        {
            if (Turn <= 0 || parametorType != ParametorType.Condition) return 0;
        }
        if (eventTiming == EventTiming.TurnBegin && (ConditionID)value == ConditionID.Strength)
        {
            int ret = Turn;
            Turn = 0;
            return -ret;
        }
        return 0;
    }
    public override int IsBuff() => 1;
    public override ConditionID GetConditionID() => ConditionID.StrengthDown;
    public override ParametorType GetParametorType() => ParametorType.Condition;
}