using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バフデバフ
/// </summary>
public enum BuffDebuff
{
    Damage = 0,
    Block = 1,
    /// <summary>脱力:与えるダメージが25%低下</summary>
    Weakness = 2,
    /// <summary>脆弱化:得るブロックが25%低下</summary>
    Vulnerable = 3,
    /// <summary>筋力:与えるダメージがn増加</summary>
    Strength = 5,
    /// <summary>敏捷性:得るブロックがn増加</summary>
    Agile = 6,
    end = 7,
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
    [SerializeField] string m_tooltip;
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