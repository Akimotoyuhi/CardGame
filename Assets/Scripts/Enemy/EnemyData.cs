using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField, Header("敵ステータスのデータ")]
    List<EnemyDataBase> m_enemyDataBases;
    [Header("エンカウントデータ")]
    [SerializeField] List<EncountDataBase> m_encountDatas; 
    public List<EnemyDataBase> Encount(EnemyType enemyType, MapID mapID)
    {
        List<EnemyDataBase> ret = new List<EnemyDataBase>();
        List<EncountIdData> encountData;
        switch (enemyType)
        {
            case EnemyType.Enemy:
                encountData = m_encountDatas[(int)mapID].Enemies;
                break;
            case EnemyType.Elite:
                encountData = m_encountDatas[(int)mapID].Elites;
                break;
            case EnemyType.Boss:
                encountData = m_encountDatas[(int)mapID].Bosses;
                break;
            default:
                Debug.Log("変な値入れんな");
                return null;
        }
        int r = UnityEngine.Random.Range(0, encountData.Count);
        for (int i = 0; i < encountData[r].GetID.Length; i++)
        {
            ret.Add(m_enemyDataBases[encountData[r].GetID[i]]);
        }
        return ret;
    }
}
#region 敵データ設定関係
[Serializable]
public class EnemyDataBase
{
    [SerializeField, Tooltip("名前")] string m_name;
    [SerializeField, Tooltip("敵ID")] EnemyID m_id;
    [SerializeField, Tooltip("最大体力")] int m_life;
    [SerializeField, Tooltip("画像")] Sprite m_sprite;
    [SerializeField, Tooltip("画像のデカさ倍率")] float m_spriteScaleMagnification = 1;
    [SerializeField, Tooltip("敵の種類")] EnemyType m_enemyType;
    public string Name => m_name;
    public EnemyID ID => m_id;
    public int Life => m_life;
    public Sprite Image => m_sprite;
    public float ScaleMagnification => m_spriteScaleMagnification;
    public EnemyType EnemyType => m_enemyType;
    public enum NodeType { Selector, Sequence }
    private NodeType m_NodeType = NodeType.Sequence;
    public List<EnemyBaseState> m_enemyBaseState;
    public EnemyActionCommnad3 CommandSelect(EnemyBase enemy, int turn)
    {
        switch (m_NodeType)
        {
            case NodeType.Sequence: //一つでも失敗したらそこで終了
                for (int i = 0; i < m_enemyBaseState.Count; i++)
                {
                    bool flag = false;
                    for (int n = 0; n < m_enemyBaseState[i].m_conditionalCommand.Count; n++)
                    {
                        if (!m_enemyBaseState[i].m_conditionalCommand[n].Conditional(enemy, turn))
                        {
                            flag = false;
                            break;
                        }
                        flag = true;
                    }
                    if (flag)
                    {
                        return m_enemyBaseState[i].m_actionCommnad;
                    }
                }
                return null;
            default:
                Debug.LogError("無効なケース");
                return null;
        }
    }
}

[Serializable]
public class EnemyBaseState
{
    public List<EnemyConditionalCommand3> m_conditionalCommand;
    public EnemyActionCommnad3 m_actionCommnad;
}

[Serializable]
public class EnemyActionCommnad3
{
    [Header("行動データ")]
    [SerializeReference, SubclassSelector] List<ICommand> m_commands;
    [SerializeField] TargetType m_target;
    [SerializeField] List<Plan> m_plan;
    [Serializable]
    public class Plan
    {
        [SerializeField, Tooltip("行動予定")] ActionPlan m_plan;
        [SerializeField, Tooltip("行動予定に数値を入れる場合、コマンド配列のどのindexを参照するか")] int m_numIndex;
        public ActionPlan ActionPlan => m_plan;
        public int NumIndex => m_numIndex;
    }
    public List<int[]> Command
    {
        get
        {
            List<int[]> ret = new List<int[]>();
            foreach (var c in m_commands)
                ret.Add(c.Execute());
            return ret;
        }
    }
    public TargetType Target => m_target;
    public List<Plan> ActionPlans => m_plan;
}

