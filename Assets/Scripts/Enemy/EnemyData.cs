using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public List<EnemyDataBase> m_enemyDatas = new List<EnemyDataBase>();
}

[Serializable]
public class EnemyDataBase
{
    [Header("敵の基本情報")]
    [SerializeField] string m_name;
    [SerializeField] int m_hp;

    [Serializable]
    public class SetCommand
    {
        [Serializable]
        public class TurnCommand
        {
            [SerializeReference, SubclassSelector]
            public ICommand m_command;
        }
        [Header("1ターンで行う敵の行動")]
        public List<TurnCommand> m_commands = new List<TurnCommand>();
    }
    [Header("Element0は先制効果")]
    public List<SetCommand> m_commands = new List<SetCommand>();

    public EnemyCommand Action()
    {
        EnemyCommand ret = new EnemyCommand();
        for (int i = 0; i < m_commands.Count; i++)
        {
            //ret.m_attack = m_commands[i].m_command.GetParam().m_attack;
            //ret.m_block = m_commands[i].m_command.GetParam().m_block;
            //ret.m_conditions = m_commands[i].m_command.GetParam().m_conditions;
        }
        return ret;
    }
    public string Name { get { return m_name; } }
    public int HP { get { return m_hp; } }
}