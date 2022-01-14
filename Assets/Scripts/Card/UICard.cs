using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 報酬画面やショップに並ばせる用の効果が無いカード
/// </summary>
public class UICard : MonoBehaviour
{
    [SerializeField] Text m_viewName;
    [SerializeField] Image m_viewImage;
    [SerializeField] Text m_viewCost;
    [SerializeField] Text m_viewTooltip;
    private int id;

    void Start()
    {
        
    }

    public void Setup(NewCardData cardData, int id)
    {

    }
}
