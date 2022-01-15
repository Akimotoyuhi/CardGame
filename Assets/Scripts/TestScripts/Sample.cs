using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトに付けるクラス
/// </summary>
public class Sample : MonoBehaviour
{
    [SerializeReference, SubclassSelector]
    ISample sample = default;
}

/// <summary>
/// シリアライズ対象のインターフェース
/// </summary>
public interface ISample
{
    int Execute();
}
public class A : ISample
{
    [Header("Aクラス")]
    [SerializeField] int a;

    public int Execute()
    {
        return a;
    }
}
public class B : ISample
{
    [Header("Bクラス")]
    [SerializeField] int b;

    public int Execute()
    {
        return b;
    }
}