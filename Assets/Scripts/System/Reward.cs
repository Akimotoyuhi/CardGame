using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    /// <summary>��V��ʃp�l��</summary>
    [SerializeField] GameObject m_panel;
    /// <summary>�\������J�[�h�̃v���n�u</summary>
    [SerializeField] BlankCard m_cardPrefab;
    /// <summary>�e�I�u�W�F�N�g</summary>
    [SerializeField] Transform m_parent;
    /// <summary>�\�����郌���b�N�̃v���n�u</summary>
    [SerializeField] Relic m_relicPrefab;
    private List<CardInfomationData> m_cardDatas = new List<CardInfomationData>();
    private List<RelicDataBase> m_relicDatas = new List<RelicDataBase>();
    private System.Action m_nextMethod;
    public RectTransform CanvasRectTransform { private get; set; }
    //public List<CardInfomationData> CardData => m_cardDatas;
    public List<CardInfomationData> CardData { set => m_cardDatas = value; }
    public List<RelicDataBase> RelicData => m_relicDatas;

    /// <summary>
    /// ��V��ʕ\��
    /// </summary>
    /// <param name="nextMethod"></param>
    public void ShowRewardPanel(System.Action nextMethod)
    {
        RewardDisabled();
        m_nextMethod = nextMethod;
        SetCardRewardView();
    }

    /// <summary>
    /// �J�[�h�p��V��ʕ\��
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
    /// �����b�N�p��V���
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
    /// ��V�J�[�h���N���b�N������
    /// </summary>
    public void OnClickCard(CardID id, int upgrade)
    {
        BattleManager.Instance.GetCard(id, upgrade);
        RewardDisabled();
        SetRelicRewardView();
    }

    /// <summary>
    /// ��V�����b�N���N���b�N������
    /// </summary>
    public void OnClickRelic(RelicID relicId)
    {
        BattleManager.Instance.GetRelic(relicId);
        RewardDisabled();
        m_nextMethod();
    }

    /// <summary>
    /// ��V��ʂ����
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