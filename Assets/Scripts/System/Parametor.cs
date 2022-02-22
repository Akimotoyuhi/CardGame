using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戦闘で使うパラメーターを一つにまとめた構造体
/// </summary>
public struct Parametor
{
    /// <summary>攻撃</summary>
    public int Attack { get; set; }
    /// <summary>攻撃回数</summary>
    public int AttackNum { get; set; }
    /// <summary>ブロック</summary>
    public int Block { get; set; }
    /// <summary>ブロック回数</summary>
    public int BlockNum { get; set; }
    /// <summary>付与するバフデバフ</summary>
    public List<Condition> Conditions { get; set; }
}