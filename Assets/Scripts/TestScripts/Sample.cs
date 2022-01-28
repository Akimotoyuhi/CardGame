using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// オブジェクトに付けるクラス
/// </summary>
public class Sample : MonoBehaviour
{
    private void Test<T>(T a, T b)
    {
        
    }

    private void AAAA()
    {
        Test<int>(1, 2);
    }
}