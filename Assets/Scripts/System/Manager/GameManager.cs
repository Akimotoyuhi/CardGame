﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Mastar;

public enum CellClickEventType { Battle, Event, Rest }
public enum CardDisplayType { View, Upgrade }
public class GameManager : MonoBehaviour
{
    /// <summary>ゲームの進行状況</summary>
    [SerializeField] int m_step;
    [SerializeField] bool m_isSeed;
    [SerializeField] int m_seed;
    /// <summary>カードデータ</summary>
    [Space]
    [SerializeField] CardData m_cardData;
    /// <summary>カードのプレハブ</summary>
    [SerializeField] BlankCard m_cardPrefab;
    /// <summary>Relicの画像の親</summary>
    [SerializeField] Transform m_relicParent;
    /// <summary>カードを表示させるためのCanvas</summary>
    [SerializeField] Canvas m_cardDisplayCanvas;
    /// <summary>CardDisplayCanvasに表示させるカードの親</summary>
    [SerializeField] Transform m_cardDisplayParent;
    [SerializeField] GameInfomation m_gameInfo;
    /// <summary>アップグレードの確認画面</summary>
    [SerializeField] Transform m_upgradeConfirmationPanel;
    /// <summary>アップグレードの確認画面の強化前のカードを表示する親</summary>
    [SerializeField] Transform m_upgradeBeforeCardParent;
    /// <summary>アップグレードの確認画面の強化後のカードを表示する親</summary>
    [SerializeField] Transform m_upgradeAfterCardParent;
    [SerializeField] Canvas m_mapCanvas;
    [SerializeField] Map m_map;
    [SerializeField] Canvas m_eventCanvas;
    [SerializeField] GameoverScreen m_gameoverScreen;
    private Subject<int> m_onSceneReload = new Subject<int>();

    public static GameManager Instance { get; private set; }
    public int Step => DataManager.Instance.Floor;
    public CardData CardData => m_cardData;
    public BlankCard CardPrefab => m_cardPrefab;
    public int Heal { set => DataManager.Instance.CurrentLife += value; }
    public void CardUpgrade(int index) => DataManager.Instance.CardUpgrade(index);
    public IObservable<int> OnSceneReload => m_onSceneReload;

    private void Awake()
    {
        Instance = this;
        if (m_isSeed)
        {
            UnityEngine.Random.InitState(m_seed);
        }
    }

    void Start()
    {
        EffectManager.Instance.Fade(Color.black, 0);
        EffectManager.Instance.Fade(Color.clear, 0.3f);
        m_map.CreateMap();
        m_eventCanvas.enabled = false;
        m_cardDisplayCanvas.enabled = false;
        m_upgradeConfirmationPanel.gameObject.SetActive(false);
        m_step = DataManager.Instance.Floor;
        BattleManager.Instance.Setup();
        SetGameInfoPanel();
    }

    /// <summary>
    /// マップのボタンのクリック後のデータを受け取る
    /// </summary>
    /// <param name="cellState"></param>
    public void OnClick(CellState cellState)
    {
        EffectManager.Instance.Fade(Color.black, 0.3f, () =>
        {
            m_mapCanvas.enabled = false;
            switch (cellState)
            {
                case CellState.Enemy:
                    if (DataManager.Instance.Act == 1) BattleManager.Instance.BattleStart(EnemyAppearanceEria.Act1Enemy);
                    else Debug.LogError("まだ作ってない");
                    BattleManager.Instance.IsGame = true;
                    BattleManager.Instance.SetCanvas();
                    break;
                case CellState.Elite:
                    if (DataManager.Instance.Act == 1) BattleManager.Instance.BattleStart(EnemyAppearanceEria.Act1Elite);
                    else Debug.LogError("まだ作ってない");
                    BattleManager.Instance.IsGame = true;
                    BattleManager.Instance.SetCanvas();
                    break;
                case CellState.Boss:
                    if (DataManager.Instance.Act == 1) BattleManager.Instance.BattleStart(EnemyAppearanceEria.Act1Boss);
                    else Debug.LogError("まだ作ってない");
                    BattleManager.Instance.IsGame = true;
                    BattleManager.Instance.SetCanvas();
                    break;
                case CellState.Rest:
                    m_eventCanvas.GetComponent<Rest>().StartEvent();
                    m_eventCanvas.enabled = true;
                    break;
            }
            EffectManager.Instance.Fade(Color.clear, 0.3f);
        });
    }

    /// <summary>
    /// ゲームの進行情報を表示しておくテキストの更新
    /// </summary>
    public void SetGameInfoPanel(CharactorBase player = null)
    {
        if (player)
        {
            m_gameInfo.SetText(DataManager.Instance.Name, player.MaxLife.ToString(), player.CurrentLife.ToString(), DataManager.Instance.Floor.ToString());
        }
        else
        {
            m_gameInfo.SetText(DataManager.Instance.Name, DataManager.Instance.MaxLife.ToString(), DataManager.Instance.CurrentLife.ToString(), DataManager.Instance.Floor.ToString());
        }
    }

