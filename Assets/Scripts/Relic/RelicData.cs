using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RelicData")]
public class RelicData : ScriptableObject
{
    [SerializeField] List<RelicDataBase> m_relicDataBases;
    public List<RelicDataBase> RelicDataBases => m_relicDataBases;
}

[System.Serializable]
public class RelicDataBase
{
    [SerializeField] string m_name;
    [SerializeField] Sprite m_sprite;
}
public interface IRelic
{
    int[] Effect();
    //TargetType TargetType { get; }
}
public class TurnBeginRelic : IRelic
{
    public int[] Effect()
    {
        int[] ret = new int[1];
        return ret;
    }
}