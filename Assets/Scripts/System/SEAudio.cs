using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

/// <summary>SE鳴らす用クラス</summary>
[RequireComponent(typeof(AudioSource))]
public class SEAudio : MonoBehaviour
{
    [SerializeField] AudioSource m_source;
    private bool m_isFinishd = false;
    private float m_time;
    /// <summary>Audioの終了判定</summary>
    public bool IsFinishd { get => m_isFinishd; }

    public void PlayAudio(AudioClip clip, Action complete)
    {
        m_isFinishd = false;
        float time = clip.length;
        m_source.Play();
        DOVirtual.DelayedCall(m_time, () =>
        {
            m_isFinishd = true;
            complete();
        });
    }
}
