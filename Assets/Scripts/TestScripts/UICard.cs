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
    [SerializeField, Tooltip("レア度に応じたカードの色。\nそれぞれ\nCommon\nRare\nElite\nSpecial\nCurse\nBadEffect\nの順")]
    private List<Color> m_cardColor = default;
    private CardID m_id;
    private int m_isUpgrade;

    public void Setup(CardDataBase cardData, int isUpgrade)
    {
        m_viewName.text = cardData.Name;
        m_viewImage.sprite = cardData.Sprite;
        m_viewCost.text = cardData.Cost;
        m_viewTooltip.text = SetTooltip(cardData.Tooltip);
        m_id = cardData.CardId;
        m_isUpgrade = isUpgrade;
        GetComponent<Image>().color = m_cardColor[(int)cardData.Rarity];
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
        BattleManager.Instance.RewardEnd((int)m_id, m_isUpgrade);
    }
}
