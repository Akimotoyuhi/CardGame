using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カードの使用時の効果等の様々なクラスにダメージを与えたりするクラス
/// </summary>
public class CommandManager : MonoBehaviour
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
        foreach (var cmds in cardParam)
        {
            //CommandParam cp = (CommandParam)card[0];
            UseType useType = (UseType)cmds[1];
            switch (useType)
            {
                case UseType.ToPlayer:
                    m_player.GetDamage(cmds);
                    break;
                case UseType.ToEnemy:
                    if (!enemy)
                    {
                        Debug.LogError("敵データが存在しません カードの効果対象が正しいものなのかを確認してください");
                        break;
                    }
                    enemy.GetDamage(cmds);
                    break;
                case UseType.ToAllEnemies:
                    m_enemyManager.AllEnemiesDamage(cmds);
                    break;
                case UseType.ToRandomEnemy:
                    Debug.Log("未作成");
                    break;
                case UseType.System:
                    switch ((CommandParam)cmds[0])
                    {
                        case CommandParam.AddCard:
                            //Debug.Log($"{(CardID)card[2]}を{card[3]}枚{(CardAddDestination)card[4]}に追加する");
                            BattleManager.Instance.AddCard((CardAddDestination)cmds[4], (CardID)cmds[2], cmds[3], cmds[5]);
                            break;
                        case CommandParam.DrawCard:
                            if (cmds[2] == 0) //カード捨てる
                                BattleManager.Instance.CardDispose(cmds[3]);
                            else //カードを引く
                                BattleManager.Instance.CardDraw(cmds[3]);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    Debug.Log("例外");
                    break;
            }
        }
        m_player.AttackSpriteChange(AttackSpriteID.Slash, m_playerAttackSpriteDuration);
    }
}
