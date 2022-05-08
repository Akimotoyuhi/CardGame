using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float m_volume;
    [SerializeField] AudioClip[] m_bgm;
    [SerializeField] AudioSource m_aSource;
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_aSource.volume = m_volume;
    }

    /// <summary>BGMÇÃçƒê∂</summary>
    public void Play(BGM bgm)
    {
        m_aSource.clip = m_bgm[(int)bgm];
        m_aSource.Play();
    }

    /// <summary>SEÇÃçƒê∂</summary>
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