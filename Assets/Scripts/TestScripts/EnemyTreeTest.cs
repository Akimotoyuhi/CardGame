using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTreeTest : MonoBehaviour
{
    [SerializeField] int m_life = default;

    void Start()
    {
        
    }

    public bool Action(int turn)
    {
        switch (turn)
        {
            case 0:
                Debug.Log("0É^Å[Éìñ⁄ÇÃçsìÆ");
                return true;
            default:
                Debug.Log("Ç»ÇÒÇ‡ÇµÇ»Ç¢");
                return true;
        }
    }
}
