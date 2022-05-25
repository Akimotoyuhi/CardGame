using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum CellState { Enemy, Elite, Boss, Rest }
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
    [SerializeField] CellState m_cellState;
    [SerializeField] MapID m_mapID;
    /// <summary>テキスト</summary>
    [SerializeField] Text m_viewText;
    [Header("セルの状態に応じて変わる色の設定")]
    [SerializeField] Color m_enemyColor = Color.red;
    [SerializeField] Color m_restColor = Color.blue;
    [SerializeField] Color m_bossColor = Color.red;
    [SerializeField] Color m_eliteColor = Color.red;
    private Map m_map;
    private List<int> m_nextCellList = new List<int>();
    /// <summary>このセルで出現するエンカウントID</summary>
    public int m_encountId = default;
    /// <summary>点滅アニメーション用</summary>
    private Sequence m_sequence;
    public Map Map { set => m_map = value; }
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
    public MapID MapID { set => m_mapID = value; }
    /// <summary>このセルが所属するセクター番号</summary>
    public int SectorIndex { get; set; }
    /// <summary>このセルが所属するステップ数</summary>
    public int Floor { get; set; }
    public int GetNextCellIndex(int index) { return m_nextCellList[index]; }
    public void AddNextCell(int value) { m_nextCellList.Add(value); }

    /// <summary>クリックされた時<br/>
    /// UnityのButtonから呼ばれる事を想定している</summary>
    public void OnClick()
    {
        if (m_map.CanClick) return;
        if (AdjustmentFloorNum() != Floor)
        {
            Debug.Log("選択不可");
            return;
        }
        m_map.CanClick = true;
        //m_sequence.Kill();
        DOTween.KillAll();
        m_map.OnClick(m_cellState, m_mapID);
    }

    /// <summary>
    /// セルの子(線の始点or終点となる位置)
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
                m_viewText.text = "エリート";
                break;
        }
        int f = AdjustmentFloorNum();
        if (f != Floor)
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

    /// <summary>
    /// 現在のFloor数をこのクラスで扱える大きさまで調節する
    /// </summary>
    /// <returns></returns>
    private int AdjustmentFloorNum()
    {
        int f = GameManager.Instance.Floor;
        while (m_map.Sector <= f)
        {
            f -= m_map.Sector;
        }
        return f;
    }
}
