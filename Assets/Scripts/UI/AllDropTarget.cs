using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�ł��v���C���[���Ăł��Ȃ��J�[�h�̃h���b�v���󂯎��N���X
/// </summary>
public class AllDropTarget : MonoBehaviour, IDrop
{
    /// <summary>�h���b�v�\�ӏ��������t���[��</summary>
    [SerializeField] GameObject m_flame;

    private void Start()
    {
        m_flame.SetActive(false);
    }

    public bool CanDrop(UseTiming useType)
    {
        if (useType == UseTiming.System) return true;
        return false;
    }

    public void GetDrop(List<int[]> card)
    {
        BattleManager.Instance.CommandManager.CommandExecute(card, true);
    }

    public void OnCard(UseTiming? useType)
    {
        if (useType == UseTiming.System) m_flame.SetActive(true);
        else m_flame.SetActive(false);
    }
    public EnemyBase IsEnemy()
    {
        return null;
    }
}
