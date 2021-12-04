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
    [Header("�G�̊�{���")]
    [SerializeField] string m_name;
    [SerializeField] int m_hp;
    [SerializeField] Sprite m_image;
    [SerializeField] List<EnemyCommand2> m_enemyCommand2s;
    public string Name => m_name;
    public int HP => m_hp;
    public Sprite Image => m_image;
    public List<EnemyCommand2> EnemyCommand2s => m_enemyCommand2s;
}

public interface ICommand2
{
    bool Execute();
}

public class Action : ICommand2
{
    [SerializeField] int m_power;
    [SerializeField] int m_block;
    [SerializeField] ConditionSelection m_condition;
    //������ICondition�ݒ肳����ƃo�O��

    public bool Execute()
    {
        //�����ɍs���������������ǁADatabase�ɍs�����̂��̂͋L�q���邱�Ƃ��o���Ȃ��̂ō����Ă�
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