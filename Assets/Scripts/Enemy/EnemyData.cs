using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField, Header("�G�X�e�[�^�X�̃f�[�^")]
    List<EnemyDataBase> m_enemyDataBases;
    [Header("�G���J�E���g�f�[�^")]
    [SerializeField] List<EncountDataBase> m_encountDatas; 
    public List<EnemyDataBase> Encount(EnemyType enemyType, MapID mapID)
    {
        List<EnemyDataBase> ret = new List<EnemyDataBase>();
        List<EncountIdData> encountData;
        switch (enemyType)
        {
            case EnemyType.Enemy:
                encountData = m_encountDatas[(int)mapID].Enemies;
                break;
            case EnemyType.Elite:
                encountData = m_encountDatas[(int)mapID].Elites;
                break;
            case EnemyType.Boss:
                encountData = m_encountDatas[(int)mapID].Bosses;
                break;
            default:
                Debug.Log("�ςȒl������");
                return null;
        }
        int r = UnityEngine.Random.Range(0, encountData.Count);
        for (int i = 0; i < encountData[r].GetID.Length; i++)
        {
            ret.Add(m_enemyDataBases[encountData[r].GetID[i]]);
        }
        return ret;
    }
}
#region �G�f�[�^�ݒ�֌W
[Serializable]
public class EnemyDataBase
{
    [SerializeField, Tooltip("���O")] string m_name;
    [SerializeField, Tooltip("�GID")] EnemyID m_id;
    [SerializeField, Tooltip("�ő�̗�")] int m_life;
    [SerializeField, Tooltip("�摜")] Sprite m_sprite;
    [SerializeField, Tooltip("�摜�̃f�J���{��")] float m_spriteScaleMagnification = 1;
    [SerializeField, Tooltip("�G�̎��")] EnemyType m_enemyType;
    public string Name => m_name;
    public EnemyID ID => m_id;
    public int Life => m_life;
    public Sprite Image => m_sprite;
    public float ScaleMagnification => m_spriteScaleMagnification;
    public EnemyType EnemyType => m_enemyType;
    public enum NodeType { Selector, Sequence }
    private NodeType m_NodeType = NodeType.Sequence;
    public List<EnemyBaseState> m_enemyBaseState;
    public EnemyActionCommnad3 CommandSelect(EnemyBase enemy, int turn)
    {
        switch (m_NodeType)
        {
            case NodeType.Sequence: //��ł����s�����炻���ŏI��
                for (int i = 0; i < m_enemyBaseState.Count; i++)
                {
                    bool flag = false;
                    for (int n = 0; n < m_enemyBaseState[i].m_conditionalCommand.Count; n++)
                    {
                        if (!m_enemyBaseState[i].m_conditionalCommand[n].Conditional(enemy, turn))
                        {
                            flag = false;
                            break;
                        }
                        flag = true;
                    }
                    if (flag)
                    {
                        return m_enemyBaseState[i].m_actionCommnad;
                    }
                }
                return null;
            default:
                Debug.LogError("�����ȃP�[�X");
                return null;
        }
    }
}

[Serializable]
public class EnemyBaseState
{
    public List<EnemyConditionalCommand3> m_conditionalCommand;
    public EnemyActionCommnad3 m_actionCommnad;
}

[Serializable]
public class EnemyActionCommnad3
{
    [Header("�s���f�[�^")]
    [SerializeReference, SubclassSelector] List<ICommand> m_commands;
    [SerializeField] TargetType m_target;
    [SerializeField] List<Plan> m_plan;
    [Serializable]
    public class Plan
    {
        [SerializeField, Tooltip("�s���\��")] ActionPlan m_plan;
        [SerializeField, Tooltip("�s���\��ɐ��l������ꍇ�A�R�}���h�z��̂ǂ�index���Q�Ƃ��邩")] int m_numIndex;
        public ActionPlan ActionPlan => m_plan;
        public int NumIndex => m_numIndex;
    }
    public List<int[]> Command
    {
        get
        {
            List<int[]> ret = new List<int[]>();
            foreach (var c in m_commands)
                ret.Add(c.Execute());
            return ret;
        }
    }
    public TargetType Target => m_target;
    public List<Plan> ActionPlans => m_plan;
}

