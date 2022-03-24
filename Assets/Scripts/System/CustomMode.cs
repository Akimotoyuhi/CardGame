using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// チャレンジモードの管理
/// </summary>
public class CustomMode : MonoBehaviour
{
    /// <summary>各カスタムボタンのプレハブ</summary>
    [SerializeField] CustomButton m_customPrefab;
    /// <summary>カスタムデータ</summary>
    [SerializeField] CustomModeData m_customModeData;
    /// <summary>カスタムボタンの親</summary>
    [SerializeField] Transform m_customParent;
    /// <summary>現在選択されているカスタム</summary>
    private List<CustomModeDataBase> m_sellectCustomList = new List<CustomModeDataBase>();

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        for (int i = 0; i < m_customModeData.DataBases.Count; i++)
        {
            CustomButton cb = Instantiate(m_customPrefab);
            cb.transform.SetParent(m_customParent);
            cb.Setup(m_customModeData.DataBases[i]);
        }
    }
}
