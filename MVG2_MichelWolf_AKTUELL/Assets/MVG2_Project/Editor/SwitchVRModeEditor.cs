using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SwitchVRMode))]
public class SwitchVRModeEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SwitchVRMode myTarget = (SwitchVRMode)target;
        if(GUILayout.Button("Switch Mode"))
        {
            myTarget.SwitchVRModeButton();
        }
        
    }
}
