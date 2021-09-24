using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーに対して何かするカードのクラス
/// </summary>
public class BuffCard : CardBase, IBuffCard
{
    protected int m_block;
    void Start()
    {
        SetUp();
        m_tooltip.text = $"{m_block}ブロックを得る";
    }

    public override void SetUp()
    {
        base.SetUp();
        m_block = m_cardData.m_cardData.Defense;
    }

    /// <summary>
    /// 付与するブロックとバフデバフの計算処理
    /// </summary>
    /// <returns></returns>
    protected int[] SetBuff()
    {
        int[] nums = new int[(int)BuffDebuff.end];
        //バフデバフの付与
        nums = m_player.m_stateArray;
        //ブロック値はターンを跨がないので破棄
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
