using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private int m_defDrawNum = 5;
    [SerializeField] private Transform m_hand;
    [SerializeField] private Discard m_discard;

    public void Draw(int drawNum = 0)
    {
        for (int i = 0; i < m_defDrawNum + drawNum; i++)
        {
            if (this.transform.childCount == 0)
            {
                Debug.Log("山札切れ");
                m_discard.ConvartToDeck();
                if (this.transform.childCount == 0)
                {
                    Debug.Log("デッキ枚数不足");
                    return;
                }
            }
            int r = Random.Range(0, this.transform.childCount);
            transform.GetChild(r).parent = m_hand;
        }
    }
}
