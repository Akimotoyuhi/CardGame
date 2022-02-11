using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Relic : MonoBehaviour
{
    [SerializeField] Image m_image;
    public int Power { get; private set; }
    public int Block { get; private set; }
    public List<Condition> Conditions { get; private set; }
    public List<RelicTriggerTiming> RelicTriggerTiming { get; private set; }

    public void SetParam(RelicDataBase dataBase)
    {
        Power = dataBase.Power;
        Block = dataBase.Block;
        Conditions = dataBase.Conditions;
        RelicTriggerTiming = dataBase.RelicTriggerTiming;
    }
}