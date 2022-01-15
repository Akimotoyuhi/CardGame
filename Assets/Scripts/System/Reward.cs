using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    /// <summary>報酬画面パネル</summary>
    [SerializeField] GameObject m_panel;
    /// <summary>表示するカードのプレハブ</summary>
    [SerializeField] GameObject m_uiCard;
    /// <summary>カードの親オブジェクト(レイアウトグループ用)</summary>
    [SerializeField] Transform m_cardsParent;

    /// <summary>
    /// 報酬画面表示
    /// </summary>
    public void RewardView(NewCardDataBase cardDataBase)
    {
        m_panel.SetActive(true);
        Transform obj = Instantiate(m_uiCard).transform;
        obj.SetParent(m_cardsParent);
        obj.GetComponent<UICard>().Setup(cardDataBase);
    }

    public void OnClick(CardID id)
    {
        BattleManager.Instance.RewardEnd(id);
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