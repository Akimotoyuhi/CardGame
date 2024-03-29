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
    [SerializeField] bool m_isFixedMap;
    [SerializeField] MapID m_mapID;
    /// <summary>カードデータ</summary>
    [Space]
    [SerializeField] CardData m_cardData;
    /// <summary>カードのプレハブ</summary>
    [SerializeField] BlankCard m_cardPrefab;
    /// <summary>レリックデータ</summary>
    [SerializeField] RelicData m_relicData;
    /// <summary>レリックのプレハブ</summary>
    [SerializeField] Relic m_relicPrefab;
    /// <summary>レリックの親オブジェクト</summary>
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
    /// <summary>ゲームオーバー時のフェードアウトまでの時間</summary>
    [SerializeField] float m_gameoverFadeDuration = 3;
    private List<Relic> m_haveRelics = new List<Relic>();
    private Subject<int> m_onSceneReload = new Subject<int>();
    public static GameManager Instance { get; private set; }
    /// <summary>ゲーム進行度</summary>
    public int Floor => DataManager.Instance.Floor;
    /// <summary>カスタムの合計危険度</summary>
    public int TotalRisk => DataManager.Instance.TotalRisk;
    public CardData CardData => m_cardData;
    public BlankCard CardPrefab => m_cardPrefab;
    public RelicData RelicData => m_relicData;
    public List<Relic> HaveRelics => m_haveRelics;
    public int Heal { set => DataManager.Instance.CurrentLife += value; }
    public void CardUpgrade(int index) => DataManager.Instance.CardUpgrade(index);
    public IObservable<int> OnSceneReload => m_onSceneReload;

    private void Awake()
    {
        Instance = this;
        //ランダムの初期化
        if (m_isSeed)
        {
            UnityEngine.Random.InitState(m_seed);
        }
        else
        {
            //Seed値を適当に決める　RandomクラスにはSeed値を取得する関数が無いのでTickCountから取っている
            int seed = Environment.TickCount;
            UnityEngine.Random.InitState(seed);
            Debug.Log("シード値:" + seed);
        }
    }

    void Start()
    {
        EffectManager.Instance.Fade(Color.black, 0);
        EffectManager.Instance.Fade(Color.clear, 0.3f);
        if (m_isFixedMap)
            m_map.SetFixedMapDebug(m_mapID);
        m_map.CreateMap();
        m_eventCanvas.enabled = false;
        m_cardDisplayCanvas.enabled = false;
        m_upgradeConfirmationPanel.gameObject.SetActive(false);
        m_step = DataManager.Instance.Floor;
        BattleManager.Instance.Setup();
        SetGameInfoPanel();
        m_relicData.Setup();
    }

    /// <summary>
    /// マップのボタンのクリック後のデータを受け取る
    /// </summary>
    /// <param name="cellState"></param>
    public void Encount(CellState cellState, MapID mapID)
    {
        EffectManager.Instance.Fade(Color.black, 0.3f, () =>
        {
            m_mapCanvas.enabled = false;
            switch (cellState)
            {
                case CellState.Enemy:
                    BattleManager.Instance.BattleStart(EnemyType.Enemy, mapID);
                    BattleManager.Instance.SetCanvas();
                    break;
                case CellState.Elite:
                    BattleManager.Instance.BattleStart(EnemyType.Elite, mapID);
                    BattleManager.Instance.SetCanvas();
                    break;
                case CellState.Boss:
                    BattleManager.Instance.BattleStart(EnemyType.Boss, mapID);
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
    public void PlayerDataSave(string name, Sprite idleSprite, Sprite[] AttackedSprite, Sprite gameoverSprite, int maxLife, int currentLife, int[] cardsID = null, int[] isCardUpgrade = null)
    {
        DataManager.Instance.SavePlayerState(name, idleSprite, AttackedSprite, gameoverSprite, maxLife, currentLife);
        if (cardsID == null) return;
        for (int i = 0; i < cardsID.Length; i++)
        {
            DataManager.Instance.AddCards((CardID)cardsID[i], isCardUpgrade[i]);
        }
    }

    /// <summary>所持レリックの保存</summary>
    /// <param name="relicID"></param>
    public void SaveRelicData(RelicID relicID)
    {
        DataManager.Instance.HaveRelic.Add(relicID);
        SetViewRelic(m_relicData.DataBases[(int)relicID]);
    }

    /// <summary>レリックを画面上に生成</summary>
    private void SetViewRelic(RelicDataBase relicData)
    {
        Relic rel = Instantiate(m_relicPrefab);
        rel.transform.SetParent(m_relicParent);
        rel.Setup(relicData);
        HaveRelics.Add(rel);
    }

    public void RelicSetup()
    {
        foreach (var r in HaveRelics)
        {
            r.Setup(null);
        }
    }

    /// <summary>レリック効果の発動</summary>
    public void RelicExecute(RelicTriggerTiming relicTriggerTiming, ParametorType parametorType, int num)
    {
        foreach (var r in m_haveRelics)
        {
            r.Execute(relicTriggerTiming, parametorType, num);
        }
    }

    /// <summary>
    /// カスタム効果の評価
    /// </summary>
    public int CustomEvaluation(CustomEntityType entityType, CustomParamType paramType, int num)
    {
        if (DataManager.Instance.CustomList == null) return num;
        int ret = num;
        foreach (var item in DataManager.Instance.CustomList)
        {
            ret = item.CustomEffect(entityType, paramType, ret);
        }
        if (ret < 1)
            ret = 1;
        return ret;
    }

    /// <summary>
    /// カード表示<br/>
    /// 自分のデッキ表示や強化画面で使われる
    /// </summary>
    public void DisplayCard(Rest rest = null)
    {
        if (m_cardDisplayParent.childCount > 0) return;
        List<int[]> data = DataManager.Instance.Cards;
        //休憩マス(カードの強化)の場合は、既に強化済みのカードを除外して表示する
        if (rest)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i][1] == 0)
                {
                    BlankCard card = Instantiate(CardPrefab);
                    card.transform.SetParent(m_cardDisplayParent, false);
                    card.SetInfo(m_cardData.CardDatas(data[i][0], 0), i, rest);
                    card.CardState = CardState.Upgrade;
                }
            }
        }
        //それ以外なら全て表示
        else
        {
            for (int i = 0; i < data.Count; i++)
            {
                BlankCard card = Instantiate(CardPrefab);
                card.transform.SetParent(m_cardDisplayParent, false);
                card.SetInfo(m_cardData.CardDatas(data[i][0], data[i][1]), i, rest);
                card.CardState = CardState.Upgrade;
            }
        }
        m_cardDisplayCanvas.enabled = true;
    }

    /// <summary>
    /// カードを表示する画面を初期化して消す
    /// </summary>
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
        card.SetInfo(m_cardData.CardDatas(DataManager.Instance.Cards[index][0], 0));
        card.CardState = CardState.None;
        //アップグレード後のカード表示
        card = Instantiate(CardPrefab);
        card.transform.SetParent(m_upgradeAfterCardParent, false);
        card.SetInfo(m_cardData.CardDatas(DataManager.Instance.Cards[index][0], 1));
        card.CardState = CardState.None;
    }

    /// <summary>
    /// アップグレードの確認画面を消す
    /// </summary>
    public void UpgradeConfirmationPanelDisabled()
    {
        Destroy(m_upgradeBeforeCardParent.GetChild(0).gameObject);
        Destroy(m_upgradeAfterCardParent.GetChild(0).gameObject);
        m_upgradeConfirmationPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 現在のフロアが終了した時の処理
    /// </summary>
    public void FloorFinished(Player player = null)
    {
        if (player)
        {
            Debug.Log($"Save:PlayerCurrentLife{player.CurrentLife}");
            PlayerDataSave(player.Name, player.sprite, player.AttackedSprite, player.GameoverSprite, player.MaxLife, player.CurrentLife);
            Destroy(player.gameObject);
        }
        DataManager.Instance.Floor++;
        if (m_map.ClearCheck(DataManager.Instance.Floor))
        {
            Gameover(true);
            return;
        }
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
    }
    /// <summary>
    /// ゲームオーバー画面を呼ぶ
    /// </summary>
    public void Gameover(bool isCrear = false)
    {
        if (!BattleManager.Instance.IsGame)
            return;
        BattleManager.Instance.IsGame = false;
        AudioManager.Instance.Play(BGM.None);
        Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(_ =>
        {
            EffectManager.Instance.Fade(Color.black, m_gameoverFadeDuration, () =>
            {
                m_gameoverScreen.ShowPanel(DataManager.Instance.Cards.Count, Floor, isCrear);
                EffectManager.Instance.Fade(Color.clear, 0);
            });
        });
    }

    /// <summary>
    /// リトライボタン押した時の処理<br/>
    /// ボタンから呼ばれる事を想定している
    /// </summary>
    public void RetryButton()
    {
        EffectManager.Instance.Fade(Color.black, 0.5f, () =>
        {
            DataManager.Instance.Init();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }

    /// <summary>
    /// タイトルボタンを押された時の処理<br/>
    /// ボタンから呼ばれる事を想定している
    /// </summary>
    public void TitleButton()
    {
        EffectManager.Instance.Fade(Color.black, 0.5f, () =>
        {
            DataManager.Instance.Init();
            SceneManager.LoadScene("Title");
        });
    }

    #region エディタ拡張
    /// <summary>
    /// インスペクタのStepを反映させる<br/>エディタ拡張用
    /// </summary>
    public void GUIUpdate()
    {
        DG.Tweening.DOTween.KillAll();
        DataManager.Instance.Init();
        DataManager.Instance.Floor = m_step;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// 保存データの初期化<br/>
    /// エディタ拡張用
    /// </summary>
    public void DataReset()
    {
        DG.Tweening.DOTween.KillAll();
        DataManager.Instance.Init();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion
}
