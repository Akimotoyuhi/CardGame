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

    public void GetDrop(int power, int block, List<Condition> conditions, UseType useType, System.Action onCast)
    {
        if (useType != UseType.ToAll) return;
        onCast();
        m_enemyManager.AllEnemiesDamage(power, block, conditions);
    }
}
