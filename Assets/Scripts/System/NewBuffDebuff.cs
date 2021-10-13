using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バフデバフ
/// </summary>
public enum BuffDebuff
{
    Damage = 0,
    Block = 1,
    /// <summary>脱力:与えるダメージが25%低下</summary>
    Weakness = 2,
    /// <summary>脆弱化:得るブロックが25%低下</summary>
    Vulnerable = 3,
    /// <summary>筋力:与えるダメージがn増加</summary>
    Strength = 5,
    /// <summary>敏捷性:得るブロックがn増加</summary>
    Agile = 6,
    end = 7,
}

public class NewBuffDebuff
{
    public void BuffDebuffManager()
    {

    }
}
