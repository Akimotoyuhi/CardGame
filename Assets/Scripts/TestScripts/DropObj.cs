using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// マウスクリックで移動させたimageがドロップされた事を判定するテスト
/// </summary>
public class DropObj : MonoBehaviour,IDropHandler
{
    //ドロップしたいオブジェクトに付ける
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name}に{eventData.pointerDrag.name}がドロップされました");
    }
}
