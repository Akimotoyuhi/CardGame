using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// UniRx�̗���[�߂悤�N���X
/// </summary>
public class UniRxSample : MonoBehaviour
{
    void Start()
    {
        //�O�b��Ƀ��O���o�͂���
        Observable.Timer(TimeSpan.FromSeconds(3)).Subscribe((l) =>
        {
            Debug.Log("3�b�o����");
        }).AddTo(this);
    }
}
