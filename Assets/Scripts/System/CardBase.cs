using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBase
{
    public int attack;
    public int block;
    public int[] conditions = new int[(int)BuffDebuff.end];
}
