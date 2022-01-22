using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// エフェクトやってくれるクラス<br/>
/// 演出用のテキストやパーティクルを表示してくれる
/// </summary>
public class EffectManager : MonoBehaviour
{
    [SerializeField] GameObject Text;

    public static EffectManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ViewText(string text, Vector2 position, Transform parent)
    {
        GameObject obj = Instantiate(Text);
        obj.transform.SetParent(parent, false);
        obj.SetText(text);
        obj.GetRectTransform().anchoredPosition = position;
    }
    public void MoveText(string text, Vector2 position, Transform parent, Vector2 endValue, float duration, System.Action action)
    {
        GameObject obj = Instantiate(Text);
        obj.transform.SetParent(parent, false);
        obj.SetText(text);
        RectTransform rt = obj.GetRectTransform();
        rt.anchoredPosition = position;
        DOTween.Sequence().Append(rt.DOAnchorPos(endValue, duration))
            .OnComplete(() => action());
    }
}
