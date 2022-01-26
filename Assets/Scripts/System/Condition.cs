using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// バフデバフ
/// </summary>
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
    /// <summary>プレートアーマー<br/>自分のターン終了時にnブロックを得る。攻撃されると効果-1</summary>
    PlateArmor,
}
/// <summary>
/// バフデバフが効果を発動するタイミング
/// </summary>
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
    Any,
}

/// <summary>
/// バフデバフ関係の基底クラス
/// </summary>
public abstract class Condition
{
    public int Turn { get; set; }
    /// <summary> 効果 </summary>
    /// <param name="eventTiming">評価されるタイミング</param>
    /// <param name="num">影響を受ける数値</param>
    /// <returns>計算後の数値</returns>
    public abstract int Effect(EventTiming eventTiming, ParametorType parametorType, int num = 0);
    /// <summary>バフかデバフかの判定</summary>
    /// <returns>0ならバフ、1ならデバフ、2ならそれ以外</returns>
    public abstract int IsBuff();
    public abstract ConditionID ConditionID();
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
            if (m_turn <= 0) return null;
            switch (m_conditionID)
            {
                case ConditionID.Weakness:
                    Condition weak = new Weakness();
                    weak.Turn = m_turn;
                    return weak;
                case ConditionID.Frail:
                    Condition vul = new Frail();
                    vul.Turn = m_turn;
                    return vul;
                case ConditionID.Strength:
                    Condition str = new Strength();
                    str.Turn = m_turn;
                    return str;
                case ConditionID.Agile:
                    Condition agile = new Agile();
                    agile.Turn = m_turn;
                    return agile;
                case ConditionID.PlateArmor:
                    Condition pa = new PlateArmor();
                    pa.Turn = m_turn;
                    return pa;
                default:
                    Debug.LogWarning("未設定のIDが渡されました");
                    return null;
            }
        }
    }
}