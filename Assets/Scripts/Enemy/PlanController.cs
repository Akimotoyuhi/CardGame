using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Enemyの行動予定を受け持つクラス<br/>EnemyBaseクラスから呼ばれる事を前提としている
/// </summary>
public class PlanController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Text m_viewText;
    [SerializeField] Image m_image;
    [SerializeField] List<PlanSetting> m_planController;
    private string m_text = default;
    [System.Serializable]
    public class PlanSetting
    {
        [Header("行動予定表示用の画像 表示順はAttack, Block, Buff, Debuff, Unknownの順")]
        [SerializeField] Sprite m_sprite;
        [SerializeField, TextArea] string m_text;
        [SerializeField] Color m_color;
        public string Text => m_text;
        public void SetImage(Image image)
        {
            image.sprite = m_sprite;
            image.color = m_color;
        }
    }

    public void SetImage(ActionPlan actionPlan, int textValue)
    {
        m_viewText.text = "";
        m_planController[(int)actionPlan].SetImage(m_image);
        m_text = m_planController[(int)actionPlan].Text;
        switch (actionPlan)
        {
            case ActionPlan.Attack:
                m_viewText.text = textValue.ToString();
                break;
            default:
                break;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        EffectManager.Instance.SetUIText(PanelType.Battle, m_text, Color.black);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        EffectManager.Instance.RemoveUIText(PanelType.Battle);
    }
}
