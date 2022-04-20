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
    bool CanDrop(UseTiming useType);
    /// <summary>カードがドロップ可能であることを強調表示する時の判定</summary>
    void OnCard(UseTiming? useType);
    EnemyBase IsEnemy();
}
