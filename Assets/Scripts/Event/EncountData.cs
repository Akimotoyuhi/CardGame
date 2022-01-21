using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class EncountData : ScriptableObject
{
    [SerializeField] List<EncountDataBase> m_enemyData = new List<EncountDataBase>();
    public EncountDataBase Encount(int index) => m_enemyData[index];
}

[Serializable]
public class EncountDataBase
{
    [SerializeField] EnemyID[] m_enemyID = new EnemyID[(int)EnemyID.endLength];

    /// <summary>
    /// 敵IDの取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int GetID(int index) { return (int)m_enemyID[index]; }
    public int GetLength { get => m_enemyID.Length; }
}
