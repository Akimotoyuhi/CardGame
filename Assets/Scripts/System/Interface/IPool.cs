using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �p������I�u�W�F�N�g�v�[���ɑΉ�������C���^�[�t�F�[�X
/// </summary>
public interface IPool
{
    bool IsFinishd { get; }
    void Execute();
}
