using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IDropHandler
{
    [SerializeField] private string m_name = "name";
    [SerializeField] private int m_maxHp = 1;
    private int m_hp;
    private int m_block = 0;
    [SerializeField] private Slider m_hpSlider;
    [SerializeField] private Slider m_blkSlider;
    [SerializeField] private Text m_text;
    private IBuffCard m_buffCard;
    public int[] m_stateArray;

    void Start()
    {
        m_hp = m_maxHp;
        m_hpSlider.maxValue = m_maxHp;
        m_hpSlider.value = m_hp;
        m_blkSlider.value = m_block;
        SetText();
        m_stateArray = new int[(int)BuffDebuff.end];
    }

    /// <summary>
    /// 被ダメージ処理
    /// </summary>
    /// <param name="damage">ブロック値を加味した被ダメージ</param>
    public void GetAcceptDamage(int[] damage)
    {
        for (int i = 0; i < m_stateArray.Length; i++)
        {
            m_stateArray[i] += damage[i];
        }
        int dmg = CalculationAcceptDamage(m_stateArray[(int)BuffDebuff.Damage]);
        dmg = m_block -= dmg;
        if (m_block < 0) { m_block = 0; }
        m_blkSlider.value = m_block;
        m_stateArray[(int)BuffDebuff.Damage] = 0;
        if (dmg > 0)
        {
            SetText();
            return;
        }
        m_hp += dmg;
        m_hpSlider.value = m_hp;
        SetText();
    }

    /// <summary>
    /// バフデバフを含めた被ダメージ計算
    /// </summary>
    /// <param name="num">被ダメージ</param>
    /// <returns>計算後の被ダメージ</returns>
    private int CalculationAcceptDamage(int num)
    {
        return num;
    }

    private int Parsent(int num, float parsent)
    {
        float t = num * (1 - parsent);
        return (int)t;
    }

    private void SetText()
    {
        if (m_block > 0)
        {
            m_blkSlider.value = m_block;
            m_text.text = $"{m_block}";
        }
        else { m_text.text = $"{m_hp} : {m_maxHp}"; }
    }

    public void OnDrop(PointerEventData pointerEvent)
    {
        m_buffCard = pointerEvent.pointerDrag.GetComponent<IBuffCard>();
        if (m_buffCard == null) { return; }
        m_stateArray = m_buffCard.SetBlock();
        m_block += m_stateArray[(int)BuffDebuff.Block];
        SetText();
    }
}
