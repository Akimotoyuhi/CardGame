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
    public RectTransform CanvasRectTransform { private get; set; }

    /// <summary>
    /// 報酬画面表示
    /// </summary>
    public void RewardView(NewCardDataBase cardDataBase)
    {
        m_panel.SetActive(true);
        GameObject obj = Instantiate(m_uiCard);
        obj.transform.SetParent(m_cardsParent, false);
        obj.transform.localScale = Vector2.one;
        //obj.GetRectTransform().anchoredPosition = Vector3.zero;
        obj.GetComponent<BlankCard>().SetInfo(cardDataBase, 0, this);
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