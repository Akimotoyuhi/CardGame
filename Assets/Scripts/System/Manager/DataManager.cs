using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mastar
{
    public class DataManager
    {
        private static DataManager m_instance = new DataManager();
        public static DataManager Instance => m_instance;
        private int m_currentLife = 0;
        /// <summary>現在の進行状況</summary>
        public int Floor { get; set; }
        #region プレイヤー情報関連
        /// <summary>所持カード</summary>
        private List<int[]> m_cards = new List<int[]>();
        private List<RelicID> m_relics = new List<RelicID>();
        /// <summary>プレイヤーの名前</summary>
        public string Name { get; private set; }
        /// <summary>通常時の画像</summary>
        public Sprite IdleSprite { get; private set; }
        /// <summary>攻撃時の画像</summary>
        public Sprite[] AttackedSprite { get; private set; }
        /// <summary>ゲームオーバー時の画像</summary>
        public Sprite GameoverSprite { get; private set; }
        /// <summary>プレイヤーの現在体力</summary>
        public int CurrentLife
        {
            get => m_currentLife;
            set
            {
                m_currentLife = value;
                if (m_currentLife > MaxLife)//現在体力が最大体力を超えないように
                {
                    m_currentLife = MaxLife;
                }
            }
        }
        /// <summary>プレイヤーの最大体力</summary>
        public int MaxLife { get; set; }
        /// <summary>プレイヤーデータがあるか</summary>
        public bool IsPlayerData { get; private set; } = false;
        /// <summary>プレイヤー情報の保存</summary>
        public void SavePlayerState(string name, Sprite idleSprite, Sprite[] attackedSprite, Sprite gameoverSprite, int maxLife, int currentLife)
        {
            Name = name;
            IdleSprite = idleSprite;
            AttackedSprite = attackedSprite;
            GameoverSprite = gameoverSprite;
            MaxLife = maxLife;
            CurrentLife = currentLife;
            IsPlayerData = true;
        }
        public List<int[]> Cards { get => m_cards; }
        /// <summary>カードの追加</summary>
        /// <param name="id"></param>
        /// <param name="isUpgrade"></param>
        public void AddCards(int id, int isUpgrade)
        {
            int[] vs = new int[] { id, isUpgrade };
            m_cards.Add(vs);
        }
        /// <summary>カードアップグレード</summary>
        public void CardUpgrade(int index)
        {
            m_cards[index][1] = 1;
        }
        /// <summary>現在所持中のレリック</summary>
        public List<RelicID> HaveRelic { get => m_relics; set => m_relics = value; }
        #endregion

        #region カスタム関連
        /// <summary>選択中のカスタム</summary>
        public List<CustomModeDataBase> CustomList { get; set; }
        /// <summary>合計危険度</summary>
        public int TotalRisk { get; set; }
        #endregion

        /// <summary>データの初期化</summary>
        public void Init()
        {
            DataManager d = new DataManager();
            d.CustomList = m_instance.CustomList;
            m_instance = d;
        }
    }
}