using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�o�t�f�o�tID</summary>
public enum ConditionID
{
    /// <summary>�E��<br/>�^����_���[�W��25%�ቺ�BX�^�[������</summary>
    Weakness,
    /// <summary>�Ǝ㉻<br/>����u���b�N��25%�ቺ�BX�^�[������</summary>
    Frail,
    /// <summary>�ؗ�<br/>�^����_���[�W��+X</summary>
    Strength,
    /// <summary>�q����<br/>����u���b�N��+X</summary>
    Agile,
    /// <summary>�v���[�g�A�[�}�[<br/>�����̃^�[���I������X�u���b�N�𓾂�B�U�������ƌ���-1</summary>
    PlateArmor,
    /// <summary>�ؗ͒ቺ<br/>�^�[���J�n���ɋؗ�X������</summary>
    StrengthDown,
    /// <summary>��s<br/>�󂯂�_���[�W��50%����</summary>
    Flying,
    /// <summary>������<br/>�����̃^�[���I������X�u���b�N�𓾂�</summary>
    Metallicize,
    /// <summary>������<br/>�^����_���[�W��25%�����BX�^�[������</summary>
    Activation,
    /// <summary>���<br/>����u���b�N��25%�����BX�^�[������</summary>
    Sturdy,
}
public class Weakness : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int power = 0)
    {
        if (parametorType != ParametorType.Other)
        {
            if (Turn <= 0 || parametorType != GetParametorType()) return new int[] { power };
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
}
public class PlateArmor : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int block = 0)
    {
        if (Turn <= 0 || parametorType != ParametorType.Other) return new int[] { block };
        switch (eventTiming)
        {
            case EventTiming.TurnEnd:
                return new int[] { Turn };
            case EventTiming.Damaged:
                Turn--;
                return new int[] { 0 };
            default:
                return new int[] { block };
        }
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
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int value = 0)
    {
        Debug.Log($"�n���ꂽ�l {eventTiming}:{parametorType}:{value}({(ConditionID)value})");
        if (parametorType != ParametorType.Other)
        {
            if (Turn <= 0 || parametorType != ParametorType.Condition) return new int[] { value };
        }
        if (eventTiming == EventTiming.TurnBegin && (ConditionID)value == ConditionID.Strength)
        {
            int[] ret = new int[] { (int)ConditionID.Strength, -Turn };
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
}
public class Ranger : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int power = 0)
    {
        if (eventTiming == EventTiming.Damaged && parametorType == ParametorType.Attack)
        {
            float ret = power * (1 - 0.25f);
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
}
public class Metallicize : Condition
{
    public override int[] Effect(EventTiming eventTiming, ParametorType parametorType, int block = 0)
    {
        if (Turn <= 0 || parametorType != ParametorType.Other) return new int[] { block };
        switch (eventTiming)
        {
            case EventTiming.TurnEnd:
                return new int[] { Turn };
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
        return true;
    }
    public override ConditionID GetConditionID() => ConditionID.Activation;
    public override int IsBuff() => 0;
    public override ParametorType GetParametorType() => ParametorType.Attack;
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
        return true;
    }
    public override ConditionID GetConditionID() => ConditionID.Sturdy;
    public override int IsBuff() => 0;
    public override ParametorType GetParametorType() => ParametorType.Block;
}