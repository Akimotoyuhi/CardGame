using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チャレンジモードの項目管理
/// </summary>
[CreateAssetMenu(fileName = "CustomModeData")]
public class CustomModeData : ScriptableObject
{
    [SerializeField] List<CustomModeDataBase> m_databases;
    public List<CustomModeDataBase> DataBases => m_databases;
}
/// <summary>チャレンジモードの個々の設定</summary>
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
    [SerializeField] CustomEntityType m_type;
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
    public CustomEntityType Type => m_type;
    public CustomParamType ParamType => m_paramType;
    public CustomParamCoefficientType ParamUpDown => m_paramCoefficientType;
    public int Point => m_point;
    public CustomID CustomID => m_customID;
}
/// <summary>カスタムID</summary>
public enum CustomID
{
    EnemyPowerUp,
    PlayerLifeDown,
}
/// <summary>カスタムモードの種類</summary>
public enum CustomEntityType
{
    AllEnemiesBuff,
    PlayerNerf,
    SpecificEnemyBuff,
}
/// <summary>チャレンジモードで増減させるパラメーター</summary>
public enum CustomParamType
{
    None,
    Power,
    Difence,
    Life,
}
/// <summary>ChallengeParamTypeで指定した値に対する係数</summary>
public enum CustomParamCoefficientType
{
    None,
    Plus,
    Minus,
    Multi,
    Division,
}