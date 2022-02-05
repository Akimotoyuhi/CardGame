using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStatsData : ScriptableObject
{
    [SerializeField] string m_name;
    [SerializeField] Sprite m_image;
    [SerializeField] int m_maxHp;
    [SerializeField] List<CardData> m_cardData;
    [System.Serializable]
    public class CardData
    {
        [SerializeField, Tooltip("開始時に所持しているカードID")] CardID m_cardID;
        [SerializeField, Tooltip("アップグレードが何度されているか")] int m_upgrade;
        public CardID CardID => m_cardID;
        public int Upgrade => m_upgrade;
    }
    public string Name => m_name;
    public Sprite Image => m_image;
    public int HP => m_maxHp;
    public int GetCardData(int index) => (int)m_cardData[index].CardID;
    public int IsUpgrade(int index) => m_cardData[index].Upgrade;
    public int GetCardLength => m_cardData.Count;
}
