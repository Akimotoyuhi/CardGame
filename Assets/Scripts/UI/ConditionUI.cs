using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// フィールド上のキャラクターがバフやデバフにかかった時に、それをUIとして表示する用のクラス
/// </summary>
public class ConditionUI : MonoBehaviour
{
    [SerializeField] Text m_viewText;
    [SerializeField] ConditionSpriteData[] m_conditionSpriteData;
    [System.Serializable]
    public class ConditionSpriteData
    {
        [SerializeField] Sprite m_sprite;
        [SerializeField] Color m_color;
        [SerializeField] string m_tooltip;
        public Sprite Sprite => m_sprite;
        public Color Color => m_color;
        public string Tooltip => m_tooltip;
    }

    public void SetUI(ConditionID conditionID, int turn)
    {
        Debug.Log($"付与されたデバフ{conditionID}, ターン{turn}");
        Image image = GetComponent<Image>();
        m_viewText.text = "";
        image.SetImage(m_conditionSpriteData[(int)conditionID].Color, m_conditionSpriteData[(int)conditionID].Sprite);
        m_viewText.text = turn.ToString();
    }

    public void OnCursorEntor()
    {

    }
}