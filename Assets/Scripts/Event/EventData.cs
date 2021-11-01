using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EventData : ScriptableObject
{
    public List<EventDataBase> m_eventDataBases = new List<EventDataBase>();
}

[System.Serializable]
public class EventDataBase
{
    [Multiline(3)] public string[] m_text;
    [SerializeField] int m_eventid;
}