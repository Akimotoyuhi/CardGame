using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// 敵グループの制御
/// </summary>
public class EnemyManager : MonoBehaviour
{
    /// <summary>敵データ</summary>
    [SerializeField] EnemyData m_enemyData;
    /// <summary>敵プレハブ</summary>
    [SerializeField] GameObject m_enemyPrefab;
    /// <summary>敵の親</summary>
    [SerializeField] Transform m_enemyParent;
    /// <summary>ターゲット</summary>
    [SerializeField] EnemiesTarget m_enemyTarget;
    /// <summary>現在出現中の全敵データ　戦闘中に使う</summary>
    private List<EnemyBase> m_enemies = new List<EnemyBase>();
    /// <summary>敵の総数。終了判定用</summary>
    private int m_enemyCount = 0;

    private void Start()
    {
        BattleManager.Instance.TurnEnd2.Subscribe(turn => EnemyTrun(turn));
        BattleManager.Instance.TurnBegin.Subscribe(turn => ActionPlan(turn));
        m_enemyTarget.Setup(this);
    }

    public void CreateEnemies(EnemyAppearanceEria eria)
    {
        List<EnemyDataBase> enemies = m_enemyData.Encount(eria);
        foreach (var item in enemies)
        {
            EnemyBase enemy = Instantiate(m_enemyPrefab, transform).GetComponent<EnemyBase>();
            enemy.transform.SetParent(m_enemyParent, false);
            enemy.SetParam(item, this);
        }
    }

    /// <summary>
    /// フィールドにいる敵を数える
    /// </summary>
    public void EnemyCount()
    {
        for (int i = 0; i < m_enemyParent.childCount; i++)
        {
            m_enemies.Add(m_enemyParent.GetChild(i).GetComponent<EnemyBase>());
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
    /// 敵全体に効果のあるカードが使われた場合に呼ばれる
    /// </summary>
    /// <param name="card"></param>
    public void AllEnemiesDamage(int power, int block, List<Condition> conditions)
    {
        for (int i = 0; i < m_enemies.Count; i++)
        {
            if (m_enemies[i].IsDead) continue;
            m_enemies[i].GetDamage(power, block, conditions);
        }
    }

    /// <summary>
    /// 敵の行動予定を表示させる
    /// </summary>
    /// <param name="turn"></param>
    private void ActionPlan(int turn)
    {
        for (int i = 0; i < m_enemies.Count; i++)
        {
            if (m_enemies[i].IsDead) continue;
            m_enemies[i].ActionPlan(turn);
        }
    }

    /// <summary>
    /// 敵がダメージを食らった時の呼ばれる<br/>
    /// 行動予定の更新用
    /// </summary>
    public void EnemyDamaged()
    {
        ActionPlan(BattleManager.Instance.CurrentTrun);
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
            for (int i = 0; i < m_enemyParent.childCount; i++)
            {
                Destroy(m_enemyParent.GetChild(i).gameObject);
            }
            Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(i => { BattleManager.Instance.BattleEnd(); }).AddTo(this);
        }
    }
}
