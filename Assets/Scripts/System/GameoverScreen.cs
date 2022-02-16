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
        m_text.DOText($"���Ȃ��͗��̓r����<color=#ff0000>{haveCardNum}</color>���̃J�[�h����ɓ��ꂽ���A<color=#ff0000>{step}</color>�K�ɂē|�ꂽ�B\n", m_textDuration).
            OnComplete(() =>
            {
                m_retryButton.SetActive(true);
                m_titleButton.SetActive(true);
            });
    }
}
