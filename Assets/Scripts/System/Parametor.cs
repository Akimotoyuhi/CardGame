using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �퓬�Ŏg���p�����[�^�[����ɂ܂Ƃ߂��\����
/// </summary>
public struct Parametor
{
    /// <summary>�U��</summary>
    public int Attack { get; set; }
    /// <summary>�U����</summary>
    public int AttackNum { get; set; }
    /// <summary>�u���b�N</summary>
    public int Block { get; set; }
    /// <summary>�u���b�N��</summary>
    public int BlockNum { get; set; }
    /// <summary>�t�^����o�t�f�o�t</summary>
    public List<Condition> Conditions { get; set; }
}