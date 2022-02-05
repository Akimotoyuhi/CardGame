using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 構造体テスト用
/// </summary>
public struct Parametor
{
    /// <summary>
    /// 攻撃力
    /// </summary>
    private int power;
    /// <summary>
    /// 防御力
    /// </summary>
    private int defence;
    /// <summary>
    /// バフデバフ状態異常
    /// </summary>
    private List<int> effectId;
    private List<int> effectTurn;
    public int Power { get => power; set => power = value; }
    public int Defence { get => defence; set => defence = value; }
    public List<int> EffectId { get => effectId; set => effectId = value; }
    public List<int> EffectTurn { get => effectTurn; set => effectTurn = value; }
    public Parametor(int power, int defence)
    {
        this.power = power;
        this.defence = defence;
        effectId = new List<int>();
        effectTurn = new List<int>();
    }
    private Vector2 Sample()
    {
        //structのお手本としてVectorを使うので置いておく
        Vector2 v = new Vector2();
        return v;
    }
    /// <summary>
    /// 受け取った数値のn％分の値を小数点以下切り捨ての整数で返します
    /// </summary>
    /// <param name="num">減算する数値</param>
    /// <param name="percentage">何％減らすか</param>
    /// <returns>計算後の数値</returns>
    public static int Percentage(float num, float percentage)
    {
        percentage /= 100;
        float t = num * (1 - percentage);
        return (int)t;
    }
    /// <summary>
    /// 受け取った数値のn％分の値を浮動小数点数で返します
    /// </summary>
    /// <param name="num">何％減らすか</param>
    /// <param name="percentage">計算後の数値</param>
    /// <returns></returns>
    public static float PerceFloat(float num, float percentage)
    {
        percentage /= 100;
        float ret = num * (1 - percentage);
        return ret;
    }
}
