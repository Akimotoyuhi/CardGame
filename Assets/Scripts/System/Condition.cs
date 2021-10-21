using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バフデバフ
/// </summary>
public enum BuffDebuff
{
    Damage,
    Block,
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

/// <summary>
/// バフデバフ関係
/// </summary>
public class Condition
{
    public Weakness weakness = new Weakness();
    public Vulnerable vulnerable = new Vulnerable();
    public Strength strength = new Strength();
    public Agile agile = new Agile();
    //public interface IAttacked { int GetEffect(int value); }
    //public interface IDamaged { int GetEffect(int value); }
    //public interface ITurnBegin { }
    //public interface ITurnEnd { }
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
            turn = 0;
            return damage;
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
            turn = 0;
            return damage;
        }
    }
    public class Strength
    {
        private int power = 0;
        public int SetTurn { set { power = value; } }
        public int GetEffect(int damage) { return damage + power; }
    }
    public class Agile
    {
        private int power = 0;
        public int SetTurn { set { power = value; } }
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
}