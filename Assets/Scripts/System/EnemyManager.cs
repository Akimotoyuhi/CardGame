using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵グループの制御
/// </summary>
public class EnemyManager : MonoBehaviour
{
    private List<EnemyBase> m_enemies = new List<EnemyBase>();
    /// <summary>敵の総数。終了判定用</summary>
    private int m_enemyCount = 0;

    void Start()
    {
        EnemyCount();
    }

    /// <summary>
    /// フィールドにいる敵を数える
    /// </summary>
    public void EnemyCount()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            m_enemies.Add(transform.GetChild(i).GetComponent<EnemyBase>());
            m_enemyCount++;
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
            if (m_enemies[i].IsDead) continue;
            m_enemies[i].TurnStart();
            m_enemies[i].Action(turn);
            m_enemies[i].TurnEnd();
        }
    }

    /// <summary>
    /// 終了判定用<br/>
    /// 敵が死んだ時に呼ばれる
    /// </summary>
    public void Removed()
    {
        m_enemyCount--;
        if (m_enemyCount <= 0)
        {
            Debug.Log("勝利");
        }
    }
}
