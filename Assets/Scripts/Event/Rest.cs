using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �x�e�}�X�̏���<br/>
/// �C�x���g�o����������C������
/// </summary>
public class Rest : MonoBehaviour
{
    /// <summary>�񕜗�</summary>
    [SerializeField] int m_healValue;
    /// <summary>�A�b�v�O���[�h������J�[�h�̃C���f�b�N�X�ۑ��p</summary>
    private int m_upgradeCardIndex;

    public void StartEvent()
    {
        GetComponent<Canvas>().enabled = true;
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
    }

    /// <summary>
    /// �����{�^���������ꂽ����<br/>
    /// Button����Ă΂�鎖��z�肵�Ă���
    /// </summary>
    public void UpgradeButton()
    {
        Debug.Log("�J�[�h����");
        GameManager.Instance.DisplayCard(this);
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
        GameManager.Instance.FloorFinished();
    }
}
