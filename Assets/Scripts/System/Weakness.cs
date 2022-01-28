using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakness : Condition
{
    public override int Effect(EventTiming eventTiming, ParametorType parametorType, int power = 0)
    {
        if (parametorType != ParametorType.Any)
        {
            if (Turn <= 0 || parametorType != ParametorType.Attack) return power;
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
    public override ConditionID GetConditionID() => global::ConditionID.Weakness;
}
public class Frail : Condition
{
    public override int Effect(EventTiming eventTiming, ParametorType parametorType, int block = 0)
    {
        if (parametorType != ParametorType.Any)
        {
            if (Turn <= 0 || parametorType != ParametorType.Block) return block;
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
    public override ConditionID GetConditionID() => global::ConditionID.Frail;
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
    public override ConditionID GetConditionID() => global::ConditionID.Strength;
    public override int IsBuff() => 0;
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
    public override ConditionID GetConditionID() => global::ConditionID.Agile;
    public override int IsBuff() => 0;
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
        if (eventTiming == EventTiming.TurnBegin)
        {
            int ret = Turn;
            Turn = 0;
            return -ret;
        }
        return 0;
    }
    public override int IsBuff() => 1;
    public override ConditionID GetConditionID() => global::ConditionID.StrengthDown;
}