using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J�[�h�̃h���b�v���󂯕t����
/// </summary>
public interface IDrop
{
    /// <summary>�h���b�v���ꂽ���̏���</summary>
    void GetDrop(int power, int block, List<Condition> conditions, UseType useType, System.Action onCast);
}
