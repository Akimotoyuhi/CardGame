using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(GameManager))]
public class GameManagerUpdateButton : Editor
{

    public override void OnInspectorGUI()
    {
        GameManager gameManager = target as GameManager;
        if (GUILayout.Button("Update"))
        {
            gameManager.GUIUpdate();
        }
        if (GUILayout.Button("Reset"))
        {
            gameManager.DataReset();
        }
        base.OnInspectorGUI();
    }

}
#endif