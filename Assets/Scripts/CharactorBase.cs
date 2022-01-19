using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 敵とプレイヤーの基底クラス<br/>
/// 共通処理をまとめておく
/// </summary>
public class CharactorBase : MonoBehaviour
{
    /// <summary>名前</summary>
    [SerializeField] protected string m_name = "name";
    /// <summary>最大HP</summary>
    [SerializeField] protected int m_maxLife = 1;
    /// <summary>現在HP</summary>
    protected int m_life;
    /// <summary>ブロック値</summary>
    protected int m_block;
    /// <summary>画像</summary>
    protected Sprite m_image;
    /// <summary>HPのスライダー</summary>
    [SerializeField] protected Slider m_hpSlider;
    /// <summary>ブロック値のスライダー</summary>
    [SerializeField] protected Slider m_blkSlider;
    /// <summary>HPバー前にあるテキスト</summary>
    [SerializeField] protected Text m_text;
    /// <summary>死んでる判定</summary>
    protected bool m_isDead = false;
    /// <summary>アニメーション中判定</summary>
    private bool m_isAnim = false;
    protected List<Condition> m_conditions = new List<Condition>();
    protected RectTransform m_rectTransform;
    public string Name => m_name;
    public int MaxLife => m_maxLife;
    public int CurrentLife
    {
        get => m_life;
        set
        {
            m_life += value;
            if (m_life >= m_maxLife)
            {
                m_life = m_maxLife;
            }
        }
    }
    public Sprite Image => m_image;
    public bool IsDead { get => m_isDead; }
    protected enum GetCardType { Damage, Block }
    protected virtual void SetUp()
    {
        GetComponent<Image>().sprite = m_image;
        m_life = m_maxLife;
        m_hpSlider.maxValue = m_maxLife;
        m_hpSlider.value = m_life;
        m_blkSlider.value = m_block;
        m_rectTransform = GetComponent<RectTransform>();
        SetUI();
    }

    /// <summary>
    /// キャラクター下のスライダーとテキストの処理
    /// </summary>
    protected void SetUI()
    {
        if (m_block > 0) //ブロック値がある時
        {
            m_blkSlider.value = m_block;
            m_text.text = $"{m_block}";
        }
        else
        {
            m_block = 0;
            m_blkSlider.value = m_block;
            m_hpSlider.value = m_life;
            m_text.text = $"{m_life} : {m_maxLife}";
        }
    }

    public int ConditionEffect(EventTiming eventTiming, ParametorType parametorType, int value)
    {
        int ret = value;
        foreach (var item in m_conditions)
        {
            ret = item.Effect(eventTiming, parametorType, value);
        }
        return ret;
    }

    /// <summary>
    /// バフデバフを付与された時の加算
    /// </summary>
    /// <param name="conditions"></param>
    protected void AddEffect(List<Condition> conditions)
    {
        for (int i = 0; i < conditions.Count; i++)
        {
            bool flag = false;
            for (int n = 0; n < m_conditions.Count; n++)
            {
                if (conditions[i].ConditionID() == m_conditions[n].ConditionID())
                {
                    m_conditions[i].Turn += conditions[n].Turn;
                    flag = true;
                }
            }
            if (!flag)
            {
                //同じエフェクトが一つも見つからなかったら新たに追加
                m_conditions.Add(conditions[i]);
            }
        }
    }

    /// <summary>
    /// 効果が切れたバフデバフを消す
    /// </summary>
    protected void RemoveEffect()
    {
        for (int i = 0; i < m_conditions.Count; i++)
        {
            if (m_conditions[i].Turn <= 0)
            {
                m_conditions.RemoveAt(i);
                Debug.Log("デバフを除去");
            }
        }
    }

    /// <summary>
    /// 連続で攻撃食らったorブロック張った時の処理
    /// </summary>
    /// <param name="getCardType">攻撃かブロックか</param>
    /// <param name="value">値</param>
    /// <param name="num">回数</param>
    /// <returns></returns>
    protected IEnumerator ContinuousReaction(GetCardType getCardType, int value, int num)
    {
        switch (getCardType)
        {
            case GetCardType.Damage:
                for (int i = 0; i < num; i++)
                {
                    m_life -= value;
                    SetUI();
                    yield return new WaitForSeconds(0.1f);
                }
                break;
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
    Vector2 defpos = default;
    protected void AttackAnim(bool isRightMove)
    {
        //float moveDura = 50;
        //if (!isRightMove)
        //{
        //    moveDura *= -1;
        //}
        //if (!m_isAnim)
        //{
        //    m_isAnim = true;
        //    defpos = transform.position;
        //    Sequence s = DOTween.Sequence();
        //    s.Append(transform.DOMoveX(defpos.y + moveDura, 0.1f))
        //        .Append(transform.DOMoveX(defpos.y, 0.1f))
        //        .OnComplete(() => m_isAnim = false);
        //}
    }

    public void SetParam(string name, Sprite image, int hp)
    {
        m_name = name;
        m_image = image;
        m_maxLife = hp;
    }

    /// <summary>
    /// 状態異常のターン加算処理
    /// </summary>
    /// <param name="states"></param>
    protected void SetCondisionTurn(int[] states)
    {
        if (states == null) return;
        int[] nums = new int[(int)ConditionID.end];
        nums = states;
        for (int i = 0; i < states.Length; i++)
        {
            if (i == (int)ConditionID.Vulnerable)
            {
                //m_condition.vulnerable.turn += nums[(int)BuffDebuff.Vulnerable];
                return;
            }
            else if (i == (int)ConditionID.Weakness)
            {
                //m_condition.weakness.turn += nums[(int)BuffDebuff.Weakness];
                return;
            }
        }
    }

    /// <summary>
    /// ターン終了時に起こる効果
    /// </summary>
    public virtual void TurnEnd(int i = 0)
    {
        foreach (var c in m_conditions)
        {
            c.Effect(EventTiming.TurnEnd, ParametorType.Attack);
        }
        RemoveEffect();
    }

    /// <summary>
    /// ターンの開始時に起こる効果
    /// </summary>
    public virtual void TurnStart()
    {
        m_block = 0;
        SetUI();
    }
}
