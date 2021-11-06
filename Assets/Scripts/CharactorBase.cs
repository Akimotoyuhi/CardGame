using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactorBase : MonoBehaviour
{
    /// <summary>名前</summary>
    [SerializeField] protected string m_name = "name";
    /// <summary>最大HP</summary>
    [SerializeField] protected int m_maxHp = 1;
    /// <summary>現在HP</summary>
    protected int m_hp;
    /// <summary>ブロック値</summary>
    protected int m_block;
    /// <summary>画像</summary>
    protected Sprite m_image;
    [SerializeField] protected Slider m_hpSlider;
    [SerializeField] protected Slider m_blkSlider;
    [SerializeField] protected Text m_text;
    [NonSerialized] public int[] m_stateArray;
    /// <summary>死んでる判定</summary>
    protected bool m_isDead = false;
    protected Condition m_condition;
    protected GameManager m_gamemanager;

    public bool IsDead { get { return m_isDead; } }

    protected virtual void SetUp()
    {
        m_condition = new Condition();
        GetComponent<Image>().sprite = m_image;
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

    public void SetParam(string name, Sprite image, int hp, GameManager gm)
    {
        m_name = name;
        m_image = image;
        m_maxHp = hp;
        m_gamemanager = gm;
    }

    /// <summary>
    /// 状態異常のターン加算処理
    /// </summary>
    /// <param name="states"></param>
    protected void SetCondisionTurn(int[] states)
    {
        if (states == null) return;
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
                return;
            }
        }
    }

    /// <summary>
    /// ターン終了時に起こる効果
    /// </summary>
    public virtual void TurnEnd(int i = 0)
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
