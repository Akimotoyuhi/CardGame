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
    [SerializeField] AudioClip[] m_bgmClips;
    [SerializeField] AudioClip[] m_seClips;
    [SerializeField] SEAudio m_seAudioPrefab;
    private List<SEAudio> m_sePools = new List<SEAudio>();
    private BGM m_nowPlaying;
    /// <summary>現在再生中のBGM</summary>
    public BGM NowPlaying => m_nowPlaying;
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>BGMの再生</summary>
    public void Play(BGM bgm)
    {
        m_nowPlaying = bgm;
        if (m_source.clip)//既にclipが入ってたらフェードする
        {
            float v = m_source.volume;
            m_source.DOFade(0f, m_fadeDuration).OnComplete(() =>
            {
                if (bgm == BGM.None)
                {
                    m_source.Stop();
                    m_source.clip = null;
                }
                else
                {
                    m_source.clip = m_bgmClips[(int)bgm];
                    m_source.DOFade(v, m_fadeDuration).OnComplete(() => m_source.Play());
                }
            });
        }
        else
        {
            m_source.clip = m_bgmClips[(int)bgm];
            m_source.Play();
        }
    }

    /// <summary>SEの再生</summary>
    public void Play(SE se)
    {
        if (se == SE.None)
            return;
        foreach (var item in m_sePools)
        {
            if (item.IsFinishd)//再生済みSEがあるならそれを使う
            {
                item.PlayAudio(m_seClips[(int)se]);
                return;
            }
        }
        SEAudio audio = Instantiate(m_seAudioPrefab);
        m_sePools.Add(audio);
        audio.PlayAudio(m_seClips[(int)se]);
    }
}

public enum BGM
{
    None = -1,
    Battle1,
    Battle2,
    Battle3,
    Boss1,
    Boss2,
    Boss3,
}
public enum SE
{
    None = -1,
    System,
}