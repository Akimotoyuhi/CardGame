using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>敵に対して何かするカードに付けるインターフェース</summary>
public interface IAttackCard
{
    int GetDamage();
}

/// <summary>プレイヤー対して何かするカードに付けるインターフェース</summary>
public interface IBuffCard
{
    int GetBlock();
}
