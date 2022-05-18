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
    private bool m_isActive;

    /// <summary>
    /// 表示テキストの設定
    /// </summary>
    /// <param name="discarding"></param>
    /// <param name="ethereal"></param>
    /// <param name="conditions"></param>
    public void SetText(bool discarding, bool ethereal, List<Condition> conditions)
    {
        string s = string.Empty;
        if (discarding)
            s += m_discardingTooltip + "\n";
        if (ethereal)
            s += m_etherealTooltip + "\n";
        foreach (var c in conditions)
            s += c.Tooltip + "\n";
        if (s.Length > 0)//テキストの長さが0なら表示しない
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
        if (value)
            m_background.SetActive(m_isActive);
        else
            m_background.SetActive(value);
    }
}
