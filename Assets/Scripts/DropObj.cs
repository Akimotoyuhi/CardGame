using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropObj : MonoBehaviour,IDropHandler
{
    //ドロップしたいオブジェクトに付ける
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name}に{eventData.pointerDrag.name}がドロップされました");
    }
}
