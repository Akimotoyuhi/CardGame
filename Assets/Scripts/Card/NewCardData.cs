﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "All Card Data")]
public class AllCardData : ScriptableObject
{
    [SerializeField] List<NewCardData> m_cardData = new List<NewCardData>();

}
[CreateAssetMenu(fileName = "Card Data"), Serializable]
public class NewCardData : ScriptableObject
{
    public List<NewCardDataBase> m_cardData = new List<NewCardDataBase>();
}
#region CardID
public enum SpecialCardID
{
    PowerfulStrike,
    DEFStrengthening,
    StructuralFortification,
    TacticalCoordination,
    Meltdown,
    Conviction
}
public enum CommonCardID
{
    HammerOn,
    CatScratch,
}
public enum RareCardID
{
    ShellShapedDEF
}
public enum EliteCardID
{
    TidalElegy
}
public enum CurseCardID
{

}
#endregion
public enum UseType
{
    ToPlayer,
    ToEnemy,
}

[Serializable]
public class NewCardDataBase
{
    /// <summary>カードの名前</summary>
    [SerializeField] string m_name;
    /// <summary>コスト</summary>
    [SerializeField] string m_cost;
    /// <summary>画像</summary>
    [SerializeField] Sprite m_image;
    /// <summary>効果の説明文</summary>
    [TextArea(0, 5), Tooltip("変数に差し替えたい部分は{%value}のように記述する事")]
    [SerializeField] string m_tooltip;
    /// <summary>攻撃</summary>
    [SerializeField] int m_power;
    /// <summary>攻撃回数</summary>
    [SerializeField] int m_attackNum;
    /// <summary>ブロック</summary>
    [SerializeField] int m_block;
    /// <summary>ブロック回数</summary>
    [SerializeField] int m_blockNum;
    //[Header("カードを使用した際に付与するコンディションの設定")]
    //[SerializeField, SerializeReference, SubclassSelector]
    //List<ICondition> m_cardConditionSets = new List<ICondition>();
    [SerializeField] List<ConditionSelection> m_concitions;
    /// <summary>使用する標的</summary>
    [SerializeField] UseType m_cardType = new UseType();

    public string Name => m_name;
    public Sprite Sprite => m_image;
    public string Tooltip => m_tooltip;
    public int Attack => m_power;
    public int AttackNum => m_attackNum;
    public int Block => m_block;
    public int BlockNum => m_blockNum;
    //コストは数字じゃない可能性があるのでとりあえずstringにしとく
    public string Cost => m_cost;
    public List<Condition> Conditions
    {
        get
        {
            List<Condition> ret = new List<Condition>();
            foreach (var item in m_concitions)
            {
                ret.Add(item.GetCondition);
            }
            return ret;
        }
    }
    public UseType UseType => m_cardType;
}