using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�t�F�N�g����Ă����N���X
/// </summary>
public class EffectManager : MonoBehaviour
{

    public static EffectManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }
}
