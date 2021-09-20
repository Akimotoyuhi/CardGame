using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>経過ターン数</summary>
    private int m_progressTurn = 0;
    /// <summary>プレイヤーとエネミーのターン判別用(最初はプレイヤーのターン)</summary>
    private bool m_isPlayerTurn = false;
    [SerializeField] private Deck m_deck;
    [SerializeField] private Discard m_discard;

    void Start()
    {
        m_deck.Draw();
    }

    void Update()
    {
        
    }

    /// <summary>ターン終了</summary>
    public void TurnEnd()
    {
        m_deck.Draw();
    }
}
