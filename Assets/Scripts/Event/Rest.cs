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

    public void StartEvent()
    {
        GetComponent<Canvas>().enabled = true;
    }

    public void HealButton()
    {
        Debug.Log($"Player�̗̑͂�{m_healValue}�񕜂���");
        GameManager.Instance.Heal = m_healValue;
        GameManager.Instance.FloorFinished();
    }

    public void UpgradeButton()
    {
        Debug.Log("�J�[�h����");
        GameManager.Instance.DisplayCard(this);
    }

    public void OnUpgrade(int index)
    {
        GameManager.Instance.CardUpgrade(index);
        GameManager.Instance.FloorFinished();
    }
}
