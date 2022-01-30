using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// オブジェクトに付けるテスト用のクラス
/// </summary>
public class Sample : MonoBehaviour
{
    [SerializeField] GameObject m_image;
    [SerializeField] Transform m_paraent;

    private void Start()
    {
        GameObject obj = Instantiate(m_image);
        obj.transform.SetParent(m_paraent);
        obj.GetRectTransform().position = Vector2.zero;
    }
}