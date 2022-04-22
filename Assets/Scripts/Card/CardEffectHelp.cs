using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>カード効果の詳細を表示するためのクラス</summary>
public class CardEffectHelp : MonoBehaviour
{
    [SerializeField] Text m_text;
    [SerializeField] GameObject m_background;
    /// <summary>廃棄の説明</summary>
    [SerializeField, TextArea] string m_discardingTooltip;
    /// <summary>エセリアルの説明</summary>
    [SerializeField, TextArea] string m_etherealTooltip;
    [SerializeField] ConditionUI m_conditionUI;
    private bool m_isActive;

    public void SetText(bool discarding, bool ethereal)
    {
        string s = string.Empty;
        if (discarding)
            s += m_discardingTooltip + "\n";
        if (ethereal)
            s += m_etherealTooltip + "\n";
        //foreach (var c in conditions)
        //    m_text.text += m_conditionUI.GetTooltip(c.GetConditionID()) + "\n";
        if (s.Length > 0)
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
    /// ヘルプテキストの表示非表示
    /// </summary>
    /// <param name="value"></param>
    public void SetActive(bool value)
    {
        if (value)//テキストの長さが0なら表示しない
            m_background.SetActive(m_isActive);
        else
            m_background.SetActive(value);
    }
}
