﻿using System.Collections;
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

    /// <summary>
    /// 付与するブロックとバフデバフの計算処理
    /// </summary>
    /// <returns></returns>
    protected int[] SetBuff()
    {
        //バフデバフの付与
        int[] nums = m_player.m_stateArray;
        //ブロック値は永続させないので破棄
        nums[(int)BuffDebuff.Block] = 0;
        for (int i = 0; i < m_cardData.m_cardData.GiveStateNum; i++)
        {
            nums[(int)m_cardData.m_cardData.GiveState(i)] += m_cardData.m_cardData.GiveStateTrun(i);
        }
        //防御の計算
        int block = m_cardData.m_cardData.Defense;
        if (nums[(int)BuffDebuff.Vulnerable] > 0)
        {
            Parsent(block, 0.25f);
        }
        block += nums[(int)BuffDebuff.Agile];
        nums[(int)BuffDebuff.Block] = block;
        return nums;
    }

    public int[] SetBlock()
    {
        OnCast();
        return SetBuff();
    }
}