using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private Transform m_deck;

    /// <summary>
    /// 手札にある全てのカードを捨て札に移動させる
    /// </summary>
    public void AllCast(int n = 0)
    {
        if (transform.childCount == 0) { return; }
        for (int i = transform.childCount - 1; 0 <= i; i--)
        {
            //transform.GetChild(i).parent = m_deck;
            transform.GetChild(i).SetParent(m_deck, false);
        }
    }
}
