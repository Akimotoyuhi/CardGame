using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Audioを管理するクラス
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] float m_fadeDuration;
    [SerializeField] AudioClip[] m_bgm;
    [SerializeField] SEAudio m_seAudioPrefab;
    [SerializeField] AudioSource m_source;
    private List<SEAudio> m_seSources;
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
    }

    /// <summary>BGMの再生</summary>
    public void Play(BGM bgm)
    {
        if (m_source.clip)//既にclipが入ってたらフェードする
        {
            m_source.DOFade(0f, m_fadeDuration).OnComplete(() =>
            {
                m_source.clip = m_bgm[(int)bgm];
                m_source.DOFade(1f, m_fadeDuration).OnComplete(() => m_source.Play());
            });
        }
        else
        {
            m_source.clip = m_bgm[(int)bgm];
            m_source.Play();
        }
    }

    /// <summary>SEの再生</summary>
    public void Play(SE se)
    {
    }
}

public enum BGM
{
    Battle1,
    Battle2,
    Boss1,
}
public enum SE
{

}