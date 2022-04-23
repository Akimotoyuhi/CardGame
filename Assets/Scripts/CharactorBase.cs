using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 敵とプレイヤーの基底クラス
/// </summary>
public class CharactorBase : MonoBehaviour
{
    #region Field
    /// <summary>名前</summary>
    [SerializeField] protected string m_name = "name";
    /// <summary>最大HP</summary>
    [SerializeField] protected int m_maxLife = 1;
    /// <summary>現在HP</summary>
    [SerializeField] protected int m_life;
    /// <summary>現在のブロック値</summary>
    protected int m_block;
    /// <summary>画像</summary>
    protected Sprite m_sprite;
    /// <summary>HPのスライダー</summary>
    [SerializeField] protected Slider m_hpSlider;
    /// <summary>ブロック値のスライダー</summary>
    [SerializeField] protected Slider m_blkSlider;
    /// <summary>HPバー前にあるテキスト</summary>
    [SerializeField] protected Text m_text;
    /// <summary>バフデバフアイコンを表示するオブジェクトのプレハブ</summary>
    [SerializeField] ConditionUI m_conditionUIPrefab;
    [SerializeField] Image m_viewConditionImage;
    /// <summary>バフデバフアイコンを表示するオブジェクトのプレハブの親</summary>
    [SerializeField] Transform m_conditionUIParent;
    [SerializeField] protected Image m_image;
    /// <summary>キャラ画像のフレーム</summary>
    [SerializeField] protected GameObject m_flame;
    /// <summary>死んでる判定</summary>
    protected bool m_isDead = false;
    /// <summary>敵かどうか</summary>
    protected bool m_isEnemy = false;
    /// <summary>アニメーション中判定</summary>
    private bool m_isAnim = false;
    /// <summary>バフデバフを付与された/解除した事をハイライトする用のimageを動かすためのSequenceリスト</summary>
    private List<Sequence> m_conditionSequence = new List<Sequence>();
    /// <summary>所持中のバフデバフ</summary>
    protected List<Condition> m_conditions = new List<Condition>();
    /// <summary>位置保存用　アニメーション用に作ったが今は使ってない</summary>
    protected RectTransform m_rectTransform;
    #endregion
    #region プロパティ
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
    public int CurrentBlock => m_block;
    public Sprite sprite => m_sprite;
    /// <summary>死んでる判定</summary>
    public bool IsDead { get => m_isDead; }
    /// <summary>このオブジェクトが配置されている親キャンバス</summary>
    public Transform Canvas { get; set; }
    protected enum GetCardType { Damage, Block }
    #endregion

