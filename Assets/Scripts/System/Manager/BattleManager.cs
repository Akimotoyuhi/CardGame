using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mastar;
using System;
using UniRx;
using UnityEngine.UI;

enum Timing { Start, End }

public class BattleManager : MonoBehaviour
{
    #region Player�֘A�̃����o
    [Header("�v���C���[�֘A")]
    /// <summary>�v���C���[�����f�[�^</summary>
    [SerializeField] PlayerStatsData m_playerStatsData;
    /// <summary>�v���C���[�̃v���n�u</summary>
    [SerializeField] GameObject m_playerPrefab;
    /// <summary>�v���C���[�̔z�u�ꏊ</summary>
    [SerializeField] RectTransform m_playerPos;
    /// <summary>�v���C���[�N���X</summary>
    private Player m_player;
    /// <summary>�v���C���[�̃R�X�g�\���p�e�L�X�g</summary>
    [SerializeField] Text m_costViewText;
    #endregion
    #region Enemy�֘A�̃����o
    [Header("�G�֘A")]
    /// <summary>�G�O���[�v</summary>
    [SerializeField] GameObject m_enemies;
    /// <summary>�G�f�[�^�Ǘ��N���X</summary>
    [SerializeField] EnemyData m_enemyData;
    /// <summary>�G�O���[�v�̊Ǘ��N���X</summary>
    private EnemyManager m_enemyManager;
    [Header("�o�g�����̃p�����[�^�[�Ǘ�")]
    /// <summary>�o�߃^�[����</summary>
    private int m_currentTurn = 0;
    #endregion
    #region ���̑��̃����o
    /// <summary>�o�g����ʕ\���p�L�����o�X</summary>
    [Space, SerializeField] Canvas m_battleCanvas;
    /// <summary>�o�g����ʂ�UI�\���p�L�����o�X</summary>
    [SerializeField] Canvas m_battleUICanvas;
    /// <summary>�f�b�L</summary>
    [SerializeField] Deck m_deck;
    /// <summary>�̂ĎD</summary>
    [SerializeField] Discard m_discard;
    /// <summary>��D</summary>
    [SerializeField] Hand m_hand;
    /// <summary>��V���</summary>
    [SerializeField] Reward m_reward;
    /// <summary>�퓬����UI�Ǘ��N���X</summary>
    [SerializeField] BattleUIController m_battleUIController;
    /// <summary>�_���[�W�\���p�̃e�L�X�g</summary>
    [SerializeField] GameObject m_damageTextPrefab;
    /// <summary>��V����</summary>
    [SerializeField] int m_rewardNum = 3;
    /// <summary>�J����</summary>
    [SerializeField] Camera m_camera;
    /// <summary>�J�[�h�f�[�^</summary>
    private CardData m_cardData;
    /// <summary>�J�[�h�̃v���n�u</summary>
    private BlankCard m_cardPrefab;
    /// <summary>�{�^���̎�t</summary>
    private bool m_isPress = true;
    /// <summary>�o�g�������ǂ����̃t���O</summary>
    private bool m_isGame = false;
    /// <summary>�퓬���̃R���[�`���Ɏg���t���O</summary>
    private bool m_battleFlag = false;
    /// <summary>�^�[���J�n��ʒm����</summary>
    private Subject<int> m_turnBegin = new Subject<int>();
    /// <summary>�^�[���I����ʒm����</summary>
    private Subject<int> m_turnEnd = new Subject<int>();
    #endregion
    #region �v���p�e�B
    public static BattleManager Instance { get; private set; }
    public IObservable<int> TurnBegin => m_turnBegin;
    public IObservable<int> TurnEnd2 => m_turnEnd;
    public int GetDrowNum => m_player.DrowNum;
    public bool IsGame { get => m_isGame; set => m_isGame = value; }
    public int CurrentTrun => m_currentTurn;
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    public void Setup()
    {
        m_cardData = GameManager.Instance.CardData;
        m_cardPrefab = GameManager.Instance.CardPrefab;
        m_cardData.Setup();
        SetCanvas();
        m_reward.RewardDisabled();
        m_reward.CanvasRectTransform = m_battleUICanvas.GetComponent<RectTransform>();
        //�v���C���[�f�[�^�̕ۑ�
        List<int> cards = new List<int>();
        List<int> isUpgrade = new List<int>();
        for (int i = 0; i < m_playerStatsData.GetCardLength; i++)
        {
            cards.Add(m_playerStatsData.GetCardData(i));
            isUpgrade.Add(m_playerStatsData.IsUpgrade(i));
        }
        GameManager.Instance.PlayerDataSave(m_playerStatsData.Name, m_playerStatsData.IdleSprite, m_playerStatsData.GameoverSprite, m_playerStatsData.HP, m_playerStatsData.HP, cards.ToArray(), isUpgrade.ToArray());
    }

