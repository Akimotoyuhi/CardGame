using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    /// <summary>報酬画面パネル</summary>
    [SerializeField] GameObject m_panel;
    /// <summary>表示するカードのプレハブ</summary>
    [SerializeField] BlankCard m_cardPrefab;
    /// <summary>親オブジェクト</summary>
    [SerializeField] Transform m_parent;
    /// <summary>表示するレリックのプレハブ</summary>
    [SerializeField] Relic m_relicPrefab;
    private List<CardInfomationData> m_cardDatas = new List<CardInfomationData>();
    private List<RelicDataBase> m_relicDatas = new List<RelicDataBase>();
    private System.Action m_nextMethod;
    public RectTransform CanvasRectTransform { private get; set; }
    //public List<CardInfomationData> CardData => m_cardDatas;
    public List<CardInfomationData> CardData { set => m_cardDatas = value; }
    public List<RelicDataBase> RelicData => m_relicDatas;

    /// <summary>
    /// 報酬画面表示
    /// </summary>
    /// <param name="nextMethod"></param>
    public void ShowRewardPanel(System.Action nextMethod)
    {
        RewardDisabled();
        m_nextMethod = nextMethod;
        SetCardRewardView();
    }

    /// <summary>
    /// カード用報酬画面表示
    /// </summary>
    private void SetCardRewardView()
    {
        if (m_cardDatas != null && m_cardDatas[0] != null)
        {
            m_panel.SetActive(true);
            foreach (var data in m_cardDatas)
            {
                BlankCard card = Instantiate(m_cardPrefab);
                card.transform.SetParent(m_parent, false);
                card.transform.localScale = Vector2.one;
                card.SetInfo(data, 0, this);
                card.CardState = CardState.Reward;
            }
        }
        else
        {
            SetRelicRewardView();
        }
        m_cardDatas.Clear();
    }

    /// <summary>
    /// レリック用報酬画面
    /// </summary>
    private void SetRelicRewardView()
    {
        if (m_relicDatas != null && m_relicDatas[0] != null)
        {
            m_panel.SetActive(true);
            foreach (var data in m_relicDatas)
            {
                Relic relic = Instantiate(m_relicPrefab);
                relic.transform.SetParent(m_parent, false);
                //relic.transform.localScale = relic.transform.localScale * 2;
                relic.gameObject.GetRectTransform().sizeDelta = relic.gameObject.GetRectTransform().sizeDelta * 2;
                relic.Setup(data, this);
            }
        }
        else
        {
            m_nextMethod();
        }
        m_relicDatas.Clear();
    }

    /// <summary>
    /// 報酬カードをクリックした時
    /// </summary>
    public void OnClickCard(CardID id, int upgrade)
    {
        BattleManager.Instance.GetCard(id, upgrade);
        RewardDisabled();
        SetRelicRewardView();
    }

    /// <summary>
    /// 報酬レリックをクリックした時
    /// </summary>
    public void OnClickRelic(RelicID relicId)
    {
        BattleManager.Instance.GetRelic(relicId);
        RewardDisabled();
        m_nextMethod();
    }

    /// <summary>
    /// 報酬画面を閉じる
    /// </summary>
    public void RewardDisabled()
    {
        for (int i = 0; i < m_parent.childCount; i++)
        {
            Destroy(m_parent.GetChild(i).gameObject);
        }
        m_panel.SetActive(false);
    }
}