using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAAA
{
    public List<BBBB> listB = new List<BBBB>();
    public enum BBBB
    {
        B
    }
    public enum CCCC
    {
        C
    }
}

public class Sample : MonoBehaviour
{
    AAAA a = new AAAA();
    //List<AAAA> list = new List<AAAA>() { AAAA.BBBB.B };
    void Start()
    {
        //list.Add(new AAAA.BBBB());
    }

    void Update()
    {
        
    }
}
