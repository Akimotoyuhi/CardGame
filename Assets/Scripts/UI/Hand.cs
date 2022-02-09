﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : CardManagement
{
    [SerializeField] private Deck m_deck;

    /// <summary>
    /// 手札にある全てのカードを捨て札に移動させる
    /// </summary>
    public void AllCast(int n = 0)
    {
        if (transform.childCount == 0) { return; }
        for (int i = transform.childCount - 1; 0 <= i; i--)
        {
            //m_deck.GetComponent<Deck>().SetParent(transform.GetChild(i));
            //transform.GetChild(i).SetParent(m_deck, false);
            m_cardParent.GetChild(i).SetParent(m_deck.transform, false);
        }
    }
    public void UpdateTooltip()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<BlankCard>().GetPlayerEffect();
        }
    }
}
