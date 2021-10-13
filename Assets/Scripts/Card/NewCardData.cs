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
    kyougeki = 0,
    bougyoryokuUp = 1
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
        [SerializeReference, SubclassSelector]
        public IEffect m_effect;
    }
    public List<CardEffectSet> m_cardEffectSets = new List<CardEffectSet>();
    public CardType m_cardType = CardType.NotPlay;
}