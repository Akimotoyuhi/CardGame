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

    void Start()
    {
        //初期デッキ構築　とりあえず
        for (int i = 0; i < 5; i++)
        {
            CreateCard((int)CardID.kyougeki);
        }
        for (int i = 0; i < 5; i++)
        {
            CreateCard((int)CardID.bougyoryokuUp);
        }
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

    /// <summary>
    /// カードの作成
    /// </summary>
    public void CreateCard(int num)
    {
        GameObject obj = Instantiate((GameObject)Resources.Load("BlankCard"));
        BlankCard card = obj.GetComponent<BlankCard>();
        NewCardDataBase cardData = m_cardData.m_cardData[num];
        Debug.Log(cardData);
        Debug.Log($"{cardData.m_image}, {cardData.m_name}, {cardData.m_cost}, {cardData.m_cardEffectSets[num].m_effect.GetTooltip()}, {cardData.m_cardEffectSets[num].m_effect.GetParam()}, {cardData.m_cardType}");
        card.SetInfo(cardData.m_image, cardData.m_name, cardData.m_cost, cardData.m_cardEffectSets[num].m_effect.GetTooltip(), cardData.m_cardEffectSets[num].m_effect.GetParam(), cardData.m_cardType);
        obj.transform.parent = m_deck.transform;
    }
}
