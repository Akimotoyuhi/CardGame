﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// バフデバフ
/// </summary>
public enum ConditionID
{
    /// <summary>脱力:与えるダメージが25%低下</summary>
    Weakness,
    /// <summary>脆弱化:得るブロックが25%低下</summary>
    Vulnerable,
    /// <summary>筋力:与えるダメージが+X</summary>
    Strength,
    /// <summary>敏捷性:得るブロックが+X</summary>
    Agile,
    end,
}

public enum EventTiming
{
    TurnBegin,
    TurnEnd,
    Hit,
    Attacked
}

/// <summary>
/// バフデバフ関係の基底クラス
/// </summary>
public abstract class Condition
{
    public int m_turn = default;
    /// <summary> 効果 </summary>
    /// <param name="eventTiming">評価されるタイミング</param>
    /// <param name="num">影響を受ける数値</param>
    /// <returns>計算後の数値</returns>
    public abstract int Effect(EventTiming eventTiming, int num = 0);
    /// <summary>バフかデバフかの判定</summary>
    /// <returns>0ならバフ、1ならデバフ、2ならそれ以外</returns>
    public abstract int IsBuff();
}

[Serializable]
public class ConditionCelection
{
    [SerializeField] ConditionID m_conditionID;
    [SerializeField] int m_turn;

    public Condition Condition
    {
        get
        {
            switch (m_conditionID)
            {
                case ConditionID.Weakness:
                    Condition weak = new Weakness();
                    weak.m_turn = m_turn;
                    return weak;
                case ConditionID.Vulnerable:
                    Condition vul = new Vulnerable();
                    vul.m_turn = m_turn;
                    return vul;
                default:
                    Debug.Log("無効なパラメーター");
                    return null;
            }
        }
    }
}