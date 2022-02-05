using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 何しようとしてるか忘れた
/// </summary>
public class View : MonoBehaviour
{
    [SerializeField] private GameObject m_thisObj;
    private GameObject[] m_cards;

    private void Start()
    {
        m_thisObj.GetComponent<Canvas>().enabled = false;
    }

    public void OnClick()
    {
        
    }
}
