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
    [SerializeField] Player m_playerPrefab;
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
    //[SerializeField] EnemyData m_enemyData;
    /// <summary>�G�O���[�v�̊Ǘ��N���X</summary>
    [SerializeField] EnemyManager m_enemyManager;
    /// <summary>�G�O���[�v��Drop�Ώ�</summary>
    [SerializeField] EnemiesTarget m_enemiesTarget;
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
    /// <summary>�S�Ẵh���b�v���󂯎��N���X</summary>
    [SerializeField] AllDropTarget m_allDropTarget;
    /// <summary>��V���</summary>
    [SerializeField] Reward m_reward;
    /// <summary>�퓬����UI�Ǘ��N���X</summary>
    [SerializeField] BattleUIController m_battleUIController;
    /// <summary>��V����</summary>
    [SerializeField] int m_rewardNum = 3;
    [SerializeField] CommandManager m_dropManager;
    /// <summary>�J����</summary>
    [SerializeField] Camera m_camera;
    /// <summary>�J�[�h�f�[�^</summary>
    private CardData m_cardData;
    /// <summary>�J�[�h�̃v���n�u</summary>
    private BlankCard m_cardPrefab;
    /// <summary>�h���b�O���̃J�[�h��UseType�ۑ��p</summary>
    private UseType? m_dragCardUseType = null;
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
    public CommandManager DropManager => m_dropManager;
    public bool IsGame { get => m_isGame; set => m_isGame = value; }
    public int CurrentTurn => m_currentTurn;
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
        Vector2 size = m_cardPrefab.gameObject.GetRectTransform().sizeDelta;
        m_deck.GridLayoutGroupSetting(size);
        m_discard.GridLayoutGroupSetting(size);
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
        GameManager.Instance.PlayerDataSave(m_playerStatsData.Name, m_playerStatsData.IdleSprite, m_playerStatsData.AttackedSprite, m_playerStatsData.GameoverSprite, m_playerStatsData.HP, m_playerStatsData.HP, cards.ToArray(), isUpgrade.ToArray());
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
            m_reward.RewardView(m_cardData.GetCardRarityRandom(0));
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
        //�v���C���[����
        if (!DataManager.Instance.IsPlayerData) //�f�[�^�����������獡�����Ă�f�b�L��ۑ�
        {
            List<int> cards = new List<int>();
            List<int> isUpgrade = new List<int>();
            for (int i = 0; i < m_playerStatsData.GetCardLength; i++)
            {
                cards.Add(m_playerStatsData.GetCardData(i));
                isUpgrade.Add(m_playerStatsData.IsUpgrade(i));
            }
            GameManager.Instance.PlayerDataSave(m_playerStatsData.Name, m_playerStatsData.IdleSprite, m_playerStatsData.AttackedSprite, m_playerStatsData.GameoverSprite, m_playerStatsData.HP, m_playerStatsData.HP, cards.ToArray(), isUpgrade.ToArray());
        }
        m_player = Instantiate(m_playerPrefab, m_playerPos);
        m_player.Canvas = m_battleCanvas.transform;
        m_player.SetParam(DataManager.Instance.Name, DataManager.Instance.IdleSprite, DataManager.Instance.AttackedSprite, DataManager.Instance.GameoverSprite, DataManager.Instance.MaxLife, DataManager.Instance.CurrentLife);
        //�G�O���[�v����
        m_enemyManager.Setup(m_enemiesTarget);
        m_enemyManager.CreateEnemies(eria);
        m_enemyManager.EnemyCount();
        m_dropManager.Setup(m_enemyManager, m_player);
        //�J�[�h����
        for (int i = 0; i < DataManager.Instance.Cards.Count; i++)
        {
            int[] nms = DataManager.Instance.Cards[i];
            CreateCard(nms[0], nms[1]);
        }
    }

    /// <summary>
    /// �o�g�����̗���
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnBattle()
    {
        m_isPress = true;
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
        m_isPress = false;
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
        Debug.Log(m_currentTurn + "�^�[����");
        m_player.TurnStart();
        m_turnBegin.OnNext(m_currentTurn);
        SetCostText(m_player.MaxCost.ToString(), m_player.CurrrentCost.ToString());
    }

    public void CardDraw(int drawNum)
    {
        m_deck.Draw(drawNum);
    }

    public void CardDispose(int disposeNum)
    {
        Debug.LogWarning("������");
        return;
    }

    /// <summary>
    /// �J�[�h�̍쐬
    /// </summary>
    public void CreateCard(int id, int upgradeNum)
    {
        //GameObject obj = Instantiate(m_cardPrefab);
        BlankCard card = Instantiate(m_cardPrefab);
        CardInfomationData cardData;
        cardData = m_cardData.CardDatas(id, upgradeNum);
        card.SetInfo(cardData, m_battleUICanvas.GetComponent<RectTransform>(), m_player, m_camera, m_discard);
        card.CardState = CardState.None;
        card.transform.SetParent(m_deck.CardParent, false);
        card.GetPlayerEffect();
    }

    /// <summary>
    /// ��D�̑S�J�[�h�̃e�L�X�g���X�V������
    /// <br/>�J�[�h�g�p���ɌĂ΂��
    /// </summary>
    public void CardCast()
    {
        m_hand.UpdateTooltip();
    }

    /// <summary>
    /// ��ɃJ�[�h��ǉ�����
    /// </summary>
    public void AddCard(CardAddDestination addDestination, CardID cardID, int num, int isUpgrade)
    {
        for (int i = 0; i < num; i++)
        {
            CardInfomationData c = m_cardData.CardDatas((int)cardID, isUpgrade);
            BlankCard b = Instantiate(m_cardPrefab);
            b.SetInfo(c, m_battleUICanvas.GetComponent<RectTransform>(), m_player, m_camera, m_discard);
            switch (addDestination)
            {
                case CardAddDestination.ToDeck:
                    b.transform.SetParent(m_deck.CardParent, false);
                    break;
                case CardAddDestination.ToHand:
                    b.transform.SetParent(m_hand.CardParent, false);
                    b.CardState = CardState.Play;
                    break;
                case CardAddDestination.ToDiscard:
                    b.transform.SetParent(m_discard.CardParent, false);
                    break;
                default:
                    Debug.LogError("���݂��Ȃ��̒ǉ���");
                    break;
            }
        }
    }

    public void OnCardDrag(UseType? useType)
    {
        m_enemiesTarget.OnCard(useType);
        m_player.OnCard(useType);
        m_allDropTarget.OnCard(useType);
        m_enemyManager.OnCardDrag(useType);
    }
}