using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyActionData : ScriptableObject
{
    public List<EnemyData> m_enemyDatas = new List<EnemyData>();
}

[System.Serializable]
public class EnemyData
{
    public string m_name;
    public int m_maxhp;
    [System.Serializable]
    public class SetCommand
    {
        [SerializeReference, SubclassSelector]
        public ICommand m_command;
        //[SerializeField] bool m_toPlayer;
    }
    [Header("Element0は先制効果です")]
    [SerializeField] List<SetCommand> m_commands = new List<SetCommand>();

    public EnemyCommand Action()
    {
        EnemyCommand ret = new EnemyCommand();
        for (int i = 0; i < m_commands.Count; i++)
        {
            ret.m_attack = m_commands[i].m_command.GetParam().m_attack;
            ret.m_block = m_commands[i].m_command.GetParam().m_block;
            ret.m_conditions = m_commands[i].m_command.GetParam().m_conditions;
        }
        return ret;
        //int[] nums = new int[(int)BuffDebuff.end];
        //for (int i = 0; i < m_buffDebuff.Length; i++)
        //{
        //    nums[(int)m_buffDebuff[i]] += m_turn[i];
        //}
        //return nums;
    }
}