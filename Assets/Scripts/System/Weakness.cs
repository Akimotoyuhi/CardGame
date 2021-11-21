using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 脱力<br/>
/// 与えるダメージが25%減少する
/// </summary>
public class Weakness : Condition
{
    public override int Effect(EventTiming eventTiming, int num)
    {
        if (m_turn <= 0) return num;
        float ret = default;
        switch (eventTiming)
        {
            case EventTiming.TurnBegin:
                ret = num * (1 - 0.25f);
                return (int)ret;
            case EventTiming.Attacked:
                ret = num * (1 - 0.25f);
                return (int)ret;
            case EventTiming.TurnEnd:
                m_turn--;
                break;
            default:
                return num;
        }
        return num;
    }
    public override int IsBuff() { return 1; }
}
/// <summary>
/// 脆弱化<br/>
/// 得るブロックが25%減少
/// </summary>
public class Vulnerable : Condition
{
    public override int Effect(EventTiming eventTiming, int num)
    {
        if (m_turn <= 0) return num;
        float ret = default;
        switch (eventTiming)
        {
            case EventTiming.TurnBegin:
                ret = num * (1 - 0.25f);
                return (int)ret;
            case EventTiming.Attacked:
                ret = num * (1 - 0.25f);
                return (int)ret;
            case EventTiming.TurnEnd:
                m_turn--;
                break;
        }
        return num;
    }
    public override int IsBuff() { return 1; }
}