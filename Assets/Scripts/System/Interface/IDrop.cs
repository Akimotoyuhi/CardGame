using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カードのドロップを受け付ける
/// </summary>
public interface IDrop
{
    /// <summary>ドロップされた時の処理</summary>
    void GetDrop(int power, int block, Condition condition);
    /// <summary>ドロップ可否</summary>
    bool CanDrop(UseType useType);
}
