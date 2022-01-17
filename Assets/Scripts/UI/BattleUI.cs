using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    [SerializeField] Animator m_animator;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        //m_animator.
    }
}
