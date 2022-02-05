using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �\���̃e�X�g�p
/// </summary>
public struct Parametor
{
    /// <summary>
    /// �U����
    /// </summary>
    private int power;
    /// <summary>
    /// �h���
    /// </summary>
    private int defence;
    /// <summary>
    /// �o�t�f�o�t��Ԉُ�
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
        //struct�̂���{�Ƃ���Vector���g���̂Œu���Ă���
        Vector2 v = new Vector2();
        return v;
    }
    /// <summary>
    /// �󂯎�������l��n�����̒l�������_�ȉ��؂�̂Ă̐����ŕԂ��܂�
    /// </summary>
    /// <param name="num">���Z���鐔�l</param>
    /// <param name="percentage">�������炷��</param>
    /// <returns>�v�Z��̐��l</returns>
    public static int Percentage(float num, float percentage)
    {
        percentage /= 100;
        float t = num * (1 - percentage);
        return (int)t;
    }
    /// <summary>
    /// �󂯎�������l��n�����̒l�𕂓������_���ŕԂ��܂�
    /// </summary>
    /// <param name="num">�������炷��</param>
    /// <param name="percentage">�v�Z��̐��l</param>
    /// <returns></returns>
    public static float PerceFloat(float num, float percentage)
    {
        percentage /= 100;
        float ret = num * (1 - percentage);
        return ret;
    }
}
