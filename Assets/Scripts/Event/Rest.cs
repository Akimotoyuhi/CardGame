using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �x�e�}�X�̏���<br/>
/// �C�x���g�o����������C������
/// </summary>
public class Rest : MonoBehaviour
{
    /// <summary>�񕜗�</summary>
    [SerializeField] int m_healValue;
    [SerializeField] string m_cardUpgradeText;
    [SerializeField, Tooltip("�񕜗ʂ̕�����{heal}�ƋL��")] string m_restText;
    [SerializeField] Text m_text;
    [SerializeField] Canvas m_canvas;
    /// <summary>�A�b�v�O���[�h������J�[�h�̃C���f�b�N�X�ۑ��p</summary>
    private int m_upgradeCardIndex;

    public void StartEvent()
    {
        m_canvas.enabled = true;
        TextReset();
    }

    /// <summary>
    /// �񕜃{�^���������ꂽ����<br/>
    /// Button����Ă΂�鎖��z�肵�Ă���
    /// </summary>
    public void HealButton()
    {
        Debug.Log($"Player�̗̑͂�{m_healValue}�񕜂���");
        GameManager.Instance.Heal = m_healValue;
        GameManager.Instance.FloorFinished();
        TextReset();
    }

    /// <summary>�e�L�X�g�X�V</summary>
    private void TextUpdate(string text)
    {
        m_text.text = text;
    }

    /// <summary>
    /// �����{�^���������ꂽ����<br/>
    /// Button����Ă΂�鎖��z�肵�Ă���
    /// </summary>
    public void UpgradeButton()
    {
        Debug.Log("�J�[�h����");
        GameManager.Instance.DisplayCard(this);
        TextReset();
    }

    /// <summary>
    /// �A�b�v�O���[�h�Ώۂ̃J�[�h�̃N���b�N���󂯎��
    /// </summary>
    /// <param name="index"></param>
    public void OnUpgrade(int index)
    {
        m_upgradeCardIndex = index;
        GameManager.Instance.UpgradeConfirmationPanel(index);
    }

    /// <summary>
    /// �A�b�v�O���[�h�̊m��p�{�^���������ꂽ�Ƃ��ɌĂ΂��
    /// </summary>
    public void UpgradeApplyButton()
    {
        GameManager.Instance.CardUpgrade(m_upgradeCardIndex);
        GameManager.Instance.UpgradeConfirmationPanelDisabled();
        GameManager.Instance.CrearCardDisplayPanel();
        GameManager.Instance.FloorFinished();
    }

    /// <summary>
    /// �A�b�v�O���[�h�̖߂�{�^���������ꂽ�Ƃ��ɌĂ΂��
    /// </summary>
    public void UpgradeBackButton()
    {
        GameManager.Instance.UpgradeConfirmationPanelDisabled();
    }

    /// <summary>�x�e�{�^���Ƀ|�C���^������������<br/>EventSystem����Ă΂��</summary>
    public void RestButtonOnPointer()
    {
        string s = m_restText.Replace("{heal}", m_healValue.ToString());
        TextUpdate(s);
    }

    /// <summary>�J�[�h�����{�^���Ƀ|�C���^������������<br/>EventSystem����Ă΂��</summary>
    public void CardUpgradeButtonOnPointer()
    {
        TextUpdate(m_cardUpgradeText);
    }

    /// <summary>�e�L�X�g��\��</summary>
    public void TextReset()
    {
        m_text.text = "";
    }
}
