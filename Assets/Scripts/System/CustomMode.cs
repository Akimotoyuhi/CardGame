using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �`�������W���[�h�̊Ǘ�
/// </summary>
public class CustomMode : MonoBehaviour
{
    /// <summary>�e�J�X�^���{�^���̃v���n�u</summary>
    [SerializeField] CustomButton m_customPrefab;
    /// <summary>�J�X�^���f�[�^</summary>
    [SerializeField] CustomModeData m_customModeData;
    /// <summary>�J�X�^���{�^���̐e</summary>
    [SerializeField] Transform m_customParent;
    /// <summary>���ݑI������Ă���J�X�^��</summary>
    private List<CustomModeDataBase> m_sellectCustomList = new List<CustomModeDataBase>();

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        for (int i = 0; i < m_customModeData.DataBases.Count; i++)
        {
            CustomButton cb = Instantiate(m_customPrefab);
            cb.transform.SetParent(m_customParent);
            cb.Setup(m_customModeData.DataBases[i]);
        }
    }
}
