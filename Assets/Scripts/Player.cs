using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : CharactorBase, IDropHandler, IDrop
{
    public int Cost { get; set; }

    void Start()
    {
        SetUp();
    }

    /// <summary>
    /// 被ダメージ処理
    /// </summary>
    /// <param name="damage">被ダメージ</param>
    public void GetAcceptDamage(EnemyCommand enemy)
    {
        SetCondisionTurn(enemy.m_conditions);
        int damage = CalculationAcceptDamage(enemy.m_attack);
        damage = m_block -= damage;
        if (m_block < 0) { m_block = 0; }
        m_blkSlider.value = m_block;
        if (damage > 0) { }
        else
        {
            m_hp -= damage;
            m_hpSlider.value = m_hp;
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
        m_conditions = card.Conditions;
        SetCondisionTurn(m_stateArray);
        m_block += card.OnCast().Block;
        SetUI();
    }

    public void OnDrop(PointerEventData pointerEvent)
    {
        BlankCard card = pointerEvent.pointerDrag.GetComponent<BlankCard>();
        if (card == null || card.GetCardType != UseType.ToPlayer) return;
        m_conditions = card.Conditions;
        SetCondisionTurn(m_stateArray);
        m_block += card.OnCast().Block;
        SetUI();
    }
}
