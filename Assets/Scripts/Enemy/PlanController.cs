using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Enemy�̍s���\����󂯎��N���X<br/>EnemyBase�N���X����Ă΂�鎖��O��Ƃ��Ă���
/// </summary>
public class PlanController : MonoBehaviour
{
    [SerializeField, Tooltip("�s���\��\���p�̉摜 �\������\nAttack\nBlock\nBuff\nDebuff\nUnknown\n�̏�")]
    Sprite[] m_viewPlanSprite;
    private Image m_image;

    public void SetImage(ActionPlan actionPlan)
    {
        m_image = GetComponent<Image>();
        switch (actionPlan)
        {
            case ActionPlan.Attack:
                m_image.sprite = m_viewPlanSprite[0];
                m_image.color = Color.red;
                break;
            case ActionPlan.Block:
                m_image.sprite = m_viewPlanSprite[1];
                m_image.color = Color.blue;
                break;
            case ActionPlan.Buff:
                m_image.sprite = m_viewPlanSprite[2];
                m_image.color = Color.blue;
                break;
            case ActionPlan.Debuff:
                m_image.sprite = m_viewPlanSprite[3];
                m_image.color = Color.green;
                break;
            case ActionPlan.Unknown:
                m_image.sprite = m_viewPlanSprite[4];
                m_image.color = Color.white;
                break;
            default:
                Debug.LogError("�ςȒl�n����");
                break;
        }
    }
}
