using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discard : MonoBehaviour
{
    [SerializeField] private Transform m_deck;
    public void ConvartToDeck()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).parent = m_deck;
        }
    }
}
