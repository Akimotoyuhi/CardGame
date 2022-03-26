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
    /// <summary>合計危険度を表示するテキスト</summary>
    [SerializeField] Text m_totalRiskViewText;
    /// <summary>合計危険度を表示するテキストに表示する文字列</summary>
    [SerializeField, Tooltip("合計危険度を表示するテキストに表示する文字列\n変数の部分は{risk}と記述すること")]
    string m_totalRiskText;
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
            m_customButtons.Add(cb);
        }
        TotalRiskTextUpdate(0);
    }

    /// <summary>
    /// CustomButtonのクリックを受け取る
    /// </summary>
    /// <param name="database"></param>
    public void OnClick(CustomModeDataBase database, bool IsConflict)
    {
        for (int i = 0; i < m_selectCustomList.Count; i++)
        {
            if (m_selectCustomList[i].Name == database.Name)
            {
                Debug.Log(database.Name + "を削除");
                m_selectCustomList.RemoveAt(i);
                UpdateButtons(database, true);
                return;
            }
        }
        if (IsConflict)
        {
            for (int i = 0; i < m_selectCustomList.Count; i++)
            {
                if (database.CustomID == m_selectCustomList[i].CustomID)
                {
                    m_selectCustomList.RemoveAt(i);
                    break;
                }
            }
        }
        Debug.Log(database.Name + "を追加");
        m_selectCustomList.Add(database);
        UpdateButtons(database, false);
    }

    /// <summary>
    /// ボタンの情報の更新
    /// </summary>
    private void UpdateButtons(CustomModeDataBase database, bool isRemoved)
    {
        SelectCustomListUpdate();
        foreach (var cb in m_customButtons)
        {
            cb.SelectedIDCheck(database.CustomID, isRemoved);
        }
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
        int totalRisk = 0;
        for (int i = 0; i < m_selectCustomList.Count; i++)
        {
            Text t = Instantiate(m_customPrevText);
            t.text = $"{m_selectCustomList[i].Tooltip}(+{m_selectCustomList[i].Point})";
            t.transform.SetParent(m_customTextParent);
            totalRisk += m_selectCustomList[i].Point;
        }
        TotalRiskTextUpdate(totalRisk);
    }

    /// <summary>
    /// 合計危険度を受け取ってm_totalRiskViewTextを更新する
    /// </summary>
    /// <param name="risk"></param>
    private void TotalRiskTextUpdate(int risk)
    {
        string s = m_totalRiskText.Replace("{risk}", risk.ToString());
        m_totalRiskViewText.text = s;
        if (risk > 0)
            m_totalRiskViewText.color = Color.red;
        else
            m_totalRiskViewText.color = Color.black;
    }
    /// <summary>
    /// カスタムの選択を終了して次の画面に進むボタンをクリックした時<br/>
    /// Unityのボタンから呼ばれる事を想定している
    /// </summary>
    public void OnSelectEndButtonClick()
    {
        m_titleManager.SaveCustomList(m_selectCustomList);
        m_titleManager.StateChange(TitleState.SceneChange);
    }
}
