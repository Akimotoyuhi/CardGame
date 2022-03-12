using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemiesTarget : MonoBehaviour, IDrop, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject m_flame;
    public void Setup()
    {
        m_flame.SetActive(false);
    }
    public bool CanDrop(UseType useType)
    {
        if (useType == UseType.ToAllEnemies) return true;
        else return false;
    }
    public void GetDrop(List<int[]> cardCommand)
    {
        BattleManager.Instance.DropManager.CardExecute(cardCommand);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("PointerEnter");
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);
        foreach (var hit in result)
        {
            BlankCard card = hit.gameObject.GetComponent<BlankCard>();
            if (card)
            {
                Debug.Log("OnCard");
                m_flame.SetActive(true);
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        m_flame.SetActive(false);
    }
}
