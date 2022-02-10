using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discard : CardManagement
{
    [SerializeField] private Deck m_deck;

    private void Start()
    {
        m_canvas = GetComponent<Canvas>();
        m_canvas.enabled = false;
    }
    /// <summary>捨て札から山札にカードを移す</summary>
    public void ConvartToDeck()
    {
        for (int i = m_cardParent.childCount - 1; 0 <= i; i--)
        {
            //transform.GetChild(i).GetComponent<BlankCard>().GetPlayerEffect();
            m_cardParent.GetChild(i).SetParent(m_deck.CardParent, false);
            //m_deck.SetParent(m_cardParent.GetChild(i));
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
