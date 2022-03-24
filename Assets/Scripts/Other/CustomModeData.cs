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
    [SerializeField] ChallengeType m_type;
    [SerializeField] ChallengeParamType m_paramType;
    [SerializeField] ChallengeParamCoefficientType m_paramCoefficientType;
    [SerializeField] int m_point;

    public string Name => m_name;
    public string Tooltip => m_tooltip;
    public Sprite IconSprite => m_iconSprite;
    public Color IconColor => m_iconColor;
    public Sprite SubIconSprite => m_subIconSprite;
    public Color SubIconColor => m_subIconColor;
    public float Num => m_num;
    public ChallengeType Type => m_type;
    public ChallengeParamType ParamType => m_paramType;
    public ChallengeParamCoefficientType ParamUpDown => m_paramCoefficientType;
    public int Point => m_point;
}
//[System.Serializable]
//public class ChallengeModeCommand
//{
//    [SerializeField] int num;
//    [SerializeField] ChallengeParamType m_paramType;
//    [SerializeField] ChallengeParamUpDown m_paramUpDown;
//    public int[] Execute()
//    {
//        return new int[] {num};
//    }
//    public ChallengeType Type => ChallengeType.AllEnemiesBuff;
//    public ChallengeParamType ParamType => m_paramType;
//    public ChallengeParamUpDown ParamUpDown => m_paramUpDown;
//}
/// <summary>チャレンジモードの効果部分</summary>
//public interface IChallengeCommand
//{
//    int[] Execute();
//    ChallengeType Type { get; }
//    ChallengeParamType ParamType { get; }
//    ChallengeParamUpDown ParamUpDown { get; }
//}
/// <summary>チャレンジモードの種類</summary>
public enum ChallengeType
{
    AllEnemiesBuff,
    PlayerNerf,
    SpecificEnemyBuff,
}
/// <summary>チャレンジモードで増減させるパラメーター</summary>
public enum ChallengeParamType
{
    None,
    Power,
    Difence,
    Life,
}
/// <summary>ChallengeParamTypeで指定した値に対する係数</summary>
public enum ChallengeParamCoefficientType
{
    None,
    Plus,
    Minus,
    Multi,
    Division,
}