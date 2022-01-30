using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Relic")]
public class RelicData : ScriptableObject
{
    [SerializeField] List<RelicDataBase> m_relicDataBases;
    public List<RelicDataBase> RelicDataBases => m_relicDataBases;
}

[System.Serializable]
public class RelicDataBase
{
    [SerializeField] string m_name;
}