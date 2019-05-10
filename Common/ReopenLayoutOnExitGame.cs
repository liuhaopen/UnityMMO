using System.Collections.Generic;
using UnityEngine;

namespace U3DExtends
{
public class ReopenLayoutOnExitGame : MonoBehaviour
{
#if UNITY_EDITOR
    // class ReopenInfo
    // {
    //     string path;
    //     Vector3 pos;
    // }
    private static ReopenLayoutOnExitGame Instance;

    private static Dictionary<string, Vector3> layout_open_in_playmode = new Dictionary<string, Vector3>();
    private bool isRunningGame = false;

    public static void RecordOpenLayout(string path, Vector3 pos)
    {
        Debug.Log("record : "+path+" pos:"+pos.ToString());
        if (Instance != null && Instance.isRunningGame && path!="")
        {
            layout_open_in_playmode.Add(path, pos);
        }
    }

    private void Start()
    {
        Instance = this;
        // hadSaveOnRunTime = false;
        Debug.Log("Start");
        isRunningGame = true;
    }

    private void OnDisable() {
        // Debug.Log("disable");
        Instance = null;
    }

    private void OnTransformChildrenChanged() {
        Debug.Log("OnTransformChildrenChanged");
        List<string> wait_delete_key = new List<string>();
        foreach (var item in layout_open_in_playmode)
        {
            bool had_find = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                LayoutInfo info = transform.GetChild(i).GetComponent<LayoutInfo>();
                if (info && info.LayoutPath == item.Key)
                {
                    had_find = true;
                    break;
                }
            }
            if (!had_find)
            {
                wait_delete_key.Add(item.Key);
            }
        }
        foreach (var item in wait_delete_key)
        {
            layout_open_in_playmode.Remove(item);
        }
    }
    
    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        isRunningGame = false;
        if (layout_open_in_playmode.Count>0 && U3DExtends.Configure.ReloadLayoutOnExitGame)
        {
            System.Action<UnityEditor.PlayModeStateChange> p = null;
            p = new System.Action<UnityEditor.PlayModeStateChange>((UnityEditor.PlayModeStateChange c) => {
                foreach (var item in layout_open_in_playmode)
                {
                    // Debug.Log("item.Key : "+item.Key);
                    Transform layout = UIEditorHelper.LoadLayoutByPath(item.Key);
                    if (layout != null)
                    {
                        layout.localPosition = item.Value;
                    }
                }
                layout_open_in_playmode.Clear();
                UnityEditor.EditorApplication.playModeStateChanged -= p;
            });
            UnityEditor.EditorApplication.playModeStateChanged += p;
        }
    }
#endif
}
}