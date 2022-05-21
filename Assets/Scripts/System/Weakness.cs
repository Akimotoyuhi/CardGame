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
    /// <summary>���s<br/>�^�[���J�n���ɋؗ�X������</summary>
    Corruption,
    /// <summary>�ܔM<br/>�󂯂�_���[�W��50%�����BX�^�[������</summary>
    Burning,
    /// <summary>����<br/>�^����_���[�W��50%�����BX�^�[������</summary>
    Frozen,
    /// <summary>���d<br/>�^�[���J�n�������R�X�g��X����</summary>
    ElectricShock,
    /// <summary>����<br/>�^�[���J�n���̃h���[������X������</summary>
    Silence,
    /// <summary>�F��<br/>�^�[���J�n�������R�X�g��X����</summary>
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
    public override string Tooltip => $"�E��\n�^����_���[�W��25%�ቺ�B{Turn}�^�[������";
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
    public override string Tooltip => $"�Ǝ㉻\n����u���b�N��25%�ቺ�B{Turn}�^�[������";
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
                return $"�ؗ�\n�^����_���[�W��<color=#0000ff>+{Turn}</color>";
            else
                return $"�ؗ�\n�^����_���[�W��<color=#ff0000>{Turn}</color>";
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
                return $"�q��\n����u���b�N��<color=#0000ff>+{Turn}</color>";
            else
                return $"�q��\n����u���b�N��<color=#ff0000>{Turn}</color>";
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
    public override string Tooltip => $"�v���[�g�A�[�}�[\n�����̃^�[���I������<color=#0000ff>{Turn}</color>�u���b�N�𓾂�B�_���[�W���󂯂�ƌ���-1";
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
    public override string Tooltip => $"�ؗ͒ቺ\n�^�[���J�n���ɋؗ�<color=#0000ff>{Turn}</color>������";
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
    public override string Tooltip => $"��s\n�󂯂�_���[�W��50%����";
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
    public override string Tooltip =>$"������\n�����̃^�[���I������<color=#0000ff>{Turn}</color>�u���b�N�𓾂�";
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
    public override string Tooltip => $"������\n�^����_���[�W��25%�����B<color=#0000ff>{Turn}</color>�^�[������";
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
    public override string Tooltip => $"���\n����u���b�N��25%�����B<color=#0000ff>{Turn}</color>�^�[������";
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
    public override string Tooltip => $"���s\n�^�[���J�n���ɋؗ�<color=#ff0000>{Turn}</color>������";
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
    public override string Tooltip => $"�ܔM\n����u���b�N��50%�ቺ�B<color=#0000ff>{Turn}</color>�^�[������";
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
    public override string Tooltip => $"����\n�^����_���[�W��50%�����B<color=#0000ff>{Turn}</color>�^�[������";
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
    public override string Tooltip => $"���d\n�^�[���J�n�������R�X�g��<color=#ff0000>{Turn}</color>����";
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
    public override string Tooltip => $"����\n�^�[���J�n���̃h���[������<color=#ff0000>{Turn}</color>������";
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
    public override string Tooltip => $"�F��\n�^�[���J�n�������R�X�g��<color=#0000ff>{Turn}</color>����";
}