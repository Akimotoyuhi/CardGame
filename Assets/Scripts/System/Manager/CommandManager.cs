using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/// <summary>
/// �J�[�h�̎g�p���̌��ʓ��̗l�X�ȃN���X�Ƀ_���[�W��^�����肷��N���X
/// </summary>
public class CommandManager : MonoBehaviour
{
    [SerializeField, Tooltip("�v���C���[�̍U���摜���o������")] float m_playerAttackSpriteDuration;
    [SerializeField, Tooltip("�R�}���h���s�Ԋu")] float m_delayDuration;
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

    public async void CommandExecute(List<int[]> commands, bool isPlayerAnim, EnemyBase enemy = null)
    {
        await CommandExecuteAsync(commands, isPlayerAnim, enemy);
    }

    /// <summary>
    /// �R�}���h�̎��s
    /// </summary>
    public async UniTask CommandExecuteAsync(List<int[]> commands, bool isPlayerAnim, EnemyBase enemy = null)
    {
        if (commands.Count == 0)
            return;
        foreach (var cmds in commands)
        {
            //CommandParam cp = (CommandParam)card[0];
            UseTiming useType = (UseTiming)cmds[2];
            switch (useType)
            {
                case UseTiming.ToPlayer:
                    m_player.GetDamage(cmds, (ParticleID)cmds[1]);
                    break;
                case UseTiming.ToEnemy:
                    if (!enemy)
                    {
                        Debug.LogError("�G�f�[�^�����݂��܂��� �J�[�h�̌��ʑΏۂ����������̂Ȃ̂����m�F���Ă�������");
                        break;
                    }
                    enemy.GetDamage(cmds, (ParticleID)cmds[1]);
                    break;
                case UseTiming.ToAllEnemies:
                    m_enemyManager.AllEnemiesDamage(cmds, (ParticleID)cmds[1]);
                    break;
                case UseTiming.ToRandomEnemy:
                    //Debug.Log("���쐬");
                    break;
                case UseTiming.System:
                    switch ((CommandParam)cmds[0])
                    {
                        case CommandParam.AddCard:
                            BattleManager.Instance.AddCard((CardAddDestination)cmds[5], (CardID)cmds[3], cmds[4], cmds[6]);
                            break;
                        case CommandParam.DrawCard:
                            if (cmds[3] == 0) //�J�[�h�̂Ă�
                                BattleManager.Instance.CardDispose(cmds[4]);
                            else //�J�[�h������
                                BattleManager.Instance.CardDraw(cmds[4]);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        if (isPlayerAnim)
            m_player.AttackSpriteChange(AttackSpriteID.Slash, m_playerAttackSpriteDuration);
        await DOVirtual.DelayedCall(m_delayDuration, () => { }).AsyncWaitForCompletion();
    }
}
