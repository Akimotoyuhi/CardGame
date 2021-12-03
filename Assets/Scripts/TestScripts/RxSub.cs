using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class RxSub : MonoBehaviour
{
    [SerializeField] RxObserverTest m_test;

    void Start()
    {
        m_test.Subject.Subscribe(i =>
        {
            Debug.Log(i);
        });
    }
}
