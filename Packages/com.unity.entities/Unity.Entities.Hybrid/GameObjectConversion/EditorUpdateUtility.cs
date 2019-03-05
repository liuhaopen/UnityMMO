#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
static class EditorUpdateUtility
{
    public static void EditModeQueuePlayerLoopUpdate()
    {
        if (!Application.isPlaying)
        {
            EditorApplication.QueuePlayerLoopUpdate();
            EditorApplication.update += EditorUpdate;
        }
    }
    static void EditorUpdate()
    {
        EditorApplication.update -= EditorUpdate;
        EditorApplication.QueuePlayerLoopUpdate();
    }
}
#endif
