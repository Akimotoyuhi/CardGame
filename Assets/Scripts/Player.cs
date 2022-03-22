using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class Player : CharactorBase, IDrop
{
    /// <summary>デフォルトコスト。何らかの効果で下げられた後元の値に戻す時に使う</summary>
    private int m_maxCost = 3;
    private Sprite[] m_attackedSprite;
    private Sprite m_gameoverSprite;
    [SerializeField] int m_cost = default;
    [SerializeField] int m_drowNum = 5;
    /// <summary>最大コスト</summary>
    public int MaxCost => m_maxCost;
    /// <summary>現在コスト</summary>
    public int CurrrentCost { get => m_cost; set => m_cost = value; }
    /// <summary>カードをドローする枚数</summary>
    public int DrowNum { get => m_drowNum; set => m_drowNum = value; }
    public Sprite[] AttackedSprite => m_attackedSprite;
    public Sprite GameoverSprite => m_gameoverSprite;
    public Player Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //Instance = this;
        SetUp();
    }

    public override void TurnStart()
    {
        m_cost = m_maxCost;
        base.TurnStart();
    }

    protected override void SetUp()
    {
        base.SetUp();
    }

    public void SetParam(string name, Sprite idleSprite, Sprite[] attackedSprite, Sprite gameoverSprite, int maxLife, int currentLife)
    {
        m_name = name;
        m_sprite = idleSprite;
        m_attackedSprite = attackedSprite;
        m_gameoverSprite = gameoverSprite;
        m_maxLife = maxLife;
        m_life = currentLife;
    }

    /// <summary>
    /// 被ダメージ処理
    /// </summary>
    /// <param name="damage">被ダメージ</param>
    public void GetAcceptDamage(int power, int block, List<Condition> condition)
    {
        Damage(power, block, null, true, () =>
        {
            m_image.sprite = m_gameoverSprite;
            GameManager.Instance.Gameover();
        });
        foreach (var item in condition)
        {
            Damage(0, 0, item, true, () =>
            {
                m_image.sprite = m_gameoverSprite;
                GameManager.Instance.Gameover();
            });
        }
    }

    public bool CanDrop(UseType useType)
    {
        if (useType == UseType.ToPlayer) return true;
        return false;
    }

    public void GetDrop(List<int[]> cardCommand)
    {
        BattleManager.Instance.CommandManager.CommandExecute(cardCommand, true);
    }

    public void OnCard(UseType? useType)
    {
        if (useType == UseType.ToPlayer) m_flame.SetActive(true);
        else m_flame.SetActive(false);
    }

    public override void GetDamage(int[] cardParam)
    {
        CommandParam command = (CommandParam)cardParam[0];
        switch (command)
        {
            case CommandParam.Attack:
                Damage(cardParam[2], 0, null, true, () =>
                {
                    m_image.sprite = m_gameoverSprite;
                    GameManager.Instance.Gameover();
                });
                break;
            case CommandParam.Block:
                Damage(0, cardParam[2], null, true, () =>
                {
                    m_image.sprite = m_gameoverSprite;
                    GameManager.Instance.Gameover();
                });
                break;
            case CommandParam.Conditon:
                ConditionSelection cs = new ConditionSelection();
                Damage(0, 0, cs.SetCondition((ConditionID)cardParam[2], cardParam[3]), true, () =>
                {
                    m_image.sprite = m_gameoverSprite;
                    GameManager.Instance.Gameover();
                });
                break;
            default:
                Debug.LogError("例外");
                break;
        }
    }

    public void AttackSpriteChange(AttackSpriteID attackSpriteID,float duration)
    {
        m_image.sprite = m_attackedSprite[(int)attackSpriteID];
        Observable.Timer(System.TimeSpan.FromSeconds(duration)).Subscribe(_ => m_image.sprite = m_sprite);
    }

    public void PlayerAnim()
    {
        //AttackAnim(true);
    }
}

public enum AttackSpriteID
{
    Slash,
}