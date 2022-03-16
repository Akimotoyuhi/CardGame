using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameoverScreen : MonoBehaviour
{
    /// <summary>�Q�[���I�[�o�[���</summary>
    [SerializeField] GameObject m_panel;
    /// <summary>�Q�[���I�[�o�[���ɏ���������e�L�X�g</summary>
    [SerializeField] Text m_text;
    /// <summary>�e�L�X�g�\���܂ł̎���</summary>
    [SerializeField] float m_textDuration;
    /// <summary>�Q�[���I�[�o�[���ɕ\������镶����</summary>
    [SerializeField, TextArea] string m_gameoverText;
    /// <summary>�Q�[���N���A���ɕ\������镶����</summary>
    [SerializeField, TextArea] string m_gamecrearText;
    [SerializeField] GameObject m_retryButton;
    [SerializeField] GameObject m_titleButton;

    private void Start()
    {
        m_panel.SetActive(false);
        m_retryButton.SetActive(false);
        m_titleButton.SetActive(false);
    }
    /// <summary>
    /// �Q�[���I�[�o�[��ʂ̕\��
    /// </summary>
    /// <param name="haveCardNum">�����J�[�h����</param>
    /// <param name="step">�Q�[���i�s�x</param>
    public void ShowPanel(int haveCardNum, int step, bool isCrear)
    {
        m_panel.SetActive(true);
        m_text.text = default;
        m_text.DOText(SetGameoverText(haveCardNum, step, isCrear), m_textDuration).
            OnComplete(() =>
            {
                m_retryButton.SetActive(true);
                m_titleButton.SetActive(true);
            });
    }
    /// <summary>
    /// �Q�[���I�[�o�[���̃e�L�X�g�̐��K�\���̒u������
    /// </summary>
    /// <param name="haveCardNum">�����J�[�h����</param>
    /// <param name="step">�Q�[���i�s�x</param>
    /// <returns>�u��������̃e�L�X�g</returns>
    private string SetGameoverText(int haveCardNum, int step, bool isCrear)
    {
        string ret = default;
        if (isCrear) ret = m_gamecrearText;
        else ret = m_gameoverText;
        MatchCollection matchs = Regex.Matches(ret, "{%haveCard}");
        foreach (Match m in matchs)
        {
            ret = ret.Replace(m.Value, haveCardNum.ToString());
        }
        matchs = Regex.Matches(ret, "{%step}");
        foreach (Match m in matchs)
        {
            ret = ret.Replace(m.Value, step.ToString());
        }
        return ret;
    }
}
