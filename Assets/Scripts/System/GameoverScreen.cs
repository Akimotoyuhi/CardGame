using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameoverScreen : MonoBehaviour
{
    /// <summary>ゲームオーバー画面</summary>
    [SerializeField] GameObject m_panel;
    /// <summary>ゲームオーバー時に書き換えるテキスト</summary>
    [SerializeField] Text m_text;
    /// <summary>テキスト表示までの時間</summary>
    [SerializeField] float m_textDuration;
    /// <summary>ゲームオーバー時に表示される文字列</summary>
    [SerializeField, TextArea, Tooltip("所持カードは{haveCard}\n進行度は{step}\nって書いてね")] string m_gameoverText;
    /// <summary>ゲームクリア時に表示される文字列</summary>
    [SerializeField, TextArea, Tooltip("所持カードは{haveCard}\n進行度は{step}\n危険度は{risk}\nって書いてね")] string m_gamecrearText;
    [SerializeField] GameObject m_retryButton;
    [SerializeField] GameObject m_titleButton;

    private void Start()
    {
        m_panel.SetActive(false);
        m_retryButton.SetActive(false);
        m_titleButton.SetActive(false);
    }
    /// <summary>
    /// ゲームオーバー画面の表示
    /// </summary>
    /// <param name="haveCardNum">所持カード枚数</param>
    /// <param name="step">ゲーム進行度</param>
    public void ShowPanel(int haveCardNum, int step, bool isCrear)
    {
        m_panel.SetActive(true);
        m_text.text = default;
        m_text.DOText(SetGameoverText(haveCardNum, step, isCrear), m_textDuration).
            OnComplete(() =>
            {
                m_retryButton.SetActive(true);
                m_titleButton.SetActive(true);
            });
    }
    /// <summary>
    /// ゲームオーバー時のテキストの置き換え
    /// </summary>
    /// <param name="haveCardNum">所持カード枚数</param>
    /// <param name="step">ゲーム進行度</param>
    /// <returns>置き換え後のテキスト</returns>
    private string SetGameoverText(int haveCardNum, int step, bool isCrear)
    {
        string ret;
        if (isCrear) ret = m_gamecrearText;
        else ret = m_gameoverText;
        ret = ret.Replace("{haveCard}", haveCardNum.ToString());
        ret = ret.Replace("{step}", step.ToString());
        ret = ret.Replace("{risk}", GameManager.Instance.Risk.ToString());
        return ret;
    }
}
