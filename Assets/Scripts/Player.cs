﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class Player : CharactorBase, IDrop
{
    /// <summary>デフォルトコスト</summary>
    private int m_defCost = 3;
    /// <summary>デフォルトドロー枚数</summary>
    private int m_defDrowNum = 5;
    private Sprite[] m_attackedSprite;
    private Sprite m_gameoverSprite;
    [SerializeField] int m_cost = default;
    [SerializeField] int m_drowNum = default;
    /// <summary>最大コスト</summary>
    public int MaxCost => m_defCost;
    /// <summary>現在コスト</summary>
    public int CurrrentCost { get => m_cost; set => m_cost = value; }
    /// <summary>カードをドローする枚数</summary>
    public int DrowNum { get => m_drowNum; set => m_drowNum = value; }
    public Sprite[] AttackedSprite => m_attackedSprite;
    public Sprite GameoverSprite => m_gameoverSprite;

    public override void TurnStart()
    {
        m_cost = OnBattleEffect(EventTiming.TurnBegin, ParametorType.Cost, m_defCost);
        m_drowNum = OnBattleEffect(EventTiming.TurnBegin, ParametorType.DrowNum, m_defDrowNum);
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
        SetUp();
    }

    /// <summary>
    /// 被ダメージ処理
    /// </summary>
    /// <param name="damage">被ダメージ</param>
    public void GetAcceptDamage(int power, int block, List<Condition> condition, ParticleID particleID)
    {
        Damage(power, block, null, true, particleID, false, () =>
        {
            m_image.sprite = m_gameoverSprite;
            GameManager.Instance.Gameover();
        });
        foreach (var item in condition)
        {
            Damage(0, 0, item, true, particleID, false, () =>
            {
                m_image.sprite = m_gameoverSprite;
                GameManager.Instance.Gameover();
            });
        }
    }

    protected override void LifeFluctuation(int value, bool isPlayer)
    {
        base.LifeFluctuation(value, isPlayer);
        if (m_life <= 0)
        {
            m_image.sprite = m_gameoverSprite;
            GameManager.Instance.Gameover();
        }
    }

    public bool CanDrop(UseTiming useType)
    {
        if (useType == UseTiming.ToPlayer) return true;
        return false;
    }

    public void GetDrop(List<int[]> cardCommand)
    {
        BattleManager.Instance.CommandManager.CommandExecute(cardCommand, true);
    }

    public void OnCard(UseTiming? useType)
    {
        if (useType == UseTiming.ToPlayer) m_flame.SetActive(true);
        else m_flame.SetActive(false);
    }
    public EnemyBase IsEnemy()
    {
        return null;
    }

    public override void GetDamage(int[] cardParam, ParticleID particleID)
    {
        CommandParam command = (CommandParam)cardParam[(int)CommonCmdEnum.CommandParam];
        bool b;
        switch (command)
        {
            case CommandParam.Attack:
                b = cardParam[(int)AttackCmdEnum.TrueDmg] == 1 ? true : false;
                Damage(cardParam[(int)AttackCmdEnum.Power], 0, null, true, particleID, b, () =>
                {
                    m_image.sprite = m_gameoverSprite;
                    GameManager.Instance.Gameover();
                });
                break;
            case CommandParam.Block:
                b = cardParam[(int)BlockCmdEnum.TrueBlk] == 1 ? true : false;
                Damage(0, cardParam[(int)BlockCmdEnum.Block], null, true, particleID, b, () =>
                {
                    m_image.sprite = m_gameoverSprite;
                    GameManager.Instance.Gameover();
                });
                break;
            case CommandParam.Conditon:
                ConditionSelection cs = new ConditionSelection();
                Damage(0, 0, cs.SetCondition((ConditionID)cardParam[(int)ConditionCmdEnum.ConditionID], cardParam[(int)ConditionCmdEnum.Turn]), true, particleID, false, () =>
                {
                    m_image.sprite = m_gameoverSprite;
                    GameManager.Instance.Gameover();
                });
                break;
            case CommandParam.Heal:
                LifeFluctuation(cardParam[(int)HeadCmdEnum.Value], true);
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