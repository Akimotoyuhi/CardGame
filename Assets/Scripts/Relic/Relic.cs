using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic : MonoBehaviour
{
    private string m_name;
    private List<int[]> m_relicCommnad = new List<int[]>();
    private RelicDataBase m_database;

    public void Setup()
    {

    }

    /// <summary>
    /// ƒf[ƒ^‚ğó‚¯æ‚é
    /// </summary>
    /// <param name="relicDataBase"></param>
    private void SetData(RelicDataBase relicDataBase)
    {
        m_name = relicDataBase.Name;
    }
}
