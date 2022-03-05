using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J�[�h�̃h���b�v��̌��ʔ������Ǘ�����N���X
/// </summary>
public class DropManager : MonoBehaviour
{
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
                        Debug.LogError("�G�f�[�^�����݂��܂��� �J�[�h�̌��ʑΏۂ����������̂Ȃ̂����m�F���Ă�������");
                        continue;
                    }
                    enemy.GetDamage(card);
                    break;
                case UseType.ToAll:
                    m_enemyManager.AllEnemiesDamage(card);
                    break;
                case UseType.ToRandomEnemy:
                    Debug.Log("���쐬");
                    continue;
                default:
                    Debug.Log("��O");
                    break;
            }
        }
    }
}
