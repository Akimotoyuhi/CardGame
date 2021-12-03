using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mastar;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject m_map;
    //[SerializeField] BattleManager m_battleManager;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        BattleManager.Instance.Setup();
    }

    public void Battle(int id)
    {
        m_map.GetComponent<Canvas>().enabled = false;
        BattleManager.Instance.Battle(id);
        BattleManager.Instance.SetGameManager = this;
    }

    public void SceneReload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
