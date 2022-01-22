using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// エフェクトやってくれるクラス<br/>
/// 演出用のテキストやパーティクルを表示してくれる
/// </summary>
public class EffectManager : MonoBehaviour
{
    [SerializeField] RectTransform Text;

    public static EffectManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gameObject.GetRectTransform().anchoredPosition = new Vector2(500, 500);
    }
}
