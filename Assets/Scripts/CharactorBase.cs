using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactorBase : MonoBehaviour
{
    [SerializeField] protected string m_name = "name";
    [SerializeField] protected int m_maxHp = 1;
    protected int m_hp;
    protected int m_block;
    [SerializeField] protected Slider m_hpSlider;
    [SerializeField] protected Slider m_blkSlider;
    [SerializeField] protected Text m_text;
    [NonSerialized] public int[] m_stateArray;
    protected Condition m_condition;

    protected void SetUp()
    {
        m_condition = new Condition();
        m_hp = m_maxHp;
        m_hpSlider.maxValue = m_maxHp;
        m_hpSlider.value = m_hp;
        m_blkSlider.value = m_block;
        SetText();
        m_stateArray = new int[(int)BuffDebuff.end];
    }

    /// <summary>
    /// キャラクター下のゲージとテキストの処理
    /// </summary>
    protected void SetText()
    {
        if (m_block > 0)
        {
            m_blkSlider.value = m_block;
            m_text.text = $"{m_block}";
        }
        else
        {
            m_blkSlider.value = m_block;
            m_text.text = $"{m_hp} : {m_maxHp}";
        }
    }

    /// <summary>
    /// n%引を返す
    /// </summary>
    /// <param name="num">割りたい数</param>
    /// <param name="parsent">何%引きか</param>
    /// <returns></returns>
    protected int Parsent(int num, float parsent)
    {
        parsent /= 100;
        float t = num * (1 - parsent);
        return (int)t;
    }

    /// <summary>
    /// 状態異常のターン加算処理
    /// </summary>
    /// <param name="states"></param>
    protected void SetCondisionTurn(int[] states)
    {
        Debug.Log(states.Length);
        int[] nums = new int[(int)BuffDebuff.end];
        nums = states;
        for (int i = 0; i < states.Length; i++)
        {
            if (i == (int)BuffDebuff.Vulnerable)
            {
                m_condition.vulnerable.turn += nums[(int)BuffDebuff.Vulnerable];
                return;
            }
            else if (i == (int)BuffDebuff.Weakness)
            {
                m_condition.weakness.turn += nums[(int)BuffDebuff.Weakness];
                //Debug.Log(m_condition.weakness.turn);
                return;
            }
        }
    }

    /// <summary>
    /// ターン終了時に起こる効果
    /// </summary>
    public virtual void TurnEnd()
    {
        m_condition.Decrement();
    }

    /// <summary>
    /// ターンの開始時に起こる効果
    /// </summary>
    public virtual void TurnStart()
    {
        m_block = 0;
        SetText();
    }
}
