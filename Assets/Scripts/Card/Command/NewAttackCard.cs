using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAttackCard : ICommand
{
    [SerializeField] int power;

    public int Cast()
    {
        return power;
    }
}
