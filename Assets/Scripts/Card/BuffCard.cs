using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーに対して何かするカードのクラス
/// </summary>
public class BuffCard : CardBase, IBuffCard
{
    [SerializeField] private int m_block = 0;
    void Start()
    {
        SetUp();
    }

    public int GetBlock()
    {
        OnCast();
        return m_block;
    }
}
