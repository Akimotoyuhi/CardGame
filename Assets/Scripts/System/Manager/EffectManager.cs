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
    [SerializeField] GameObject m_textPrefab;
    private Text m_text;

    public static EffectManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        m_text = m_textPrefab.GetText();
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
    public void DamageText(string text, Color color, Vector2 position, Transform parent)
    {
        m_text.text = text;
        m_text.color = color;
        GameObject obj = Instantiate(m_textPrefab);
        obj.transform.SetParent(parent, false);
        RectTransform rt = obj.GetRectTransform();
        rt.anchoredPosition = position;
        float endValueX = Random.Range(-100, 100);
        float endValueY = Random.Range(-100, 100);
        DOTween.Sequence().Append(rt.DOAnchorPos(new Vector2(endValueX, endValueY), 0.5f));
    }
}
