using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>バフデバフ</summary>
public enum ConditionID
{
    /// <summary>脱力<br/>与えるダメージが25%低下</summary>
    Weakness,
    /// <summary>脆弱化<br/>得るブロックが25%低下</summary>
    Frail,
    /// <summary>筋力<br/>与えるダメージが+X</summary>
    Strength,
    /// <summary>敏捷性<br/>得るブロックが+X</summary>
    Agile,
    /// <summary>プレートアーマー<br/>自分のターン終了時にXブロックを得る。攻撃されると効果-1</summary>
    PlateArmor,
    /// <summary>筋力低下<br/>ターン開始時に筋力Xを失う</summary>
    StrengthDown,
    /// <summary>遠距離攻撃</summary>
    Ranger,
    /// <summary>金属化</summary>
    Metallicize,
}
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
}
public enum ParametorType
{
    Attack,
    Block,
    Life,
    Condition,
    Any,
}

/// <summary>
/// バフデバフ関係の基底クラス
/// </summary>
public abstract class Condition
{
    public int Turn { get; set; }
    /// <summary>Conditionの効果</summary>
    /// <param name="eventTiming">評価されるタイミング</param>
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
            Condition ret = default;
            switch (m_conditionID)
            {
                case ConditionID.Weakness:
                    ret = new Weakness();
                    ret.Turn = m_turn;
                    return ret;
                case ConditionID.Frail:
                    ret = new Frail();
                    ret.Turn = m_turn;
                    return ret;
                case ConditionID.Strength:
                    ret = new Strength();
                    ret.Turn = m_turn;
                    return ret;
                case ConditionID.Agile:
                    ret = new Agile();
                    ret.Turn = m_turn;
                    return ret;
                case ConditionID.PlateArmor:
                    ret = new PlateArmor();
                    ret.Turn = m_turn;
                    return ret;
                case ConditionID.StrengthDown:
                    ret = new StrengthDown();
                    ret.Turn = m_turn;
                    return ret;
                case ConditionID.Ranger:
                    ret = new Ranger();
                    ret.Turn = m_turn;
                    return ret;
                case ConditionID.Metallicize:
                    ret = new Metallicize();
                    ret.Turn = m_turn;
                    return ret;
                default:
                    Debug.LogWarning("未設定のIDが渡されました");
                    return null;
            }
        }
    }
    public Condition SetCondition(ConditionID conditionID, int turn)
    {
        m_conditionID = conditionID;
        m_turn = turn;
        return GetCondition;
    }
}