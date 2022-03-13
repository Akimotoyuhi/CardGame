using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カードのドロップを受け付ける
/// </summary>
public interface IDrop
{
    /// <summary>ドロップされた時の処理</summary>
    void GetDrop(List<int[]> cardCommand);
    /// <summary>ドロップを受け付けるか否か</summary>
    bool CanDrop(UseType useType);
    void OnCard(UseType? useType);
}
