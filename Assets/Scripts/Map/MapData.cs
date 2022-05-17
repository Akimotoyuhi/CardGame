using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップデータ
/// </summary>
[CreateAssetMenu(fileName = "Map Data")]
public class MapData : ScriptableObject
{
    [SerializeField] List<MapDataBase> m_dataBases = new List<MapDataBase>();
    /// <summary>
    /// 指定されたactのマップデータを返す
    /// </summary>
    /// <param name="act"></param>
    public MapDataBase GetMapData(Act act)
    {
        List<MapDataBase> data = new List<MapDataBase>();
        foreach (var m in m_dataBases)
        {
            if (m.Act == act)
                data.Add(m);
        }
        if (data.Count == 0)
            Debug.LogError($"指定されたマップデータが存在しません 渡された値=>{act}({(int)act})");
        int r = Random.Range(0, data.Count);
        return data[r];
    }
    public MapDataBase GetMapData(MapID mapID)
    {
        foreach (var m in m_dataBases)
        {
            if (m.MapID == mapID)
                return m;
        }
        Debug.LogError($"指定されたマップデータが存在しません 渡された値 => {mapID}");
        return null;
    }
}
/// <summary>マップデータベース</summary>
[System.Serializable]
public class MapDataBase
{
    [SerializeField] string m_name;
    [SerializeField, Tooltip("適応Act")] Act m_act;
    [SerializeField] MapID m_mapID;
    [SerializeField] Sprite m_background;
    [SerializeField] BGM m_mapBgm;
    [SerializeField] BGM m_bossBgm;
    [SerializeField] DetailSettings m_detailSetting;
    public Act Act => m_act;
    public MapID MapID => m_mapID;
    public Sprite Background => m_background;
    public BGM MapBgm => m_mapBgm;
    public BGM BossBgm => m_bossBgm;
    public DetailSettings GetDetailSetting => m_detailSetting;
    [System.Serializable]
    public class DetailSettings
    {
        [SerializeField, Tooltip("休憩マスを生成する最小位置")] int m_restMinIndex;
        [SerializeField, Tooltip("休憩マスを生成する最大位置")] int m_restMaxIndex;
        [SerializeField, Tooltip("絶対に休憩マスを生成する位置")] int m_restAbsolutelyIndex;
        [SerializeField, Tooltip("休憩マスの生成確立はn分の１か")] int m_restProbability;
        [SerializeField, Tooltip("エリートマスを生成する最小位置")] int m_eliteMinIndex;
        [SerializeField, Tooltip("エリートマスを生成する最大位置")] int m_eliteMaxIndex;
        [SerializeField, Tooltip("絶対にエリートマスを生成する位置")] int m_eliteAbsolutelyIndex;
        [SerializeField, Tooltip("エリートマスの生成確立はn分の１か")] int m_eliteProbability;
        public bool RestIndex(int sector)
        {
            if (m_restAbsolutelyIndex == sector) return true;
            if (m_restMinIndex <= sector && m_restMaxIndex >= sector)
                if (Random.Range(0, m_restProbability) == 0)
                    return true;
            return false;
        }
        public bool EliteIndex(int sector)
        {
            if (m_eliteAbsolutelyIndex == sector) return true;
            if (m_eliteMinIndex <= sector && m_eliteMaxIndex >= sector)
                if (Random.Range(0, 4) == 0)
                    return true;
            return false;
        }
    }
}
#region Enums
public enum Act
{
    One = 1,
    Two,
    Three,
}
public enum MapID
{
    /// <summary>塔の始まり</summary>
    TheBeginningOfTheTower,
    /// <summary>微睡の沼地</summary>
    SwampOfFallIntoADoze,
    /// <summary>峻嶺高地</summary>
    SteepRidgeHighlands,
}
#endregion