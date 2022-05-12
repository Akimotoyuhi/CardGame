using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>SE鳴らす用クラス</summary>
[RequireComponent(typeof(AudioSource))]
public class SEAudio : MonoBehaviour
{
    [SerializeField] AudioSource m_source;
    private bool m_isFinishd = false;
    /// <summary>Audioの終了判定</summary>
    public bool IsFinishd { get => m_isFinishd; }

    /// <summary>再生</summary>
    public void PlayAudio(AudioClip clip)
    {
        m_isFinishd = false;
        float time = clip.length;
        m_source.PlayOneShot(clip);
        DOVirtual.DelayedCall(time, () => m_isFinishd = true);
    }
}
