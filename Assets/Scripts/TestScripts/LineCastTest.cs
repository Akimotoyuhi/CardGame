using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCastTest : MonoBehaviour
{
    public RectTransform obj1;
    public RectTransform obj2;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        Vector3[] v = new Vector3[2];
        v[0] = obj1.anchoredPosition;
        v[1] = obj2.anchoredPosition;
        lineRenderer.SetPositions(v);
    }
}
