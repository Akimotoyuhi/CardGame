using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>経過ターン数</summary>
    private int m_progressTurn = 1;
    [SerializeField] private Deck m_deck;
    [SerializeField] private Discard m_discard;
    [SerializeField] private Hand m_hand;
    [SerializeField] private EnemyController m_enemies;

    void Start()
    {
        m_deck.Draw();
        Debug.Log(m_progressTurn + "ターン目");
    }

    /// <summary>ターン終了</summary>
    public void TurnEnd()
    {
        m_hand.AllCast();
        m_enemies.EnemyTrun(m_progressTurn);
        m_progressTurn++;
        m_deck.Draw();
        Debug.Log(m_progressTurn + "ターン目");
    }
}
