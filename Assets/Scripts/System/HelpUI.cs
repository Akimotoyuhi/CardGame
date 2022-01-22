using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// RectTransform������Ɏ���Ă����g�����\�b�h
/// </summary>
public static class HelpUI
{
    public static RectTransform GetRectTransform(this GameObject obj)
    {
        return obj.GetComponent<RectTransform>();
    }
    public static Text GetText(this GameObject obj)
    {
        return obj.GetComponent<Text>();
    }
    public static Text SetText(this GameObject obj, string text)
    {
        Text t = obj.GetComponent<Text>();
        t.text = text;
        return t;
    }
}
