using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCommandTest : MonoBehaviour
{
    //[SerializeField] EnemyData3 m_enemyData;
    //[SerializeField] int m_id = 0;
    //[SerializeField] string m_name;
    //[SerializeField] int m_life = 0;
    //[SerializeField] int m_turn;

    void Start()
    {
        SetParam();
    }

    private void SetParam()
    {
        //m_name = m_enemyData.m_enemyDataBase3s[m_id].Name;
        //m_life = m_enemyData.m_enemyDataBase3s[m_id].Life;
        //Debug.Log($"取得した敵データ : Name={m_name}, Life={m_life}");
    }

    public void OnCheck()
    {
        //if (m_enemyData.m_enemyDataBase3s[m_id].CommandSelect(m_turn, m_life) == null)
        //{
        //    Debug.Log("null");
        //    return;
        //}
        //Debug.Log($"敵の行動:{m_enemyData.m_enemyDataBase3s[m_id].CommandSelect(m_turn, m_life).Power}ダメージ与えてきた");
    }
}
