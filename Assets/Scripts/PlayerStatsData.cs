using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStatsData : ScriptableObject
{
    [SerializeField] string m_name;
    [SerializeField] PlayerSprite m_sprite;
    [SerializeField] int m_maxHp;
    [SerializeField] List<HaveCardData> m_cardData;
    [System.Serializable]
    public class PlayerSprite
    {
        [SerializeField, Tooltip("通常時の画像")] Sprite m_idleSprite;
        [SerializeField, Tooltip("攻撃時の画像 設定順は\n斬撃\nの順")] Sprite[] m_attackedSprite;
        [SerializeField, Tooltip("ゲームオーバー時の画像")] Sprite m_gameoverSprite;
        public Sprite IdleSprite => m_idleSprite;
        public Sprite[] AttackedSprite => m_attackedSprite;
        public Sprite GameoverSprite => m_gameoverSprite;
    }
    [System.Serializable]
    public class HaveCardData
    {
        [SerializeField, Tooltip("開始時に所持しているカードID")] CardID m_cardID;
        [SerializeField, Tooltip("アップグレードが何度されているか")] int m_upgrade;
        public CardID CardID => m_cardID;
        public int Upgrade => m_upgrade;
    }
    public string Name => m_name;
    public Sprite IdleSprite => m_sprite.IdleSprite;
    public Sprite[] AttackedSprite => m_sprite.AttackedSprite;
    public Sprite GameoverSprite => m_sprite.GameoverSprite;
    public int HP => m_maxHp;
    public int GetCardData(int index) => (int)m_cardData[index].CardID;
    public int IsUpgrade(int index) => m_cardData[index].Upgrade;
    public int GetCardLength => m_cardData.Count;
}
