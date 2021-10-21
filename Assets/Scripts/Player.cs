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
    public void GetAcceptDamage(int[] damage)
    {
        for (int i = 0; i < m_stateArray.Length; i++)
        {
            m_stateArray[i] += damage[i];
        }
        int dmg = CalculationAcceptDamage(m_stateArray[(int)BuffDebuff.Damage]);
        dmg = m_block -= dmg;
        if (m_block < 0) { m_block = 0; }
        m_blkSlider.value = m_block;
        m_stateArray[(int)BuffDebuff.Damage] = 0;
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
        m_stateArray = card.GetEffect();
        SetCondisionTurn(m_stateArray);
        //if (m_stateArray[(int)BuffDebuff.Vulnerable] > 0)
        //{
        //    m_block += Parsent(m_stateArray[(int)BuffDebuff.Block], 25);
        //}
        //else
        //{
        //    m_block += m_stateArray[(int)BuffDebuff.Block];
        //}
        SetText();
    }
}
