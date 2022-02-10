using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 捨て札山札の表示切替を制御する
/// </summary>
public class View : MonoBehaviour
{
    [SerializeField] private GameObject m_thisObj;
    private GameObject[] m_cards;

    private void Start()
    {
        m_thisObj.GetComponent<Canvas>().enabled = false;
    }
}
