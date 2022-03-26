using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mastar;

/// <summary>
/// �^�C�g����ʂ𐧌䂷��}�l�[�W���[
/// </summary>
public class TitleManager : MonoBehaviour
{
    [SerializeField] string m_changeSceneName;
    [SerializeField] FadeTime m_FadeTime;
    [System.Serializable]
    public class FadeTime
    {
        [SerializeField] float m_customSelect;
        [SerializeField] float m_gameExit;
        [SerializeField] float m_sceneChange;
        public float GetFadeTime(TitleState titleState)
        {
            switch (titleState)
            {
                case TitleState.ToCustomSelect:
                    return m_customSelect;
                case TitleState.Exit:
                    return m_gameExit;
                case TitleState.SceneChange:
                    return m_sceneChange;
                default:
                    Debug.Log("��O");
                    return 0f;
            }
        }
    }
    [SerializeField] CustomMode m_CustomMode;
    [SerializeField] GameObject m_mainTitleScreen;
    [SerializeField] GameObject m_customScreen;
    public static TitleManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_CustomMode.Setup(this);
        m_customScreen.SetActive(false);
    }

    /// <summary>
    /// �n���ꂽTitleState�ɉ����ĉ�ʂ�؂�ւ���
    /// </summary>
    /// <param name="titleState"></param>
    public void StateChange(TitleState titleState)
    {
        EffectManager.Instance.Fade(Color.black, m_FadeTime.GetFadeTime(titleState), () =>
        {
            switch (titleState)
            {
                case TitleState.ToCustomSelect:
                    m_customScreen.SetActive(true);
                    m_mainTitleScreen.SetActive(false);
                    break;
                case TitleState.Exit:
                    Application.Quit();
                    break;
                case TitleState.SceneChange:
                    SceneManager.LoadScene(m_changeSceneName);
                    break;
                default:
                    break;
            }
            EffectManager.Instance.Fade(Color.clear, m_FadeTime.GetFadeTime(titleState));
        });
    }

    /// <summary>�I�𒆂̃J�X�^����ۑ�����</summary>
    public void SaveCustomList(List<CustomModeDataBase> customDataBases, int totalRisk)
    {
        DataManager.Instance.CustomList = customDataBases;
        DataManager.Instance.TotalRisk = totalRisk;
    }
}
