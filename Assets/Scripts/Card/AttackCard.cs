using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵に対して何かするカードのクラス
/// </summary>
public class AttackCard : CardBase, IAttackCard
{
    /// <summary>与えるダメージ</summary>
    [SerializeField] private int m_damage = 1;
    private void Start()
    {
        SetUp();
    }

    public int GetDamage()
    {
        OnCast();
        return m_damage;
    }
}
