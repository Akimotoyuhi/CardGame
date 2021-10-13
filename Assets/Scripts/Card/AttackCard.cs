using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵に対して何かするカードのクラス
/// </summary>
public class AttackCard : CardBase, IAttackCard
{
    /// <summary>与えるダメージ</summary>
    [SerializeField] int m_damage;

    private void Start()
    {
        SetUp();
        //m_tooltip.text = $"{m_cardData.m_cardData.Damage}ダメージを与える";
    }

    /// <summary>
    /// 与ダメージ計算。ダメージを受ける（Dropされる）側で受け取る必要がある
    /// </summary>
    /// <returns>計算後のダメージと与える状態</returns>
    public int[] GetDamage()
    {
        OnCast();
        int[] nums = new int[(int)BuffDebuff.end];
        nums[(int)BuffDebuff.Damage] = m_cardData.m_cardData.Damage;
        for (int i = 0; i < m_cardData.m_cardData.GiveStateNum; i++)
        {
            nums[(int)m_cardData.m_cardData.GiveState(i)] = m_cardData.m_cardData.GiveStateTrun(i);
        }
        return nums;
    }
}
