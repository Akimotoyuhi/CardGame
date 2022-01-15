using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

/// <summary>
/// 報酬画面やショップに並ばせる用の効果が無いカード
/// </summary>
public class UICard : MonoBehaviour
{
    [SerializeField] Text m_viewName;
    [SerializeField] Image m_viewImage;
    [SerializeField] Text m_viewCost;
    [SerializeField] Text m_viewTooltip;
    private CardID m_id;

    public void Setup(NewCardDataBase cardData, CardID id)
    {
        m_viewName.text = cardData.Name;
        m_viewImage.sprite = cardData.Sprite;
        m_viewCost.text = cardData.Cost;
        m_viewTooltip.text = SetTooltip(cardData.Tooltip);
        m_id = id;
    }

    private string SetTooltip(string text)
    {
        string ret = text;
        MatchCollection matches = Regex.Matches(ret, "{%atk([0-9]*)}");
        foreach (Match m in matches)
        {
            int num = int.Parse(m.Groups[1].Value);
            ret = ret.Replace(m.Value, num.ToString());
        }
        matches = Regex.Matches(ret, "{%def([0-9]*)}");
        foreach (Match m in matches)
        {
            int num = int.Parse(m.Groups[1].Value);
            ret = ret.Replace(m.Value, num.ToString());
        }
        return ret;
    }

    public void OnClick()
    {
        BattleManager.Instance.RewardEnd(m_id);
    }
}
