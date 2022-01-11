using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EventData : ScriptableObject
{
    public List<EventDataBase> m_eventDataBases = new List<EventDataBase>();
}

public enum EventType { Text, Choice }

[System.Serializable]
public class EventDataBase
{
    [SerializeReference, SubclassSelector]
    public List<IEventData> m_eventData = new List<IEventData>();
}

public interface IEventData
{
    string Execute();
    EventType EventType { get; }
}

public class ScenarioText : IEventData
{
    [Multiline(3)] public string[] m_text;
    public string Execute()
    {
        return "";
    }
    public EventType EventType => EventType.Text;
}