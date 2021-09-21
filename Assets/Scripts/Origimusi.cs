using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Origimusi : EnemyBase
{


    public override void Action()
    {
        m_player.Damage(3);
    }
}
