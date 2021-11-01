using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [SerializeField] EventData m_eventData;
    [SerializeField] Text m_viewText;
    [SerializeField] float m_textTime;
    private bool m_isSpeak = false;
    private int m_nowText = 0;
    public int m_eventid = 0;

    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_viewText.text = "";
            if (!m_isSpeak)
            {
                if (m_nowText < m_eventData.m_eventDataBases[m_eventid].m_text.Length)
                {
                    StartCoroutine(TextAsync(m_eventData.m_eventDataBases[m_eventid].m_text[m_nowText]));
                    m_nowText++;
                }
                else m_nowText = 0;
            }
            else
            {
                m_isSpeak = true;
            }
        }
    }

    private IEnumerator TextAsync(string text)
    {
        m_isSpeak = true;
        for (int i = 0; i < text.Length; i++)
        {
            if (!m_isSpeak)
            {
                m_viewText.text += text[i];
            }
            else
            {
                m_viewText.text += text[i];
                yield return new WaitForSeconds(m_textTime);
            }
        }
        m_isSpeak = false;
    }
}