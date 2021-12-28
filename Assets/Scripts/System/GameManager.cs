using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mastar;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject m_mapCanvas;
    [SerializeField] Map m_map;
    public static GameManager Instance { get; private set; }
    public int Step => GodGameManager.Instance.Step;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        BattleManager.Instance.Setup();
        m_map.CreateMap();
    }

    public void Battle(int id, int step)
    {
        if (step != GodGameManager.Instance.Step)
        {
            Debug.Log("選択不可");
            return;
        }
        m_mapCanvas.GetComponent<Canvas>().enabled = false;
        BattleManager.Instance.Battle(id);
        BattleManager.Instance.SetGameManager = this;
    }

    public void SceneReload()
    {
        GodGameManager.Instance.Step++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
