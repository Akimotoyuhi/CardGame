using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TitleState
{
    SceneChange,
    Exit,
    Credit,
}
public class OnDropTitleStateChange : MonoBehaviour
{
    [SerializeField] TitleState m_state;

    public void OnDrop()
    {
        TitleManager.Instance.SceneChange(m_state);
    }
}
