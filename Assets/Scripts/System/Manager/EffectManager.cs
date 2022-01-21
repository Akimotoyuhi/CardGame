using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �G�t�F�N�g����Ă����N���X
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
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(500, 500);
        rt.position = new Vector2(500, 500);
    }
}
