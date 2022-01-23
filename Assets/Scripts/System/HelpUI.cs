using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI関連を便利にする拡張メソッド
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
    public static Text SetText(this GameObject obj, string text, Color color)
    {
        Text t = obj.GetComponent<Text>();
        t.text = text;
        t.color = color;
        return t;
    }
    public static Text SetText(this GameObject obj, string text, int fontSize)
    {
        Text t = obj.GetComponent<Text>();
        t.text = text;
        t.fontSize = fontSize;
        return t;
    }
    public static Text SetText(this GameObject obj, string text, Color color, int fontSize)
    {
        Text t = obj.GetComponent<Text>();
        t.text = text;
        t.color = color;
        t.fontSize = fontSize;
        return t;
    }
}
