using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 休憩マスの処理<br/>
/// イベント出来たら消す気がする
/// </summary>
public class Rest : MonoBehaviour
{
    /// <summary>回復量</summary>
    [SerializeField] int m_healValue;
    [SerializeField] string m_cardUpgradeText;
    [SerializeField, Tooltip("回復量の部分は{heal}と記入")] string m_restText;
    [SerializeField] Text m_text;
    [SerializeField] Canvas m_canvas;
    /// <summary>アップグレードさせるカードのインデックス保存用</summary>
    private int m_upgradeCardIndex;

    public void StartEvent()
    {
        m_canvas.enabled = true;
        TextReset();
    }

    /// <summary>
    /// 回復ボタンが押された処理<br/>
    /// Buttonから呼ばれる事を想定している
    /// </summary>
    public void HealButton()
    {
        Debug.Log($"Playerの体力が{m_healValue}回復した");
        GameManager.Instance.Heal = m_healValue;
        GameManager.Instance.FloorFinished();
        TextReset();
    }

    /// <summary>テキスト更新</summary>
    private void TextUpdate(string text)
    {
        m_text.text = text;
    }

    /// <summary>
    /// 強化ボタンが押された処理<br/>
    /// Buttonから呼ばれる事を想定している
    /// </summary>
    public void UpgradeButton()
    {
        Debug.Log("カード強化");
        GameManager.Instance.DisplayCard(this);
        TextReset();
    }

    /// <summary>
    /// アップグレード対象のカードのクリックを受け取る
    /// </summary>
    /// <param name="index"></param>
    public void OnUpgrade(int index)
    {
        m_upgradeCardIndex = index;
        GameManager.Instance.UpgradeConfirmationPanel(index);
    }

    /// <summary>
    /// アップグレードの確定用ボタンが押されたときに呼ばれる
    /// </summary>
    public void UpgradeApplyButton()
    {
        GameManager.Instance.CardUpgrade(m_upgradeCardIndex);
        GameManager.Instance.UpgradeConfirmationPanelDisabled();
        GameManager.Instance.CrearCardDisplayPanel();
        GameManager.Instance.FloorFinished();
    }

    /// <summary>
    /// アップグレードの戻るボタンが押されたときに呼ばれる
    /// </summary>
    public void UpgradeBackButton()
    {
        GameManager.Instance.UpgradeConfirmationPanelDisabled();
    }

    /// <summary>休憩ボタンにポインタが当たった時<br/>EventSystemから呼ばれる</summary>
    public void RestButtonOnPointer()
    {
        string s = m_restText.Replace("{heal}", m_healValue.ToString());
        TextUpdate(s);
    }

    /// <summary>カード強化ボタンにポインタが当たった時<br/>EventSystemから呼ばれる</summary>
    public void CardUpgradeButtonOnPointer()
    {
        TextUpdate(m_cardUpgradeText);
    }

    /// <summary>テキスト非表示</summary>
    public void TextReset()
    {
        m_text.text = "";
    }
}
