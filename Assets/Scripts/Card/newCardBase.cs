using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCardBase : MonoBehaviour
{
    [Serializable]
    class CardCommandSet
    {
        [SerializeReference, SubclassSelector]
        public ICommand m_command;
        public string m_name = "name";
        public int m_cost = 0;
    }

    [SerializeField] List<CardCommandSet> m_commands = new List<CardCommandSet>();

    private void Start()
    {
        int[] i = new int[(int)BuffDebuff.end];
        i = m_commands[0].m_command.SetParam();
        Debug.Log($"CardName{m_commands[0].m_name}, Cost{m_commands[0].m_cost}");
        foreach (var a in i)
        {
            Debug.Log("Parametor" + a);
        }
    }

    private void CreateCard()
    {

    }
}
