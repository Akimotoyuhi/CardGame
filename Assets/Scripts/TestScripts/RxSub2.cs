using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

/// <summary>
/// UniRxオブザーバーを使う側のサンプルクラス
/// </summary>
public class RxSub2 : MonoBehaviour
{
    [SerializeField] RxObserverTest m_test;

    void Start()
    {
        m_test.Subject.Subscribe(num =>
        {
            Debug.Log($"awhoahfouhad{num}");
        });
    }

    void Update()
    {
        
    }
}
