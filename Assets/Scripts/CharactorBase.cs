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
    protected NewBuffDebuff m_buffDebuff = new NewBuffDebuff();

    protected void SetUp()
    {
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
    /// ターン終了時に起こる効果
    /// </summary>
    public virtual void TurnEnd()
    {
        m_buffDebuff.Decrement(m_stateArray);
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
