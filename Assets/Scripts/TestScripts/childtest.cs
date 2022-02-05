using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子オブジェクトにする処理が分からんくなったのでテスト用
/// </summary>
public class Childtest : MonoBehaviour
{
    [SerializeField] private Transform m_target;
    void Start()
    {
        transform.GetChild(0).parent = m_target;
    }
}
