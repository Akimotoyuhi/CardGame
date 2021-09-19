using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class childtest : MonoBehaviour
{
    [SerializeField] private Transform m_target;
    void Start()
    {
        transform.GetChild(0).parent = m_target;
    }
}
