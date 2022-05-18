using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>�J�[�h���ʂ̏ڍׂ�\�����邽�߂̃N���X</summary>
public class CardEffectHelp : MonoBehaviour
{
    [SerializeField] Text m_text;
    [SerializeField] GameObject m_background;
    /// <summary>�p���̐���</summary>
    [SerializeField, TextArea] string m_discardingTooltip;
    /// <summary>�G�Z���A���̐���</summary>
    [SerializeField, TextArea] string m_etherealTooltip;
    private bool m_isActive;

    /// <summary>
    /// �\���e�L�X�g�̐ݒ�
    /// </summary>
    /// <param name="discarding"></param>
    /// <param name="ethereal"></param>
    /// <param name="conditions"></param>
    public void SetText(bool discarding, bool ethereal, List<Condition> conditions)
    {
        string s = string.Empty;
        if (discarding)
            s += m_discardingTooltip + "\n";
        if (ethereal)
            s += m_etherealTooltip + "\n";
        foreach (var c in conditions)
            s += c.Tooltip + "\n";
        if (s.Length > 0)//�e�L�X�g�̒�����0�Ȃ�\�����Ȃ�
        {
            m_text.text = s;
            m_isActive = true;
        }
        else
        {
            m_isActive = false;
        }
    }

    /// <summary>
    /// �w���v�e�L�X�g�̕\����\��
    /// </summary>
    /// <param name="value"></param>
    public void SetActive(bool value)
    {
        if (value)
            m_background.SetActive(m_isActive);
        else
            m_background.SetActive(value);
    }
}
