using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mastar;
using System;
using UniRx;

enum Timing { Start, End }

public class BattleManager : MonoBehaviour
{
    [Header("�v���C���[�֘A")]
    /// <summary>�v���C���[�����f�[�^</summary>
    [SerializeField] PlayerStatsData m_playerStatsData;
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
    [Header("�o�g�����̃p�����[�^�[�Ǘ�")]
    /// <summary>�o�߃^�[����</summary>
    private int m_progressTurn = 0;
    [Space]
    /// <summary>�f�b�L</summary>
    [SerializeField] Deck m_deck;
    /// <summary>�̂ĎD</summary>
    [SerializeField] Discard m_discard;
    /// <summary>��D</summary>
    [SerializeField] Hand m_hand;
    /// <summary>�J�[�h�f�[�^</summary>
    [SerializeField] NewCardData m_cardData;
    [SerializeField] GameObject m_cardPrefab;
    /// <summary>�{�^���̎�t����</summary>
    private bool m_isPress = true;
    /// <summary>�o�g�������ǂ����̃t���O</summary>
    private bool m_isGame = false;
    private GameManager m_gameManager;
    public static BattleManager Instance { get; private set; }
    private Subject<int> m_turnBegin = new Subject<int>();
    private Subject<int> m_turnEnd = new Subject<int>();
    public IObservable<int> TurnBegin => m_turnBegin;
    public IObservable<int> TurnEnd2 => m_turnEnd;
    public int GetDrowNum => m_player.DrowNum;
    public GameManager SetGameManager { set => m_gameManager = value; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Setup();
    }

    /// <summary>
    /// �Z�b�g�A�b�v<br/>
    /// �ŏ��ɌĂ�
    /// </summary>
    public void Setup()
    {
        if (m_isGame) GetComponent<Canvas>().enabled = true;
        else GetComponent<Canvas>().enabled = false;
    }

    /// <summary>
    /// �퓬�J�n
    /// </summary>
    /// <param name="enemyid">�G���J�E���g�����G��ID</param>
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
        if (GodGameManager.Instance.StartCheck())
        {
            //�f�[�^�����݂���ꍇ�͕ۑ�����Ă���Manager�������Ă���
            Debug.Log("�ۑ����ꂽ�f�[�^����������");
            m_player = Instantiate(m_playerPrefab, m_playerPos).gameObject.GetComponent<Player>();
            m_player.SetParam(GodGameManager.Instance.Name, GodGameManager.Instance.Image, GodGameManager.Instance.MaxLife, GodGameManager.Instance.CurrentLife);
            for (int i = 0; i < GodGameManager.Instance.Cards.Length; i++)
            {
                CreateCard(GodGameManager.Instance.GetHaveCardID(i));
            }
        }
        else
        {
            //�f�[�^�������ꍇ�͏����l�̃f�[�^�������Ă���
            Debug.Log("����N��");
            m_player = Instantiate(m_playerPrefab, m_playerPos).gameObject.GetComponent<Player>();
            m_player.SetParam(m_playerStatsData.Name, m_playerStatsData.Image, m_playerStatsData.HP, m_playerStatsData.HP);
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
            Debug.Log(m_encountDatabase.GetID(i));
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
        //m_turnBegin.OnNext(m_progressTurn);
        m_progressTurn++;
        TurnStart();
    }

    /// <summary>�^�[���I��</summary>
    public void TurnEnd()
    {
        if (m_isPress) return;
        m_isPress = true;
        m_hand.AllCast();
        m_turnEnd.OnNext(m_progressTurn);
        //m_enemyManager.EnemyTrun(m_progressTurn);
        m_player.TurnEnd();
        m_progressTurn++;
        Invoke("TurnStart", 0.5f);
    }

    /// <summary>
    /// �^�[���J�n
    /// </summary>
    private void TurnStart()
    {
        m_isPress = false;
        m_turnBegin.OnNext(m_progressTurn);
        //m_deck.Draw(m_drowNum);
        Debug.Log(m_progressTurn + "�^�[����");
        m_player.TurnStart();
    }

    /// <summary>
    /// �J�[�h�̍쐬
    /// </summary>
    public void CreateCard(int id)
    {
        GameObject obj = Instantiate(m_cardPrefab);
        BlankCard card = obj.GetComponent<BlankCard>();
        NewCardDataBase cardData = m_cardData.m_cardData[id];
        card.SetInfo(cardData, m_player);
        obj.transform.SetParent(m_deck.transform, false);
        card.GetPlayerEffect();
    }

    public void BatlteEnd()
    {
        m_discard.CardDelete();
        m_deck.CardDelete();
        m_hand.CardDelete();
        GameManager.Instance.FloorFinished(m_player);
    }
}