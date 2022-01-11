using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mastar
{
    /// <summary>
    /// GameManagerより上のManager。GameManager神
    /// </summary>
    public class GodGameManager
    {
        private static GodGameManager m_instance = new GodGameManager();
        public static GodGameManager Instance => m_instance;
        private GodGameManager() { } //privateなコンストラクタ

        /// <summary>現在の進行状況</summary>
        private int m_step = 0;
        //プレイヤー情報関連
        private CardID[] m_cards = new CardID[0];
        private Player m_player = null;
        //public Player Player { set => m_player = value; }
        public int Step { get => m_instance.m_step; set => m_instance.m_step = value; }
        public string Name => m_instance.m_player.Name;
        public int CurrentLife => m_instance.m_player.CurrentLife;
        public int MaxLife => m_instance.m_player.MaxLife;
        public int Heal
        {
            set
            {
                m_instance.m_player.Heal = value;
            }
        }
        public Sprite Image { get => m_instance.m_player.Image; }
        public void SavePlayerState(string name, Sprite image, int maxLife, int CurrentLife)
        {
            m_player = new Player();
            m_player.SetParam(name, image, maxLife, CurrentLife);
        }
        public Player Player => m_player;
        public int GetHaveCardID(int index) { return (int)m_instance.m_cards[index]; }
        public CardID[] Cards => m_instance.m_cards;
        /// <summary>
        /// 初回の特別処理判定用<br/>
        /// いらんかも
        /// </summary>
        /// <returns>初回呼び出しならfalse</returns>
        public bool StartCheck()
        {
            if (m_cards.Length == 0) { return false; }
            else return true;
        }
    }
}