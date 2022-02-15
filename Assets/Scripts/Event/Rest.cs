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

    public void OnUpgrade(int index)
    {
        GameManager.Instance.CardUpgrade(index);
        GameManager.Instance.FloorFinished();
    }
}
