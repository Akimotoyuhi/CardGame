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