using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトル画面を制御するマネージャー
/// </summary>
public class TitleManager : MonoBehaviour
{
    [SerializeField] string m_changeSceneName;
    //[SerializeField] GameObject m_creditPanel;
    [SerializeField] FadeTime m_FadeTime;
    [System.Serializable]
    public class FadeTime
    {
        [SerializeField] float m_sceneChange;
        [SerializeField] float m_gameExit;
        [SerializeField] float m_credit;
        public float GetFadeTime(TitleState titleState)
        {
            switch (titleState)
            {
                case TitleState.SceneChange:
                    return m_sceneChange;
                case TitleState.Exit:
                    return m_gameExit;
                case TitleState.Credit:
                    return m_credit;
                default:
                    Debug.Log("例外");
                    return 0f;
            }
        }
    }
    public static TitleManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    //private void Start()
    //{
    //    m_creditPanel.SetActive(false);
    //}

    /// <summary>
    /// 渡されたTitleStateに応じて画面を切り替える
    /// </summary>
    /// <param name="titleState"></param>
    public void SceneChange(TitleState titleState)
    {
        EffectManager.Instance.Fade(Color.black, m_FadeTime.GetFadeTime(titleState), () =>
        {
            switch (titleState)
            {
                case TitleState.SceneChange:
                    SceneManager.LoadScene(m_changeSceneName);
                    break;
                case TitleState.Exit:
                    Application.Quit();
                    break;
                //case TitleState.Credit:
                //    m_creditPanel.SetActive(true);
                //    break;
                default:
                    break;
            }
        });
    }

    //public void PanelReset()
    //{
    //    m_creditPanel.SetActive(false);
    //}
}
