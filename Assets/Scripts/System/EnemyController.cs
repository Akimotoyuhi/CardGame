using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private List<EnemyBase> m_enemies = new List<EnemyBase>();

    void Start()
    {
        EnemyCount();
    }

    private void EnemyCount()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            m_enemies.Add(transform.GetChild(i).GetComponent<EnemyBase>());
        }
    }

    public void EnemyTrun(int turn)
    {
        for (int i = 0; i < m_enemies.Count; i++)
        {
            m_enemies[i].Action(turn);
        }
    }
}
