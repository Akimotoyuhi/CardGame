using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TitleMove : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private Vector2 m_defpos;

    public void OnDrop(PointerEventData eventData)
    {
        var result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);
        foreach (var hit in result)
        {
            var v = hit.gameObject.GetComponent<OnDropTitleStateChange>();
            if (!v) continue;
            v.OnDrop();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_defpos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.DOMove(m_defpos, 0.1f);
    }
}
