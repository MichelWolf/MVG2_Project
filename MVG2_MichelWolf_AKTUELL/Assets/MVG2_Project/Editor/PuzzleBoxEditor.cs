using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PuzzleBoxManager))]
public class PuzzleBoxEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PuzzleBoxManager myTarget = (PuzzleBoxManager)target;
        if(GUILayout.Button("Simon Success"))
        {
            myTarget.SetSimonSuccess();
        }
        if (GUILayout.Button("Multi Success"))
        {
            myTarget.SetMultiSuccess();
        }
        if (GUILayout.Button("Labyrinth Success"))
        {
            myTarget.SetLabyrinthSuccess();
        }

    }
}
