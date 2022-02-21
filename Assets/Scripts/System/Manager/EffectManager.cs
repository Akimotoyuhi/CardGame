using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum ParticleID
{
    a,
}

/// <summary>
/// エフェクトやってくれるクラス<br/>
/// 演出用のテキストやパーティクルを表示したりフェードの管理をしたりする
/// </summary>
public class EffectManager : MonoBehaviour
{
    [SerializeField] Image m_fadePanel;
    [SerializeField] Text m_overfrowTextPrefab;
    [SerializeField] Text m_textPrefab;
    [SerializeField] GameObject m_battleUI;
    [SerializeField] GameObject[] m_particlePrefab;
    /// <summary>パーティクルを止めてから削除するまでの時間</summary>
    [SerializeField] float m_particleDestroyDuration;
    private Text m_text;
    private Sequence m_particleSequence;

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

    public Text ViewText(string text, Vector2 position, Transform parent)
    {
        Text txt = Instantiate(m_textPrefab);
        txt.transform.SetParent(parent, false);
        //txt.GetText(text);
        txt.gameObject.GetRectTransform().anchoredPosition = position;
        return txt;
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
        GameObject obj = Instantiate(m_overfrowTextPrefab).gameObject;
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
    public void DamageText(string text, Color color, Vector2 position, Transform parent, bool isScaleChange = false)
    {
        Text txt = Instantiate(m_overfrowTextPrefab);
        Text viewText = txt.SetText(text, Color.clear);
        txt.transform.SetParent(parent, false);
        RectTransform rt = txt.gameObject.GetRectTransform();
        rt.localScale = isScaleChange ? new Vector2(-1, 1) : Vector2.one;
        rt.anchoredPosition = position;
        float endValueX = Random.Range(-100, 100);
        float endValueY = Random.Range(-100, 100);
        DOTween.Sequence().Append(rt.DOAnchorPos(new Vector2(endValueX, endValueY), 1f))
            .Join(viewText.DOColor(color, 0.1f))
            .Append(viewText.DOColor(Color.clear, 1f))
            .OnComplete(() => Destroy(txt));
    }
    /// <summary>
    /// フェード
    /// </summary>
    /// <param name="duration">間隔</param>
    /// <param name="color">フェード後の色</param>
    /// <param name="onComplete">フェード後にする事があれば書く</param>
    public void Fade(Color color, float duration, System.Action onComplete = null)
    {
        m_fadePanel.DOColor(color, duration)
                .OnComplete(() =>
                {
                    if (onComplete != null)
                    {
                        onComplete();
                    }
                });
    }

    /// <summary>
    /// パーティクルを出す
    /// </summary>
    /// <param name="particleID">出すパーティクルのID</param>
    /// <param name="stopDuration">止めるまでの時間</param>
    public void ShowParticle(ParticleID particleID, float stopDuration, Vector3 position)
    {
        GameObject obj = Instantiate(m_particlePrefab[(int)particleID], position, Quaternion.identity);
        List<ParticleSystem> pss = new List<ParticleSystem>();
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            pss.Add(obj.transform.GetChild(i).GetComponent<ParticleSystem>());
        }
        DOVirtual.DelayedCall(stopDuration, () =>
        {
            foreach (var p in pss)
            {
                if (!p) continue;
                p.Stop();
            }
            DOVirtual.DelayedCall(m_particleDestroyDuration, () => Destroy(obj.gameObject));
        });
    }
}
