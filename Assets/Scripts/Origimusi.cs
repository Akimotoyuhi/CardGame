using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Origimusi : EnemyBase
{
    public override void Action(int Turn)
    {
        int[] nums = new int[(int)BuffDebuff.end];

        //攻撃パターン(仮)
        if (Turn % 2 != 0)
        {
            nums[(int)BuffDebuff.Vulnerable] = 1;
        }
        else if (Turn % 2 == 0)
        {
            nums[(int)BuffDebuff.Damage] = SetAttack(3);
        }
        m_player.GetAcceptDamage(nums);
    }
}
