using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I�u�W�F�N�g�ɕt����N���X
/// </summary>
public class Sample : MonoBehaviour
{
    [SerializeReference, SubclassSelector]
    ISample sample = default;
}

/// <summary>
/// �V���A���C�Y�Ώۂ̃C���^�[�t�F�[�X
/// </summary>
public interface ISample
{
    int Execute();
}
public class A : ISample
{
    [Header("A�N���X")]
    [SerializeField] int a;

    public int Execute()
    {
        return a;
    }
}
public class B : ISample
{
    [Header("B�N���X")]
    [SerializeField] int b;

    public int Execute()
    {
        return b;
    }
}