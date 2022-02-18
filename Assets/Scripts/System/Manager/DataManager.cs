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
        public int Floor { get; set; } = 0;
        public int Act { get; set; } = 1;

        //プレイヤー情報関連
        /// <summary>所持カード</summary>
        private List<int[]> m_cards = new List<int[]>();
        /// <summary>プレイヤーの名前</summary>
        public string Name { get; private set; }
        /// <summary>通常時の画像</summary>
        public Sprite IdleSprite { get; private set; }
        /// <summary>ゲームオーバー時の画像</summary>
        public Sprite GameoverSprite { get; private set; }
        /// <summary>プレイヤーの現在体力</summary>
        public int CurrentLife
        {
            get => m_currentLife;
            set
            {
                m_currentLife = value;
                if (m_currentLife > MaxLife)
                {
                    m_currentLife = MaxLife;
                }
            }
        }
        /// <summary>プレイヤーの最大体力</summary>
        public int MaxLife { get; set; }
        /// <summary>
        /// プレイヤーのステータスを保存しておく
        /// </summary>
        public void SavePlayerState(string name, Sprite idleSprite, Sprite gameoverSprite, int maxLife, int currentLife)
        {
            Name = name;
            IdleSprite = idleSprite;
            GameoverSprite = gameoverSprite;
            MaxLife = maxLife;
            CurrentLife = currentLife;
            IsPlayerData = true;
        }
        //public List<CardID> Cards { get => m_cards; set => m_cards = value; }
        public List<int[]> Cards { get => m_cards; }
        public void AddCards(int id, int isUpgrade)
        {
            int[] vs = new int[] { id, isUpgrade };
            m_cards.Add(vs);
        }
        /// <summary>
        /// カードアップグレード<br/>
        /// 後に変える
        /// </summary>
        /// <param name="index"></param>
        public void CardUpgrade(int index)
        {
            m_cards[index][1] = 1;
        }
        public bool IsPlayerData { get; private set; } = false;
    }
}