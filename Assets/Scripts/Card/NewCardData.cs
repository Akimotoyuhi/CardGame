using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

[CreateAssetMenu]
public class NewCardData : ScriptableObject
{
    public List<NewCardDataBase> m_cardData = new List<NewCardDataBase>();
}

public enum CardID
{
    PowerfulStrike, //デフォルトカード
    DEFStrengthening, //デフォルトカード
    CatScratch,
    StructuralFortification, //特殊カード
    TacticalCoordination, //特殊カード
    Meltdown, //特殊カード
    Conviction
}

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
    [TextArea(0,5), Tooltip("変数に差し替えたい部分は{%value}のように記述する事")]
    [SerializeField] string m_tooltip;
    /// <summary>攻撃</summary>
    [SerializeField] int m_power;
    /// <summary>攻撃回数</summary>
    [SerializeField] int m_attackNum;
    /// <summary>ブロック</summary>
    [SerializeField] int m_block;
    /// <summary>ブロック回数</summary>
    [SerializeField] int m_blockNum;
    //[Header("効果設定")]
    //[SerializeField, SerializeReference, SubclassSelector]
    //List<IEffect> m_effects = new List<IEffect>();
    //public IEffect m_effect;
    [Header("カードを使用した際に付与するコンディションの設定")]
    [SerializeField, SerializeReference, SubclassSelector]
    List<ICondition> m_cardConditionSets = new List<ICondition>();
    /// <summary>使用する標的</summary>
    [SerializeField] UseType m_cardType = new UseType();

    public string CardName => m_name;
    public Sprite Image => m_image;
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
            foreach (var item in m_cardConditionSets)
            {
                var con = item as Condition;
                if (con == null) continue;
                ret.Add(con);
            }
            return ret;
        }
    }
    public UseType UseType => m_cardType;

    //エラー回避用 後で消す
    [Serializable]
    public class Iranai
    {
        public Kari m_effect = new Kari();

        public class Kari
        {
            public string GetTooltip()
            {
                return "";
            }
            public CardBase GetParam()
            {
                CardBase ret = new CardBase();
                return ret;
            }
        }
    }
    public List<Iranai> m_cardEffectSets = new List<Iranai>();

    /// <summary>
    /// カードの説明分組み立て
    /// </summary>
    /// <returns></returns>
    public string GetTooltip()
    {
        string ret = "";
        for (int i = 0; i < m_cardEffectSets.Count; i++)
        {
            ret += m_cardEffectSets[i].m_effect.GetTooltip();
            if (m_cardType == UseType.ToPlayer) ret += "得る";
            else if (m_cardType == UseType.ToEnemy) ret += "与える";
            if (i < m_cardEffectSets.Count - 1) ret += "\n";
        }
        return ret;
    }

    /// <summary>
    /// カードのパラメーター組み立て
    /// </summary>
    /// <returns></returns>
    public CardBase GetParam()
    {
        CardBase cardBase = new CardBase();
        for (int i = 0; i < m_cardEffectSets.Count; i++)
        {
            cardBase.m_attack += m_cardEffectSets[i].m_effect.GetParam().m_attack;
            cardBase.m_block += m_cardEffectSets[i].m_effect.GetParam().m_block;
            for (int n = 0; n < (int)BuffDebuff.end; n++)
            {
                cardBase.conditions[n] += m_cardEffectSets[i].m_effect.GetParam().conditions[n];
            }
        }
        return cardBase;
    }
}

[Serializable]
public class AttackOrBlock
{
    public enum Type { Attack, Block }
    [SerializeField] Type m_type;
    /// <summary>ぱわあ</summary>
    [SerializeField] int m_power;
    /// <summary>回数</summary>
    [SerializeField] int m_number;
}