using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CellState
{
    Enemy,
    Rest
}

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
    /// <summary>このセルで出現するエンカウントID</summary>
    public int m_encountId = default;
    public CellState CellState { set => m_cellState = value; }
    /// <summary>このセルが所属するセクター番号</summary>
    public int SectorIndex { get; set; }
    public List<int> NextCell { get; set; }

    void Start()
    {
        //Debug.Log($"Sector:{transform.parent.gameObject.name}, Cell{gameObject.name}, RectPos:{(RectTransform)transform}");
        //Debug.Log($"Sector:{transform.parent.gameObject.name}, Cell{gameObject.name}, RectPos:{ConvertCanvasPos(transform.position, transform.root.gameObject.GetComponent<Canvas>())}");
    }

    public void OnClick()
    {
        //とりあえず
        GameManager.Instance.Battle(m_encountId);
        //GameManager.Instance.Battle(0);
    }

    public void LineCast()
    {

    }

    public void ListChecker()
    {
        //foreach (var list in m_objList)
        //{
        //    Debug.Log(list.transform.position);
        //}
    }

    public void ColorChange()
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
