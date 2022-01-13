using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mastar
{
    public class DataManager
    {
        private static DataManager m_instance = new DataManager();
        public static DataManager Instance => m_instance;
        //private DataManager() { }

        /// <summary>現在の進行状況</summary>
        public int Step { get; set; } = 0;
        //プレイヤー情報関連
        List<CardID> m_cards = new List<CardID>();
        public string Name { get; private set; }
        public Sprite Sprite { get; private set; }
        public int CurrentLife { get; set; }
        public int MaxLife { get; set; }
        public void SavePlayerState(string name, Sprite sprite, int maxLife, int currentLife)
        {
            Name = name;
            Sprite = sprite;
            MaxLife = maxLife;
            CurrentLife = currentLife;
            StartFlag = true;
        }
        public List<CardID> Cards { get => m_cards; set => m_cards = value; }
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