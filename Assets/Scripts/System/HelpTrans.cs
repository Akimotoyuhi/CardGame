using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RectTransform������Ɏ���Ă����g�����\�b�h
/// </summary>
public static class HelpTrans
{
    public static RectTransform GetRectTransform(this GameObject obj)
    {
        return obj.GetComponent<RectTransform>();
    }
}
