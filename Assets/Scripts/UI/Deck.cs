using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Deck : CardManagement
{
    [SerializeField] private Hand m_hand;
    [SerializeField] private Discard m_discard;

    private void Start()
    {
        BattleManager.Instance.TurnBegin.Subscribe(turn => Draw(BattleManager.Instance.GetDrowNum));
    }

    /// <summary>山札から手札にカードを移す</summary>
    /// <param name="drawNum">ドロー枚数</param>
    public void Draw(int drawNum)
    {
        for (int i = 0; i < drawNum; i++)
        {
            if (m_cardParent.childCount == 0)
            {
                Debug.Log("山札切れ");
                m_discard.ConvartToDeck(); //山札が無かったら捨て札からカードを戻す
                if (m_cardParent.childCount == 0)
                {
                    Debug.Log("デッキ枚数不足"); //捨て札からカードを戻しても山札がないなら引くのをやめる
                    //m_hand.GetComponent<Hand>().SetChildDefpos();
                    return;
                }
            }
            int r = Random.Range(0, m_cardParent.childCount);
            BlankCard b = m_cardParent.GetChild(r).GetComponent<BlankCard>();
            b.GetPlayerEffect();
            b.IsPlayCard = true;
            //m_hand.SetParent(m_cardParent.GetChild(r));
            m_cardParent.GetChild(r).SetParent(m_hand.CardParent, false);
        }
        //m_hand.GetComponent<Hand>().SetChildDefpos();
    }
}
