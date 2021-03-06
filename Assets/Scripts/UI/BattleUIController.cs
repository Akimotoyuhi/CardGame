using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public enum BattleUIType { BattleStart, PlayerTurn, EnemyTurn }
public class BattleUIController : MonoBehaviour
{
    [SerializeField] Text m_battleStartText;
    [SerializeField] Text m_playerTurn;
    private RectTransform m_playerRecttra;
    [SerializeField] Text m_enemyTurn;
    private RectTransform m_enemyRecttra;
    [SerializeField] Color m_battleStartTextColor = Color.white;
    [SerializeField] Color m_playerTurnTextColor = Color.white;
    [SerializeField] Color m_enemyTurnTextColor = Color.white;
    [SerializeField, Tooltip("¶EÉ®©·UIÌÒ­")]
    float m_moveX = 1000;
    [SerializeField, Tooltip("¶EÉ®©·UIÌÚ®Ôu")]
    float m_moveDura = 0.1f;

    private void Start()
    {
        m_playerRecttra = GetComponent<RectTransform>();
        m_enemyRecttra = GetComponent<RectTransform>();
        Setup();
    }

    private void Setup()
    {
        m_battleStartText.color = Color.clear;
        m_playerTurn.color = Color.clear;
        m_enemyTurn.color = Color.clear;
        m_playerRecttra.anchoredPosition = Vector2.zero;
    }

    public void Play(BattleUIType type, System.Action action)
    {
        Sequence s = default;
        switch (type)
        {
            case BattleUIType.BattleStart:
                s = DOTween.Sequence();
                s.Append(m_battleStartText.DOColor(m_battleStartTextColor, 0.5f))
                .AppendInterval(0.5f)
                .Append(m_battleStartText.DOColor(Color.clear, 0.5f));
                break;
            case BattleUIType.PlayerTurn:
                m_playerTurn.color = m_playerTurnTextColor;
                m_playerRecttra.anchoredPosition = new Vector2(-m_moveX, 0);
                s = DOTween.Sequence();
                s.Append(m_playerRecttra.DOAnchorPosX(0, m_moveDura))
                .AppendInterval(0.5f)
                .Append(m_playerRecttra.DOAnchorPosX(m_moveX, m_moveDura));
                break;
            case BattleUIType.EnemyTurn:
                m_enemyTurn.color = m_enemyTurnTextColor;
                m_enemyRecttra.anchoredPosition = new Vector2(m_moveX, 0);
                s = DOTween.Sequence();
                s.Append(m_enemyRecttra.DOAnchorPosX(0, m_moveDura))
                .AppendInterval(0.5f)
                .Append(m_enemyRecttra.DOAnchorPosX(-m_moveX, m_moveDura));
                break;
        }
        s.OnComplete(() =>
        {
            Setup();
            action();
        });
    }
}
