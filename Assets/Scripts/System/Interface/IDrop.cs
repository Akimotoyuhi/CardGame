using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J�[�h�̃h���b�v���󂯕t����
/// </summary>
public interface IDrop
{
    /// <summary>�h���b�v���ꂽ���̏���</summary>
    void GetDrop(List<int[]> cardCommand);
    /// <summary>�h���b�v���󂯕t���邩�ۂ�</summary>
    bool CanDrop(UseType useType);
    /// <summary>�J�[�h���h���b�v�\�ł��邱�Ƃ������\�����鎞�̔���</summary>
    void OnCard(UseType? useType);
    EnemyBase IsEnemy();
}
