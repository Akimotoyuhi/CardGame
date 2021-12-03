using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class EnemyData2 : ScriptableObject
{
    public List<EnemyDataBase2> enemyDataBase2s = new List<EnemyDataBase2>();
}

[Serializable]
public class EnemyDataBase2
{
    [Header("敵の基本情報")]
    [SerializeField] string m_name;
    [SerializeField] int m_hp;
    [SerializeField] Sprite m_image;
    [Serializable]
    public class SetCommand
    {
        public enum ForkingNode { Selector, Sequence }
        public ForkingNode m_forkingNode;
        [SerializeReference, SubclassSelector]
        public List<ICommand2> m_command2s = new List<ICommand2>();
    }
    public SetCommand m_command;
}

public interface ICommand2
{
    bool Execute();
}

public class Action : ICommand2
{
    [SerializeField] int m_power;
    [SerializeField] int m_block;
    [SerializeField, SerializeReference, SubclassSelector]
    Condition m_condition;
    //ここにICondition設定させるとバグる

    public bool Execute()
    {
        //ここに行動を書きたいけど、Databaseに行動そのものは記述することが出来ないので困ってる
        return true;
    }
}

public class Conditional : ICommand2
{
    public enum Type { a,b,c }

    public bool Execute()
    {
        return true;
    }
}