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
    public bool CanDrop(UseType useType)
    {
        if (useType == UseType.ToAll) return true;
        else return false;
    }
    public void GetDrop(List<int[]> cardCommand)
    {
        //m_enemyManager.AllEnemiesDamage(power, block, condition);
    }
}
