using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EventData : ScriptableObject
{
    public List<EventDataBase> eventDataBases = new List<EventDataBase>();
}

[System.Serializable]
public class EventDataBase
{
    [SerializeField, Multiline(10)] string[] text;
    [SerializeField] int eventid;
}

public interface ITalk
{
    string Talk();
}