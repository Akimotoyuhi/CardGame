using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TitleState
{
    ToCustomSelect,
    Exit,
    SceneChange,
}
public class OnDropTitleStateChange : MonoBehaviour
{
    [SerializeField] TitleState m_state;

    public void OnDrop()
    {
        TitleManager.Instance.StateChange(m_state);
    }
}
