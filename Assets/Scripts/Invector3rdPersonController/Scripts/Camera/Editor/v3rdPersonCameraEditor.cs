using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor (typeof(vThirdPersonCamera))]
[CanEditMultipleObjects]
public class v3rdPersonCameraEditor : Editor 
{
    GUISkin skin;
    vThirdPersonCamera tpCamera;

    public override void OnInspectorGUI()
    {
        if (!skin) skin = Resources.Load("skin") as GUISkin;
        GUI.skin = skin;                

        GUILayout.BeginVertical("Basic Camera LITE by Invector", "window");

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginVertical();

        base.OnInspectorGUI();

        GUILayout.EndVertical();
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }

}
