using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>SE�炷�p�N���X</summary>
[RequireComponent(typeof(AudioSource))]
public class SEAudio : MonoBehaviour
{
    [SerializeField] AudioSource m_source;
    private bool m_isFinishd = false;
    /// <summary>Audio�̏I������</summary>
    public bool IsFinishd { get => m_isFinishd; }

    public void PlayAudio(AudioClip clip)
    {
        m_isFinishd = false;
        float time = clip.length;
        Debug.Log($"�Đ�����{time}, AudioName{clip.name}");
        m_source.PlayOneShot(clip);
        Debug.Log("�Đ�");
        DOVirtual.DelayedCall(time, () =>
        {
            Debug.Log("��~");
            m_isFinishd = true;
        });
    }
}
