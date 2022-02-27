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
    public int m_seed;

    private void Start()
    {
        int[] nums = new int[10];
        for (int i = 0; i < nums.Length; i++)
        {
            nums[i] = UnityEngine.Random.Range(0, 100);
        }
        foreach (var item in nums)
        {
            Debug.Log(item);
        }
    }
}