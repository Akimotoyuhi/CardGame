using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バフデバフ
/// </summary>
public enum BuffDebuff
{
    Damage = 0,
    Weakness = 1,
    end = 2,
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
    [SerializeField] BuffDebuff[] m_buffDebuff;
    [SerializeField] int[] m_effectTurn;

    public string Name { get { return m_cardName; } }

    public int Cost { get { return m_cost; } }

    public int Damage { get { return m_damage; } }

    public int Defense { get { return m_defence; } }
    
    public int GiveStateNum { get { return m_buffDebuff.Length; } }

    public BuffDebuff GiveState(int i) { return m_buffDebuff[i]; }

    public int GiveStateTrun(int i) { return m_effectTurn[i]; }
}