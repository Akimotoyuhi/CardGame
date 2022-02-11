using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RelicData")]
public class RelicData : ScriptableObject
{
    [SerializeField] List<RelicDataBase> m_relicDataBases;
    //public List<RelicDataBase> RelicDataBases => m_relicDataBases;
    public RelicDataBase GetRelic(RelicID relicID)
    {
        return m_relicDataBases[(int)relicID];
    }
}
/// <summary>レリックの効果発動タイミング</summary>
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
/// <summary>レリックID</summary>
public enum RelicID
{
    StrangeMask,
}
[System.Serializable]
public class RelicDataBase
{
    [SerializeField] string m_name;
    [SerializeField] Sprite m_sprite;
    [SerializeField] List<RelicTriggerTiming> m_timing;
    [SerializeField] int m_triggerCount;
    [SerializeField] int m_power;
    [SerializeField] int m_block;
    [SerializeField] List<ConditionSelection> m_condition;

    public string Name => m_name;
    public Sprite Sprite => m_sprite;
    public List<RelicTriggerTiming> RelicTriggerTiming => m_timing;
    public int TriggerCount => m_triggerCount;
    public int Power => m_power;
    public int Block => m_block;
    public List<Condition> Conditions
    {
        get
        {
            List<Condition> ret = new List<Condition>();
            foreach (var item in m_condition)
            {
                ret.Add(item.GetCondition);
            }
            return ret;
        }
    }
}