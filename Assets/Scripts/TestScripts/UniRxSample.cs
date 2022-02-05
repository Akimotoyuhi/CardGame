using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// UniRxの理解深めようクラス
/// </summary>
public class UniRxSample : MonoBehaviour
{
    void Start()
    {
        //三秒後にログを出力する
        Observable.Timer(TimeSpan.FromSeconds(3)).Subscribe((l) =>
        {
            Debug.Log("3秒経った");
        }).AddTo(this);
    }
}
