using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStatsData : ScriptableObject
{
    [SerializeField] string m_name;
    [SerializeField] Sprite m_image;
    [SerializeField] int m_maxHp;
    [SerializeField, Tooltip("初期カード")] CardID[] m_cardID;

    public string Name { get => m_name; }
    public Sprite Image { get => m_image; }
    public int HP { get => m_maxHp; }
    public int GetCard(int index) { return (int)m_cardID[index]; }
    public int GetCardLength { get { return m_cardID.Length; } }
}
