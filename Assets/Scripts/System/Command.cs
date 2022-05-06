using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Command
{
    /// <summary>攻撃</summary>
    public int Power { get; set; }
    /// <summary>ブロック</summary>
    public int Block { get; set; }
    /// <summary>体力</summary>
    public int Life { get; set; }
    /// <summary>付与するバフデバフ</summary>
    public List<Condition> Conditions { get; set; }
}