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
    /// <summary>カスタム競合時のテキスト</summary>
    [SerializeField] string m_conflictText;
    /// <summary>既に選択済み時のテキスト</summary>
    [SerializeField] string m_selectedText;
    private CustomModeDataBase m_database;
    /// <summary>選択済みフラグ</summary>
    private bool m_isSelected;
    /// <summary>競合フラグ</summary>
    private bool m_isConflict;
    CustomMode m_manager;
    public bool IsSelected { set => m_isSelected = value; }
    public bool IsConflict { set => m_isConflict = value; }

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
    /// Unityのボタンから呼ばれる事を想定している
    /// </summary>
    public void OnClick()
    {
        if (m_isConflict) return;
        if (m_isSelected)
            m_isSelected = false;
        else
            m_isSelected = true;
        //AppearanceChange();
        m_manager.OnClick(m_database, m_isConflict);
        //m_isConflict = false;
    }

    /// <summary>
    /// フラグを見てボタンの見た目を変化させる
    /// </summary>
    private void AppearanceChange()
    {
        if (m_isConflict)
        {
            m_mask.SetActive(true);
            m_text.text = m_conflictText;
            m_isSelected = false;
            return;
        }
        if (m_isSelected)
        {
            m_mask.SetActive(true);
            m_text.text = m_selectedText;
            m_isConflict = false;
            return;
        }
        m_mask.SetActive(false);
        m_text.text = "";
    }

    /// <summary>
    /// 選択されたIDを受け取って自分のIDと同じかを評価する
    /// </summary>
    public void SelectedIDCheck(CustomID id, bool isRemoved)
    {
        if (id == m_database.CustomID)
        {
            if (m_isSelected)
            {
                AppearanceChange();
                return;
            }
            if (isRemoved)
            {
                m_isConflict = false;
            }
            else
            {
                m_isConflict = true;
            }
        }
        AppearanceChange();
    }
}
