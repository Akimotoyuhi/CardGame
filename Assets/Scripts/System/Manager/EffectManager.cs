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
    [SerializeField] int m_textFontSize = 40;
    [SerializeField] GameObject m_textPrefab;
    private Text m_text;

    public static EffectManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ViewText(string text, Vector2 position, Transform parent)
    {
        GameObject obj = Instantiate(m_textPrefab);
        obj.transform.SetParent(parent, false);
        obj.SetText(text);
        obj.GetRectTransform().anchoredPosition = position;
    }
    public void MoveText(string text, Color color, Vector2 position, Transform parent, Vector2 endValue, float duration, System.Action action)
    {
        m_text.text = text;
        m_text.color = color;
        GameObject obj = Instantiate(m_textPrefab);
        obj.transform.SetParent(parent, false);
        RectTransform rt = obj.GetRectTransform();
        rt.anchoredPosition = position;
        DOTween.Sequence().Append(rt.DOAnchorPos(endValue, duration))
            .OnComplete(() => action());
    }
    /// <summary>
    /// ダメージ表示用のテキストを表示して動かす
    /// </summary>
    /// <param name="text">表示するテキスト</param>
    /// <param name="color">テキストの色</param>
    /// <param name="position">生成位置(anchoredPosition)</param>
    /// <param name="parent">生成するテキストの親</param>
    public void DamageText(string text, Color color, Vector2 position, Transform parent, bool scaleChanged = false)
    {
        GameObject obj = Instantiate(m_textPrefab);
        Text viewText = obj.SetText(text, Color.clear);
        obj.transform.SetParent(parent, false);
        RectTransform rt = obj.GetRectTransform();
        rt.localScale = scaleChanged ? new Vector2(-1, 1) : Vector2.one;
        rt.anchoredPosition = position;
        float endValueX = Random.Range(-100, 100);
        float endValueY = Random.Range(-100, 100);
        DOTween.Sequence().Append(rt.DOAnchorPos(new Vector2(endValueX, endValueY), 1f))
            .Join(viewText.DOColor(color, 0.1f))
            .Append(viewText.DOColor(Color.clear, 1f))
            .OnComplete(() => Destroy(obj));
    }
}
