using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 左右に動かすだけ<br/>
/// アニメーションだと挙動がおかしくなったから書くことにした
/// </summary>
public class LRMove : MonoBehaviour
{
    [SerializeField] RectTransform m_rectTransform;
    [SerializeField] float m_xMove;
    [SerializeField] float m_onDuration;
    [SerializeField] float m_returnDuration;
    private Vector2 m_defpos;

    void Start()
    {
        Move();
    }

    private void Move()
    {
        m_defpos = m_rectTransform.anchoredPosition;
        Sequence s = DOTween.Sequence();
        s.Append(m_rectTransform.DOAnchorPosX(m_defpos.x + m_xMove, m_onDuration))
            .Append(m_rectTransform.DOAnchorPosX(m_defpos.x, m_returnDuration))
            .SetLoops(-1);
    }
}