    /// <summary>
    /// プレイヤーデータの保存
    /// </summary>
    public void PlayerDataSave(string name, Sprite idleSprite, Sprite gameoverSprite, int maxLife, int currentLife, int[] cardsID = null, int[] isCardUpgrade = null)
    {
        DataManager.Instance.SavePlayerState(name, idleSprite, gameoverSprite, maxLife, currentLife);
        if (cardsID == null) return;
        for (int i = 0; i < cardsID.Length; i++)
        {
            DataManager.Instance.AddCards(cardsID[i], isCardUpgrade[i]);
        }
    }
    
    /// <summary>
    /// カード表示<br/>
    /// 自分のデッキ表示や強化画面で使われる
    /// </summary>
    public void DisplayCard(Rest rest = null)
    {
        List<int[]> data = DataManager.Instance.Cards;
        if (rest)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i][1] == 0)
                {
                    BlankCard card = Instantiate(CardPrefab);
                    card.transform.SetParent(m_cardDisplayParent, false);
                    card.SetInfo(m_cardData.CardDatas[data[i][0]], i, rest);
                    card.CardState = CardState.Upgrade;
                }
            }
        }
        else
        {
            for (int i = 0; i < data.Count; i++)
            {
                BlankCard card = Instantiate(CardPrefab);
                card.transform.SetParent(m_cardDisplayParent, false);
                if (data[i][1] == 0) card.SetInfo(m_cardData.CardDatas[data[i][0]], i, rest);
                else card.SetInfo(m_cardData.CardDatas[data[i][0]].UpgradeData, i, rest);
                card.CardState = CardState.Upgrade;
            }
        }
        m_cardDisplayCanvas.enabled = true;
    }

    public void CrearCardDisplayPanel()
    {
        for (int i = 0; i < m_cardDisplayParent.childCount; i++)
        {
            Destroy(m_cardDisplayParent.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// アップグレードの確認画面表示
    /// </summary>
    public void UpgradeConfirmationPanel(int index)
    {
        m_upgradeConfirmationPanel.gameObject.SetActive(true);
        //アップグレード前のカード表示
        BlankCard card = Instantiate(CardPrefab);
        card.transform.SetParent(m_upgradeBeforeCardParent, false);
        card.SetInfo(m_cardData.CardDatas[DataManager.Instance.Cards[index][0]]);
        card.CardState = CardState.None;
        //アップグレード後のカード表示
        card = Instantiate(CardPrefab);
        card.transform.SetParent(m_upgradeAfterCardParent, false);
        card.SetInfo(m_cardData.CardDatas[DataManager.Instance.Cards[index][0]].UpgradeData);
        card.CardState = CardState.None;
    }

    /// <summary>
    /// アップグレードの確認画面を消す
    /// </summary>
    public void UpgradeConfirmationPanelDisabled()
    {
        Debug.Log(DataManager.Instance.Cards.Count);
        Destroy(m_upgradeBeforeCardParent.GetChild(0).gameObject);
        Destroy(m_upgradeAfterCardParent.GetChild(0).gameObject);
        CrearCardDisplayPanel();
        m_upgradeConfirmationPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 現在のフロアが終了した時の処理
    /// </summary>
    public void FloorFinished(Player player = null)
    {
        if (player)
        {
            PlayerDataSave(player.Name, player.sprite, player.GameoverSprite, player.MaxLife, player.CurrentLife);
            Destroy(player.gameObject);
        }
        DataManager.Instance.Floor++;
        SetGameInfoPanel();
        m_step = DataManager.Instance.Floor;
        m_map.AllColorChange();
        BattleManager.Instance.IsGame = false;
        EffectManager.Instance.Fade(Color.black, 0.5f, () =>
        {
            BattleManager.Instance.SetCanvas();
            m_mapCanvas.enabled = true;
            m_eventCanvas.enabled = false;
            m_cardDisplayCanvas.enabled = false;
            m_upgradeConfirmationPanel.gameObject.SetActive(false);
            EffectManager.Instance.Fade(Color.clear, 0.2f);
        });
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// ゲームオーバー
    /// </summary>
    public void Gameover()
    {
        BattleManager.Instance.IsGame = false;
        Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(_ =>
        {
            EffectManager.Instance.Fade(Color.black, 3f, () =>
            {
                m_gameoverScreen.ShowPanel(DataManager.Instance.Cards.Count, Step);
                //EffectManager.Instance.Fade(Color.clear, 0.1f);
            });
        });
    }
    #region エディタ拡張
    /// <summary>
    /// インスペクタのStepを反映させる<br/>エディタ拡張用
    /// </summary>
    public void GUIUpdate()
    {
        DG.Tweening.DOTween.KillAll();
        DataManager.Instance.Floor = m_step;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion
}
