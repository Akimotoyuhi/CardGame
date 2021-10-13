using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect
{
    int[] GetParam();

    string GetTooltip();
}
