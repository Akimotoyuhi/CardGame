using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class GameManager : MonoBehaviour
{
    /// <summary>経過ターン数</summary>
    private int m_progressTurn = 1;
    /// <summary>デッキ</summary>
    [SerializeField] Deck m_deck;
    /// <summary>捨て札</summary>
    [SerializeField] Discard m_discard;
    /// <summary>手札</summary>
    [SerializeField] Hand m_hand;
    /// <summary>敵グループ</summary>
    [SerializeField] EnemyController m_enemies;
    /// <summary>カードのデータベース</summary>
    [SerializeField] NewCardData m_cardData;
    /// <summary>プレイヤー</summary>
    private Player m_player;
    /// <summary>ボタンの受付拒否</summary>
    private bool m_isPress = false;
    delegate void Dele(int i);
    Dele d = default;

    void Start()
    {
        //初期デッキ構築　とりあえず
        CreateCard((int)CardID.kyougeki);
        CreateCard((int)CardID.bougyoryokuUp);
        CreateCard((int)CardID.hikkaki);
        //CreateCard((int)CardID.kouzoukyouka);
        //CreateCard((int)CardID.sennjuturennkei);
        //CreateCard((int)CardID.meltdown);
        m_deck.Draw();
        m_player = GameObject.Find("Player").GetComponent<Player>();
        Debug.Log(m_progressTurn + "ターン目");
        d += m_hand.AllCast;
        d += m_enemies.EnemyTrun;
        d += m_player.TurnEnd;
    }

    /// <summary>ターン終了</summary>
    public void TurnEnd()
    {
        if (m_isPress) return;
        m_isPress = true;
        //m_hand.AllCast();
        //m_enemies.EnemyTrun(m_progressTurn);
        m_progressTurn++;
        //m_player.TurnEnd();
        d(m_progressTurn);
        Invoke("TurnStart", 1f);
    }

    /// <summary>
    /// ターン開始
    /// </summary>
    private void TurnStart()
    {
        m_isPress = false;
        m_deck.Draw();
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