    /// <summary>
    /// �o�g�����t���O�ɂ����Canvas�̕\����؂�ւ���
    /// </summary>
    public void SetCanvas()
    {
        if (m_isGame)
        {
            m_battleCanvas.enabled = true;
            m_battleUICanvas.enabled = true;
        }
        else
        {
            m_battleCanvas.enabled = false;
            m_battleUICanvas.enabled = false;
        }
    }

    /// <summary>
    /// �R�X�g�e�L�X�g�̍X�V
    /// </summary>
    public void SetCostText(string maxCost, string currentCost)
    {
        m_costViewText.text = currentCost + "/" + maxCost;
    }

    /// <summary>
    /// �퓬�J�n
    /// </summary>
    /// <param name="enemyid">�G���J�E���g�����G��ID</param>
    public void BattleStart(EnemyAppearanceEria eria)
    {
        m_currentTurn = 0;
        m_isGame = true;
        SetCanvas();
        CreateField(eria);
        StartCoroutine(OnBattle());
        //m_battleUIController.Play(BattleUIType.BattleStart, FirstTurn);
        //FirstTurn();
    }

    /// <summary>
    /// �퓬�I��
    /// </summary>
    public void BattleEnd()
    {
        m_discard.CardDelete();
        m_deck.CardDelete();
        m_hand.CardDelete();
        for (int i = 0; i < m_rewardNum; i++)
        {
            m_reward.RewardView(m_cardData.GetCardRarityRandom());
        }
    }

    /// <summary>
    /// ��V��ʏI��
    /// </summary>
    /// <param name="getCardId"></param>
    public void RewardEnd(int getCardId, int isUpgrade)
    {
        DataManager.Instance.AddCards(getCardId, isUpgrade);
        m_reward.RewardDisabled();
        GameManager.Instance.FloorFinished(m_player);
    }

    /// <summary>
    /// �v���C���[��G�̐������s��
    /// </summary>
    /// <param name="enemyid"></param>
    private void CreateField(EnemyAppearanceEria eria)
    {
        //�f�b�L�ƃv���C���[�\�z
        if (!DataManager.Instance.IsPlayerData)
        {
            List<int> cards = new List<int>();
            List<int> isUpgrade = new List<int>();
            for (int i = 0; i < m_playerStatsData.GetCardLength; i++)
            {
                cards.Add(m_playerStatsData.GetCardData(i));
                isUpgrade.Add(m_playerStatsData.IsUpgrade(i));
            }
            GameManager.Instance.PlayerDataSave(m_playerStatsData.Name, m_playerStatsData.IdleSprite, m_playerStatsData.GameoverSprite, m_playerStatsData.HP, m_playerStatsData.HP, cards.ToArray(), isUpgrade.ToArray());
        }
        m_player = Instantiate(m_playerPrefab, m_playerPos).gameObject.GetComponent<Player>();
        m_player.SetParam(DataManager.Instance.Name, DataManager.Instance.IdleSprite, DataManager.Instance.GameoverSprite, DataManager.Instance.MaxLife, DataManager.Instance.CurrentLife);
        for (int i = 0; i < DataManager.Instance.Cards.Count; i++)
        {
            int[] nms = DataManager.Instance.Cards[i];
            CreateCard(nms[0], nms[1]);
        }
        //�G�O���[�v����
        m_enemyManager = m_enemies.GetComponent<EnemyManager>();
        m_enemyManager.CreateEnemies(eria);
        m_enemyManager.EnemyCount();
    }

