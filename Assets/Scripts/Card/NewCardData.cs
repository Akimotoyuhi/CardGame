using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    Conviction,
    StructuralFortification, //特殊カード
    TacticalCoordination, //特殊カード
    Meltdown //特殊カード
}

public enum CardType
{
    ToPlayer,
    ToEnemy,
}

[System.Serializable]
public class NewCardDataBase
{
    public string m_name;
    public int m_cost;
    //public int Cost //コストｘのカード作る時に使う
    //{
    //    get
    //    {
    //    }
    //}
    public Sprite m_image;
    [System.Serializable]
    public class CardEffectSet
    {
        [Header("効果設定")]
        [SerializeReference, SubclassSelector]
        public IEffect m_effect;
    }
    public List<CardEffectSet> m_cardEffectSets = new List<CardEffectSet>();
    public CardType m_cardType;

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
            if (m_cardType == CardType.ToPlayer) ret += "得る";
            else if (m_cardType == CardType.ToEnemy) ret += "与える";
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
            cardBase.attack += m_cardEffectSets[i].m_effect.GetParam().attack;
            cardBase.block += m_cardEffectSets[i].m_effect.GetParam().block;
            for (int n = 0; n < (int)BuffDebuff.end; n++)
            {
                cardBase.conditions[n] += m_cardEffectSets[i].m_effect.GetParam().conditions[n];
            }
        }
        return cardBase;
    }
}