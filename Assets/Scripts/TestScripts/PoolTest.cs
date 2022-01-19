using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Toolkit;

/// <summary>
/// UniRx�̃I�u�W�F�N�g�v�[��
/// �Q�l�T�C�g https://qiita.com/toRisouP/items/2a5fb86654525a4a8453
/// </summary>
public class PoolTest : ObjectPool<PoolObj>
{
    private PoolObj m_prefab;

    public PoolTest(PoolObj prefab)
    {
        m_prefab = prefab;
    }

    protected override PoolObj CreateInstance()
    {
        var g = GameObject.Instantiate(m_prefab);
        return g;
    }
}
