using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 休憩マスの処理<br/>
/// イベント出来たら消す気がする
/// </summary>
public class Rest : MonoBehaviour
{
    /// <summary>回復量</summary>
    [SerializeField] int m_healValue;
    /// <summary>アップグレードさせるカードのインデックス保存用</summary>
    private int m_upgradeCardIndex;

    public void StartEvent()
    {
        GetComponent<Canvas>().enabled = true;
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
    }

    /// <summary>
    /// 強化ボタンが押された処理<br/>
    /// Buttonから呼ばれる事を想定している
    /// </summary>
    public void UpgradeButton()
    {
        Debug.Log("カード強化");
        GameManager.Instance.DisplayCard(this);
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
        GameManager.Instance.FloorFinished();
    }
}
