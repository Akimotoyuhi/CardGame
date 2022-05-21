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
    /// <summary>腐敗<br/>ターン開始時に筋力Xを失う</summary>
    Corruption,
    /// <summary>灼熱<br/>受けるダメージが50%増加。Xターン持続</summary>
    Burning,
    /// <summary>凍結<br/>与えるダメージが50%減少。Xターン持続</summary>
    Frozen,
    /// <summary>感電<br/>ターン開始時所持コストがX減少</summary>
    ElectricShock,
    /// <summary>沈黙<br/>ターン開始時のドロー枚数がX枚減少</summary>
    Silence,
    /// <summary>祈祷<br/>ターン開始時所持コストがX増加</summary>
    Prayer,
}
public class Weakness : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int power = 0)
    {
        if (parametorType != ParametorType.Other)
        {
            if (Turn <= 0 || parametorType != GetParametorType())
                return new int[] { power };
        }
        float ret = default;
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                ret = power * (1 - 0.25f);
                return new int[] { (int)ret };
            case EventTiming.Drow:
                ret = power * (1 - 0.25f);
                return new int[] { (int)ret };
            case EventTiming.TurnEnd:
                if (Turn > 0) Turn--;
                break;
        }
        return new int[] { power };
    }
    public override bool IsRemove()
    {
        if (Turn <= 0) return true;
        return false;
    }
    public override int IsBuff() => 1;
    public override ConditionID GetConditionID() => ConditionID.Weakness;
    public override ParametorType GetParametorType() => ParametorType.Attack;
    public override string Tooltip => $"脱力\n与えるダメージが25%低下。{Turn}ターン持続";
}
public class Frail : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int block = 0)
    {
        if (parametorType != ParametorType.Other)
        {
            if (Turn <= 0 || parametorType != GetParametorType()) return new int[] { block };
        }
        float ret = default;
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                ret = block * (1 - 0.25f);
                return new int[] { (int)ret };
            case EventTiming.Drow:
                ret = block * (1 - 0.25f);
                return new int[] { (int)ret };
            case EventTiming.TurnEnd:
                if (Turn > 0) Turn--;
                break;
        }
        return new int[] { block };
    }
    public override bool IsRemove()
    {
        if (Turn <= 0) return true;
        return false;
    }
    public override int IsBuff() => 1;
    public override ConditionID GetConditionID() => ConditionID.Frail;
    public override ParametorType GetParametorType() => ParametorType.Block;
    public override string Tooltip => $"脆弱化\n得るブロックが25%低下。{Turn}ターン持続";
}
public class Strength : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int power = 0)
    {
        if (parametorType != ParametorType.Other)
        {
            if (Turn == 0 || parametorType != ParametorType.Attack) return new int[] { power };
        }
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                return new int[] { power + Turn };
            default:
                return new int[] { power };
        }
    }
    public override bool IsRemove()
    {
        if (Turn == 0) return true;
        return false;
    }
    public override ConditionID GetConditionID() => ConditionID.Strength;
    public override int IsBuff() => 0;
    public override ParametorType GetParametorType() => ParametorType.Attack;
    public override string Tooltip
    {
        get
        {
            if (Turn >= 0)
                return $"筋力\n与えるダメージが<color=#0000ff>+{Turn}</color>";
            else
                return $"筋力\n与えるダメージが<color=#ff0000>{Turn}</color>";
        }
    }
}
public class Agile : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int block = 0)
    {
        if (parametorType != ParametorType.Other)
        {
            if (Turn == 0 || parametorType != ParametorType.Block) return new int[] { block };
        }
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                return new int[] { block + Turn };
            default:
                return new int[] { block };
        }
    }
    public override bool IsRemove()
    {
        if (Turn == 0) return true;
        return false;
    }
    public override ConditionID GetConditionID() => ConditionID.Agile;
    public override int IsBuff() => 0;
    public override ParametorType GetParametorType() => ParametorType.Block;
    public override string Tooltip
    {
        get
        {
            if (Turn >= 0)
                return $"敏捷\n得るブロックが<color=#0000ff>+{Turn}</color>";
            else
                return $"敏捷\n得るブロックが<color=#ff0000>{Turn}</color>";
        }
    }
}
public class PlateArmor : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int block = 0)
    {
        if (Turn <= 0 || parametorType != ParametorType.Other)
            return new int[] { block };
        switch (eventTiming)
        {
            case EventTiming.TurnEnd:
                return new int[] { (int)ParametorType.Block, Turn };
            case EventTiming.Damaged:
                Turn--;
                return new int[] { 0 };
            default:
                return new int[] { 0 };
        }
    }
    public override bool IsRemove()
    {
        if (Turn <= 0) return true;
        return false;
    }
    public override ConditionID GetConditionID() => ConditionID.PlateArmor;
    public override int IsBuff() => 0;
    public override ParametorType GetParametorType() => ParametorType.Other;
    public override string Tooltip => $"プレートアーマー\n自分のターン終了時に<color=#0000ff>{Turn}</color>ブロックを得る。ダメージを受けると効果-1";
}
public class StrengthDown : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int value = 0)
    {
        if (parametorType != ParametorType.Other)
        {
            if (Turn <= 0 || parametorType != ParametorType.Condition) return new int[] { value };
        }
        if (eventTiming == EventTiming.TurnBegin && (ConditionID)value == ConditionID.Strength)
        {
            int[] ret = new int[] { (int)ParametorType.Condition, (int)ConditionID.Strength, -Turn };
            Turn = 0;
            return ret;
        }
        return new int[] { 0 };
    }
    public override bool IsRemove()
    {
        if (Turn <= 0) return true;
        return false;
    }
    public override int IsBuff() => 1;
    public override ConditionID GetConditionID() => ConditionID.StrengthDown;
    public override ParametorType GetParametorType() => ParametorType.Condition;
    public override string Tooltip => $"筋力低下\nターン開始時に筋力<color=#0000ff>{Turn}</color>を失う";
}
public class Flying : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int power = 0)
    {
        if (eventTiming == EventTiming.Damaged && parametorType == ParametorType.Attack)
        {
            float ret = power * (1 - 0.5f);
            return new int[] { (int)ret };
        }
        else return new int[] { power };
    }
    public override int IsBuff() => 0;
    public override bool IsRemove()
    {
        if (Turn <= 0) return true;
        return false;
    }
    public override ParametorType GetParametorType() => ParametorType.Attack;
    public override ConditionID GetConditionID() => ConditionID.Flying;
    public override string Tooltip => $"飛行\n受けるダメージが50%減少";
}
public class Metallicize : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int block = 0)
    {
        if (Turn <= 0 || parametorType != ParametorType.Other) return new int[] { block };
        switch (eventTiming)
        {
            case EventTiming.TurnEnd:
                return new int[] { block + Turn };
            default:
                return new int[] { block };
        }
    }
    public override bool IsRemove()
    {
        if (Turn < 0) return true;
        return false;
    }
    public override ConditionID GetConditionID() => ConditionID.Metallicize;
    public override int IsBuff() => 0;
    public override ParametorType GetParametorType() => ParametorType.Other;
    public override string Tooltip =>$"金属化\n自分のターン終了時に<color=#0000ff>{Turn}</color>ブロックを得る";
}
public class Activation : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int power = 0)
    {
        if (parametorType != ParametorType.Other)
        {
            if (Turn <= 0 || parametorType != GetParametorType()) return new int[] { power };
        }
        float ret;
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                ret = power * (1 + 0.25f);
                return new int[] { (int)ret };
            case EventTiming.Drow:
                ret = power * (1 + 0.25f);
                return new int[] { (int)ret };
            case EventTiming.TurnEnd:
                if (Turn > 0) Turn--;
                break;
        }
        return new int[] { power };
    }
    public override bool IsRemove()
    {
        if (Turn <= 0) return true;
        return false;
    }
    public override ConditionID GetConditionID() => ConditionID.Activation;
    public override int IsBuff() => 0;
    public override ParametorType GetParametorType() => ParametorType.Attack;
    public override string Tooltip => $"活性化\n与えるダメージが25%増加。<color=#0000ff>{Turn}</color>ターン持続";
}
public class Sturdy : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int block = 0)
    {
        if (parametorType != ParametorType.Other)
        {
            if (Turn <= 0 || parametorType != GetParametorType()) return new int[] { block };
        }
        float ret;
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                ret = block * (1 + 0.25f);
                return new int[] { (int)ret };
            case EventTiming.Drow:
                ret = block * (1 + 0.25f);
                return new int[] { (int)ret };
            case EventTiming.TurnEnd:
                if (Turn > 0) Turn--;
                break;
        }
        return new int[] { block };
    }
    public override bool IsRemove()
    {
        if (Turn <= 0) return true;
        return false;
    }
    public override ConditionID GetConditionID() => ConditionID.Sturdy;
    public override int IsBuff() => 0;
    public override ParametorType GetParametorType() => ParametorType.Block;
    public override string Tooltip => $"頑丈\n得るブロックが25%増加。<color=#0000ff>{Turn}</color>ターン持続";
}
public class Corruption : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int num = 0)
    {
        if (parametorType != ParametorType.Other)
            return new int[] { num };
        if (eventTiming == EventTiming.TurnBegin)
        {
            return new int[] { (int)ConditionID.Strength, -Turn };
        }
        return new int[] { 0 };
    }

    public override ConditionID GetConditionID() => ConditionID.Corruption;

    public override ParametorType GetParametorType() => ParametorType.Other;

    public override int IsBuff() => 1;

    public override bool IsRemove()
    {
        if (Turn < 0) return true;
        return false;
    }
    public override string Tooltip => $"腐敗\nターン開始時に筋力<color=#ff0000>{Turn}</color>を失う";
}
public class Burning : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int block = 0)
    {
        if (parametorType != ParametorType.Other)
        {
            if (Turn <= 0 || parametorType != GetParametorType()) return new int[] { block };
        }
        float ret = default;
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                ret = block * (1 - 0.5f);
                return new int[] { (int)ret };
            case EventTiming.Drow:
                ret = block * (1 - 0.5f);
                return new int[] { (int)ret };
            case EventTiming.TurnEnd:
                if (Turn > 0) Turn--;
                break;
        }
        return new int[] { block };
    }

    public override ConditionID GetConditionID() => ConditionID.Burning;

    public override ParametorType GetParametorType() => ParametorType.Attack;

    public override int IsBuff() => 1;

    public override bool IsRemove()
    {
        if (Turn <= 0)
            return true;
        else
            return false;
    }
    public override string Tooltip => $"灼熱\n得るブロックが50%低下。<color=#0000ff>{Turn}</color>ターン持続";
}
public class Frozen : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int power = 0)
    {
        if (parametorType != ParametorType.Other)
        {
            if (Turn <= 0 || parametorType != GetParametorType())
                return new int[] { power };
        }
        float ret = default;
        switch (eventTiming)
        {
            case EventTiming.Attacked:
                ret = power * (1 - 0.5f);
                return new int[] { (int)ret };
            case EventTiming.Drow:
                ret = power * (1 - 0.5f);
                return new int[] { (int)ret };
            case EventTiming.TurnEnd:
                if (Turn > 0) Turn--;
                break;
        }
        return new int[] { power };
    }

    public override ConditionID GetConditionID() => ConditionID.Frozen;

    public override ParametorType GetParametorType() => ParametorType.Attack;

    public override int IsBuff() => 1;

    public override bool IsRemove()
    {
        if (Turn <= 0)
            return true;
        else
            return false;
    }
    public override string Tooltip => $"凍結\n与えるダメージが50%減少。<color=#0000ff>{Turn}</color>ターン持続";
}
public class ElectricShock : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int cost = 0)
    {
        switch (eventTiming)
        {
            case EventTiming.TurnBegin:
                if (parametorType == GetParametorType())
                {
                    int ret = cost - Turn;
                    Turn = 0;
                    return new int[] { ret };
                }
                break;
            default:
                break;
        }
        return new int[] { cost };
    }

    public override ConditionID GetConditionID() => ConditionID.ElectricShock;

    public override ParametorType GetParametorType() => ParametorType.Cost;

    public override int IsBuff() => 1;

    public override bool IsRemove()
    {
        if (Turn <= 0)
            return true;
        else
            return false;
    }
    public override string Tooltip => $"感電\nターン開始時所持コストが<color=#ff0000>{Turn}</color>減少";
}
public class Silence : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int drowNum = 0)
    {
        switch (eventTiming)
        {
            case EventTiming.TurnBegin:
                if (parametorType == GetParametorType())
                {
                    int ret = drowNum - Turn;
                    Turn = 0;
                    return new int[] { ret };
                }
                break;
            default:
                break;
        }
        return new int[] { drowNum };
    }

    public override ConditionID GetConditionID() => ConditionID.Silence;

    public override ParametorType GetParametorType() => ParametorType.DrowNum;

    public override int IsBuff() => 1;

    public override bool IsRemove()
    {
        if (Turn <= 0)
            return true;
        else
            return false;
    }
    public override string Tooltip => $"沈黙\nターン開始時のドロー枚数が<color=#ff0000>{Turn}</color>枚減少";
}
public class Prayer : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int cost = 0)
    {
        switch (eventTiming)
        {
            case EventTiming.TurnBegin:
                if (parametorType == GetParametorType())
                {
                    int ret = cost + Turn;
                    Turn = 0;
                    return new int[] { ret };
                }
                break;
            default:
                break;
        }
        return new int[] { cost };
    }

    public override ConditionID GetConditionID() => ConditionID.Prayer;

    public override ParametorType GetParametorType() => ParametorType.Cost;

    public override int IsBuff() => 0;

    public override bool IsRemove()
    {
        if (Turn <= 0)
            return true;
        else
            return false;
    }
    public override string Tooltip => $"祈祷\nターン開始時所持コストが<color=#0000ff>{Turn}</color>増加";
}