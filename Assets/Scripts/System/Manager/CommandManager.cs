using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/// <summary>
/// カードの使用時の効果等の様々なクラスにダメージを与えたりするクラス
/// </summary>
public class CommandManager : MonoBehaviour
{
    [SerializeField, Tooltip("プレイヤーの攻撃画像を出す時間")] float m_playerAttackSpriteDuration;
    [SerializeField, Tooltip("コマンド実行間隔")] float m_delayDuration;
    private EnemyManager m_enemyManager;
    private Player m_player;
    private Hand m_hand;
    private Discard m_discard;
    private Deck m_deck;

    public void Setup(EnemyManager enemyManager, Player player, Hand hand, Discard discard, Deck deck)
    {
        m_enemyManager = enemyManager;
        m_player = player;
        m_hand = hand;
        m_discard = discard;
        m_deck = deck;
    }

    public void CommandExecute(List<int[]> commands, bool isPlayerAnim, EnemyBase enemy = null)
    {
        CommandExecuteAsync(commands, isPlayerAnim, enemy);
    }

    /// <summary>
    /// コマンドの実行
    /// </summary>
    private void CommandExecuteAsync(List<int[]> commands, bool isPlayerAnim, EnemyBase enemy = null)
    {
        if (commands.Count == 0)
            return;
        foreach (var cmds in commands)
        {
            UseTiming useType = (UseTiming)cmds[(int)CommonCmdEnum.UseType];
            switch (useType)
            {
                case UseTiming.ToPlayer:
                    m_player.GetDamage(cmds, (ParticleID)cmds[(int)CommonCmdEnum.ParticleID]);
                    break;
                case UseTiming.ToEnemy:
                    if (!enemy)
                    {
                        Debug.LogError("敵データが存在しません カードの効果対象が正しいものなのかを確認してください");
                        break;
                    }
                    enemy.GetDamage(cmds, (ParticleID)cmds[(int)CommonCmdEnum.ParticleID]);
                    break;
                case UseTiming.ToAllEnemies:
                    m_enemyManager.AllEnemiesDamage(cmds, (ParticleID)cmds[(int)CommonCmdEnum.ParticleID]);
                    break;
                case UseTiming.ToRandomEnemy:
                    //Debug.Log("未作成");
                    break;
                case UseTiming.System:
                    switch ((CommandParam)cmds[(int)CommonCmdEnum.CommandParam])
                    {
                        case CommandParam.AddCard:
                            BattleManager.Instance.AddCard((CardAddDestination)cmds[(int)CardAddCmdEnum.AddDestination], (CardID)cmds[(int)CardAddCmdEnum.CardID], cmds[(int)CardAddCmdEnum.Num], cmds[(int)CardAddCmdEnum.IsUpGrade]);
                            break;
                        case CommandParam.DrawCard:
                            if (cmds[(int)DrowCardCmdEnum.IsDrow] == 0) //カード捨てる
                                BattleManager.Instance.CardDispose(cmds[(int)DrowCardCmdEnum.Num]);
                            else //カードを引く
                                BattleManager.Instance.CardDraw(cmds[(int)DrowCardCmdEnum.Num]);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            //AudioManager.Instance.Play(SE.System);
        }
        if (isPlayerAnim)
            m_player.AttackSpriteChange(AttackSpriteID.Slash, m_playerAttackSpriteDuration);
        //Debug.Log("待機");
        //await UniTask.Delay(System.TimeSpan.FromSeconds(m_delayDuration));
        //Debug.Log("再開");
    }
}
