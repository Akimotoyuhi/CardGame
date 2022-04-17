using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// レリックの実体
/// </summary>
public class Relic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image m_image;
    private string m_name;
    private string m_tooltip;
    private int m_triggerCount;
    private List<int[]> m_commands;
    private List<RelicConditional> m_conditional;

    public void Setup(RelicDataBase dataBase = null)
    {
        if (dataBase != null)
        {
            m_name = dataBase.Name;
            m_tooltip = dataBase.Tooltip;
            m_image.sprite = dataBase.Sprite;
            m_commands = dataBase.Commands.Command;
            m_conditional = dataBase.Commands.Conditional;
        }
        m_triggerCount = 0;
    }

    public void Execute(RelicTriggerTiming triggerTiming, ParametorType parametorType)
    {
        foreach (var cond in m_conditional)
        {
            if (!cond.Conditional(m_triggerCount, triggerTiming, parametorType))
            {
                Debug.Log("false");
                return;
            }
        }
        Debug.Log("true");
        m_triggerCount++;
        BattleManager.Instance.CommandManager.CommandExecute(m_commands, false);
    }

    #region インターフェースの実装
    public void OnPointerEnter(PointerEventData eventData)
    {
        EffectManager.Instance.SetUIText(PanelType.Info, $"{m_name}\n{m_tooltip}", Color.black);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EffectManager.Instance.RemoveUIText(PanelType.Info);
    }
    #endregion
}
