using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J�[�h�̎g�p���̌��ʓ��̗l�X�ȃN���X�Ƀ_���[�W��^�����肷��N���X
/// </summary>
public class CommandManager : MonoBehaviour
{
    [SerializeField, Tooltip("�v���C���[�̍U���摜���o������")] float m_playerAttackSpriteDuration;
    private EnemyManager m_enemyManager = default;
    private Player m_player = default;

    public void Setup(EnemyManager enemyManager, Player player)
    {
        m_enemyManager = enemyManager;
        m_player = player;
    }

    /// <summary>
    /// �J�[�h�̌��ʂ𔭓�������
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
                        Debug.LogError("�G�f�[�^�����݂��܂��� �J�[�h�̌��ʑΏۂ����������̂Ȃ̂����m�F���Ă�������");
                        break;
                    }
                    enemy.GetDamage(cmds);
                    break;
                case UseType.ToAllEnemies:
                    m_enemyManager.AllEnemiesDamage(cmds);
                    break;
                case UseType.ToRandomEnemy:
                    Debug.Log("���쐬");
                    break;
                case UseType.System:
                    switch ((CommandParam)cmds[0])
                    {
                        case CommandParam.AddCard:
                            //Debug.Log($"{(CardID)card[2]}��{card[3]}��{(CardAddDestination)card[4]}�ɒǉ�����");
                            BattleManager.Instance.AddCard((CardAddDestination)cmds[4], (CardID)cmds[2], cmds[3], cmds[5]);
                            break;
                        case CommandParam.DrawCard:
                            if (cmds[2] == 0) //�J�[�h�̂Ă�
                                BattleManager.Instance.CardDispose(cmds[3]);
                            else //�J�[�h������
                                BattleManager.Instance.CardDraw(cmds[3]);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    Debug.Log("��O");
                    break;
            }
        }
        m_player.AttackSpriteChange(AttackSpriteID.Slash, m_playerAttackSpriteDuration);
    }
}
