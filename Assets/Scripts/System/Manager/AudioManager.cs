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
    [SerializeField] AudioSource m_source;
    [SerializeField] float m_fadeDuration;
    [SerializeField] AudioClip[] m_bgm;
    [SerializeField] SEAudio m_seAudioPrefab;
    private List<SEAudio> m_seSources;
    private BGM m_nowPlaying;
    /// <summary>現在再生中のBGM</summary>
    public BGM NowPlaying => m_nowPlaying;
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
        m_nowPlaying = bgm;
        if (m_source.clip)//既にclipが入ってたらフェードする
        {
            m_source.DOFade(0f, m_fadeDuration).OnComplete(() =>
            {
                if (bgm == BGM.None)
                {
                    m_source.Stop();
                    m_source.clip = null;
                }
                else
                {
                    m_source.clip = m_bgm[(int)bgm];
                    m_source.DOFade(1f, m_fadeDuration).OnComplete(() => m_source.Play());
                }
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
    None = -1,
    Battle1,
    Battle2,
    Boss1,
}
public enum SE
{

}