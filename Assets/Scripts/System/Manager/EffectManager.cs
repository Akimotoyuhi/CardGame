using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �G�t�F�N�g����Ă����N���X<br/>
/// ���o�p�̃e�L�X�g��p�[�e�B�N����\�����Ă����
/// </summary>
public class EffectManager : MonoBehaviour
{
    [SerializeField] int m_textFontSize = 40;
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
    }

    public GameObject ViewText(string text, Vector2 position, Transform parent)
    {
        GameObject obj = Instantiate(m_textPrefab);
        obj.transform.SetParent(parent, false);
        obj.GetText(text);
        obj.GetRectTransform().anchoredPosition = position;
        return obj;
    }
    public void SetBattleUIText(string text, Color color)
    {
        m_battleUI.transform.GetChild(0).gameObject.GetText().SetText(text, color);
        m_battleUI.SetActive(true);
    }
    public void SetBattleUIText(string text, Color color, float removeTime)
    {
        SetBattleUIText(text, color);
        DOVirtual.DelayedCall(removeTime, () => RemoveBattleUIText());
    }
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
    /// �_���[�W�\���p�̃e�L�X�g��\�����ē�����
    /// </summary>
    /// <param name="text">�\������e�L�X�g</param>
    /// <param name="color">�e�L�X�g�̐F</param>
    /// <param name="position">�����ʒu(anchoredPosition)</param>
    /// <param name="parent">��������e�L�X�g�̐e</param>
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
}
