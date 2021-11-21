using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICondition
{
    int Effect(EventTiming eventTiming, int num = 0);
    int IsBuff();
}
