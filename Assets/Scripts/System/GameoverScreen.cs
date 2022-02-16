using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameoverScreen : MonoBehaviour
{
    [SerializeField] GameObject m_panel;
    [SerializeField] Text m_text;
    [SerializeField] float m_textDuration;
    [SerializeField] GameObject m_retryButton;
    [SerializeField] GameObject m_titleButton;

    private void Start()
    {
        m_panel.SetActive(false);
        m_retryButton.SetActive(false);
        m_titleButton.SetActive(false);
    }

    public void ShowPanel(int haveCardNum, int step)
    {
        m_panel.SetActive(true);
        m_text.text = default;
        m_text.DOText($"あなたは旅の途中に<color=#ff0000>{haveCardNum}</color>枚のカードを手に入れたが、<color=#ff0000>{step}</color>階にて倒れた。\n", m_textDuration).
            OnComplete(() =>
            {
                m_retryButton.SetActive(true);
                m_titleButton.SetActive(true);
            });
    }
}
