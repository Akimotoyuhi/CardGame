using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵グループの制御
/// </summary>
public class EnemyController : MonoBehaviour
{
    private List<EnemyBase> m_enemies = new List<EnemyBase>();

    void Start()
    {
        EnemyCount();
    }

    /// <summary>
    /// フィールドにいる敵を数える
    /// </summary>
    private void EnemyCount()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            m_enemies.Add(transform.GetChild(i).GetComponent<EnemyBase>());
        }
    }

    /// <summary>
    /// 敵のターン
    /// </summary>
    /// <param name="turn">現在ターン数</param>
    public void EnemyTrun(int turn)
    {
        for (int i = 0; i < m_enemies.Count; i++)
        {
            m_enemies[i].TurnStart();
            m_enemies[i].Action(turn);
            m_enemies[i].TurnEnd();
        }
    }
}
