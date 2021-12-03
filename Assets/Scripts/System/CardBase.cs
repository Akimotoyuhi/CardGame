using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>カードデータからBlankCardに渡しやすいようにしてある</summary>
public class CardBase
{
    public int m_attack;
    public int m_block;
    public string m_cost;
    public string m_tooltip;
    public Sprite m_image;
    public List<Condition> m_condition;
    public int[] conditions = new int[(int)ConditionID.end];
}
