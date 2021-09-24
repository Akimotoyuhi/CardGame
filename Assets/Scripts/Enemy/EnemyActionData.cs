using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyActionData : ScriptableObject
{
    public EnemyData[] m_enemyDatas;
}

[System.Serializable]
public class EnemyData
{
    [Header("バフデバフの設定")]
    [SerializeField] private BuffDebuff[] m_buffDebuff;
    [SerializeField] private int[] m_turn;
    //[SerializeField] private bool m_toPlayer;

    public int[] Action()
    {
        int[] nums = new int[(int)BuffDebuff.end];
        for (int i = 0; i < m_buffDebuff.Length; i++)
        {
            nums[(int)m_buffDebuff[i]] += m_turn[i];
        }
        /*
        if (turn % 2 != 0)
        {
            nums[(int)BuffDebuff.Vulnerable] = 1;
        }
        else if (turn % 2 == 0)
        {
            nums[(int)BuffDebuff.Damage] = SetAttack(m_buffDebuff == );
        }
        */
        return nums;
    }
}