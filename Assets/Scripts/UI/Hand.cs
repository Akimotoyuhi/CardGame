using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : CardManagement
{
    [SerializeField] private Discard m_discard;

    /// <summary>
    /// 手札にある全てのカードを捨て札に移動させる
    /// </summary>
    public void AllCast()
    {
        if (m_cardParent.childCount == 0) { return; }
        for (int i = m_cardParent.childCount - 1; 0 <= i; i--)
        {
            m_cardParent.GetChild(i).GetComponent<BlankCard>().OnCast(false);
        }
    }
    /// <summary>
    /// カードのTooltipの更新
    /// </summary>
    public void UpdateTooltip()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            BlankCard card = transform.GetChild(i).GetComponent<BlankCard>();
            card.GetPlayerEffect();
        }
    }
}
