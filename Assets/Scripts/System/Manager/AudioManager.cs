using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Audio���Ǘ�����N���X
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

    /// <summary>BGM�̍Đ�</summary>
    public void Play(BGM bgm)
    {
        if (m_source.clip)//����clip�������Ă���t�F�[�h����
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

    /// <summary>SE�̍Đ�</summary>
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