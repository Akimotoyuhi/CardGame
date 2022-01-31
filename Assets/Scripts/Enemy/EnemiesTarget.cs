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
        Debug.Log("‚¨‚¬‚á");
        if (!card || card.GetCardType != UseType.ToAllEnemies) return;
        
        //m_enemyManager.AllEnemiesDamage(card);
    }
}
