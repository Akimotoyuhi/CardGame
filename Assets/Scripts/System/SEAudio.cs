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

    public void PlayAudio(AudioClip clip)
    {
        m_isFinishd = false;
        float time = clip.length;
        Debug.Log($"再生時間{time}, AudioName{clip.name}");
        m_source.PlayOneShot(clip);
        Debug.Log("再生");
        DOVirtual.DelayedCall(time, () =>
        {
            Debug.Log("停止");
            m_isFinishd = true;
        });
    }
}
