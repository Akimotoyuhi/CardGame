﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Player : CharactorBase, IDrop
{
    private int m_defaultCost = 5;
    private int m_cost = default;
    private int m_defaultDrowNum = 5;
    private int m_drowNum = default;
    public int Cost { get { return m_cost; } set { m_cost = value; } }
    public int DrowNum { get { return m_drowNum; } set { m_drowNum = value; } }

    void Start()
    {
        SetUp();
    }

    public override void TurnStart()
    {
        m_cost = m_defaultCost;
        base.TurnStart();
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
        damage *= -1; //ブロック値計算の後ダメージの符号が反転するので戻す
        if (damage < 0) { }
        else
        {
            Debug.Log("GetDamage" + damage);
            m_hp -= damage;
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
        foreach (var item in card.Conditions)
        {
            m_conditions.Add(item);
        }
        m_block += card.OnCast().Block;
        SetUI();
    }
}
