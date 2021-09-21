using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーに対して何かするカードのクラス
/// </summary>
public class BuffCard : CardBase, IBuffCard
{
    void Start()
    {
        SetUp();
        m_tooltip.text = $"{m_cardData.m_cardData.Defense}ブロックを得る";
    }

    public int GetBlock()
    {
        OnCast();
        return m_cardData.m_cardData.Defense;
    }
}
