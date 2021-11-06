using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    /// <summary>プレイヤー初期データ</summary>
    [SerializeField] PlayerStatsData m_playerStatsData;
    /// <summary>プレイヤーのプレハブ</summary>
    [SerializeField] GameObject m_playerPrefab;
    /// <summary>デッキ</summary>
    [SerializeField] Deck m_deck;
    /// <summary>捨て札</summary>
    [SerializeField] Discard m_discard;
    /// <summary>手札</summary>
    [SerializeField] Hand m_hand;
    /// <summary>敵グループ</summary>
    [SerializeField] EnemyManager m_enemies;
    /// <summary>カードのデータベース</summary>
    [SerializeField] NewCardData m_cardData;
    /// <summary>プレイヤー</summary>
    private Player m_player;
    /// <summary>経過ターン数</summary>
    private int m_progressTurn = 0;
    /// <summary>ボタンの受付拒否</summary>
    private bool m_isPress = true;
    delegate void Dele(int i);
    Dele d = default;

    void Start()
    {
        CreateFirld();
        m_player = GameObject.Find("Player").GetComponent<Player>();
        d += m_hand.AllCast;
        d += m_enemies.EnemyTrun;
        d += m_player.TurnEnd;
        Invoke("FirstTurn", 0.1f);
    }

    /// <summary>
    /// プレイヤー、敵、カードを生成してゲーム画面を作る
    /// </summary>
    private void CreateFirld()
    {
        //初期デッキとプレイヤー構築
        if (GodGameManager.Instance().StartCheck())
        {
            for (int i = 0; i < GodGameManager.Instance().Cards.Length; i++)
            {
                CreateCard(GodGameManager.Instance().GetHaveCardID(i));
            }
        }
        else
        {
            for (int i = 0; i < m_playerStatsData.GetCardLength; i++)
            {

                CreateCard(m_playerStatsData.GetCard(i));
            }
        }

        //敵グループ生成
    }

    /// <summary>
    /// 最初のターンの特別処理
    /// いらんかも
    /// </summary>
    private void FirstTurn()
    {
        Debug.Log(m_progressTurn + "ターン目");
        m_deck.Draw();
        m_enemies.EnemyTrun(m_progressTurn);
        m_progressTurn++;
        m_isPress = false;
    }

    /// <summary>ターン終了</summary>
    public void TurnEnd()
    {
        if (m_isPress) return;
        m_isPress = true;
        d(m_progressTurn);
        m_progressTurn++;
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
    public void CreateCard(int id)
    {
        GameObject obj = Instantiate((GameObject)Resources.Load("BlankCard"));
        BlankCard card = obj.GetComponent<BlankCard>();
        NewCardDataBase cardData = m_cardData.m_cardData[id];
        card.SetInfo(cardData.m_image, cardData.m_name, cardData.m_cost, cardData.GetTooltip(), cardData.GetParam(), cardData.m_cardType);
        //obj.transform.parent = m_deck.transform;
        obj.transform.SetParent(m_deck.transform, false);
    }
}
