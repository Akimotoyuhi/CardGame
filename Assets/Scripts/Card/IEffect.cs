using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect
{
    CardBase GetParam();

    string GetTooltip();
}
