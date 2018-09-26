using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneObjectLoadController))]
public class SceneObjectLoadControllerEditor : Editor {

    private SceneObjectLoadController m_Target;

    void OnEnable()
    {
        m_Target = (SceneObjectLoadController) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
#if UNITY_EDITOR
        GUILayout.Label("调试：");
        bool drawTree = GUILayout.Toggle(m_Target.debug_DrawMinDepth >= 0 && m_Target.debug_DrawMaxDepth >= 0, "显示四叉树包围盒");
        if (drawTree == false)
        {
            m_Target.debug_DrawMaxDepth = -1;
            m_Target.debug_DrawMinDepth = -1;
        }
        else
        {
            m_Target.debug_DrawMaxDepth = m_Target.debug_DrawMaxDepth < 0 ? 0 : m_Target.debug_DrawMaxDepth;
            m_Target.debug_DrawMinDepth = m_Target.debug_DrawMinDepth < 0 ? 0 : m_Target.debug_DrawMinDepth;
        }
        m_Target.debug_DrawObj = GUILayout.Toggle(m_Target.debug_DrawObj, "显示场景对象包围盒");
        if (drawTree)
        {
            GUILayout.Label("显示四叉树深度范围：");
            m_Target.debug_DrawMinDepth = Mathf.Max(0, EditorGUILayout.IntField("最小深度", m_Target.debug_DrawMinDepth));
            m_Target.debug_DrawMaxDepth = Mathf.Max(0, EditorGUILayout.IntField("最大深度", m_Target.debug_DrawMaxDepth));
        }
#endif
    }
}
