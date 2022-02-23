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
    public static TitleManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }

    public void SceneChange(TitleState stateName)
    {
        EffectManager.Instance.Fade(Color.black, 0.5f, () =>
        {
            switch (stateName)
            {
                case TitleState.SceneChange:
                    SceneManager.LoadScene(m_changeSceneName);
                    break;
                case TitleState.Exit:
                    Application.Quit();
                    break;
                default:
                    break;
            }
        });
    }
}
