using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CellState { Enemy, Rest, Boss }
public enum CellChildType { Begin, End }

public class Cell : MonoBehaviour
{
    /// <summary>開始位置</summary>
    [SerializeField] Image m_startPos;
    /// <summary>終了位置</summary>
    [SerializeField] Image m_endPos;
    /// <summary>セルの状態</summary>
    [SerializeField] CellState m_cellState = default;
    //[SerializeField] EnemyAppearanceEria m_enemyAppearanceEria = default;
    [Header("セルの状態に応じて変わる色の設定")]
    [SerializeField] Color m_enemyColor = Color.red;
    [SerializeField] Color m_restColor = Color.blue;
    [SerializeField] Color m_bossColor = Color.red;
    private List<int> m_nextCellList = new List<int>();
    /// <summary>このセルで出現するエンカウントID</summary>
    public int m_encountId = default;
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
        Image image = GetComponent<Image>();
        switch (m_cellState)
        {
            case CellState.Enemy:
                image.color = m_enemyColor;
                break;
            case CellState.Rest:
                image.color = m_restColor;
                break;
            case CellState.Boss:
                image.color = m_bossColor;
                break;
        }
        if (GameManager.Instance.Step != Step)
        {
            image.color -= new Color(0, 0, 0, 0.5f);
        }
    }
}