[Serializable]
public class EnemyConditionalCommand3
{
    [Header("�����f�[�^")]
    [SerializeField, Tooltip("����")] WhereType m_type;
    [SerializeField, Tooltip("�]������l")] int m_value;
    [SerializeField, Tooltip("���̍s��������m��"), Range(0, 100)] int m_probability;
    [SerializeField, Tooltip("���̍s������x����ɂ��邩�ǂ���")] bool m_isOnlyOnce;
    private bool m_isOnlyFlag;
    public bool SetOnlyFlag
    {
        set
        {
            if (m_isOnlyOnce) //��x����ɂ���t���O��true�o�Ȃ���Ώ��������͂��Ȃ�
            {
                m_isOnlyFlag = value;
            }
        }
    }
    /// <summary>
    /// ������
    /// </summary>
    /// <returns>������</returns>
    public bool Conditional(EnemyBase enemy, int turn)
    {
        if (m_isOnlyFlag) return false;
        if (turn == 0)
        {
            if (m_type == WhereType.BattleBegin) { return true; }
            else { return false; }
        }

        switch (m_type)
        {
            case WhereType.Any:
                return true;
            case WhereType.Turn:
                if (turn == m_value)
                {
                    return true;
                }
                return false;
            case WhereType.RowTurn:
                if (turn <= m_value)
                {
                    return true;
                }
                return false;
            case WhereType.HighTurn:
                if (turn >= m_value)
                {
                    return true;
                }
                return false;
            case WhereType.MultipleTurn:
                if (turn % m_value == 0)
                {
                    return true;
                }
                return false;
            case WhereType.RowLife:
                if (enemy.CurrentLife <= m_value)
                {
                    return true;
                }
                return false;
            case WhereType.HighLife:
                if (enemy.CurrentLife >= m_value)
                {
                    return true;
                }
                return false;
            case WhereType.BattleBegin:
                return false;
            default:
                Debug.LogError("�����ȏ����P�[�X");
                return false;
        }
    }
}
#endregion
[Serializable]
public class EncountDataBase
{
    [SerializeField] string m_label;
    [SerializeField, Header("Enemy")] List<EncountIdData> m_enemies;
    [SerializeField, Header("Elite")] List<EncountIdData> m_elites;
    [SerializeField, Header("Boss")] List<EncountIdData> m_bosses;
    public List<EncountIdData> Enemies => m_enemies;
    public List<EncountIdData> Elites => m_elites;
    public List<EncountIdData> Bosses => m_bosses;
}
[Serializable]
public class EncountIdData
{
    [SerializeField] EnemyID[] m_enemyID;
    /// <summary>
    /// �GID�̎擾
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int[] GetID
    {
        get
        {
            int[] ret = new int[m_enemyID.Length];
            for (int i = 0; i < m_enemyID.Length; i++)
            {
                ret[i] = (int)m_enemyID[i];
            }
            return ret;
        }
    }
}
#region Enums
/// <summary>�GID</summary>
public enum EnemyID
{
    /// <summary>�J�}�N�W</summary>
    Kamakuji,
    /// <summary>�K�C�R�c</summary>
    Skelton,
    /// <summary>���̎���</summary>
    TowerGuardian,
    /// <summary>�J�}�N�W��</summary>
    KamakujiAlpha,
    /// <summary>�]���r��</summary>
    ZombieDog,
    /// <summary>�y�����g��</summary>
    Peryton,
    /// <summary>�[���҂ǂ�</summary>
    DeepOnes,
    /// <summary>�����B�A�^��</summary>
    Leviathan,
    /// <summary>�T�[�x���^�C�K�[</summary>
    SavelTiger,
    /// <summary>�^�c�m�I�g�V�S</summary>
    Seahorse,
    /// <summary>�Ȃ��ׂ�</summary>
    Nasobema,
    /// <summary>�f�X�l�Y�~</summary>
    DeathRat,
    /// <summary>���C�o�[��</summary>
    Ybarn,
    /// <summary>���̖��p�t</summary>
    FlameWizard,
    /// <summary>���̖��p�t</summary>
    WaterWizard,
    /// <summary>���̖��p�t</summary>
    WindWizard,
    /// <summary>���̖��p�t</summary>
    LightningWizard,
    /// <summary>�G�^�[�i���h���S��</summary>
    EternalDragon,
    /// <summary>�A�U�g�[�X</summary>
    Azathoth,
    /// <summary>�n�[�s�[</summary>
    Harpy,
}
/// <summary>�G�̎��</summary>
public enum EnemyType
{
    Enemy,
    Elite,
    Boss,
}
/// <summary>�s���\��</summary>
public enum ActionPlan
{
    Attack,
    Block,
    Buff,
    Debuff,
    Unknown,
}
/// <summary>�U���̕W�I</summary>
public enum TargetType
{
    ToPlayer,
    ToEnemy,
}
/// <summary>�G�s������</summary>
public enum WhereType
{
    Any,
    Turn,
    RowTurn,
    HighTurn,
    MultipleTurn,
    RowLife,
    HighLife,
    BattleBegin,
}
#endregion