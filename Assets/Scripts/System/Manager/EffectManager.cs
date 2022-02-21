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
/// �G�t�F�N�g����Ă����N���X<br/>
/// ���o�p�̃e�L�X�g��p�[�e�B�N����\��������t�F�[�h�̊Ǘ��������肷��
/// </summary>
public class EffectManager : MonoBehaviour
{
    [SerializeField] Image m_fadePanel;
    [SerializeField] Text m_overfrowTextPrefab;
    [SerializeField] Text m_textPrefab;
    [SerializeField] GameObject m_battleUI;
    [SerializeField] GameObject[] m_particlePrefab;
    /// <summary>�p�[�e�B�N�����~�߂Ă���폜����܂ł̎���</summary>
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
    /// �퓬���̐����p�̃e�L�X�g�ɕ\������
    /// </summary>
    /// <param name="text">�\�����镶����</param>
    /// <param name="color">�e�L�X�g�̐F</param>
    public void SetBattleUIText(string text, Color color)
    {
        m_battleUI.transform.GetChild(0).gameObject.GetText().SetText(text, color);
        m_battleUI.SetActive(true);
    }
    /// <summary>
    /// �퓬���̐����p�̃e�L�X�g��\�����An�b��ɏ���
    /// </summary>
    /// <param name="text">�\�����镶����</param>
    /// <param name="color">�e�L�X�g�̐F</param>
    /// <param name="removeTime">�����܂ł̎���</param>
    public void SetBattleUIText(string text, Color color, float removeTime)
    {
        SetBattleUIText(text, color);
        DOVirtual.DelayedCall(removeTime, () => RemoveBattleUIText());
    }
    /// <summary>
    /// ���ݕ\�����̐����p�̃e�L�X�g���\���ɂ���
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
    /// �_���[�W�\���p�̃e�L�X�g��\�����ē�����
    /// </summary>
    /// <param name="text">�\������e�L�X�g</param>
    /// <param name="color">�e�L�X�g�̐F</param>
    /// <param name="position">�����ʒu(anchoredPosition)</param>
    /// <param name="parent">��������e�L�X�g�̐e</param>
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
    /// �t�F�[�h
    /// </summary>
    /// <param name="duration">�Ԋu</param>
    /// <param name="color">�t�F�[�h��̐F</param>
    /// <param name="onComplete">�t�F�[�h��ɂ��鎖������Ώ���</param>
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
    /// �p�[�e�B�N�����o��
    /// </summary>
    /// <param name="particleID">�o���p�[�e�B�N����ID</param>
    /// <param name="stopDuration">�~�߂�܂ł̎���</param>
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
