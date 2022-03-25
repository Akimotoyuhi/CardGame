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
    /// <summary>選択中のカスタムを表示するテキストプレハブ</summary>
    [SerializeField] Text m_customPrevText;
    /// <summary>カスタムデータ</summary>
    [SerializeField] CustomModeData m_customModeData;
    /// <summary>カスタムボタンの親</summary>
    [SerializeField] Transform m_customParent;
    /// <summary>選択中のカスタムの親</summary>
    [SerializeField] Transform m_customTextParent;
    /// <summary>現在選択されているカスタム</summary>
    private List<CustomModeDataBase> m_selectCustomList = new List<CustomModeDataBase>();
    private List<CustomButton> m_customButtons = new List<CustomButton>();
    private TitleManager m_titleManager;

    public void Setup(TitleManager titleManager)
    {
        m_titleManager = titleManager;
        for (int i = 0; i < m_customModeData.DataBases.Count; i++)
        {
            CustomButton cb = Instantiate(m_customPrefab);
            cb.transform.SetParent(m_customParent);
            cb.Setup(m_customModeData.DataBases[i], this);
        }
    }

    /// <summary>
    /// CustomButtonのクリックを受け取る
    /// </summary>
    /// <param name="database"></param>
    public void OnClick(CustomModeDataBase database)
    {
        for (int i = 0; i < m_selectCustomList.Count; i++)
        {
            if (m_selectCustomList[i].Name == database.Name)
            {
                Debug.Log(database.Name + "を削除");
                m_selectCustomList.RemoveAt(i);
                SelectCustomListUpdate();
                return;
            }
        }
        Debug.Log(database.Name + "を追加");
        m_selectCustomList.Add(database);
        SelectCustomListUpdate();
    }

    /// <summary>
    /// 選択中のカスタムをリストに反映させる
    /// </summary>
    private void SelectCustomListUpdate()
    {
        for (int i = 0; i < m_customTextParent.childCount; i++)
        {
            Destroy(m_customTextParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < m_selectCustomList.Count; i++)
        {
            Text t = Instantiate(m_customPrevText);
            t.text = $"{m_selectCustomList[i].Tooltip}(+{m_selectCustomList[i].Point})";
            t.transform.SetParent(m_customTextParent);
        }
    }

    /// <summary>
    /// カスタムの選択を終了して次の画面に進むボタンをクリックした時<br/>
    /// Unityのボタンから呼ばれる事を想定している
    /// </summary>
    public void OnSelectEndButtonClick()
    {
        m_titleManager.StateChange(TitleState.SceneChange);
    }
}
