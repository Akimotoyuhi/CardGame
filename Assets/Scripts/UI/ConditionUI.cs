using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �t�B�[���h��̃L�����N�^�[���o�t��f�o�t�ɂ����������ɁA�����UI�Ƃ��ĕ\������p�̃N���X
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
        public Sprite Sprite => m_sprite;
        public Color Color => m_color;
    }

    public void SetUI(ConditionID conditionID, int turn)
    {
        Debug.Log($"�t�^���ꂽ�f�o�t{conditionID}, �^�[��{turn}");
        Image image = GetComponent<Image>();
        m_viewText.text = "";
        switch (conditionID)
        {
            case ConditionID.Weakness:
                image.SetImage(m_conditionSpriteData[0].Color, m_conditionSpriteData[0].Sprite);
                break;
            case ConditionID.Frail:
                image.SetImage(m_conditionSpriteData[1].Color, m_conditionSpriteData[1].Sprite);
                break;
            case ConditionID.Strength:
                image.SetImage(m_conditionSpriteData[2].Color, m_conditionSpriteData[2].Sprite);
                break;
            case ConditionID.Agile:
                image.SetImage(m_conditionSpriteData[3].Color, m_conditionSpriteData[3].Sprite);
                break;
            case ConditionID.PlateArmor:
                image.SetImage(m_conditionSpriteData[4].Color, m_conditionSpriteData[4].Sprite);
                break;
            default:
                Debug.LogError("�ςȒl�n����");
                break;
        }
        m_viewText.text = turn.ToString();
    }
}