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
        Random.InitState(m_seed);
        List<int> vs = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            int r = Random.Range(0, 100);
            vs.Add(r);
        }
        foreach (var item in vs)
        {
            Debug.Log(item);
        }
    }
}