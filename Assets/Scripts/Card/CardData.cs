using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "Card Data")]
public class CardData : ScriptableObject
{
    [SerializeField, Range(0, 100)] int m_eliteProbability = 5;
    [SerializeField, Range(0, 100)] int m_rareProbability = 30;
    [SerializeField] List<CardDataBase> m_cardData = new List<CardDataBase>();
    public CardInfomationData CardDatas(int id, int upgrade) => m_cardData[id].GetCardInfo(upgrade);
    public void Setup()
    {
        for (int i = 0; i < m_cardData.Count; i++)
        {
            m_cardData[i].Setup((CardID)i);
        }
    }
    /// <summary>
    /// ランダムで抽選したレア度の中からランダムで１つカードを抽選する
    /// </summary>
    /// <param name="upgradeNum">強化回数</param>
    /// <returns>カードデータ</returns>
    public CardInfomationData GetCardRarityRandom(int upgradeNum)
    {
        Rarity rarity;
        int r = UnityEngine.Random.Range(0, 100);
        if (r < m_eliteProbability)
        {
            rarity = Rarity.Elite;
        }
        else if (r < m_rareProbability)
        {
            rarity = Rarity.Rare;
        }
        else
        {
            rarity = Rarity.Common;
        }
        return GetCardRarityRandom(rarity, upgradeNum);
    }
    /// <summary>
    /// 指定したレア度のカードを１つ抽選する
    /// </summary>
    /// <param name="rarity">抽選するレア度</param>
    /// <param name="upgradeNum">強化回数</param>
    /// <returns>カードデータ</returns>
    public CardInfomationData GetCardRarityRandom(Rarity rarity, int upgradeNum)
    {
        var list = m_cardData.Where(card => card.GetCardInfo(upgradeNum).Rarity == rarity).ToList();
        int r = UnityEngine.Random.Range(0, list.Count);
        return list[r].GetCardInfo(upgradeNum);
    }
}
#region Enums
public enum CardID
{
    /// <summary>強撃</summary>
    Slashing, //スターター
    /// <summary>防御力強化</summary>
    Defense, //スターター
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
    ToAll,
}
#endregion
[Serializable]
public class CardDataBase
{
    public string m_rabel;
    [SerializeField] CardInfomationData m_cardInfo;
    [SerializeField] CardInfomationData m_cardUpgrade;
    public void Setup(CardID id)
    {
        m_cardInfo.CardId = id;
        m_cardUpgrade.CardId = id;
    }
    public CardInfomationData GetCardInfo(int upgradeNum)
    {
        switch (upgradeNum)
        {
            case 0:
                return m_cardInfo;
            case 1:
                return m_cardUpgrade;
            default:
                Debug.Log("");
                return null;
        }
    }
}
[Serializable]
public class CardInfomationData
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
    [SerializeField] List<ConditionSelection> m_conditions;
    [SerializeField] UseType m_cardType = new UseType();
    [SerializeField] bool m_isDiscarding = false;
    //[SerializeField] List<CardInfomationData> m_upgradeDatas;
    /// <summary>カードの名前</summary>
    public string Name => m_name;
    /// <summary>コスト</summary>
    public string Cost => m_cost; //コストは数字じゃない可能性があるのでとりあえずstringにしとく
    /// <summary>画像</summary>
    public Sprite Sprite => m_image;
    /// <summary>効果の説明文</summary>
    public string Tooltip => m_tooltip;
    /// <summary>カードID</summary>
    public CardID CardId { get; set; }
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
            foreach (var item in m_conditions)
            {
                ret.Add(item.GetCondition);
            }
            return ret;
        }
    }
    /// <summary>使用する標的</summary>
    public UseType UseType => m_cardType;
    /// <summary>廃棄カード</summary>
    public bool IsDiscarding => m_isDiscarding;
    /// <summary>アップグレード後のカードデータ</summary>
    //public CardInfomationData UpgradeData => m_upgradeDatas[0];
    //public class CardUpgradeData
    //{

    //}
}