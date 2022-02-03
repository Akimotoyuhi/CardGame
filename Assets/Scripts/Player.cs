using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class Player : CharactorBase, IDrop
{
    /// <summary>デフォルトコスト。何らかの効果で下げられた後元の値に戻す時に使う</summary>
    private int m_maxCost = 3;
    [SerializeField] int m_cost = default;
    [SerializeField] int m_drowNum = 5;
    /// <summary>最大コスト</summary>
    public int MaxCost => m_maxCost;
    /// <summary>現在コスト</summary>
    public int CurrrentCost { get => m_cost; set => m_cost = value; }
    /// <summary>カードをドローする枚数</summary>
    public int DrowNum { get => m_drowNum; set => m_drowNum = value; }
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
        GetComponent<Image>().sprite = m_image;
        m_hpSlider.maxValue = m_maxLife;
        m_hpSlider.value = m_life;
        m_blkSlider.value = m_block;
        SetUI();
    }

    public void SetParam(string name, Sprite image, int maxLife, int currentLife)
    {
        m_name = name;
        m_image = image;
        m_maxLife = maxLife;
        m_life = currentLife;
    }

    /// <summary>
    /// 被ダメージ処理
    /// </summary>
    /// <param name="damage">被ダメージ</param>
    //public void GetAcceptDamage(EnemyActionCommnad3 enemy)
    //{
    //    AddEffect(enemy.Conditions);
    //    int damage = CalculationAcceptDamage(enemy.Power);
    //    Debug.Log($"受けたダメージ{damage}");
    //    damage = m_block -= damage;
    //    if (m_block < 0) { m_block = 0; }
    //    damage *= -1; //ブロック値計算の後ダメージの符号が反転するので戻す
    //    if (damage < 0) { }
    //    else
    //    {
    //        m_life -= damage;
    //    }
    //    SetUI();
    //}
    public void GetAcceptDamage(int power, int blk, List<Condition> conditions)
    {
        EffectChecker(EventTiming.Damaged, ParametorType.Any);
        AddEffect(conditions);
        if (power <= 0)
        {
            Debug.Log($"受けたダメージ{power}");
            int damage = m_block -= power;
            if (m_block < 0)
            {
                m_block = 0;
            }
            else
            {
                EffectManager.Instance.DamageText(power.ToString(), Color.blue, Vector2.zero, transform);
            }
            damage *= -1; //ブロック値計算の後ダメージの符号が反転するので戻す
            if (damage < 0) { }
            else
            {
                m_life -= damage;
                if (m_life <= 0)
                {
                    Debug.Log("がめおべｒ");
                }
                else
                {
                    EffectManager.Instance.DamageText(damage.ToString(), Color.red, Vector2.zero, transform);
                }
            }
        }
        SetUI();
    }

    public void GetDrop(BlankCard card)
    {
        if (card == null || card.GetCardType != UseType.ToPlayer) return;
        AddEffect(card.Conditions);
        m_block += card.Block;
        SetUI();
        card.OnCast();
    }

    public void PlayerAnim()
    {
        AttackAnim(true);
    }
}
