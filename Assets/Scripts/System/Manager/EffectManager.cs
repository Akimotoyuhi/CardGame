using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エフェクトやってくれるクラス
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
