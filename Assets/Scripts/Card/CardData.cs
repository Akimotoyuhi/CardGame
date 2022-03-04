using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

/// <summary>
/// カードデータ<br/>
/// 報酬画面に出現する確率比重を管理している。
/// </summary>
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
/// <summary>カードのレア度</summary>
public enum CardID
{
    /// <summary>斬撃</summary>
    Slashing, //スターター
    /// <summary>防御</summary>
    Defense, //スターター
}
/// <summary>カードのレア度</summary>
public enum Rarity
{
    Common,
    Rare,
    Elite,
    Special,
    Curse,
    BadEffect
}
/// <summary>カードの使用対象</summary>
public enum UseType
{
    ToPlayer,
    ToEnemy,
    ToAll,
    ToRandom,
}
/// <summary>カードの使用時の効果が何なのか</summary>
public enum CommandParam
{
    Attack,
    Block,
    Conditon,
}
#endregion
/// <summary>カードのデータベース<br/>カードの効果の部分</summary>
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
    /// <summary>強化前後のカード情報<br/>引数は複数回のカード強化を想定してintにしている</summary>
    /// <param name="upgradeNum">強化前後。0が前、1が後</param>
    /// <returns>カード情報</returns>
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
/// <summary>カードデータの使用時の効果の部分</summary>
[Serializable]
public class CardInfomationData
{
    [SerializeField] string m_name;
    [SerializeField] string m_cost;
    [SerializeField] Sprite m_image;
    [TextArea(0, 5), Tooltip("変数に差し替えたい部分は{length0}(数値の部分は配列番号)のように記述する事")]
    [SerializeField] string m_tooltip;
    [SerializeField] Rarity m_rarity;
    [SerializeReference, SubclassSelector] List<ICardCommand> m_commands;
    [SerializeField] UseType m_cardType = new UseType();
    [SerializeField] bool m_isDiscarding = false;
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
    /// <summary>
    /// カード使用時の効果<br/>{ 使用対象(UseType), 効果(int) }
    /// </summary>
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
    /// <summary>使用する標的</summary>
    public UseType UseType => m_cardType;
    /// <summary>廃棄カード</summary>
    public bool IsDiscarding => m_isDiscarding;
}

public interface ICardCommand
{
    /// <summary>カードを使用した時の効果</summary>
    /// <returns>{ 使用対象(UseType), 効果の種類(CommandParam), 効果(int) }</returns>
    int[] Execute();
}
[Serializable]
public class CardAttackCommand : ICardCommand
{
    [SerializeField] int m_power;
    [SerializeField] UseType m_useType;
    public int[] Execute() => new int[] { (int)m_useType, (int)CommandParam.Attack, m_power };
}
[Serializable]
public class CardBlockCommnad : ICardCommand
{
    [SerializeField] int m_block;
    [SerializeField] UseType m_useType;
    public int[] Execute() => new int[] { (int)m_useType, (int)CommandParam.Block, m_block };
}
[Serializable]
public class CardConditionCommand : ICardCommand
{
    [SerializeField] ConditionSelection m_condition;
    [SerializeField] UseType m_useType;
    public int[] Execute() => new int[] { (int)m_useType, (int)CommandParam.Conditon, (int)m_condition.GetCondition.GetConditionID(), m_condition.GetCondition.Turn };
}