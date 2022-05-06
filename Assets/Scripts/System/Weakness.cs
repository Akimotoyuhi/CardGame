using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>バフデバフID</summary>
public enum ConditionID
{
    /// <summary>脱力<br/>与えるダメージが25%低下。Xターン持続</summary>
    Weakness,
    /// <summary>脆弱化<br/>得るブロックが25%低下。Xターン持続</summary>
    Frail,
    /// <summary>筋力<br/>与えるダメージが+X</summary>
    Strength,
    /// <summary>敏捷性<br/>得るブロックが+X</summary>
    Agile,
    /// <summary>プレートアーマー<br/>自分のターン終了時にXブロックを得る。攻撃されると効果-1</summary>
    PlateArmor,
    /// <summary>筋力低下<br/>ターン開始時に筋力Xを失う</summary>
    StrengthDown,
    /// <summary>飛行<br/>受けるダメージが50%減る</summary>
    Flying,
    /// <summary>金属化<br/>自分のターン終了時にXブロックを得る</summary>
    Metallicize,
    /// <summary>活性化<br/>与えるダメージが25%増加。Xターン持続</summary>
    Activation,
    /// <summary>頑丈<br/>得るブロックが25%増加。Xターン持続</summary>
    Sturdy,
    /// <summary>腐敗<br/>ターン終了時に筋力Xを失う</summary>
    Corruption,
}
public class Weakness : Condition
{
    public override Command Effect(EventTiming eventTiming, Command command)
    {
        Command ret = new Command();
        float f;
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                f = command.Power * (1 - 0.25f);
                ret.Power = (int)f;
                return ret;
            case EventTiming.Drow:
                f = command.Power * (1 - 0.25f);
                ret.Power = (int)f;
                return ret;
            case EventTiming.TurnEnd:
                if (Turn > 0) Turn--;
                break;
        }
        return command;
    }
    public override bool IsRemove()
    {
        if (Turn <= 0) return true;
        return false;
    }
    public override int IsBuff() => 1;
    public override ConditionID GetConditionID() => ConditionID.Weakness;
    public override ParametorType GetParametorType() => ParametorType.Attack;
}
public class Frail : Condition
{
    public override Command Effect(EventTiming eventTiming, Command command)
    {
        Command ret = new Command();
        float f;
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                f = command.Block * (1 - 0.25f);
                ret.Block = (int)f;
                return ret;
            case EventTiming.Drow:
                f = command.Block * (1 - 0.25f);
                ret.Block = (int)f;
                return ret;
            case EventTiming.TurnEnd:
                if (Turn > 0) Turn--;
                break;
        }
        return command;
    }
    public override bool IsRemove()
    {
        if (Turn <= 0) return true;
        return false;
    }
    public override int IsBuff() => 1;
    public override ConditionID GetConditionID() => ConditionID.Frail;
    public override ParametorType GetParametorType() => ParametorType.Block;
}
public class Strength : Condition
{
    public override Command Effect(EventTiming eventTiming, Command command)
    {
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                command.Power += Turn;
                break;
            default:
                break;
        }
        return command;
    }
    public override bool IsRemove()
    {
        if (Turn == 0) return true;
        return false;
    }
    public override ConditionID GetConditionID() => ConditionID.Strength;
    public override int IsBuff() => 0;
    public override ParametorType GetParametorType() => ParametorType.Attack;
}
public class Agile : Condition
{
    public override Command Effect(EventTiming eventTiming, Command command)
    {
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                command.Block += Turn;
                break;
            default:
                break;
        }
        return command;
    }
    public override bool IsRemove()
    {
        if (Turn == 0) return true;
        return false;
    }
    public override ConditionID GetConditionID() => ConditionID.Agile;
    public override int IsBuff() => 0;
    public override ParametorType GetParametorType() => ParametorType.Block;
}
public class PlateArmor : Condition
{
    public override Command Effect(EventTiming eventTiming, Command command)
    {
        switch (eventTiming)
        {
            case EventTiming.TurnEnd:
                command.Block += Turn;
                return command;
            case EventTiming.Damaged:
                Turn--;
                break;
            default:
                break;
        }
        return command;
    }
    public override bool IsRemove()
    {
        if (Turn < 0) return true;
        return false;
    }
    public override ConditionID GetConditionID() => ConditionID.PlateArmor;
    public override int IsBuff() => 0;
    public override ParametorType GetParametorType() => ParametorType.Other;
}
public class StrengthDown : Condition
{
    public override Command Effect(EventTiming eventTiming, Command command)
    {
        switch (eventTiming)
        {
            case EventTiming.TurnBegin:
                Command ret = new Command();
                ConditionSelection cs = new ConditionSelection();
                ret.Conditions.Add(cs.SetCondition(ConditionID.Strength, Turn));
                return ret;
            default:
                break;
        }
        return new Command();
    }
    public override bool IsRemove()
    {
        if (Turn <= 0) return true;
        return false;
    }
    public override int IsBuff() => 1;
    public override ConditionID GetConditionID() => ConditionID.StrengthDown;
    public override ParametorType GetParametorType() => ParametorType.Condition;
}
public class Flying : Condition
{
    public override Command Effect(EventTiming eventTiming, Command command)
    {
        switch (eventTiming)
        {
            case EventTiming.Damaged:
                float f = command.Power * (1 - 0.5f);
                command.Power = (int)f;
                return command;
            default:
                break;
        }
        return command;
    }
    public override int IsBuff() => 0;
    public override bool IsRemove()
    {
        if (Turn <= 0) return true;
        return false;
    }
    public override ParametorType GetParametorType() => ParametorType.Attack;
    public override ConditionID GetConditionID() => ConditionID.Flying;
}
public class Metallicize : Condition
{
    public override Command Effect(EventTiming eventTiming, Command command)
    {
        switch (eventTiming)
        {
            case EventTiming.TurnEnd:
                command.Block += Turn;
                return command;
            default:
                break;
        }
        return command;
    }
    public override bool IsRemove()
    {
        if (Turn < 0) return true;
        return false;
    }
    public override ConditionID GetConditionID() => ConditionID.Metallicize;
    public override int IsBuff() => 0;
    public override ParametorType GetParametorType() => ParametorType.Other;
}
public class Activation : Condition
{
    public override Command Effect(EventTiming eventTiming, Command command)
    {
        Command ret = new Command();
        float f;
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                f = command.Power * (1 + 0.25f);
                ret.Power = (int)f;
                return ret;
            case EventTiming.Drow:
                f = command.Power * (1 + 0.25f);
                ret.Power = (int)f;
                return ret;
            case EventTiming.TurnEnd:
                if (Turn > 0) Turn--;
                break;
        }
        return command;
    }
    public override bool IsRemove()
    {
        if (Turn <= 0) return true;
        return false;
    }
    public override ConditionID GetConditionID() => ConditionID.Activation;
    public override int IsBuff() => 0;
    public override ParametorType GetParametorType() => ParametorType.Attack;
}
public class Sturdy : Condition
{
    public override Command Effect(EventTiming eventTiming, Command command)
    {
        Command ret = new Command();
        float f;
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                f = command.Block * (1 + 0.25f);
                ret.Power = (int)f;
                return ret;
            case EventTiming.Drow:
                f = command.Block * (1 + 0.25f);
                ret.Power = (int)f;
                return ret;
            case EventTiming.TurnEnd:
                if (Turn > 0) Turn--;
                break;
        }
        return command;
    }
    public override bool IsRemove()
    {
        if (Turn <= 0) return true;
        return false;
    }
    public override ConditionID GetConditionID() => ConditionID.Sturdy;
    public override int IsBuff() => 0;
    public override ParametorType GetParametorType() => ParametorType.Block;
}
public class Corruption : Condition
{
    public override Command Effect(EventTiming eventTiming, Command command)
    {
        switch (eventTiming)
        {
            case EventTiming.TurnBegin:
                Command ret = new Command();
                ConditionSelection cs = new ConditionSelection();
                ret.Conditions.Add(cs.SetCondition(ConditionID.Strength, Turn));
                return ret;
            default:
                break;
        }
        return new Command();
    }

    public override ConditionID GetConditionID() => ConditionID.Corruption;

    public override ParametorType GetParametorType() => ParametorType.Other;

    public override int IsBuff() => 1;

    public override bool IsRemove()
    {
        if (Turn < 0) return true;
        return false;
    }
}