using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RectTransformを勝手に取ってくれる拡張メソッド
/// </summary>
public static class HelpTrans
{
    public static RectTransform GetRectTransform(this GameObject obj)
    {
        return obj.GetComponent<RectTransform>();
    }
}
