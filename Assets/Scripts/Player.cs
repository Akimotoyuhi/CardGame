using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : CharactorBase, IDropHandler
{
    void Start()
    {
        SetUp();
    }

    /// <summary>
    /// 被ダメージ処理
    /// </summary>
    /// <param name="damage">ブロック値を加味した被ダメージ</param>
    public void GetAcceptDamage(EnemyCommand enemy)
    {
        SetCondisionTurn(enemy.m_conditions);
        int dmg = CalculationAcceptDamage(enemy.m_attack);
        dmg = m_block -= dmg;
        if (m_block < 0) { m_block = 0; }
        m_blkSlider.value = m_block;
        if (dmg > 0)
        {
            SetText();
            return;
        }
        m_hp += dmg;
        m_hpSlider.value = m_hp;
        SetText();
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

    public void OnDrop(PointerEventData pointerEvent)
    {
        BlankCard card = pointerEvent.pointerDrag.GetComponent<BlankCard>();
        if (card == null || card.GetCardType() != CardType.ToPlayer) return;
        m_stateArray = card.GetEffect().conditions;
        SetCondisionTurn(m_stateArray);
        m_block += card.GetEffect().block;
        SetText();
    }
}
