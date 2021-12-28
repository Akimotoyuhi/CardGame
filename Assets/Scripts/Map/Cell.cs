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
    [Header("セルの状態に応じて変わる色の設定")]
    [SerializeField] Color m_enemyColor = Color.red;
    [SerializeField] Color m_restColor = Color.blue;
    private List<int> m_nextCellList = new List<int>();
    /// <summary>このセルで出現するエンカウントID</summary>
    public int m_encountId = default;
    public CellState CellState
    {
        set
        {
            m_cellState = value;
            ColorChange();
        }
    }
    /// <summary>このセルが所属するセクター番号</summary>
    public int SectorIndex { get; set; }
    /// <summary>このセルが所属するステップ数</summary>
    public int Step { get; set; }
    public int GetNextCellIndex(int index) { return m_nextCellList[index]; }
    public void AddNextCell(int value) { m_nextCellList.Add(value); }

    public void OnClick()
    {
        //とりあえず
        GameManager.Instance.Battle(m_encountId, Step);
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
            return transform.GetChild(0).position;
        }
        else
        {
            return transform.GetChild(1).position;
        }
    }

    private void ColorChange()
    {
        Image image = GetComponent<Image>();
        switch (m_cellState)
        {
            case CellState.Enemy:
                image.color = m_enemyColor;
                break;
            case CellState.Rest:
                image.color = m_restColor;
                break;
        }
    }
}
