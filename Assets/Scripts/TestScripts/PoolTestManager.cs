using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTestManager : MonoBehaviour
{
    [SerializeField] PoolObj m_prefab;
    PoolTest m_poolTest;

    private void Start()
    {
        m_poolTest = new PoolTest(m_prefab);
    }
}
