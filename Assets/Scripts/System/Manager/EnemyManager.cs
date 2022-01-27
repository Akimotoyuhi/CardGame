using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 敵グループの制御
/// </summary>
public class EnemyManager : MonoBehaviour
{
    /// <summary>敵データ</summary>
    //[SerializeField] EnemyData m_enemydata;
    [SerializeField] EnemyData m_enemyData;
    private EnemyDataBase m_enemyDatabase;
    /// <summary>敵プレハブ</summary>
    [SerializeField] GameObject m_enemyPrefab;
    /// <summary>現在出現中の全敵データ　戦闘中に使う</summary>
    private List<EnemyBase> m_enemies = new List<EnemyBase>();
    /// <summary>敵の総数。終了判定用</summary>
    private int m_enemyCount = 0;

    private void Start()
    {
        BattleManager.Instance.TurnEnd2.Subscribe(turn => EnemyTrun(turn));
        BattleManager.Instance.TurnBegin.Subscribe(turn => ActionPlan(turn));
    }

    public void CreateEnemies(CellState cellstate)
    {
        EnemyDataBase enemy = default;
        switch (cellstate)
        {
            case CellState.Enemy:
                enemy = m_enemyData.Act1Enemy[Random.Range(0, m_enemyData.Act1Enemy.Count)];
                break;
            case CellState.Boss:
                enemy = m_enemyData.Act1Boss[Random.Range(0, m_enemyData.Act1Boss.Count)];
                break;
        }
        Transform tra = Instantiate(m_enemyPrefab, transform).transform;
        tra.SetParent(transform, false);
        EnemyBase e = tra.GetComponent<EnemyBase>();
        e.SetParam(enemy);
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

    private void ActionPlan(int turn)
    {
        for (int i = 0; i < m_enemies.Count; i++)
        {
            if (m_enemies[i].IsDead) continue;
            m_enemies[i].ActionPlan(turn);
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
            BattleManager.Instance.BatlteEnd();
        }
    }
}
