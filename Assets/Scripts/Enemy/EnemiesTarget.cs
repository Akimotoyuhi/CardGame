using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemiesTarget : MonoBehaviour, IDrop
{
    [SerializeField] GameObject m_flame;
    public void Setup()
    {
        m_flame.SetActive(false);
    }
    public bool CanDrop(UseTiming useType)
    {
        if (useType == UseTiming.ToAllEnemies) return true;
        else return false;
    }
    public void GetDrop(List<int[]> cardCommand)
    {
        BattleManager.Instance.CommandManager.CommandExecute(cardCommand, true);
    }
    public void OnCard(UseTiming? useType)
    {
        if (useType == UseTiming.ToAllEnemies) m_flame.SetActive(true);
        else m_flame.SetActive(false);
    }
    public EnemyBase IsEnemy()
    {
        return null;
    }
}
