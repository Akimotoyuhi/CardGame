using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Command
{
    /// <summary>�U��</summary>
    public int Power { get; set; }
    /// <summary>�u���b�N</summary>
    public int Block { get; set; }
    /// <summary>�̗�</summary>
    public int Life { get; set; }
    /// <summary>�t�^����o�t�f�o�t</summary>
    public List<Condition> Conditions { get; set; }
}