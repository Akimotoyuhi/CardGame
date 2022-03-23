using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �`�������W���[�h�̍��ڊǗ�
/// </summary>
[CreateAssetMenu(fileName = "ChallengeModeData")]
public class ChallengeModeData : ScriptableObject
{
    [SerializeField] List<ChallengeModeDataBase> m_databases;
    public List<ChallengeModeDataBase> DataBases => m_databases;
}
/// <summary>�`�������W���[�h�̌X�̐ݒ�</summary>
[System.Serializable]
public class ChallengeModeDataBase
{
    [SerializeField] string m_name;
    [SerializeField, TextArea] string m_tooltip;
    [SerializeField] float m_num;
    [SerializeField] ChallengeType m_type;
    [SerializeField] ChallengeParamType m_paramType;
    [SerializeField] ChallengeParamCoefficientType m_paramCoefficientType;
    [SerializeField] int m_point;
    public ChallengeType Type => m_type;
    public ChallengeParamType ParamType => m_paramType;
    public ChallengeParamCoefficientType ParamUpDown => m_paramCoefficientType;
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
/// <summary>�`�������W���[�h�̌��ʕ���</summary>
//public interface IChallengeCommand
//{
//    int[] Execute();
//    ChallengeType Type { get; }
//    ChallengeParamType ParamType { get; }
//    ChallengeParamUpDown ParamUpDown { get; }
//}
/// <summary>�`�������W���[�h�̎��</summary>
public enum ChallengeType
{
    AllEnemiesBuff,
    PlayerNerf,
    SpecificEnemyBuff,
}
/// <summary>�`�������W���[�h�ő���������p�����[�^�[</summary>
public enum ChallengeParamType
{
    None,
    Power,
    Difence,
    Life,
}
/// <summary>ChallengeParamType�Ŏw�肵���l�ɑ΂���W��</summary>
public enum ChallengeParamCoefficientType
{
    None,
    Plus,
    Minus,
    Multi,
    Division,
}