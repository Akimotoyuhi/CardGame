using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum CellState { Enemy, Rest, Boss, Elite }
public enum CellChildType { Begin, End }

public class Cell : MonoBehaviour
{
    /// <summary>点滅間隔</summary>
    [SerializeField] float m_highLightsDuration;
    [SerializeField] Image m_image;
    /// <summary>開始位置</summary>
    [SerializeField] Image m_startPos;
    /// <summary>終了位置</summary>
    [SerializeField] Image m_endPos;
    /// <summary>セルの状態</summary>
    [SerializeField] CellState m_cellState = default;
    /// <summary>テキスト</summary>
    [SerializeField] Text m_viewText;
    [Header("セルの状態に応じて変わる色の設定")]
    [SerializeField] Color m_enemyColor = Color.red;
    [SerializeField] Color m_restColor = Color.blue;
    [SerializeField] Color m_bossColor = Color.red;
    [SerializeField] Color m_eliteColor = Color.red;
    private List<int> m_nextCellList = new List<int>();
    /// <summary>このセルで出現するエンカウントID</summary>
    public int m_encountId = default;
    /// <summary>点滅アニメーション用</summary>
    private Sequence m_sequence;
    /// <summary>生成済みフラグ とりあえず</summary>
    public bool CreatedFlag { get; set; } = false;
    public CellState SetCellState
    {
        set
        {
            m_cellState = value;
            ColorChange();
        }
    }
    //public EnemyAppearanceEria EnemyAppearanceEria
    //{
    //    set
    //    {
    //        m_enemyAppearanceEria = value;
    //        ColorChange();
    //    }
    //}
    /// <summary>このセルが所属するセクター番号</summary>
    public int SectorIndex { get; set; }
    /// <summary>このセルが所属するステップ数</summary>
    //public int Step { get; set; }
    public int Step;
    public int GetNextCellIndex(int index) { return m_nextCellList[index]; }
    public void AddNextCell(int value) { m_nextCellList.Add(value); }

    public void OnClick()
    {
        if (GameManager.Instance.Step != Step)
        {
            Debug.Log("選択不可");
            return;
        }
        DOTween.KillAll();
        //とりあえず
        GameManager.Instance.OnClick(m_cellState);
    }

    /// <summary>
    /// セルの子(線の始点or終点となる位置)を知れる
    /// </summary>
    /// <param name="isBeginPos">始点かどうか</param>
    /// <returns>BeginPosition or EndPosition</returns>
    public Vector3 GetChildPosition(CellChildType type)
    {
        if (type == CellChildType.Begin)
        {
            return transform.GetChild(0).gameObject.GetRectTransform().anchoredPosition;
        }
        else
        {
            return transform.GetChild(1).gameObject.GetRectTransform().anchoredPosition;
        }
    }

    /// <summary>
    /// 現在の自分のStateに合わせて自身の色を変更する
    /// </summary>
    public void ColorChange()
    {
        //bool flag = GetComponent<Button>().interactable;
        //if (!flag) return; //使用されていないセルの場合は設定しない
        //Image image = GetComponent<Image>();
        switch (m_cellState)
        {
            case CellState.Enemy:
                m_image.color = m_enemyColor;
                m_viewText.text = "戦闘"; //画像来るまでとりあえず
                break;
            case CellState.Rest:
                m_image.color = m_restColor;
                m_viewText.text = "休憩";
                break;
            case CellState.Boss:
                m_image.color = m_bossColor;
                m_viewText.text = "強敵";
                break;
            case CellState.Elite:
                m_image.color = m_eliteColor;
                break;
        }
        if (GameManager.Instance.Step != Step)
        {
            m_image.color -= new Color(0, 0, 0, 0.5f);
        }
        else
        {
            Color color = m_image.color;
            m_sequence = DOTween.Sequence();
            m_sequence.Append(m_image.DOColor(Color.white, m_highLightsDuration))
                .Append(m_image.DOColor(color, m_highLightsDuration))
                .SetLoops(-1);
        }
    }
}
