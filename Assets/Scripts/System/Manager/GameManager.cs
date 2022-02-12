using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mastar;

public enum CellClickEventType { Battle, Event, Rest }
public enum CardDisplayType { View, Upgrade }
public class GameManager : MonoBehaviour
{
    /// <summary>ゲームの進行状況</summary>
    [SerializeField] int m_step;
    /// <summary>カードデータ</summary>
    [Space]
    [SerializeField] NewCardData m_cardData;
    /// <summary>カードのプレハブ</summary>
    [SerializeField] BlankCard m_cardPrefab;
    /// <summary>Relicの画像の親</summary>
    [Header("UI関連")]
    [SerializeField] Transform m_relicParent;
    /// <summary>カードを表示させるためのCanvas</summary>
    [SerializeField] Canvas m_cardDisplayCanvas;
    /// <summary>CardDisplayCanvasに表示させるカードの親</summary>
    [SerializeField] Transform m_cardDisplayParent;
    [SerializeField] Canvas m_mapCanvas;
    [SerializeField] Map m_map;
    [SerializeField] Canvas m_eventCanvas;

    public static GameManager Instance { get; private set; }
    public int Step => DataManager.Instance.Step;
    public NewCardData CardData => m_cardData;
    public BlankCard CardPrefab => m_cardPrefab;
    public int Heal { set => DataManager.Instance.CurrentLife += value; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //if (DataManager.Instance.IsSaveData())
        //{

        //}
        //else
        //{
        //    m_map.CreateMap();
        //}
        m_map.CreateMap();
        m_eventCanvas.enabled = false;
        m_cardDisplayCanvas.enabled = false;
        m_step = DataManager.Instance.Step;
        BattleManager.Instance.Setup();
    }

    public void OnClick(CellState cellState)
    {
        m_mapCanvas.enabled = false;
        switch (cellState)
        {
            case CellState.Enemy:
                if (DataManager.Instance.Act == 1) { BattleManager.Instance.BattleStart(EnemyAppearanceEria.Act1Enemy); }
                else { Debug.LogError("まだ作ってないねん"); }
                BattleManager.Instance.IsGame = true;
                BattleManager.Instance.SetCanvas();
                break;
            case CellState.Boss:
                if (DataManager.Instance.Act == 1) { BattleManager.Instance.BattleStart(EnemyAppearanceEria.Act1Boss); }
                else { Debug.LogError("まだ作ってないねん"); }
                BattleManager.Instance.IsGame = true;
                BattleManager.Instance.SetCanvas();
                break;
            case CellState.Rest:
                m_eventCanvas.GetComponent<Rest>().StartEvent();
                m_eventCanvas.enabled = true;
                break;
        }
    }

    /// <summary>
    /// カード表示<br/>
    /// 自分のデッキ表示や強化画面で使われる
    /// </summary>
    public void DisplayCard(CardDisplayType cardDisplayType)
    {
        List<int[]> data;
        switch (cardDisplayType)
        {
            case CardDisplayType.View:
                data = DataManager.Instance.Cards;
                break;
            case CardDisplayType.Upgrade:
                data = new List<int[]>(DataManager.Instance.Cards.Where(i => i[1] == 0));
                break;
            default:
                Debug.Log("無効なケース");
                return;
        }
        for (int i = 0; i < data.Count; i++)
        {
            BlankCard card = Instantiate(CardPrefab);
            card.transform.SetParent(m_cardDisplayParent, false);
            card.SetInfo(m_cardData.CardDatas[data[i][0]]);
        }
    }
    public void DisplayCard()
    {
        List<int[]> data = DataManager.Instance.Cards;
        for (int i = 0; i < data.Count; i++)
        {
            BlankCard card = Instantiate(CardPrefab);
            card.transform.SetParent(m_cardDisplayParent, false);
            card.SetInfo(m_cardData.CardDatas[data[i][0]]);
            card.CardState = CardState.Upgrade;
        }
        m_cardDisplayCanvas.enabled = true;
    }

    /// <summary>
    /// 現在のフロアが終了した時の処理
    /// </summary>
    public void FloorFinished(Player player = null)
    {
        if (player)
        {
            DataManager.Instance.SavePlayerState(player.Name, player.Image, player.MaxLife, player.CurrentLife);
            Destroy(player.gameObject);
        }
        DataManager.Instance.Step++;
        m_step = DataManager.Instance.Step;
        m_map.AllColorChange();
        m_mapCanvas.enabled = true;
        BattleManager.Instance.IsGame = false;
        BattleManager.Instance.SetCanvas();
        m_eventCanvas.enabled = false;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #region エディタ拡張
    /// <summary>
    /// インスペクタのStepを反映させる<br/>エディタ拡張用
    /// </summary>
    public void GUIUpdate()
    {
        DataManager.Instance.Step = m_step;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion
}
