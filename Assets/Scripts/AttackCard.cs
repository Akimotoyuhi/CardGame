using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵に対して何かするカードのクラス
/// </summary>
public class AttackCard : CardBase
{
    /// <summary>与えるダメージ</summary>
    [SerializeField] private int m_damage = 1;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public int GetDamage()
    {
        return m_damage;
    }
}
