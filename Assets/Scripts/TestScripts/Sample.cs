using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
using DG.Tweening;

/// <summary>
/// �I�u�W�F�N�g�ɕt����e�X�g�p�̃N���X<br/>
/// ���������������@�\�𓮂����ׂ̂��̂Ȃ̂Ő������������
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