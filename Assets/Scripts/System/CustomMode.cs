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
    /// <summary>���v�댯�x��\������e�L�X�g</summary>
    [SerializeField] Text m_totalRiskViewText;
    /// <summary>���v�댯�x��\������e�L�X�g�ɕ\�����镶����</summary>
    [SerializeField, Tooltip("���v�댯�x��\������e�L�X�g�ɕ\�����镶����\n�ϐ��̕�����{risk}�ƋL�q���邱��")]
    string m_totalRiskText;
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
            m_customButtons.Add(cb);
        }
        TotalRiskTextUpdate(0);
    }

    /// <summary>
    /// CustomButton�̃N���b�N���󂯎��
    /// </summary>
    /// <param name="database"></param>
    public void OnClick(CustomModeDataBase database, bool IsConflict)
    {
        for (int i = 0; i < m_selectCustomList.Count; i++)
        {
            if (m_selectCustomList[i].Name == database.Name)
            {
                Debug.Log(database.Name + "���폜");
                m_selectCustomList.RemoveAt(i);
                UpdateButtons(database, true);
                return;
            }
        }
        if (IsConflict)
        {
            for (int i = 0; i < m_selectCustomList.Count; i++)
            {
                if (database.CustomID == m_selectCustomList[i].CustomID)
                {
                    m_selectCustomList.RemoveAt(i);
                    break;
                }
            }
        }
        Debug.Log(database.Name + "��ǉ�");
        m_selectCustomList.Add(database);
        UpdateButtons(database, false);
    }

    /// <summary>
    /// �{�^���̏��̍X�V
    /// </summary>
    private void UpdateButtons(CustomModeDataBase database, bool isRemoved)
    {
        SelectCustomListUpdate();
        foreach (var cb in m_customButtons)
        {
            cb.SelectedIDCheck(database.CustomID, isRemoved);
        }
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
        int totalRisk = 0;
        for (int i = 0; i < m_selectCustomList.Count; i++)
        {
            Text t = Instantiate(m_customPrevText);
            t.text = $"{m_selectCustomList[i].Tooltip}(+{m_selectCustomList[i].Point})";
            t.transform.SetParent(m_customTextParent);
            totalRisk += m_selectCustomList[i].Point;
        }
        TotalRiskTextUpdate(totalRisk);
    }

    /// <summary>
    /// ���v�댯�x���󂯎����m_totalRiskViewText���X�V����
    /// </summary>
    /// <param name="risk"></param>
    private void TotalRiskTextUpdate(int risk)
    {
        string s = m_totalRiskText.Replace("{risk}", risk.ToString());
        m_totalRiskViewText.text = s;
        if (risk > 0)
            m_totalRiskViewText.color = Color.red;
        else
            m_totalRiskViewText.color = Color.black;
    }
    /// <summary>
    /// �J�X�^���̑I�����I�����Ď��̉�ʂɐi�ރ{�^�����N���b�N������<br/>
    /// Unity�̃{�^������Ă΂�鎖��z�肵�Ă���
    /// </summary>
    public void OnSelectEndButtonClick()
    {
        m_titleManager.SaveCustomList(m_selectCustomList);
        m_titleManager.StateChange(TitleState.SceneChange);
    }
}
