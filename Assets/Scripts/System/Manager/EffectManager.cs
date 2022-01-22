using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �G�t�F�N�g����Ă����N���X<br/>
/// ���o�p�̃e�L�X�g��p�[�e�B�N����\�����Ă����
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
