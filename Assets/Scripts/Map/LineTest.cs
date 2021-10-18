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
        AddVert(vh, new Vector2(m_position1.position.x, m_position1.position.y - m_weight / 2));
        AddVert(vh, new Vector2(m_position1.position.x, m_position1.position.y + m_weight / 2));
        AddVert(vh, new Vector2(m_position2.position.x, m_position2.position.y - m_weight / 2));
        AddVert(vh, new Vector2(m_position2.position.x, m_position2.position.y + m_weight / 2));

        //頂点リストを元にメッシュを貼る
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(1, 2, 3);
    }

    private void AddVert(VertexHelper vh, Vector2 pos)
    {
        var vert = UIVertex.simpleVert;
        vert.position = pos;
        vert.color = color;
        vh.AddVert(vert);
    }
}
