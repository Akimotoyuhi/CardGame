using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] Button m_button;
    [SerializeField] TitleState m_titleState;

    void Start()
    {
        m_button.OnClickAsObservable()
            .Subscribe(_ => TitleManager.Instance.StateChange(m_titleState))
            .AddTo(this);
    }
}
