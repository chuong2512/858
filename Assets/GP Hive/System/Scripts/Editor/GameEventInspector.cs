using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEvent))]
public class GameEventInspector : Editor
{
    public override void OnInspectorGUI()
    {
        GameEvent myTarget = (GameEvent)target;

        DrawDefaultInspector();
        if (GUILayout.Button("Raise"))
            myTarget.Raise();
    }
}
