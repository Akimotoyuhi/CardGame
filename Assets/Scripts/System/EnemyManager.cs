using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 敵グループの制御
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameManager m_gamemanager;
    /// <summary>敵データ</summary>
    //[SerializeField] EnemyData m_enemydata;
    [SerializeField] EnemyData3 m_enemyData;
    private EnemyDataBase3 m_enemyDatabase;
    /// <summary>敵プレハブ</summary>
    [SerializeField] GameObject m_enemyPrefab;
    private List<EnemyBase> m_enemies = new List<EnemyBase>();
    /// <summary>敵の総数。終了判定用</summary>
    private int m_enemyCount = 0;

    //public void CreateEnemies(int id)
    //{
    //    m_enemyDatabase = m_enemydata.m_enemyDatas[id];
    //    Transform tra = Instantiate(m_enemyPrefab, transform).transform;
    //    tra.SetParent(transform, false);
    //    EnemyBase e = tra.GetComponent<EnemyBase>();
    //    e.SetParam(m_enemyDatabase.Name, m_enemyDatabase.Image, m_enemyDatabase.HP, m_enemyDatabase.SetAction());
    //}
    public void CreateEnemies(int id)
    {
        m_enemyDatabase = m_enemyData.m_enemyDataBase3s[id];
        Transform tra = Instantiate(m_enemyPrefab, transform).transform;
        tra.SetParent(transform, false);
        EnemyBase e = tra.GetComponent<EnemyBase>();
        e.SetParam(m_enemyDatabase);
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

    private void Start()
    {
        BattleManager.Instance.TurnBegin.Subscribe(turn =>
        {
            for (int i = 0; i < m_enemies.Count; i++)
            {
                if (m_enemies[i].IsDead) continue;
                m_enemies[i].TurnStart();
                m_enemies[i].Action(turn);
                m_enemies[i].TurnEnd();
            }
        });
    }

    /// <summary>
    /// 敵のターン
    /// </summary>
    /// <param name="turn">現在ターン数</param>
    //public void EnemyTrun(int turn)
    //{
    //    for (int i = 0; i < m_enemies.Count; i++)
    //    {
    //        if (m_enemies[i].IsDead) continue;
    //        m_enemies[i].TurnStart();
    //        m_enemies[i].Action(turn);
    //        m_enemies[i].TurnEnd();
    //    }
    //}

    /// <summary>
    /// 終了判定用<br/>
    /// 敵が死んだ時に呼ばれる
    /// </summary>
    public void Removed()
    {
        m_enemyCount--;
        if (m_enemyCount <= 0)
        {
            m_gamemanager.BattleEnd();
        }
    }
}
