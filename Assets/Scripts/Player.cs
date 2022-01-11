using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class Player : CharactorBase, IDrop
{
    /// <summary>デフォルトコスト。何らかの効果で下げられた後元の値に戻す時に使う</summary>
    private int m_defaultCost = 3;
    [SerializeField]
    private int m_cost = default;
    /// <summary>デフォルトドロー枚数。何らかの効果で下げられた後元の値に戻す時に使う　いらんかも</summary>
    private int m_defaultDrowNum = 5;
    private int m_drowNum = 5;
    /// <summary>現在コスト</summary>
    public int Cost { get => m_cost; set => m_cost = value; }
    /// <summary>現在のドロー枚数</summary>
    public int DrowNum { get => m_drowNum; set => m_drowNum = value; }

    void Start()
    {
        SetUp();
    }

    public override void TurnStart()
    {
        m_cost = m_defaultCost;
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
    public void GetAcceptDamage(EnemyActionCommnad3 enemy)
    {
        //SetCondisionTurn(enemy.Condition);
        int damage = CalculationAcceptDamage(enemy.Power);
        damage = m_block -= damage;
        if (m_block < 0) { m_block = 0; }
        damage *= -1; //ブロック値計算の後ダメージの符号が反転するので戻す
        if (damage < 0) { }
        else
        {
            m_life -= damage;
        }
        SetUI();
    }

    /// <summary>
    /// バフデバフを含めた被ダメージ計算
    /// </summary>
    /// <param name="num">被ダメージ</param>
    /// <returns>計算後の被ダメージ</returns>
    private int CalculationAcceptDamage(int num)
    {
        return num;
    }

    public void GetDrop(BlankCard card)
    {
        if (card == null || card.GetCardType != UseType.ToPlayer) return;
        BlankCard cards = card;
        foreach (var item in card.Conditions)
        {
            m_conditions.Add(item);
        }
        m_block += card.Block;
        SetUI();
        card.OnCast();
    }
}
