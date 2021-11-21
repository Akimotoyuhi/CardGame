using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mastar;

public class BattleManager : MonoBehaviour
{
    [Header("プレイヤー関連")]
    /// <summary>プレイヤー初期データ</summary>
    [SerializeField]PlayerStatsData m_playerStatsData;
    /// <summary>プレイヤーのプレハブ</summary>
    [SerializeField] GameObject m_playerPrefab;
    /// <summary>プレイヤーの配置場所</summary>
    [SerializeField] RectTransform m_playerPos;
    /// <summary>プレイヤークラス</summary>
    private Player m_player;
    [Header("敵関連")]
    /// <summary>敵グループ</summary>
    [SerializeField] GameObject m_enemies;
    /// <summary>敵グループのデータ</summary>
    [SerializeField] EncountData m_encountData;
    private EncountDataBase m_encountDatabase;
    /// <summary>敵グループの管理クラス</summary>
    private EnemyManager m_enemyManager;
    [Space]
    /// <summary>デッキ</summary>
    [SerializeField] Deck m_deck;
    /// <summary>捨て札</summary>
    [SerializeField] Discard m_discard;
    /// <summary>手札</summary>
    [SerializeField] Hand m_hand;
    /// <summary>バトル中かどうかのフラグ</summary>
    private bool m_isGame = false;
    /// <summary>カードデータ</summary>
    [SerializeField] NewCardData m_cardData;
    /// <summary>経過ターン数</summary>
    private int m_progressTurn = 0;
    /// <summary>ボタンの受付拒否</summary>
    private bool m_isPress = true;

    void Start()
    {
        Setup();
    }

    /// <summary>
    /// バトル中かどうかを見てキャンバス表示したりしなかったりするメソッド
    /// </summary>
    public void Setup()
    {
        if (m_isGame) GetComponent<Canvas>().enabled = true;
        else GetComponent<Canvas>().enabled = false;
    }

    public void Battle(int enemyid)
    {
        m_progressTurn = 0;
        m_isGame = true;
        Setup();
        CreateField(enemyid);
        FirstTurn();
    }

    private void CreateField(int enemyid)
    {
        //デッキとプレイヤー構築
        if (GodGameManager.Instance().StartCheck())
        {
            m_player = Instantiate(m_playerPrefab, m_playerPos).gameObject.GetComponent<Player>();
            GodGameManager inst = GodGameManager.Instance();
            m_player.SetParam(inst.Name, inst.Image, inst.Hp);
            for (int i = 0; i < GodGameManager.Instance().Cards.Length; i++)
            {
                CreateCard(GodGameManager.Instance().GetHaveCardID(i));
            }
        }
        else
        {
            m_player = Instantiate(m_playerPrefab, m_playerPos).gameObject.GetComponent<Player>();
            m_player.SetParam(m_playerStatsData.Name, m_playerStatsData.Image, m_playerStatsData.HP);
            for (int i = 0; i < m_playerStatsData.GetCardLength; i++)
            {
                CreateCard(m_playerStatsData.GetCard(i));
            }
        }
        //敵グループ生成
        m_encountDatabase = m_encountData.m_data[enemyid];
        m_enemyManager = m_enemies.GetComponent<EnemyManager>();
        for (int i = 0; i < m_encountDatabase.GetLength; i++)
        {
            m_enemyManager.CreateEnemies(m_encountDatabase.GetID(i));
        }
        m_enemyManager.EnemyCount();
    }

    /// <summary>
    /// 最初のターンの特別処理<br/>
    /// いらんかも
    /// </summary>
    private void FirstTurn()
    {
        Debug.Log(m_progressTurn + "ターン目");
        //m_deck.Draw();
        m_enemyManager.EnemyTrun(m_progressTurn);
        m_progressTurn++;
        m_isPress = false;
        TurnStart();
    }

    /// <summary>ターン終了</summary>
    public void TurnEnd()
    {
        if (m_isPress) return;
        m_isPress = true;
        m_hand.AllCast();
        m_enemyManager.EnemyTrun(m_progressTurn);
        m_player.TurnEnd();
        //d(m_progressTurn);
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
        card.SetInfo(cardData.CardName, cardData.Image, cardData.Tooltip, cardData.Attack, cardData.AttackNum, cardData.Block, cardData.BlockNum, cardData.Cost, cardData.Conditions, cardData.UseType, m_player);
        //obj.transform.parent = m_deck.transform;
        obj.transform.SetParent(m_deck.transform, false);
    }
}
