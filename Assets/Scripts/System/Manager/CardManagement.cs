using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManagement : MonoBehaviour
{
    [SerializeField] protected Transform m_cardParent;
    public Transform CardParent => m_cardParent;

    /// <summary>
    /// このクラスの子オブジェクトを全て破棄する
    /// </summary>
    public void CardDelete()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(m_cardParent.GetChild(i).gameObject);
        }
    }
    //public void SetParent(Transform child)
    //{
    //    child.SetParent(m_cardParent, false);
    //}
}
