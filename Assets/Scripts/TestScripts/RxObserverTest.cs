using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// UniRx�I�u�U�[�o�[�̃e�X�g�p
/// </summary>
public class RxObserverTest : MonoBehaviour
{
    Subject<int> m_subject = new Subject<int>();
    private int num = 0;

    public IObservable<int> Subject => m_subject;

    public void OnClick()
    {
        m_subject.OnNext(num);
        num++;
    }
}
