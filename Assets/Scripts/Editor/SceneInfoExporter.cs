using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SceneInfoExporter : Editor
{
    class ExportSceneInfo 
    {
        [SerializeField]
        private Bounds m_Bounds;
        [SerializeField]
        private string m_ResPath;
        [SerializeField]
        private Vector3 m_Position;
        [SerializeField]
        private Vector3 m_Rotation;
        [SerializeField]
        private Vector3 m_Size;
        public ExportSceneInfo(Bounds bounds, Vector3 position, Vector3 rotation, Vector3 size, string resPath)
        {
            m_Bounds = bounds;
            m_Position = position;
            m_Rotation = rotation;
            m_Size = size;
            m_ResPath = resPath;
        }
    }

    const string SavePath = "Assets/AssetBundleRes/scene/";

    [MenuItem("Terrain/Export Scene Info")]
    private static void Export()
    {
        //先把所有选中的场景节点信息导出来
        List<ExportSceneInfo> result = new List<ExportSceneInfo>();
        foreach (var item in Selection.gameObjects)
        {
            PickChild(item.transform, result);
        }
        //预处理成四叉树结构,方便过滤

        //序列化为二进制文件
    }

    private static void PickChild(Transform transform, List<ExportSceneInfo> sceneObjectList)
    {
        var obj = PrefabUtility.GetCorrespondingObjectFromSource(transform);
        if (obj != null)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            path = path.Replace(SavePath, "");
            string ext = Path.GetExtension(path);
            path = path.Replace(ext, "");
            var o = GetChildInfo(transform, path);
            if (o != null)
                sceneObjectList.Add(o);
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                PickChild(transform.GetChild(i), sceneObjectList);
            }
        }
    }

    private static ExportSceneInfo GetChildInfo(Transform transform, string resPath)
    {
        if (string.IsNullOrEmpty(resPath))
            return null;
        Renderer[] renderers = transform.gameObject.GetComponentsInChildren<MeshRenderer>();
        if (renderers == null || renderers.Length == 0)
            return null;
        Vector3 min = renderers[0].bounds.min;
        Vector3 max = renderers[0].bounds.max;
        for (int i = 1; i < renderers.Length; i++)
        {
            min = Vector3.Min(renderers[i].bounds.min, min);
            max = Vector3.Max(renderers[i].bounds.max, max);
        }
        Vector3 size = max - min;
        Bounds bounds = new Bounds(min + size/2, size);
        if (size.x <= 0)
            size.x = 0.2f;
        if (size.y <= 0)
            size.y = 0.2f;
        if (size.z <= 0)
            size.z = 0.2f;
        bounds.size = size;
        
        ExportSceneInfo obj = new ExportSceneInfo(bounds, transform.position, transform.eulerAngles, transform.localScale, resPath);
        return obj;
        
    }
}