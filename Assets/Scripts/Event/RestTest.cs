using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �x�e�}�X�̏���<br/>
/// �C�x���g�o���������
/// </summary>
public class RestTest : MonoBehaviour
{
    /// <summary>�񕜗�</summary>
    [SerializeField] int m_healValue;

    public void StartEvent()
    {
        GetComponent<Canvas>().enabled = true;
    }

    public void OnClick()
    {
        Debug.Log($"Player�̗̑͂�{m_healValue}�񕜂���");
        GameManager.Instance.Heal = m_healValue;
        GameManager.Instance.FloorFinished();
    }
}
