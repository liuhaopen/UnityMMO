using UnityEngine;
using UnityEditor;
using System.Collections;
using Invector.CharacterController;
using System.Reflection;

[CustomEditor(typeof(vThirdPersonMotor), true)]
public class vCharacterEditor : Editor
{
    GUISkin skin;
    SerializedObject character;
    bool showWindow;

    void OnEnable()
    {
        vThirdPersonMotor motor = (vThirdPersonMotor)target;
    }

    public override void OnInspectorGUI()
    {
        if (!skin) skin = Resources.Load("skin") as GUISkin;
        GUI.skin = skin;

        vThirdPersonMotor motor = (vThirdPersonMotor)target;

        if (!motor) return;

        GUILayout.BeginVertical("Basic Controller LITE by Invector", "window");

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Purchase FULL Version"))
        {
            UnityEditorInternal.AssetStore.Open("/content/59332");
        }

        EditorGUILayout.Space();     

        EditorGUILayout.BeginVertical();        

        base.OnInspectorGUI();
      
        GUILayout.EndVertical();
        EditorGUILayout.EndVertical();        

        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }   
}

public class PopUpInfoEditor : EditorWindow
{
    GUISkin skin;
    Vector2 rect = new Vector2(360, 100);

    void OnGUI()
    {
        this.titleContent = new GUIContent("Warning!");
        this.minSize = rect;

        EditorGUILayout.HelpBox("Please assign your ThirdPersonController to the Layer 'Player'.", MessageType.Warning);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("OK", GUILayout.Width(80), GUILayout.Height(20)))
            this.Close();
    }
}