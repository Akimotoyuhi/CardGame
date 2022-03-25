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
    /// <summary>�I�𒆂̃J�X�^����\������e�L�X�g�v���n�u</summary>
    [SerializeField] Text m_customPrevText;
    /// <summary>�J�X�^���f�[�^</summary>
    [SerializeField] CustomModeData m_customModeData;
    /// <summary>�J�X�^���{�^���̐e</summary>
    [SerializeField] Transform m_customParent;
    /// <summary>�I�𒆂̃J�X�^���̐e</summary>
    [SerializeField] Transform m_customTextParent;
    /// <summary>���ݑI������Ă���J�X�^��</summary>
    private List<CustomModeDataBase> m_selectCustomList = new List<CustomModeDataBase>();
    private List<CustomButton> m_customButtons = new List<CustomButton>();
    private TitleManager m_titleManager;

    public void Setup(TitleManager titleManager)
    {
        m_titleManager = titleManager;
        for (int i = 0; i < m_customModeData.DataBases.Count; i++)
        {
            CustomButton cb = Instantiate(m_customPrefab);
            cb.transform.SetParent(m_customParent);
            cb.Setup(m_customModeData.DataBases[i], this);
        }
    }

    /// <summary>
    /// CustomButton�̃N���b�N���󂯎��
    /// </summary>
    /// <param name="database"></param>
    public void OnClick(CustomModeDataBase database)
    {
        for (int i = 0; i < m_selectCustomList.Count; i++)
        {
            if (m_selectCustomList[i].Name == database.Name)
            {
                Debug.Log(database.Name + "���폜");
                m_selectCustomList.RemoveAt(i);
                SelectCustomListUpdate();
                return;
            }
        }
        Debug.Log(database.Name + "��ǉ�");
        m_selectCustomList.Add(database);
        SelectCustomListUpdate();
    }

    /// <summary>
    /// �I�𒆂̃J�X�^�������X�g�ɔ��f������
    /// </summary>
    private void SelectCustomListUpdate()
    {
        for (int i = 0; i < m_customTextParent.childCount; i++)
        {
            Destroy(m_customTextParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < m_selectCustomList.Count; i++)
        {
            Text t = Instantiate(m_customPrevText);
            t.text = $"{m_selectCustomList[i].Tooltip}(+{m_selectCustomList[i].Point})";
            t.transform.SetParent(m_customTextParent);
        }
    }

    /// <summary>
    /// �J�X�^���̑I�����I�����Ď��̉�ʂɐi�ރ{�^�����N���b�N������<br/>
    /// Unity�̃{�^������Ă΂�鎖��z�肵�Ă���
    /// </summary>
    public void OnSelectEndButtonClick()
    {
        m_titleManager.StateChange(TitleState.SceneChange);
    }
}
