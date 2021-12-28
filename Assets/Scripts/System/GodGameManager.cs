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
        private string m_name = default;
        private int m_hp = default;
        private Sprite m_image = default;
        private CardID[] m_cards = new CardID[0];
        public int Step { get => m_instance.m_step; set => m_instance.m_step = value; }
        public string Name => m_instance.m_name;
        public int Hp => m_instance.m_hp;
        public Sprite Image { get => m_image; }
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