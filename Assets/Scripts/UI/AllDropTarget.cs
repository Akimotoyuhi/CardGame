using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�ł��v���C���[���Ăł��Ȃ��J�[�h�̃h���b�v���󂯎��N���X
/// </summary>
public class AllDropTarget : MonoBehaviour, IDrop
{
    public bool CanDrop(UseType useType)
    {
        if (useType == UseType.System) return true;
        return false;
    }

    public void GetDrop(List<int[]> card)
    {
        BattleManager.Instance.DropManager.CardExecute(card);
    }

    public void OnCard(UseType? useType)
    {
        if (useType == UseType.System)
        {
            Debug.Log("System");
        }
    }
}