    protected virtual void SetUp()
    {
        m_flame.SetActive(false);
        GetComponent<Image>().sprite = m_sprite;
        m_hpSlider.maxValue = m_maxLife;
        m_hpSlider.value = m_life;
        m_blkSlider.value = m_block;
        m_rectTransform = GetComponent<RectTransform>();
        SetSlider();
        m_viewConditionImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// キャラクター下のスライダーとテキストの処理
    /// </summary>
    protected void SetSlider()
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
            m_text.text = $"{m_life} / {m_maxLife}";
        }
    }

    /// <summary>
    /// 戦闘時にバフデバフを評価する
    /// </summary>
    /// <param name="eventTiming">評価タイミング</param>
    /// <param name="parametorType">評価するパラメーター名</param>
    /// <param name="value">評価する数値</param>
    /// <returns>評価された後のパラメーター</returns>
    public int ConditionEffect(EventTiming eventTiming, ParametorType parametorType, int value)
    {
        int ret = value;
        foreach (var item in m_conditions)
        {
            ret = item.Effect(eventTiming, parametorType, ret)[0];
        }
        return ret;
    }

    /// <summary>
    /// 被ダメージ情報を受け取る
    /// </summary>
    public virtual void GetDamage(int[] cardParam, ParticleID particleID) { }

    /// <summary>
    /// バフデバフを付与された時の加算
    /// </summary>
    /// <param name="condition"></param>
    protected void AddEffect(Condition condition)
    {
        if (condition == null) return;
        Condition c = condition.Copy();
        bool flag = false;
        for (int i = 0; i < m_conditions.Count; i++)
        {
            if (c.GetConditionID() == m_conditions[i].GetConditionID())
            {
                m_conditions[i].Turn += c.Turn;
                flag = true;
            }
        }
        if (!flag)
        {
            //同じエフェクトが一つも見つからなかったら新たに追加
            m_conditions.Add(c);
            HighLightConditionUI(c.GetConditionID(), true);
        }
        ViewConditionUI();
    }

    /// <summary>
    /// デバフにかかった/解除した事を表示する<br/>
    /// </summary>
    private void HighLightConditionUI(ConditionID conditionID, bool isAddEffect)
    {
        m_viewConditionImage.gameObject.SetActive(true);
        m_viewConditionImage.sprite = m_conditionUIPrefab.GetSprite(conditionID);
        m_viewConditionImage.color = m_conditionUIPrefab.GetColor(conditionID);
        RectTransform rt = m_viewConditionImage.gameObject.GetRectTransform();
        Vector2 v = rt.anchoredPosition;
        Sequence s = DOTween.Sequence();
        m_conditionSequence.Add(s);
        if (isAddEffect)
            s.Append(rt.DOAnchorPosY(rt.anchoredPosition.y + 20, 0.5f));
        else
            s.Append(rt.DOAnchorPosY(rt.anchoredPosition.y - 20, 0.5f));
        s.Append(m_viewConditionImage.DOColor(Color.clear, 0.5f));
        s.OnComplete(() =>
        {
            rt.anchoredPosition = v;
            m_conditionSequence.Remove(s);
            m_viewConditionImage.gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// 効果が切れたバフデバフを消す
    /// </summary>
    protected void RemoveEffect()
    {
        for (int i = m_conditions.Count - 1; i >= 0; i--)
        {
            if (m_conditions[i].IsRemove())
            {
                Debug.Log($"{m_conditions[i].GetConditionID()}デバフを除去");
                m_conditions.RemoveAt(i);
            }
        }
        ViewConditionUI();
    }

    /// <summary>
    /// バフデバフをUIに表示する
    /// </summary>
    private void ViewConditionUI()
    {
        //一旦Spriteを全部消す
        for (int i = 0; i < m_conditionUIParent.childCount; i++)
        {
            Destroy(m_conditionUIParent.GetChild(i).gameObject);
        }
        foreach (var con in m_conditions)
        {
            ConditionUI c = Instantiate(m_conditionUIPrefab);
            c.transform.SetParent(m_conditionUIParent, false);
            c.SetUI(con.GetConditionID(), con.Turn);
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
                    SetSlider();
                    yield return new WaitForSeconds(0.1f);
                }
                break;
        }
    }

    /// <summary>
    /// 被ダメージ処理
    /// </summary>
    protected void Damage(int damage, int block, Condition condition, bool isPlayer, ParticleID particleID, Action dead)
    {
        if (IsDead) return;
        AddEffect(condition);
        EffectManager.Instance.ShowParticle(particleID, 0.5f, new Vector3(transform.position.x, transform.position.y, 100));
        if (damage > 0)
        {
            int dmg = ConditionEffect(EventTiming.Damaged, ParametorType.Attack, damage);
            dmg = m_block -= dmg;
            if (m_block < 0) m_block = 0;
            else
            {
                EffectManager.Instance.DamageText(damage.ToString(), Color.blue, Vector2.zero, transform);
            }
            dmg *= -1; //ブロック値計算の後ダメージの符号が反転してうざいので戻す
            if (dmg <= 0) { }
            else
            {
                m_life -= dmg;
                EffectChecker(EventTiming.Damaged, ParametorType.Other);
                if (isPlayer) GameManager.Instance.SetGameInfoPanel(this);
                if (m_life <= 0)
                {
                    m_life = 0;
                    dead();
                }
                else
                {
                    EffectManager.Instance.DamageText(dmg.ToString(), Color.red, Vector2.zero, transform);
                }
            }
        }
        m_block += block;
        SetSlider();
    }
    public bool ConditionIDCheck(ConditionID id)
    {
        foreach (var c in m_conditions)
        {
            if (c.GetConditionID() == id)
                return true;
        }
        return false;
    }
    public void SetParam(string name, Sprite image, int hp)
    {
        m_name = name;
        m_sprite = image;
        m_maxLife = hp;
    }

    /// <summary>
    /// ターン終了時に起こる効果
    /// </summary>
    public virtual void TurnEnd(int i = 0)
    {
        EffectChecker(EventTiming.TurnEnd, ParametorType.Other);
        RemoveEffect();
    }

    /// <summary>
    /// ターンの開始時に起こる効果
    /// </summary>
    public virtual void TurnStart()
    {
        m_block = 0;
        EffectChecker(EventTiming.TurnBegin, ParametorType.Other);
        SetSlider();
    }
    /// <summary>
    /// バフデバフ効果の発動
    /// </summary>
    /// <param name="eventTiming"></param>
    /// <param name="parametorType"></param>
    protected void EffectChecker(EventTiming eventTiming, ParametorType parametorType)
    {
        foreach (var c in m_conditions)
        {
            switch (c.GetParametorType())
            {
                case ParametorType.Condition:
                    for (int evaluation = 0; evaluation < m_conditions.Count; evaluation++)
                    {
                        int[] i = c.Effect(eventTiming, parametorType, (int)m_conditions[evaluation].GetConditionID());
                        if (i.Length >= 2)
                        {
                            Debug.Log($"{(ConditionID)i[0]}を{i[1]}ターン付与");
                            ConditionSelection cs = new ConditionSelection();
                            AddEffect(cs.SetCondition((ConditionID)i[0], i[1]));
                        }
                    }
                    break;
                default:
                    break;
            }
            switch (c.GetConditionID())
            {
                case ConditionID.PlateArmor:
                    m_block += c.Effect(eventTiming, parametorType)[0];
                    break;
                case ConditionID.Metallicize:
                    m_block += c.Effect(eventTiming, parametorType)[0];
                    break;
                default:
                    c.Effect(eventTiming, parametorType);
                    break;
            }
        }
        RemoveEffect();
    }
}
