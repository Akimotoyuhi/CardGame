using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UniRxのオブジェクトプールを使う用クラス
/// </summary>
public class PoolTestManager : MonoBehaviour
{
    [SerializeField] PoolObj m_prefab;
    PoolTest m_poolTest;

    private void Start()
    {
        m_poolTest = new PoolTest(m_prefab);
    }
}
