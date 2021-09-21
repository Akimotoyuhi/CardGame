using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Origimusi : EnemyBase
{
    [SerializeField] private int m_atk;

    public override void Action()
    {
        m_player.Damage(SetAttack(m_atk));
        Debug.Log(SetAttack(m_atk));
    }
}
