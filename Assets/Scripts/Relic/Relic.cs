using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// レリックの実体
/// </summary>
public class Relic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] Image m_image;
    private string m_name;
    private string m_tooltip;
    private int m_triggerCount;
    private RelicID m_id;
    private Reward m_reward;
    private List<int[]> m_commands;
    private List<RelicConditional> m_conditional;

    public void Setup(RelicDataBase dataBase)
    {
        if (dataBase != null)
        {
            SetParam(dataBase);
        }
        m_triggerCount = 0;
    }

    public void Setup(RelicDataBase dataBase, Reward reward)
    {
        if (dataBase != null)
        {
            SetParam(dataBase);
        }
        m_reward = reward;
    }

    private void SetParam(RelicDataBase dataBase)
    {
        m_name = dataBase.Name;
        m_tooltip = dataBase.Tooltip;
        m_image.sprite = dataBase.Sprite;
        m_commands = dataBase.Commands.Command;
        m_conditional = dataBase.Commands.Conditional;
        m_id = dataBase.RelicID;
    }

    public void Execute(RelicTriggerTiming triggerTiming, ParametorType parametorType, int num)
    {
        foreach (var cond in m_conditional)
        {
            //if (cond.Evaluation())
            if (!cond.Conditional(m_triggerCount, triggerTiming, parametorType, num))
            {
                return;
            }
        }
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

    public void OnPointerDown(PointerEventData eventData)
    {
        //これが無いとOnPointerUpが呼ばれないので
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (m_reward)
        {
            m_reward.OnClickRelic(m_id);
            m_reward = null;
            EffectManager.Instance.RemoveUIText(PanelType.Info);
        }
    }
    #endregion
}
