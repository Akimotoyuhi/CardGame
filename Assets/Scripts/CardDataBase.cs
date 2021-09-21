using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardID
{
    kyougekiBeta = 0,
    defenceUpArufa = 1,
}

[CreateAssetMenu]
public class CardDataBase : ScriptableObject
{
    //public List<CardData> m_cardDataList = new List<CardData>();
    public CardData m_cardData = new CardData();
}

[System.Serializable]
public class CardData
{
    [SerializeField] string m_cardName = "";
    [SerializeField] int m_cost;
    [SerializeField] int m_damage;
    [SerializeField] int m_defence;
    

    public string Name { get { return m_cardName; } }

    public int Cost { get { return m_cost; } }

    public int Damage { get { return m_damage; } }

    public int Defense { get { return m_defence; } }
}