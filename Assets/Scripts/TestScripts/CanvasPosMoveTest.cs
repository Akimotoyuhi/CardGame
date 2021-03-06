using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 3D座標と2D座標での移動の挙動の違い検証用
/// </summary>
public class CanvasPosMoveTest : MonoBehaviour
{
    [SerializeField] Image m_image;
    [SerializeField] Image m_image2;

    void Start()
    {
        Sequence sequence = DOTween.Sequence();
        //sequence.Append(m_rect.DOAnchorPos(new Vector2(500, 500), 1f));//ちゃんと動いた
        RectTransform r = m_image.GetComponent<RectTransform>();
        sequence.Append(r.DOAnchorPos(new Vector2(500, 500), 1f));
        sequence = DOTween.Sequence();
        RectTransform r2 = m_image2.GetComponent<RectTransform>();
        sequence.Append(r2.DOAnchorPos(new Vector2(-500, -500), 1f));
    }

    void Update()
    {

    }
}
