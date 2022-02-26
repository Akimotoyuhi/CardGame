using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    /// <summary>��V��ʃp�l��</summary>
    [SerializeField] GameObject m_panel;
    /// <summary>�\������J�[�h�̃v���n�u</summary>
    [SerializeField] BlankCard m_uiCard;
    /// <summary>�J�[�h�̐e�I�u�W�F�N�g(���C�A�E�g�O���[�v�p)</summary>
    [SerializeField] Transform m_cardsParent;
    public RectTransform CanvasRectTransform { private get; set; }

    /// <summary>
    /// ��V��ʕ\��
    /// </summary>
    public void RewardView(CardInfomationData cardDataBase)
    {
        m_panel.SetActive(true);
        BlankCard card = Instantiate(m_uiCard);
        card.transform.SetParent(m_cardsParent, false);
        card.transform.localScale = Vector2.one;
        //obj.GetRectTransform().anchoredPosition = Vector3.zero;
        card.SetInfo(cardDataBase, 0, this);
        card.CardState = CardState.Reward;
    }

    public void OnClick(CardID id, int upgrade)
    {
        BattleManager.Instance.RewardEnd((int)id, upgrade);
        m_panel.SetActive(false);
    }

    public void RewardDisabled()
    {
        for (int i = 0; i < m_cardsParent.childCount; i++)
        {
            Destroy(m_cardsParent.GetChild(i).gameObject);
        }
        m_panel.SetActive(false);
    }
}