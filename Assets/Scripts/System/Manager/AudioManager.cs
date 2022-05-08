using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] float m_fadeDuration;
    [SerializeField] AudioClip[] m_bgm;
    [SerializeField] AudioSource m_aSource;
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
        if (m_aSource.clip)//����clip�������Ă���t�F�[�h����
        {
            m_aSource.DOFade(0f, m_fadeDuration).OnComplete(() =>
            {
                m_aSource.clip = m_bgm[(int)bgm];
                m_aSource.DOFade(1f, m_fadeDuration).OnComplete(() => m_aSource.Play());
            });
        }
        else
        {
            m_aSource.clip = m_bgm[(int)bgm];
            m_aSource.Play();
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