using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J�[�h�̃h���b�v���󂯕t����
/// </summary>
public interface IDrop
{
    /// <summary>�h���b�v���ꂽ���̏���</summary>
    void GetDrop(int power, int block, Condition condition);
    /// <summary>�h���b�v��</summary>
    bool CanDrop(UseType useType);
}
