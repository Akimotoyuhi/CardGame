using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>経過ターン数</summary>
    private int m_progressTurn = 1;
    [SerializeField] Deck m_deck;
    [SerializeField] Discard m_discard;
    [SerializeField] Hand m_hand;
    [SerializeField] EnemyController m_enemies;
    [SerializeField] NewCardData m_cardData;
    private Player m_player;

    void Start()
    {
        //初期デッキ構築　とりあえず
        //CreateCard((int)CardID.kyougeki);
        //CreateCard((int)CardID.bougyoryokuUp);
        //CreateCard((int)CardID.hikkaki);
        Debug.Log($"CardID:{(int)CardID.kouzoukyouka}, {CardID.kouzoukyouka}");
        CreateCard((int)CardID.kouzoukyouka);
        Debug.Log($"CardID:{(int)CardID.sennjuturennkei}, {CardID.sennjuturennkei}");
        CreateCard((int)CardID.sennjuturennkei);
        Debug.Log($"CardID:{(int)CardID.meltdown}, {CardID.meltdown}");
        CreateCard((int)CardID.meltdown);
        m_deck.Draw();
        m_player = GameObject.Find("Player").GetComponent<Player>();
        Debug.Log(m_progressTurn + "ターン目");
    }

    /// <summary>ターン終了</summary>
    public void TurnEnd()
    {
        m_hand.AllCast();
        m_enemies.EnemyTrun(m_progressTurn);
        m_progressTurn++;
        m_deck.Draw();
        m_player.TurnEnd();
        TurnStart();
    }

    /// <summary>
    /// ターン開始
    /// </summary>
    private void TurnStart()
    {
        Debug.Log(m_progressTurn + "ターン目");
        m_player.TurnStart();
    }

    /// <summary>
    /// カードの作成
    /// </summary>
    public void CreateCard(int num)
    {
        GameObject obj = Instantiate((GameObject)Resources.Load("BlankCard"));
        BlankCard card = obj.GetComponent<BlankCard>();
        NewCardDataBase cardData = m_cardData.m_cardData[num];
        card.SetInfo(cardData.m_image, cardData.m_name, cardData.m_cost, cardData.GetTooltip(), cardData.GetParam(), cardData.m_cardType);
        //obj.transform.parent = m_deck.transform;
        obj.transform.SetParent(m_deck.transform, false);
    }
}
