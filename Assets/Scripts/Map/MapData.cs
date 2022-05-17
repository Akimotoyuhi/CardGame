using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�b�v�f�[�^
/// </summary>
[CreateAssetMenu(fileName = "Map Data")]
public class MapData : ScriptableObject
{
    [SerializeField] List<MapDataBase> m_dataBases = new List<MapDataBase>();
    /// <summary>
    /// �w�肳�ꂽact�̃}�b�v�f�[�^��Ԃ�
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
            Debug.LogError($"�w�肳�ꂽ�}�b�v�f�[�^�����݂��܂��� �n���ꂽ�l=>{act}({(int)act})");
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
        Debug.LogError($"�w�肳�ꂽ�}�b�v�f�[�^�����݂��܂��� �n���ꂽ�l => {mapID}");
        return null;
    }
}
/// <summary>�}�b�v�f�[�^�x�[�X</summary>
[System.Serializable]
public class MapDataBase
{
    [SerializeField] string m_name;
    [SerializeField, Tooltip("�K��Act")] Act m_act;
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
        [SerializeField, Tooltip("�x�e�}�X�𐶐�����ŏ��ʒu")] int m_restMinIndex;
        [SerializeField, Tooltip("�x�e�}�X�𐶐�����ő�ʒu")] int m_restMaxIndex;
        [SerializeField, Tooltip("��΂ɋx�e�}�X�𐶐�����ʒu")] int m_restAbsolutelyIndex;
        [SerializeField, Tooltip("�x�e�}�X�̐����m����n���̂P��")] int m_restProbability;
        [SerializeField, Tooltip("�G���[�g�}�X�𐶐�����ŏ��ʒu")] int m_eliteMinIndex;
        [SerializeField, Tooltip("�G���[�g�}�X�𐶐�����ő�ʒu")] int m_eliteMaxIndex;
        [SerializeField, Tooltip("��΂ɃG���[�g�}�X�𐶐�����ʒu")] int m_eliteAbsolutelyIndex;
        [SerializeField, Tooltip("�G���[�g�}�X�̐����m����n���̂P��")] int m_eliteProbability;
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
    /// <summary>���̎n�܂�</summary>
    TheBeginningOfTheTower,
    /// <summary>�����̏��n</summary>
    SwampOfFallIntoADoze,
    /// <summary>�s�䍂�n</summary>
    SteepRidgeHighlands,
}
#endregion