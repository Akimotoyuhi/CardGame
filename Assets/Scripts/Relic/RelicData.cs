using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "RelicData")]
public class RelicData : ScriptableObject
{
    [SerializeField, Range(0, 100)] int m_rareProbability;
    [SerializeField, Range(0, 100)] int m_superRareProbability;
    [SerializeField] List<RelicDataBase> m_relicDataBases;
    
    public List<RelicDataBase> DataBases => m_relicDataBases;
    public void Setup()
    {
        for (int i = 0; i < m_relicDataBases.Count; i++)
        {
            m_relicDataBases[i].Setup((RelicID)i);
        }
    }
    /// <summary>
    /// レリックデータの取得
    /// </summary>
    public RelicDataBase GetRelic(EnemyType enemy)
    {
        switch (enemy)
        {
            case EnemyType.Elite:
                int r = Random.Range(0, 100);
                RelicRarity rarity;
                if (r < m_superRareProbability)
                    rarity = RelicRarity.SuperRare;
                else if (r < m_rareProbability)
                    rarity = RelicRarity.Rare;
                else
                    rarity = RelicRarity.Common;
                return GetRelic(rarity);
            case EnemyType.Boss:
                return GetRelic(RelicRarity.Boss);
            default:
                return null;
        }
    }
    /// <summary>
    /// レリックデータのレアリティを指定して取得
    /// </summary>
    public RelicDataBase GetRelic(RelicRarity rarity)
    {
        var list = m_relicDataBases.Where((relic) => relic.Rarity == rarity).ToList();
        int r = Random.Range(0, list.Count);
        return list[r];
    }
    
}
[System.Serializable]
public class RelicDataBase
{
    [SerializeField] string m_name;
    [SerializeField, TextArea] string m_tooltip;
    [SerializeField] Sprite m_sprite;
    [SerializeField] RelicRarity m_rarity;
    [SerializeField] RelicCommand m_command;

    public string Name => m_name;
    public string Tooltip => m_tooltip;
    public Sprite Sprite => m_sprite;
    public RelicRarity Rarity => m_rarity;
    public RelicCommand Commands => m_command;
    public RelicID RelicID { get; private set; }
    public void Setup(RelicID relicID)
    {
        RelicID = relicID;
    }
}
/// <summary>レリックの発動時の効果</summary>
[System.Serializable]
public class RelicCommand
{
    [SerializeReference, SubclassSelector] List<ICommand> m_relicCommand;
    [SerializeField] List<RelicConditional> m_conditional;
    public List<int[]> Command
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
    public List<RelicConditional> Conditional => m_conditional;
}
/// <summary>レリックの発動条件</summary>
[System.Serializable]
public class RelicConditional
{
    /// <summary>効果の発動タイミング</summary>
    [SerializeField] RelicTriggerTiming m_timing;
    /// <summary>評価する値</summary>
    [SerializeField] ParametorType m_parametorType;
    /// <summary>効果が最大何回発動するか</summary>
    [SerializeField, Tooltip("最大何回発動するか\n-1以下なら制限なし")] int m_maxTriggerNum;
    public bool Conditional(int currentTriggerNum, RelicTriggerTiming triggerTiming, ParametorType parametorType, int num)
    {
        if (triggerTiming == m_timing && parametorType == m_parametorType)
        {
            if (m_maxTriggerNum < 0 || m_maxTriggerNum > currentTriggerNum)
                return true;
            else
                return false;
        }
        else return false;
    }
}
#region Enums
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
    /// <summary>勇気の石</summary>
    StoneOfCourage,
    /// <summary>呪いの本</summary>
    CurseBook,
    /// <summary>究極破壊兵器</summary>
    Dynamite,
    /// <summary>ひも付き硬化おにぎり</summary>
    Bomb,
    /// <summary>狂気のルーン</summary>
    MadnessRune,
}
/// <summary>レリックのレアリティ</summary>
public enum RelicRarity
{
    Common,
    Rare,
    SuperRare,
    Boss,
    Event,
}
#endregion