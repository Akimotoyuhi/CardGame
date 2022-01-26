using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Enemyの行動予定を受け持つクラス<br/>EnemyBaseクラスから呼ばれる事を前提としている
/// </summary>
public class PlanController : MonoBehaviour
{
    [SerializeField, Tooltip("行動予定表示用の画像 表示順は\nAttack\nBlock\nBuff\nDebuff\nUnknown\nの順")]
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
                Debug.LogError("変な値渡すな");
                break;
        }
    }
}
