using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManagement : MonoBehaviour
{
    [SerializeField] protected Transform m_cardParent;
    [SerializeField] RectTransform m_addCardMoveingAtPos;
    protected Canvas m_canvas = default;
    /// <summary>カードの親オブジェクト</summary>
    public Transform CardParent => m_cardParent;
    /// <summary>カードが追加された際に飛ばされるアニメーションの終着点</summary>
    public Vector2 AddCardMoveingAtPos => m_addCardMoveingAtPos.anchoredPosition;

    public virtual void GridLayoutGroupSetting(Vector2 size) { }
    /// <summary>
    /// このクラスの子オブジェクトを全て破棄する
    /// </summary>
    public void CardDelete()
    {
        for (int i = m_cardParent.childCount - 1; i >= 0; i--)
        {
            Destroy(m_cardParent.GetChild(i).gameObject);
        }
    }
    public void OnClick(bool flag)
    {
        if (!m_canvas) return;
        m_canvas.enabled = flag;
    }
    public virtual void OnPointer(bool flag) { }
}
