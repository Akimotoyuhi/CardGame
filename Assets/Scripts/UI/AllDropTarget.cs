using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵でもプレイヤー宛てでもないカードのドロップを受け取るクラス
/// </summary>
public class AllDropTarget : MonoBehaviour, IDrop
{
    public bool CanDrop(UseType useType)
    {
        if (useType == UseType.System) return true;
        return false;
    }

    public void GetDrop(List<int[]> card)
    {
        BattleManager.Instance.DropManager.CardExecute(card);
    }

    public void OnCard(UseType? useType)
    {
        if (useType == UseType.System)
        {
            Debug.Log("System");
        }
    }
}
