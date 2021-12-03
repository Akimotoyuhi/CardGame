using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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
    [SerializeField] string m_tooltip;
    [SerializeField] int m_damage;
    [SerializeField] int m_defence;
    [SerializeField] ConditionID[] m_buffDebuff;
    [SerializeField] int[] m_effectTurn;

    public string Name => m_cardName;
    public int Cost => m_cost;
    public string Tooltip => m_tooltip;
    public int Damage => m_damage;
    public int Defense => m_defence;
    public int GiveStateNum => m_buffDebuff.Length;
    public ConditionID GiveState(int i) { return m_buffDebuff[i]; }
    public int GiveStateTrun(int i) { return m_effectTurn[i]; }
}