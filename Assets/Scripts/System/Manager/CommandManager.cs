using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カードの使用時の効果等の様々なクラスにダメージを与えたりするクラス
/// </summary>
public class CommandManager : MonoBehaviour
{
    [SerializeField, Tooltip("プレイヤーの攻撃画像を出す時間")] float m_playerAttackSpriteDuration;
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

    /// <summary>
    /// コマンドの実行
    /// </summary>
    /// <param name="cardParam"></param>
    public void CommandExecute(List<int[]> cardParam, bool isPlayerAnim, EnemyBase enemy = null)
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
                    //Debug.Log("未作成");
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
        if (isPlayerAnim) m_player.AttackSpriteChange(AttackSpriteID.Slash, m_playerAttackSpriteDuration);
    }
    /// <summary>
    /// カードの使用条件を評価するためのパラメーターを返す
    /// </summary>
    /// <param name="evaluationParam"></param>
    /// <param name="evaluationType"></param>
    /// <param name="enemy"></param>
    /// <returns></returns>
    //public int CardUsedConditionalCheck(CardConditionalEvaluationParam evaluationParam, CardConditionalEvaluationType evaluationType, EnemyBase enemy = null)
    //{
    //    int ret;
    //    CharactorBase cb = default;
    //    switch (evaluationType)
    //    {
    //        case CardConditionalEvaluationType.Player:
    //            cb = m_player;
    //            break;
    //        case CardConditionalEvaluationType.Enemy:
    //            cb = enemy;
    //            break;
    //        case CardConditionalEvaluationType.Hand:
    //            ret = m_hand.CardParent.childCount;
    //            return ret;
    //        case CardConditionalEvaluationType.Discard:
    //            ret = m_discard.CardParent.childCount;
    //            return ret;
    //        case CardConditionalEvaluationType.Deck:
    //            ret = m_deck.CardParent.childCount;
    //            return ret;
    //        default:
    //            Debug.LogError("例外エラー");
    //            ret = -1;
    //            break;
    //    }
    //    switch (evaluationParam)
    //    {
    //        case CardConditionalEvaluationParam.Power:
    //            //Debug.LogError("未実装");
    //            ret = -1;
    //            break;
    //        case CardConditionalEvaluationParam.Block:
    //            ret = cb.CurrentBlock;
    //            break;
    //        case CardConditionalEvaluationParam.Life:
    //            ret = cb.CurrentLife;
    //            break;
    //        case CardConditionalEvaluationParam.BuffDebuff:
    //            //ret = cb.ConditionIDCheck((ConditionID)num)
    //            //Debug.LogError("未実装");
    //            ret = -1;
    //            break;
    //        default:
    //            Debug.LogError("例外エラー");
    //            ret = -1;
    //            break;
    //    }
    //    return ret;
    //}
}
