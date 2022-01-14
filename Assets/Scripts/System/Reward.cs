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
    public void RewardView(NewCardDataBase cardDataBase, int id)
    {
        m_panel.SetActive(true);
        Transform obj = Instantiate(m_uiCard).transform;
        obj.SetParent(m_cardsParent);
        obj.GetComponent<UICard>().Setup(cardDataBase, id, this);
    }

    public void OnClick(int id)
    {
        m_panel.SetActive(false);
    }
}