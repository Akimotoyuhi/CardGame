using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SIngletonTest : MonoBehaviour
{
    void Start()
    {
        Debug.Log(GodGameManager.getInstance().get());
    }

    void Update()
    {
        GodGameManager.getInstance().set(GodGameManager.getInstance().get() + 1);
        Debug.Log(GodGameManager.getInstance().get());
    }

    public void SceneChanged()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
