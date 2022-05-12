using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 継承先をオブジェクトプールに対応させるインターフェース
/// </summary>
public interface IPool
{
    bool IsFinishd { get; }
    void Execute();
}
