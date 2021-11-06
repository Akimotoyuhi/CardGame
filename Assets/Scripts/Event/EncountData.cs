using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class EncountData : ScriptableObject
{
    public List<EncountDataBase> m_data = new List<EncountDataBase>();
}

[Serializable]
public class EncountDataBase
{
    [SerializeField] EnemyID[] m_enemyID = new EnemyID[(int)EnemyID.endLength];

    public int GetID(int index) { return (int)m_enemyID[index]; } //バグりそう
    public int GetLength { get => m_enemyID.Length; }
}
