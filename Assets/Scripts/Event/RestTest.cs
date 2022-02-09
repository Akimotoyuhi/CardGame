using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 休憩マスの処理<br/>
/// イベント出来たら消す気がする
/// </summary>
public class RestTest : MonoBehaviour
{
    /// <summary>回復量</summary>
    [SerializeField] int m_healValue;

    public void StartEvent()
    {
        GetComponent<Canvas>().enabled = true;
    }

    public void HealButton()
    {
        Debug.Log($"Playerの体力が{m_healValue}回復した");
        GameManager.Instance.Heal = m_healValue;
        GameManager.Instance.FloorFinished();
    }

    public void UpgradeButton()
    {

    }
}
