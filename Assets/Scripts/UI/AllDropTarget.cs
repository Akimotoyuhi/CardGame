using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵でもプレイヤー宛てでもないカードのドロップを受け取るクラス
/// </summary>
public class AllDropTarget : MonoBehaviour, IDrop
{
    /// <summary>ドロップ可能箇所を示すフレーム</summary>
    [SerializeField] GameObject m_flame;

    private void Start()
    {
        m_flame.SetActive(false);
    }

    public bool CanDrop(UseType useType)
    {
        if (useType == UseType.System) return true;
        return false;
    }

    public void GetDrop(List<int[]> card)
    {
        BattleManager.Instance.CommandManager.CommandExecute(card, true);
    }

    public void OnCard(UseType? useType)
    {
        if (useType == UseType.System) m_flame.SetActive(true);
        else m_flame.SetActive(false);
    }
    public EnemyBase IsEnemy()
    {
        return null;
    }
}
