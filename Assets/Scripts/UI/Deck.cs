using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Deck : CardManagement
{
    [SerializeField] Hand m_hand;
    [SerializeField] Discard m_discard;
    [SerializeField] GridLayoutGroup m_layoutGroup;

    private void Start()
    {
        BattleManager.Instance.TurnBegin.Subscribe(turn => Draw(BattleManager.Instance.GetDrowNum));
        m_canvas = GetComponent<Canvas>();
        m_canvas.enabled = false;

    }

    public override void GridLayoutGroupSetting(Vector2 size)
    {
        m_layoutGroup.cellSize = size;
    }

    /// <summary>山札から手札にカードを移す</summary>
    /// <param name="drawNum">ドロー枚数</param>
    public void Draw(int drawNum)
    {
        for (int i = 0; i < drawNum; i++)
        {
            if (m_cardParent.childCount <= 0)
            {
                Debug.Log("山札切れ");
                m_discard.ConvartToDeck(); //山札が無かったら捨て札からカードを戻す
                if (m_cardParent.childCount == 0)
                {
                    Debug.Log("デッキ枚数不足"); //捨て札からカードを戻しても山札がないなら引くのをやめる
                    return;
                }
            }
            int r = Random.Range(0, m_cardParent.childCount);
            BlankCard b = m_cardParent.GetChild(r).GetComponent<BlankCard>();
            b.GetPlayerEffect();
            b.CardState = CardState.Play;
            m_cardParent.GetChild(r).SetParent(m_hand.CardParent, false);
        }
    }
    public override void OnPointer(bool flag)
    {
        if (flag)
        {
            EffectManager.Instance.SetBattleUIText("山札を表示する", Color.black);
        }
        else
        {
            EffectManager.Instance.RemoveBattleUIText();
        }
    }
}
