using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "Card Data")]
public class NewCardData : ScriptableObject
{
    [SerializeField] List<NewCardDataBase> m_cardData = new List<NewCardDataBase>();
    public List<NewCardDataBase> CardDatas => m_cardData;
    public NewCardDataBase GetDardRarityRandom(Rarity rarity)
    {
        List<NewCardDataBase> ret = new List<NewCardDataBase>();
        var i = m_cardData.Where(card => 
        {
            Debug.Log("a");
            if (card.Rarity == rarity) return true;
            else return false;
            });
        ret = (List<NewCardDataBase>)i;
        Debug.Log(ret);
        int r = UnityEngine.Random.Range(0, ret.Count);
        return ret[r];
    }
}
public enum CardID
{
    /// <summary>強撃</summary>
    PowerfulStrike, //スターター
    /// <summary>防御力強化</summary>
    DEFStrengthening, //スターター
    /// <summary>指令：構造強化</summary>
    StructuralFortification,
    /// <summary>指令：戦術指令</summary>
    TacticalCoordination,
    /// <summary>指令：メルトダウン</summary>
    Meltdown,
    /// <summary>断罪</summary>
    Conviction, //デバッグ用
    /// <summary>ハンマリング・オン</summary>
    HammerOn,
    /// <summary>ひっかき！</summary>
    CatScratch,
    /// <summary>シェルガード</summary>
    ShellShapedDEF,
    /// <summary>海嘯の悲歌</summary>
    TidalElegy,
}
public enum Rarity
{
    Common,
    Rare,
    Elite,
    Special,
    Curse,
    BadEffect
}
public enum UseType
{
    ToPlayer,
    ToEnemy,
}
[Serializable]
public class NewCardDataBase
{
    [SerializeField] string m_name;
    [SerializeField] string m_cost;
    [SerializeField] Sprite m_image;
    [TextArea(0, 5), Tooltip("変数に差し替えたい部分は{%value}のように記述する事")]
    [SerializeField] string m_tooltip;
    [SerializeField] Rarity m_rarity;
    [SerializeField] int m_power;
    [SerializeField] int m_attackNum;
    [SerializeField] int m_block;
    [SerializeField] int m_blockNum;
    [SerializeField] List<ConditionSelection> m_concitions;
    [SerializeField] UseType m_cardType = new UseType();
    /// <summary>カードの名前</summary>
    public string Name => m_name;
    /// <summary>コスト</summary>
    public string Cost => m_cost; //コストは数字じゃない可能性があるのでとりあえずstringにしとく
    /// <summary>画像</summary>
    public Sprite Sprite => m_image;
    /// <summary>効果の説明文</summary>
    public string Tooltip => m_tooltip;
    /// <summary>レアリティ</summary>
    public Rarity Rarity => m_rarity;
    /// <summary>攻撃</summary>
    public int Attack => m_power;
    /// <summary>攻撃回数</summary>
    public int AttackNum => m_attackNum;
    /// <summary>ブロック</summary>
    public int Block => m_block;
    /// <summary>ブロック回数</summary>
    public int BlockNum => m_blockNum;
    /// <summary>付与するバフデバフ</summary>
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
    /// <summary>使用する標的</summary>
    public UseType UseType => m_cardType;
}