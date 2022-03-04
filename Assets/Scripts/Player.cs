using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class Player : CharactorBase, IDrop
{
    /// <summary>デフォルトコスト。何らかの効果で下げられた後元の値に戻す時に使う</summary>
    private int m_maxCost = 3;
    private Sprite m_gameoverSprite;
    [SerializeField] int m_cost = default;
    [SerializeField] int m_drowNum = 5;
    /// <summary>最大コスト</summary>
    public int MaxCost => m_maxCost;
    /// <summary>現在コスト</summary>
    public int CurrrentCost { get => m_cost; set => m_cost = value; }
    /// <summary>カードをドローする枚数</summary>
    public int DrowNum { get => m_drowNum; set => m_drowNum = value; }
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
        GetComponent<Image>().sprite = m_sprite;
        m_hpSlider.maxValue = m_maxLife;
        m_hpSlider.value = m_life;
        m_blkSlider.value = m_block;
        SetUI();
    }

    public void SetParam(string name, Sprite idleSprite, Sprite gameoverSprite, int maxLife, int currentLife)
    {
        m_name = name;
        m_sprite = idleSprite;
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

    public void GetDrop(int power, int block, Condition condition, UseType useType, System.Action onCast)
    {
        if (useType != UseType.ToPlayer) return;
        Damage(power, block, condition, true, () =>
        {
            m_image.sprite = m_gameoverSprite;
            GameManager.Instance.Gameover();
        });
        onCast();
    }
    //public override void Damage(int damage, int block, List<Condition> conditions)
    //{
    //    //AddEffect(conditions);
    //    //m_block += block;
    //    //SetUI();
    //}

    public void PlayerAnim()
    {
        //AttackAnim(true);
    }
}
