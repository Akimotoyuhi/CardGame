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
        public int Step { get; set; } = 0;
        public int Act { get; set; } = 1;
        //プレイヤー情報関連
        //private List<CardID> m_cards = new List<CardID>();
        private List<int[]> m_cards = new List<int[]>();
        public string Name { get; private set; }
        public Sprite Sprite { get; private set; }
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
        public int MaxLife { get; set; }
        public void SavePlayerState(string name, Sprite sprite, int maxLife, int currentLife)
        {
            Name = name;
            Sprite = sprite;
            MaxLife = maxLife;
            CurrentLife = currentLife;
            StartFlag = true;
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
        public bool StartFlag { get; private set; } = false;
        /// <summary>
        /// 初回の特別処理判定用<br/>
        /// いらんかも
        /// </summary>
        /// <returns>初回呼び出しならfalse</returns>
        public bool IsSaveData()
        {
            if (StartFlag) { return true; }
            else return false;
        }
    }
}