    /// <summary>
    /// �o�g�����̗���
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnBattle()
    {
        if (m_currentTurn == 0)
        {
            m_battleUIController.Play(BattleUIType.BattleStart, () => m_battleFlag = true);
            while (!m_battleFlag)
            {
                yield return null;
            }
            FirstTurn();
            m_battleFlag = false;
        }
        else
        {
            m_battleUIController.Play(BattleUIType.EnemyTurn, () => m_battleFlag = true);
            while (!m_battleFlag)
            {
                yield return null;
            }
            TurnEnd();
            m_battleFlag = false;
        }
        m_battleUIController.Play(BattleUIType.PlayerTurn, () => m_battleFlag = true);
        while (!m_battleFlag)
        {
            yield return null;
        }
        TurnStart();
        m_battleFlag = false;
    }

    /// <summary>
    /// �ŏ��̃^�[���̓��ʏ���<br/>
    /// ����񂩂�
    /// </summary>
    private void FirstTurn()
    {
        Debug.Log(m_currentTurn + "�^�[����");
        m_enemyManager.EnemyTrun(m_currentTurn);
        m_currentTurn++;
    }

    public void OnClick()
    {
        if (m_isPress) return;
        StartCoroutine(OnBattle());
    }

    /// <summary>
    /// �^�[���I��
    /// </summary>
    private void TurnEnd()
    {
        m_isPress = true;
        m_hand.AllCast();
        m_player.TurnEnd();
        m_turnEnd.OnNext(m_currentTurn);
        m_currentTurn++;
        //m_battleUIController.Play(BattleUIType.PlayerTurn, TurnStart);
    }

    /// <summary>
    /// �^�[���J�n
    /// </summary>
    private void TurnStart()
    {
        m_isPress = false;
        Debug.Log(m_currentTurn + "�^�[����");
        m_player.TurnStart();
        m_turnBegin.OnNext(m_currentTurn);
        SetCostText(m_player.MaxCost.ToString(), m_player.CurrrentCost.ToString());
    }

    /// <summary>
    /// �J�[�h�̍쐬
    /// </summary>
    public void CreateCard(int id, int isUpgrade)
    {
        //GameObject obj = Instantiate(m_cardPrefab);
        BlankCard card = Instantiate(m_cardPrefab);
        CardDataBase cardData;
        if (isUpgrade == 1) { cardData = m_cardData.CardDatas[id].UpgradeData; }
        else { cardData = m_cardData.CardDatas[id]; }
        card.SetInfo(cardData, m_battleUICanvas.GetComponent<RectTransform>(), m_player, m_camera, m_discard);
        card.CardState = CardState.Play;
        card.transform.SetParent(m_deck.CardParent, false);
        card.GetPlayerEffect();
    }
    //public void ViewText(string str, RectTransform tra, ColorType colorType)
    //{
    //    DamageText text = Instantiate(m_damageTextPrefab).GetComponent<DamageText>();
    //    text.transform.SetParent(tra);
    //    text.transform.position = tra.anchoredPosition;
    //    text.Color(colorType);
    //    text.ChangeText(str);
    //    text.RandomMove();
    //}
    /// <summary>
    /// ��D�̑S�J�[�h�̃e�L�X�g���X�V������
    /// <br/>�J�[�h�g�p���ɌĂ΂��
    /// </summary>
    public void CardCast()
    {
        m_hand.UpdateTooltip();
    }
}