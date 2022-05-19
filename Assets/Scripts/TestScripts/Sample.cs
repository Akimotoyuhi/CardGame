using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
using DG.Tweening;

/// <summary>
/// オブジェクトに付けるテスト用のクラス<br/>
/// 試したい小さい機能を動かす為のものなので随時書き換わる
/// </summary>
public class Sample : MonoBehaviour
{
    CommandParam m_cp = CommandParam.AddCard;

    private void Start()
    {
        Debug.Log(m_cp);
        AAAA(ref m_cp);
        Debug.Log(m_cp);
    }

    private void AAAA(ref CommandParam cp)
    {
        cp = CommandParam.Attack;
    }
}