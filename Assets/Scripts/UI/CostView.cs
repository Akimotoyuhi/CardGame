using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CostView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, TextArea] string m_text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        EffectManager.Instance.SetUIText(PanelType.Battle, m_text, Color.black);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EffectManager.Instance.RemoveUIText(PanelType.Battle);
    }
}
