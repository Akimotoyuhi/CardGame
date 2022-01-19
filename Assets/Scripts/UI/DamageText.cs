using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    public void ChangeText(string text)
    {
        GetComponent<Text>().text = text;
    }
    public void RandomMove()
    {
        float x = Random.Range(-100, 100);
        float y = Random.Range(-100, 100);
        RectTransform rectTransform = GetComponent<RectTransform>();
        Sequence s = DOTween.Sequence();
        s.Append(rectTransform.DOAnchorPos(new Vector2(x, y), 1.5f))
            .OnComplete(() => Destroy(gameObject));
    }
    public void Color(ColorType colorType)
    {
        GetComponent<Text>().color = ColorChange.GetColor(colorType);
    }
}

public enum ColorType
{
    Red,
    Blue,
    Green,
}

public class ColorChange
{
    public static Color GetColor(ColorType colorType)
    {
        switch (colorType)
        {
            case ColorType.Red:
                return Color.red;
            case ColorType.Blue:
                return Color.blue;
            case ColorType.Green:
                return Color.green;
            default:
                throw new System.IndexOutOfRangeException("サポートされていないパターンが渡されました");
        }
    }
}