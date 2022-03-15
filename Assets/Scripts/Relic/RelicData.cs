using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RelicData")]
public class RelicData : ScriptableObject
{
    [SerializeField] List<RelicDataBase> m_relicDataBases;
    public void Setup()
    {
        foreach (var item in m_relicDataBases)
        {
            item.Setup();
        }
    }

    public List<int[]> RelicExecute(int index, RelicTriggerTiming triggerTiming, ParametorType parametorType)
    {
        if (m_relicDataBases[index].Conditional.Conditional(triggerTiming, parametorType))
        {
            return m_relicDataBases[index].Command.Execute;
        }
        else return null;
    }
}
#region Enums
/// <summary>�����b�N�̌��ʔ����^�C�~���O</summary>
public enum RelicTriggerTiming
{
    BattleBegin,
    BattleEnd,
    TurnBegin,
    TurnEnd,
    Attacked,
    Damaged,
    Obtained,
}
/// <summary>�����b�NID</summary>
public enum RelicID
{
    StrangeMask,
}
#endregion
[System.Serializable]
public class RelicDataBase
{
    [SerializeField] string m_name;
    [SerializeField] Sprite m_sprite;
    [SerializeField] RelicCommand m_command;
    [SerializeField] RelicConditional m_conditional;
    public string Name => m_name;
    public Sprite Sprite => m_sprite;
    public RelicCommand Command => m_command;
    public RelicConditional Conditional => m_conditional;
    public void Setup()
    {
        m_conditional.Setup();
    }
}
/// <summary>�����b�N�̔������̌���</summary>
[System.Serializable]
public class RelicCommand
{
    [SerializeReference, SubclassSelector] List<ICommand> m_relicCommand;
    public List<int[]> Execute
    {
        get
        {
            List<int[]> ret = new List<int[]>();
            foreach (var c in m_relicCommand)
            {
                ret.Add(c.Execute());
            }
            return ret;
        }
    }
}
/// <summary>�����b�N�̔�������</summary>
[System.Serializable]
public class RelicConditional
{
    /// <summary>���ʂ̔����^�C�~���O</summary>
    [SerializeField] RelicTriggerTiming m_timing;
    /// <summary>�]������l</summary>
    [SerializeField] ParametorType m_parametorType;
    /// <summary>���ʂ��ő剽�񔭓����邩</summary>
    [SerializeField, Tooltip("�ő剽�񔭓����邩\n-1�Ȃ琧���Ȃ�")] int m_maxTriggerNum;
    /// <summary>���݉��񔭓��������̕ۑ��p</summary>
    private int m_currentTriggerNum;
    public void Setup()
    {
        m_currentTriggerNum = 0;
    }
    public bool Conditional(RelicTriggerTiming triggerTiming, ParametorType parametorType)
    {
        if (triggerTiming == m_timing && parametorType == m_parametorType)
        {
            if (m_maxTriggerNum < 0) return true;
            if (m_maxTriggerNum <= m_currentTriggerNum)
            {
                m_currentTriggerNum++;
                return true;
            }
            else return false;
        }
        else return false;
    }
}