using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Origimusi : EnemyBase
{
    //[SerializeField] private int m_atk;

    public override void Action()
    {
        int[] nums = new int[(int)BuffDebuff.end];
        int r = Random.Range(0, 2);

        //攻撃パターン(仮)
        if (r == 0)
        {
            nums[(int)BuffDebuff.Vulnerable] = 1;
        }
        else if (r == 1)
        {
            nums[(int)BuffDebuff.Damage] = SetAttack(3);
        }
        m_player.GetAcceptDamage(nums);
    }
}
