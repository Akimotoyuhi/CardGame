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
    private Sequence m_sequence;

    void Start()
    {
        //GameManager.Instance.OnSceneReload.Subscribe(_ => Dokill(0));
        Move();
    }

    private void Move()
    {
        m_defpos = m_rectTransform.anchoredPosition;
        m_sequence = DOTween.Sequence();
        m_sequence.Append(m_rectTransform.DOAnchorPosX(m_defpos.x + m_xMove, m_onDuration))
            .Append(m_rectTransform.DOAnchorPosX(m_defpos.x, m_returnDuration))
            .SetLoops(-1);
    }

    private int Dokill(int i)
    {
        DOTween.KillAll();
        return 0;
    }
}
