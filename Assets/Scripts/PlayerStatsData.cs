using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStatsData : ScriptableObject
{
    [SerializeField] string m_name;
    [SerializeField] int m_maxHp;
    [SerializeField, Tooltip("初期カード")] CardID[] m_cardID;

    public string Name { get { return m_name; } }
    public int HP { get { return m_maxHp; } }
    public int GetCard(int index) { return (int)m_cardID[index]; }
    public int GetCardLength { get { return m_cardID.Length; } }
}
