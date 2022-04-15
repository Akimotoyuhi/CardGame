using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Enemy�̍s���\����󂯎��N���X<br/>EnemyBase�N���X����Ă΂�鎖��O��Ƃ��Ă���
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
        [Header("�s���\��\���p�̉摜 �\������Attack, Block, Buff, Debuff, Unknown�̏�")]
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
