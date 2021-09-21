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
    [SerializeField] private Hand m_hand;
    [SerializeField] private Transform m_enemies;

    void Start()
    {
        m_deck.Draw();
    }

    /// <summary>ターン終了</summary>
    public void TurnEnd()
    {
        m_hand.AllCast();
        for (int i = 0; i < m_enemies.childCount; i++)
        {
            m_enemies.GetChild(i).GetComponent<EnemyBase>().Action();
        }
        m_deck.Draw();
    }
}
