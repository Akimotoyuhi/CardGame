using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// バフデバフ
/// </summary>
public enum BuffDebuff
{
    /// <summary>脱力:与えるダメージが25%低下</summary>
    Weakness,
    /// <summary>脆弱化:得るブロックが25%低下</summary>
    Vulnerable,
    /// <summary>筋力:与えるダメージが+X</summary>
    Strength,
    /// <summary>敏捷性:得るブロックが+X</summary>
    Agile,
    end,
}

public enum EventTiming
{
    TurnBegin,
    TurnEnd,
    Hit,
    Attacked
}

/// <summary>
/// バフデバフ関係の基底クラス
/// </summary>
public abstract class Condition : ICondition
{
    public int m_turn = default;
    /// <summary> 効果 </summary>
    /// <param name="eventTiming">評価されるタイミング</param>
    /// <param name="num">影響を受ける数値</param>
    /// <returns>計算後の数値</returns>
    public abstract int Effect(EventTiming eventTiming, int num = 0);
    /// <summary>バフかデバフかの判定</summary>
    /// <returns>0ならバフ、1ならデバフ、2ならそれ以外</returns>
    public abstract int IsBuff();




    /*
    public class Weakness
    {
        public int turn = 0;
        private float parsent = 0.25f;
        public int GetEffect(int damage)
        {
            if (turn > 0)
            {
                float ret = damage * (1 - parsent);
                return (int)ret;
            }
            return damage;
        }
        public void Dec()
        {
            turn--;
        }
    }
    public class Vulnerable
    {
        public int turn = 0;
        private float parsent = 0.25f;
        public int GetEffect(int damage)
        {
            if (turn > 0)
            {
                float ret = damage * (1 - parsent);
                return (int)ret;
            }
            return damage;
        }
        public void Dec()
        {
            turn--;
        }
    }
    public class Strength
    {
        private int power = 0;
        public int SetTurn { set => power = value; }
        public int GetEffect(int damage) { return damage + power; }
    }
    public class Agile
    {
        private int power = 0;
        public int SetTurn { set => power = value; }
        public int GetEffect(int block) { return block + power; }
    }

    /// <summary>
    /// 持続ターン数を減らす用
    /// </summary>
    /// <param name="nums">state</param>
    public void Decrement()
    {
        if (weakness.turn > 0) weakness.turn--;
        if (vulnerable.turn > 0) vulnerable.turn--;
    }
    /// <summary>
    /// 攻撃時に効果が出るもの
    /// </summary>
    /// <returns></returns>
    public int AtAttack(int power)
    {
        int ret = weakness.GetEffect(power);
        ret = strength.GetEffect(ret);
        return ret;
    }
    /// <summary>
    /// 被弾時に効果が出るもの
    /// </summary>
    /// <returns></returns>
    public int AtDamage(int damage)
    {
        int ret = agile.GetEffect(damage);
        return ret;
    }
    /// <summary>
    /// ターン開始時に効果が出るもの
    /// </summary>
    public void AtTurnBigin()
    {
        Debug.Log("AtTurnBigin");
    }
    /// <summary>
    /// ターン終了時に効果が出るもの
    /// </summary>
    public void AtTurnEnd()
    {
        Debug.Log("AtTurnEnd");
    }
    */
}