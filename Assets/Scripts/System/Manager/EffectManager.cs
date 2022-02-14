using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// エフェクトやってくれるクラス<br/>
/// 演出用のテキストやパーティクルを表示したりフェードの管理をしたりする
/// </summary>
public class EffectManager : MonoBehaviour
{
    [SerializeField] Image m_fadePanel;
    [SerializeField] GameObject m_overfrowTextPrefab;
    [SerializeField] GameObject m_textPrefab;
    [SerializeField] GameObject m_battleUI;
    private Text m_text;
    public static EffectManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        RemoveBattleUIText();
        //Fade(Color.clear, 0.1f);
    }

    public GameObject ViewText(string text, Vector2 position, Transform parent)
    {
        GameObject obj = Instantiate(m_textPrefab);
        obj.transform.SetParent(parent, false);
        obj.GetText(text);
        obj.GetRectTransform().anchoredPosition = position;
        return obj;
    }
    /// <summary>
    /// 戦闘時の説明用のテキストに表示する
    /// </summary>
    /// <param name="text">表示する文字列</param>
    /// <param name="color">テキストの色</param>
    public void SetBattleUIText(string text, Color color)
    {
        m_battleUI.transform.GetChild(0).gameObject.GetText().SetText(text, color);
        m_battleUI.SetActive(true);
    }
    /// <summary>
    /// 戦闘時の説明用のテキストを表示し、n秒後に消す
    /// </summary>
    /// <param name="text">表示する文字列</param>
    /// <param name="color">テキストの色</param>
    /// <param name="removeTime">消すまでの時間</param>
    public void SetBattleUIText(string text, Color color, float removeTime)
    {
        SetBattleUIText(text, color);
        DOVirtual.DelayedCall(removeTime, () => RemoveBattleUIText());
    }
    /// <summary>
    /// 現在表示中の説明用のテキストを非表示にする
    /// </summary>
    public void RemoveBattleUIText()
    {
        m_battleUI.transform.GetChild(0).gameObject.GetText("");
        m_battleUI.SetActive(false);
    }
    public void MoveText(string text, Color color, Vector2 position, Transform parent, Vector2 endValue, float duration, System.Action action)
    {
        m_text.text = text;
        m_text.color = color;
        GameObject obj = Instantiate(m_overfrowTextPrefab);
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
        GameObject obj = Instantiate(m_overfrowTextPrefab);
        Text viewText = obj.GetText(text, Color.clear);
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
    /// <summary>
    /// フェード
    /// </summary>
    /// <param name="duration">間隔</param>
    /// <param name="color">フェード後の色</param>
    /// <param name="action">フェード後にする事があれば書く</param>
    public void Fade(Color color, float duration, System.Action action = null)
    {
        m_fadePanel.DOColor(color, duration)
                .OnComplete(() =>
                {
                    if (action != null)
                    {
                        action();
                    }
                });
    }
}
