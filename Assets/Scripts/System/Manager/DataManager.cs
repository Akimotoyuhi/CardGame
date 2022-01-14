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
        private List<SpecialCardID> m_specialCards = new List<SpecialCardID>();
        private List<CommonCardID> m_commonCards = new List<CommonCardID>();
        private List<RareCardID> m_rareCards = new List<RareCardID>();
        private List<EliteCardID> m_eliteCards = new List<EliteCardID>();
        private List<CurseCardID> m_curseCards = new List<CurseCardID>();
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
        public List<SpecialCardID> SpecialCards { get => m_specialCards; set => m_specialCards = value; }
        public List<CommonCardID> CommonCards { get => m_commonCards; set => m_commonCards = value; }
        public List<RareCardID> RareCards { get => m_rareCards; set => m_rareCards = value;}
        public List<EliteCardID> EliteCards { get => m_eliteCards; set => m_eliteCards = value; }
        public List<CurseCardID> CurseCards { get => m_curseCards; set => m_curseCards = value; }
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