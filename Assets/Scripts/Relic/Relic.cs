using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// レリックの実体
/// </summary>
public class Relic : MonoBehaviour
{
    [SerializeField] Image m_image;
    [SerializeField] Text m_tooltipText;
    private string m_name;
    private string m_tooltip;
    private List<int[]> m_commands;
    private List<RelicConditional> m_conditional;

    public void Setup(RelicDataBase dataBase)
    {
        m_name = dataBase.Name;
        m_tooltip = dataBase.Tooltip;
        m_image.sprite = dataBase.Sprite;
        m_commands = dataBase.Commands.Command;
        m_conditional = dataBase.Commands.Conditional;
    }

    /// <summary>
    /// データを受け取る
    /// </summary>
    /// <param name="relicDataBase"></param>
    private void SetData(RelicDataBase relicDataBase)
    {
        m_name = relicDataBase.Name;
        m_tooltip = relicDataBase.Tooltip;
    }
}
