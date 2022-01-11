using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mastar;

public enum CellClickEventType { Battle, Event, Rest }
public class GameManager : MonoBehaviour
{
    [SerializeField] Canvas m_mapCanvas;
    [SerializeField] Map m_map;
    [SerializeField] Canvas m_battleCanvas;
    [SerializeField] Canvas m_eventCanvas;
    public static GameManager Instance { get; private set; }
    public int Step => GodGameManager.Instance.Step;
    public int Heal { set => GodGameManager.Instance.Heal = value; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //BattleManager.Instance.Setup();
        if (GodGameManager.Instance.StartCheck())
        {

        }
        else
        {
            m_map.CreateMap();
        }
        m_eventCanvas.enabled = false;
    }

    public void OnClick(CellState cellState, int id)
    {
        m_mapCanvas.enabled = false;
        switch (cellState)
        {
            case CellState.Enemy:
                BattleManager.Instance.Battle(id);
                BattleManager.Instance.SetGameManager = this;
                m_battleCanvas.enabled = true;
                break;
            case CellState.Rest:
                m_eventCanvas.GetComponent<RestTest>().StartEvent();
                m_eventCanvas.enabled = true;
                break;
        }
        
    }
    public void FloorFinished(Player player = null)
    {
        if (player)
        {
            GodGameManager.Instance.SavePlayerState(player.Name, player.Image, player.MaxLife, player.CurrentLife);
        }
        Destroy(player.gameObject);
        GodGameManager.Instance.Step++;
        m_mapCanvas.enabled = true;
        m_battleCanvas.enabled = false;
        m_eventCanvas.enabled = false;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
