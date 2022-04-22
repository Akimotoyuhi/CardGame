using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

/// <summary>
/// �t�B�[���h��̃L�����N�^�[���o�t��f�o�t�ɂ����������ɁA�����UI�Ƃ��ĕ\������p�̃N���X
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
        [SerializeField] Sprite m_sprite;
        [SerializeField] Color m_color;
        [SerializeField, TextArea] string m_tooltip;
        public Sprite Sprite => m_sprite;
        public Color Color => m_color;
        public string Tooltip => m_tooltip;
    }
    public string GetTooltip(ConditionID conditionID) => m_conditionSpriteData[(int)conditionID].Tooltip;
    public Sprite GetSprite(ConditionID conditionID) => m_conditionSpriteData[(int)conditionID].Sprite;
    public Color GetColor(ConditionID conditionID) => m_conditionSpriteData[(int)conditionID].Color;

    public void SetUI(ConditionID conditionID, int turn)
    {
        m_viewText.text = "";
        m_image.SetImage(m_conditionSpriteData[(int)conditionID].Color, m_conditionSpriteData[(int)conditionID].Sprite);
        string s = m_conditionSpriteData[(int)conditionID].Tooltip;
        m_text = s;
        m_viewText.text = turn.ToString();
    }

    /// <summary>
    /// �Q�[����ʂɃe�L�X�g��\��<br/>EventTrigger����Ă΂�鎖��z�肵�Ă���
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