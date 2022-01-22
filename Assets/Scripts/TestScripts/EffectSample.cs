using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EffectSample : MonoBehaviour
{
    private bool m_flag = false;

    void Start()
    {
        //Vector2 v = gameObject.GetRectTransform().anchoredPosition;
        //EffectManager.Instance.ViewText("aaaa", Vector2.zero, transform);
        //EffectManager.Instance.MoveText("Move", Vector2.zero, transform);
        StartCoroutine(SampleAsync());
    }

    private IEnumerator SampleAsync()
    {
        EffectManager.Instance.MoveText("Move", Vector2.zero, transform, new Vector2(transform.position.x + 100, transform.position.y - 200), 1f, () => m_flag = true);
        while (!m_flag)
        {
            yield return null;
        }
        Debug.Log("コルーチンを出た");
    }
}
