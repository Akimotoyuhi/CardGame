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
    kyougeki, //デフォルトカード
    bougyoryokuUp, //デフォルトカード
    hikkaki
}

public enum CardType
{
    ToPlayer,
    ToEnemy,
    NotPlay
}

[System.Serializable]
public class NewCardDataBase
{
    public string m_name;
    public int m_cost;
    public Sprite m_image;
    [System.Serializable]
    public class CardEffectSet
    {
        [Header("効果設定")]
        [SerializeReference, SubclassSelector]
        public IEffect m_effect;
    }
    public List<CardEffectSet> m_cardEffectSets = new List<CardEffectSet>();
    public CardType m_cardType = CardType.NotPlay;

    public string GetTooltip()
    {
        string ret = "";
        for (int i = 0; i < m_cardEffectSets.Count; i++)
        {
            ret += m_cardEffectSets[i].m_effect.GetTooltip();
            if (i < m_cardEffectSets.Count - 1) continue;
            ret += "\n";
        }
        return ret;
    }

    public int[] GetParam()
    {
        int[] nums = new int[(int)BuffDebuff.end];
        for (int i = 0; i < m_cardEffectSets.Count; i++) //設定されたエフェクトの回数分
        {
            for (int n = 0; n < m_cardEffectSets[i].m_effect.GetParam().Length; n++) //エフェクト全てを入れる
            {
                nums[n] += m_cardEffectSets[i].m_effect.GetParam()[n]; //index1は５になってる
            }
        }
        return nums;
    }
}