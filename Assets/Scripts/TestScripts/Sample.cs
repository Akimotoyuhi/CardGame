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
    private int x;

    private void Start()
    {
        AAA aaa = new AAA();
        Debug.Log(aaa.AX);
        x = aaa.AX;
        x *= 2;
        Debug.Log(aaa.AX);
        //GameObject obj = Instantiate(m_image);
        //obj.transform.SetParent(m_paraent);
        //obj.GetRectTransform().position = Vector2.zero;
    }
}
public class AAA
{
    public int AX { get; set; } = 5;
}