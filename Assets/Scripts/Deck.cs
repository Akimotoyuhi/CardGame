using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private int m_defDrawNum = 5;
    [SerializeField] private Transform m_hand;
    [SerializeField] private Discard m_discard;

    /// <summary>山札から手札にカードを移す</summary>
    /// <param name="drawNum">ドロー数が変化した場合この引数が必要になる</param>
    public void Draw(int drawNum = 0)
    {
        for (int i = 0; i < m_defDrawNum + drawNum; i++)
        {
            if (this.transform.childCount == 0)
            {
                Debug.Log("山札切れ");
                m_discard.ConvartToDeck(); //山札が無かったら捨て札からカードを戻す
                if (this.transform.childCount == 0)
                {
                    Debug.Log("デッキ枚数不足"); //捨て札からカードを戻しても山札がないなら引くのをやめる
                    return;
                }
            }
            int r = Random.Range(0, this.transform.childCount);
            transform.GetChild(r).SetParent(m_hand, false);
            //transform.GetChild(r).parent = m_hand;
        }
    }
}
