using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

/// <summary>
/// フィールド上のキャラクターがバフやデバフにかかった時に、それをUIとして表示する用のクラス
/// </summary>
public class ConditionUI : MonoBehaviour
{
    [SerializeField] Text m_viewText;
    [SerializeField] Image m_image;
    [SerializeField] ConditionSpriteData[] m_conditionSpriteData;
    private string m_text = "";
    [System.Serializable]
    public class ConditionSpriteData
    {
        public string m_name;
        [SerializeField] Sprite m_sprite;
        [SerializeField] Color m_color;
        //[SerializeField, TextArea] string m_tooltip;
        public Sprite Sprite => m_sprite;
        public Color Color => m_color;
        //public string Tooltip => m_tooltip;
    }
    //public string GetTooltip(ConditionID conditionID) => m_conditionSpriteData[(int)conditionID].Tooltip;
    public Sprite GetSprite(ConditionID conditionID) => m_conditionSpriteData[(int)conditionID].Sprite;
    public Color GetColor(ConditionID conditionID) => m_conditionSpriteData[(int)conditionID].Color;

    public void SetUI(Condition condition)
    {
        m_viewText.text = "";
        m_image.SetImage(m_conditionSpriteData[(int)condition.GetConditionID()].Color, m_conditionSpriteData[(int)condition.GetConditionID()].Sprite);
        m_viewText.text = condition.Turn.ToString();
        m_text = condition.Tooltip;
        //string s = m_conditionSpriteData[(int)conditionID].Tooltip;
        //m_text = text;
        //m_viewText.text = turn.ToString();
    }

    /// <summary>
    /// ゲーム画面にテキストを表示<br/>EventTriggerから呼ばれる事を想定している
    /// </summary>
    /// <param name="flag"></param>
    public void OnCursor(bool flag)
    {
        if (flag)
        {
            EffectManager.Instance.SetUIText(PanelType.Battle, m_text, Color.black);
        }
        else
        {
            EffectManager.Instance.RemoveUIText(PanelType.Battle);
        }
    }
}