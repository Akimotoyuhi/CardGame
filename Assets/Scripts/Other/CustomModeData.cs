using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �`�������W���[�h�̍��ڊǗ�
/// </summary>
[CreateAssetMenu(fileName = "CustomModeData")]
public class CustomModeData : ScriptableObject
{
    [SerializeField] List<CustomModeDataBase> m_databases;
    public List<CustomModeDataBase> DataBases => m_databases;
}
/// <summary>�`�������W���[�h�̌X�̐ݒ�</summary>
[System.Serializable]
public class CustomModeDataBase
{
    [SerializeField] string m_name;
    [SerializeField, TextArea] string m_tooltip;
    [SerializeField] Sprite m_iconSprite;
    [SerializeField] Color m_iconColor = Color.black;
    [SerializeField] Sprite m_subIconSprite;
    [SerializeField] Color m_subIconColor = Color.black;
    [SerializeField] float m_num;
    [SerializeField] CustomEntityType m_entityType;
    [SerializeField] CustomParamType m_paramType;
    [SerializeField] CustomParamCoefficientType m_paramCoefficientType;
    [SerializeField] int m_point;
    [SerializeField] CustomID m_customID;
    public string Name => m_name;
    public string Tooltip => m_tooltip;
    public Sprite IconSprite => m_iconSprite;
    public Color IconColor => m_iconColor;
    public Sprite SubIconSprite => m_subIconSprite;
    public Color SubIconColor => m_subIconColor;
    public float Num => m_num;
    public CustomEntityType Type => m_entityType;
    public CustomParamType ParamType => m_paramType;
    //public CustomParamCoefficientType ParamCoefficientType => m_paramCoefficientType;
    public int Point => m_point;
    public CustomID CustomID => m_customID;
    /// <summary>�^����ꂽ���l���w�肵���p�����[�^�[�ő���������</summary>
    public int CustomEffect(CustomEntityType entityType, CustomParamType paramType, int num)
    {
        if (entityType != m_entityType || paramType != m_paramType) return num;
        float ret;
        switch (m_paramCoefficientType)
        {
            case CustomParamCoefficientType.None:
                ret = num;
                break;
            case CustomParamCoefficientType.Plus:
                ret = num + m_num;
                break;
            case CustomParamCoefficientType.Minus:
                ret = num - m_num;
                break;
            case CustomParamCoefficientType.Multi:
                ret = num * m_num;
                break;
            case CustomParamCoefficientType.Division:
                ret = num / m_num;
                break;
            default:
                Debug.LogError("��O�G���[");
                ret = num;
                break;
        }
        return (int)ret;
    }
}
/// <summary>�J�X�^��ID</summary>
public enum CustomID
{
    EnemyPowerUp,
    PlayerLifeDown,
}
/// <summary>�J�X�^�����[�h�̎��</summary>
public enum CustomEntityType
{
    AllEnemiesBuff,
    PlayerNerf,
    SpecificEnemyBuff,
}
/// <summary>�`�������W���[�h�ő���������p�����[�^�[</summary>
public enum CustomParamType
{
    None,
    Power,
    Difence,
    Life,
}
/// <summary>ChallengeParamType�Ŏw�肵���l�ɑ΂���W��</summary>
public enum CustomParamCoefficientType
{
    None,
    Plus,
    Minus,
    Multi,
    Division,
}