using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カードのドロップ後の効果発動を管理するクラス
/// </summary>
public class DropManager : MonoBehaviour
{
    [SerializeField, Tooltip("プレイヤーの攻撃画像を出す時間")] float m_playerAttackSpriteDuration;
    private EnemyManager m_enemyManager = default;
    private Player m_player = default;

    public void Setup(EnemyManager enemyManager, Player player)
    {
        m_enemyManager = enemyManager;
        m_player = player;
    }

    /// <summary>
    /// カードの効果を発動させる
    /// </summary>
    /// <param name="cardParam"></param>
    public void CardExecute(List<int[]> cardParam, EnemyBase enemy = null)
    {
        foreach (var card in cardParam)
        {
            //CommandParam cp = (CommandParam)card[0];
            UseType useType = (UseType)card[1];
            switch (useType)
            {
                case UseType.ToPlayer:
                    m_player.GetDamage(card);
                    break;
                case UseType.ToEnemy:
                    if (!enemy)
                    {
                        Debug.LogError("敵データが存在しません カードの効果対象が正しいものなのかを確認してください");
                        break;
                    }
                    enemy.GetDamage(card);
                    break;
                case UseType.ToAllEnemies:
                    m_enemyManager.AllEnemiesDamage(card);
                    break;
                case UseType.ToRandomEnemy:
                    Debug.Log("未作成");
                    break;
                case UseType.System:
                    if ((CommandParam)card[0] == CommandParam.AddCard)
                        BattleManager.Instance.AddCard((CardAddDestination)card[4], (CardID)card[2], card[3], card[5]);
                        //Debug.Log($"{(CardID)card[2]}を{card[3]}枚{(CardAddDestination)card[4]}に追加する");
                    break;
                default:
                    Debug.Log("例外");
                    break;
            }
        }
        m_player.AttackSpriteChange(AttackSpriteID.Slash, m_playerAttackSpriteDuration);
    }
}
