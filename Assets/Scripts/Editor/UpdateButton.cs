using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(GameManager))]
public class UpdateButton : Editor
{

    public override void OnInspectorGUI()
    {
        GameManager gameManager = target as GameManager;
        if (GUILayout.Button("Update"))
        {
            gameManager.GUIUpdate();
        }
        base.OnInspectorGUI();
    }

}
#endif