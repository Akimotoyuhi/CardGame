using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public List<EnemyDataBase> m_enemyDatas = new List<EnemyDataBase>();
}

/// <summary>
/// 敵ID
/// </summary>
public enum EnemyID
{
    origimusi,
    Soldier,
    endLength
}

[Serializable]
public class EnemyDataBase
{
    [Header("敵の基本情報")]
    [SerializeField] string m_name;
    [SerializeField] int m_hp;
    [SerializeField] Sprite m_image;
 
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
        public List<TurnCommand> m_turnCommands = new List<TurnCommand>();
    }
    [Header("Element0は先制効果")]
    public List<SetCommand> m_setCommands = new List<SetCommand>();

    public EnemyCommand[] SetAction()
    {
        EnemyCommand[] ret = new EnemyCommand[m_setCommands.Count];
        for (int i = 0; i < m_setCommands.Count; i++) //敵の行動(全体)
        {
            for (int n = 0; n < m_setCommands[i].m_turnCommands.Count; n++) //敵の行動(１ターンあたり)
            {
                ret[i] = m_setCommands[i].m_turnCommands[n].m_command.GetParam();
            }
        }
        return ret;
    }
    public string Name { get { return m_name; } }
    public int HP { get { return m_hp; } }
}