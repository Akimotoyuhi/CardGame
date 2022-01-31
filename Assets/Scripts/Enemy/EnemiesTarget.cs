using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesTarget : MonoBehaviour, IDrop
{
    private EnemyManager m_enemyManager = default;

    public void Setup(EnemyManager enemyManager)
    {
        m_enemyManager = enemyManager;
    }

    public void GetDrop(BlankCard card)
    {
        if (!card || card.GetCardType != UseType.ToAllEnemies) return;
        card.OnCast();
        m_enemyManager.AllEnemiesDamage(card);
    }
}
