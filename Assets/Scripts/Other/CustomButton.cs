using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour
{
    [SerializeField] Image m_icon;
    [SerializeField] Image m_subIcon;
    [SerializeField] GameObject m_mask;
    [SerializeField] Text m_text;
    /// <summary>�J�X�^���������̃e�L�X�g</summary>
    [SerializeField] string m_conflictText;
    /// <summary>���ɑI���ςݎ��̃e�L�X�g</summary>
    [SerializeField] string m_selectedText;
    private CustomModeDataBase m_database;
    /// <summary>�I���ς݃t���O</summary>
    private bool m_isSelected;
    CustomMode m_manager;

    public void Setup(CustomModeDataBase database, CustomMode manager)
    {
        m_database = database;
        m_icon.SetImage(database.IconColor, database.IconSprite);
        m_subIcon.SetImage(database.SubIconColor, database.SubIconSprite);
        m_manager = manager;
        m_text.text = "";
        m_mask.SetActive(false);
    }

    /// <summary>
    /// Unity�̃{�^������Ă΂�鎖��z�肵�Ă���
    /// </summary>
    public void OnClick()
    {
        m_isSelected = true;
        StateChange();
        m_manager.OnClick(m_database);
    }

    /// <summary>
    /// �t���O�����ă{�^���̌����ڂ�ω�������
    /// </summary>
    private void StateChange()
    {
        if (m_isSelected)
        {
            m_text.text = m_selectedText;
        }
    }
}
