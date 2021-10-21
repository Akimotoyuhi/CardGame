using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineTest : Graphic
{
    [SerializeField] Transform m_position1;
    [SerializeField] Transform m_position2;
    private float m_weight = 50f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        //頂点を頂点リストに追加
        AddVert(vh, ConvertCanvasPos
            (new Vector2(m_position1.position.x, m_position1.position.y - m_weight / 2), 
            transform.root.gameObject.GetComponent<Canvas>()));
        AddVert(vh, ConvertCanvasPos
            (new Vector2(m_position1.position.x, m_position1.position.y + m_weight / 2), 
            transform.root.gameObject.GetComponent<Canvas>()));
        AddVert(vh, ConvertCanvasPos
            (new Vector2(m_position2.position.x, m_position2.position.y - m_weight / 2), 
            transform.root.gameObject.GetComponent<Canvas>()));
        AddVert(vh, ConvertCanvasPos
            (new Vector2(m_position2.position.x, m_position2.position.y + m_weight / 2),
            transform.root.gameObject.GetComponent<Canvas>()));

        //頂点リストを元にメッシュを貼る
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(1, 2, 3);
    }

    private void AddVert(VertexHelper vh, Vector3 pos)
    {
        var vert = UIVertex.simpleVert;
        vert.position = pos;
        vert.color = color;
        vh.AddVert(vert);
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