[Serializable]
public class EnemyConditionalCommand3
{
    [Header("条件データ")]
    [SerializeField, Tooltip("条件")] WhereType m_type;
    [SerializeField, Tooltip("評価する値")] int m_value;
    [SerializeField, Tooltip("この行動をする確率"), Range(0, 100)] int m_probability;
    [SerializeField, Tooltip("この行動を一度きりにするかどうか")] bool m_isOnlyOnce;
    private bool m_isOnlyFlag;
    public bool SetOnlyFlag
    {
        set
        {
            if (m_isOnlyOnce) //一度きりにするフラグがtrue出なければ書き換えはしない
            {
                m_isOnlyFlag = value;
            }
        }
    }
    /// <summary>
    /// 条件式
    /// </summary>
    /// <returns>成功可否</returns>
    public bool Conditional(EnemyBase enemy, int turn)
    {
        if (m_isOnlyFlag) return false;
        if (turn == 0)
        {
            if (m_type == WhereType.BattleBegin) { return true; }
            else { return false; }
        }

        switch (m_type)
        {
            case WhereType.Any:
                return true;
            case WhereType.Turn:
                if (turn == m_value)
                {
                    return true;
                }
                return false;
            case WhereType.RowTurn:
                if (turn <= m_value)
                {
                    return true;
                }
                return false;
            case WhereType.HighTurn:
                if (turn >= m_value)
                {
                    return true;
                }
                return false;
            case WhereType.MultipleTurn:
                if (turn % m_value == 0)
                {
                    return true;
                }
                return false;
            case WhereType.RowLife:
                if (enemy.CurrentLife <= m_value)
                {
                    return true;
                }
                return false;
            case WhereType.HighLife:
                if (enemy.CurrentLife >= m_value)
                {
                    return true;
                }
                return false;
            case WhereType.BattleBegin:
                return false;
            default:
                Debug.LogError("無効な条件ケース");
                return false;
        }
    }
}
#endregion
[Serializable]
public class EncountDataBase
{
    [SerializeField] string m_label;
    [SerializeField, Header("Enemy")] List<EncountIdData> m_enemies;
    [SerializeField, Header("Elite")] List<EncountIdData> m_elites;
    [SerializeField, Header("Boss")] List<EncountIdData> m_bosses;
    public List<EncountIdData> Enemies => m_enemies;
    public List<EncountIdData> Elites => m_elites;
    public List<EncountIdData> Bosses => m_bosses;
}
[Serializable]
public class EncountIdData
{
    [SerializeField] EnemyID[] m_enemyID;
    /// <summary>
    /// 敵IDの取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int[] GetID
    {
        get
        {
            int[] ret = new int[m_enemyID.Length];
            for (int i = 0; i < m_enemyID.Length; i++)
            {
                ret[i] = (int)m_enemyID[i];
            }
            return ret;
        }
    }
}
#region Enums
/// <summary>敵ID</summary>
public enum EnemyID
{
    /// <summary>カマクジ</summary>
    Kamakuji,
    /// <summary>ガイコツ</summary>
    Skelton,
    /// <summary>塔の守護者</summary>
    TowerGuardian,
    /// <summary>カマクジα</summary>
    KamakujiAlpha,
    /// <summary>ゾンビ犬</summary>
    ZombieDog,
    /// <summary>ペリュトン</summary>
    Peryton,
    /// <summary>深き者ども</summary>
    DeepOnes,
    /// <summary>リヴィアタン</summary>
    Leviathan,
    /// <summary>サーベルタイガー</summary>
    SavelTiger,
    /// <summary>タツノオトシゴ</summary>
    Seahorse,
    /// <summary>なそべま</summary>
    Nasobema,
    /// <summary>デスネズミ</summary>
    DeathRat,
    /// <summary>ワイバーン</summary>
    Ybarn,
    /// <summary>炎の魔術師</summary>
    FlameWizard,
    /// <summary>水の魔術師</summary>
    WaterWizard,
    /// <summary>風の魔術師</summary>
    WindWizard,
    /// <summary>雷の魔術師</summary>
    LightningWizard,
    /// <summary>エターナルドラゴン</summary>
    EternalDragon,
    /// <summary>アザトース</summary>
    Azathoth,
    /// <summary>ハーピー</summary>
    Harpy,
}
/// <summary>敵の種類</summary>
public enum EnemyType
{
    Enemy,
    Elite,
    Boss,
}
/// <summary>行動予定</summary>
public enum ActionPlan
{
    Attack,
    Block,
    Buff,
    Debuff,
    Unknown,
}
/// <summary>攻撃の標的</summary>
public enum TargetType
{
    ToPlayer,
    ToEnemy,
}
/// <summary>敵行動条件</summary>
public enum WhereType
{
    Any,
    Turn,
    RowTurn,
    HighTurn,
    MultipleTurn,
    RowLife,
    HighLife,
    BattleBegin,
}
#endregion