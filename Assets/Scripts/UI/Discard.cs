using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Discard : CardManagement
{
    [SerializeField] Deck m_deck;
    [SerializeField] GridLayoutGroup m_layoutGroup;

    private void Start()
    {
        m_canvas = GetComponent<Canvas>();
        m_canvas.enabled = false;
    }

    public override void GridLayoutGroupSetting(Vector2 size)
    {
        m_layoutGroup.cellSize = size;
    }

    /// <summary>捨て札から山札にカードを移す</summary>
    public void ConvartToDeck()
    {
        for (int i = m_cardParent.childCount - 1; 0 <= i; i--)
        {
            m_cardParent.GetChild(i).SetParent(m_deck.CardParent, false);
        }
    }
    public override void OnPointer(bool flag)
    {
        if (flag)
        {
            EffectManager.Instance.SetBattleUIText("捨て札を表示する", Color.black);
        }
        else
        {
            EffectManager.Instance.RemoveBattleUIText();
        }
    }
}
