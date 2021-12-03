using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    /// <summary>マップ生成時に使うフラグ</summary>
    //private bool m_isChecked = false;
    /// <summary>次セルの保存用</summary>
    public List<GameObject> m_objList = new List<GameObject>();
    private LineRenderer m_lineRenderer;
    /// <summary>このセルで出現する敵ID</summary>
    public int m_encountId = default;

    void Start()
    {
        //Debug.Log($"Sector:{transform.parent.gameObject.name}, Cell{gameObject.name}, RectPos:{(RectTransform)transform}");
        //Debug.Log($"Sector:{transform.parent.gameObject.name}, Cell{gameObject.name}, RectPos:{ConvertCanvasPos(transform.position, transform.root.gameObject.GetComponent<Canvas>())}");
    }

    public void OnClick()
    {
        //とりあえず
        //GameManager.Instance.Battle(m_encountId);
        GameManager.Instance.Battle(0);
    }

    public void LineCaster()
    {
        Vector3[] vec = new Vector3[m_objList.Count];
        //vec[0] = ConvertCanvasPos(transform.position, transform.root.GetComponent<Canvas>());
        for (int i = 0; i < vec.Length; i++)
        {
            Vector3 v = m_objList[i].transform.position;
            vec[i] = ConvertCanvasPos(v, this.transform.root.GetComponent<Canvas>());
            //m_objList[i].GetComponent<Cell>().ListChecker();
            m_objList[i].GetComponent<Cell>().LineCaster();
        }
        m_lineRenderer = GetComponent<LineRenderer>();
        m_lineRenderer.SetPositions(vec);
    }

    public void ListChecker()
    {
        foreach (var list in m_objList)
        {
            Debug.Log(list.transform.position);
        }
    }

    /// <summary>
    /// canvasの2d座標に変換
    /// </summary>
    /// <param name="pos">変換する座標</param>
    /// <param name="canvas">canvas</param>
    /// <returns>変換後の座標</returns>
    private Vector3 ConvertCanvasPos(Vector3 pos, Canvas canvas)
    {
        RectTransform rect = canvas.GetComponent<RectTransform>();
        pos.x += rect.transform.position.x;
        pos.y += rect.transform.position.y;
        return new Vector3(pos.x, pos.y, rect.transform.position.z - 10);
    }
}
