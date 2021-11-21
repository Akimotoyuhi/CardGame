using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mastar;

public class BattleManager : MonoBehaviour
{
    [Header("�v���C���[�֘A")]
    /// <summary>�v���C���[�����f�[�^</summary>
    [SerializeField]PlayerStatsData m_playerStatsData;
    /// <summary>�v���C���[�̃v���n�u</summary>
    [SerializeField] GameObject m_playerPrefab;
    /// <summary>�v���C���[�̔z�u�ꏊ</summary>
    [SerializeField] RectTransform m_playerPos;
    /// <summary>�v���C���[�N���X</summary>
    private Player m_player;
    [Header("�G�֘A")]
    /// <summary>�G�O���[�v</summary>
    [SerializeField] GameObject m_enemies;
    /// <summary>�G�O���[�v�̃f�[�^</summary>
    [SerializeField] EncountData m_encountData;
    private EncountDataBase m_encountDatabase;
    /// <summary>�G�O���[�v�̊Ǘ��N���X</summary>
    private EnemyManager m_enemyManager;
    [Space]
    /// <summary>�f�b�L</summary>
    [SerializeField] Deck m_deck;
    /// <summary>�̂ĎD</summary>
    [SerializeField] Discard m_discard;
    /// <summary>��D</summary>
    [SerializeField] Hand m_hand;
    /// <summary>�o�g�������ǂ����̃t���O</summary>
    private bool m_isGame = false;
    /// <summary>�J�[�h�f�[�^</summary>
    [SerializeField] NewCardData m_cardData;
    /// <summary>�o�߃^�[����</summary>
    private int m_progressTurn = 0;
    /// <summary>�{�^���̎�t����</summary>
    private bool m_isPress = true;

    void Start()
    {
        Setup();
    }

    /// <summary>
    /// �o�g�������ǂ��������ăL�����o�X�\�������肵�Ȃ������肷�郁�\�b�h
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
        //�f�b�L�ƃv���C���[�\�z
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
        //�G�O���[�v����
        m_encountDatabase = m_encountData.m_data[enemyid];
        m_enemyManager = m_enemies.GetComponent<EnemyManager>();
        for (int i = 0; i < m_encountDatabase.GetLength; i++)
        {
            m_enemyManager.CreateEnemies(m_encountDatabase.GetID(i));
        }
        m_enemyManager.EnemyCount();
    }

    /// <summary>
    /// �ŏ��̃^�[���̓��ʏ���<br/>
    /// ����񂩂�
    /// </summary>
    private void FirstTurn()
    {
        Debug.Log(m_progressTurn + "�^�[����");
        //m_deck.Draw();
        m_enemyManager.EnemyTrun(m_progressTurn);
        m_progressTurn++;
        m_isPress = false;
        TurnStart();
    }

    /// <summary>�^�[���I��</summary>
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
    /// �^�[���J�n
    /// </summary>
    private void TurnStart()
    {
        m_isPress = false;
        m_deck.Draw();
        Debug.Log(m_progressTurn + "�^�[����");
        m_player.TurnStart();
    }

    /// <summary>
    /// �J�[�h�̍쐬
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
