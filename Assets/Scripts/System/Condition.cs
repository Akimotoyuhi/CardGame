using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>バフデバフが効果を発動するタイミング</summary>
public enum EventTiming
{
    /// <summary>ターン開始時</summary>
    TurnBegin,
    /// <summary>ターン終了時</summary>
    TurnEnd,
    /// <summary>被弾時</summary>
    Damaged,
    /// <summary>攻撃時(カードプレイ時)</summary>
    Attacked,
    /// <summary>カードを引いた時</summary>
    Drow,
    /// <summary>ブロック成功</summary>
    Blocked,
}
/// <summary>Conditionが評価したい値</summary>
public enum ParametorType
{
    None = -1,
    Attack,
    Block,
    Life,
    Condition,
    Other,
    Cost,
    DrowNum,
    Turn,
}

/// <summary>
/// バフデバフ関係の基底クラス
/// </summary>
public abstract class Condition
{
    public int Turn { get; set; }
    /// <summary>Conditionの効果</summary>
    /// <param name="eventTiming">評価されるタイミング</param>
    /// <param name="parametorType">numで渡すパラメーターが何なのか</param>
    /// <param name="num">影響を受ける数値</param>
    /// <returns>Condition評価後の数値</returns>
    public abstract int[] Effect(EventTiming eventTiming, ParametorType parametorType, int num = 0);
    /// <summary>バフかデバフかの判定</summary>
    /// <returns>0ならバフ、1ならデバフ、2ならそれ以外</returns>
    public abstract int IsBuff();
    /// <summary>このConditionが持つID</summary>
    /// <returns></returns>
    public abstract ConditionID GetConditionID();
    /// <summary>このConditionが評価したい値</summary>
    /// <returns></returns>
    public abstract ParametorType GetParametorType();
    /// <summary>Conditionが消去される条件</summary>
    /// <returns></returns>
    public abstract bool IsRemove();
    public abstract string Tooltip { get; }
    public Condition Copy() => (Condition)MemberwiseClone();
}

[Serializable]
public class ConditionSelection
{
    [SerializeField] ConditionID m_conditionID;
    [SerializeField] int m_turn;
    public Condition GetCondition
    {
        get
        {
            //if (m_turn <= 0) return null;
            Condition ret;
            switch (m_conditionID)
            {
                case ConditionID.Weakness:
                    ret = new Weakness();
                    break;
                case ConditionID.Frail:
                    ret = new Frail();
                    break;
                case ConditionID.Strength:
                    ret = new Strength();
                    break;
                case ConditionID.Agile:
                    ret = new Agile();
                    break;
                case ConditionID.PlateArmor:
                    ret = new PlateArmor();
                    break;
                case ConditionID.StrengthDown:
                    ret = new StrengthDown();
                    break;
                case ConditionID.Flying:
                    ret = new Flying();
                    break;
                case ConditionID.Metallicize:
                    ret = new Metallicize();
                    break;
                case ConditionID.Activation:
                    ret = new Activation();
                    break;
                case ConditionID.Sturdy:
                    ret = new Sturdy();
                    break;
                case ConditionID.Corruption:
                    ret = new Corruption();
                    break;
                case ConditionID.Burning:
                    ret = new Burning();
                    break;
                case ConditionID.Frozen:
                    ret = new Frozen();
                    break;
                case ConditionID.ElectricShock:
                    ret = new ElectricShock();
                    break;
                case ConditionID.Silence:
                    ret = new Silence();
                    break;
                case ConditionID.Prayer:
                    ret = new Prayer();
                    break;
                case ConditionID.Poison:
                    ret = new Poison();
                    break;
                default:
                    Debug.LogWarning("未設定のIDが渡されました");
                    return null;
            }
            ret.Turn = m_turn;
            return ret;
        }
    }
    public Condition SetCondition(ConditionID conditionID, int turn)
    {
        m_conditionID = conditionID;
        m_turn = turn;
        return GetCondition;
    }
}