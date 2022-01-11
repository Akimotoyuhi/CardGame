using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discard : CardManagement
{
    [SerializeField] private Transform m_deck;

    /// <summary>捨て札から山札にカードを移す</summary>
    public void ConvartToDeck()
    {
        for (int i = transform.childCount - 1; 0 <= i; i--)
        {
            transform.GetChild(i).GetComponent<BlankCard>().GetPlayerEffect();
            transform.GetChild(i).SetParent(m_deck, false);
        }
    }
}
