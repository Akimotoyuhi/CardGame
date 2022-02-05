using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵行動クラスが正しく動くかのテスト用
/// </summary>
public class EnemyTreeTest : MonoBehaviour
{
    [SerializeField] int m_life = default;

    void Start()
    {
        
    }

    public bool Action(int turn)
    {
        switch (turn)
        {
            case 0:
                Debug.Log("0ターン目の行動");
                return true;
            default:
                Debug.Log("なんもしない");
                return true;
        }
    }
}
