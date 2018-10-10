using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace UnityMMO {

public class SceneInfoExporter : Editor
{
    const string SavePath = "Assets/AssetBundleRes/scene/";

    [MenuItem("Terrain/Export Scene Info")]
    private static void Export()
    {
        SceneInfo export_info = new SceneInfo();
        //先把所有选中的场景节点信息导出来
        export_info.ObjectInfoList = new List<SceneStaticObject>();
        PickChild(Selection.activeTransform, export_info.ObjectInfoList);
        if (export_info.ObjectInfoList.Count <= 0)
        {
            Debug.Log("export scene info failed!");
            return;
        }

        float maxX, maxY, maxZ, minX, minY, minZ;
        maxX = maxY = maxZ = -Mathf.Infinity;
        minX = minY = minZ = Mathf.Infinity;
        // Debug.Log("export_info.ObjectInfoList.Count : "+export_info.ObjectInfoList.Count.ToString());
        for (int i = 0; i < export_info.ObjectInfoList.Count; i++)
        {
            maxX = Mathf.Max(export_info.ObjectInfoList[i].Bounds.max.x, maxX);
            maxY = Mathf.Max(export_info.ObjectInfoList[i].Bounds.max.y, maxY);
            maxZ = Mathf.Max(export_info.ObjectInfoList[i].Bounds.max.z, maxZ);

            minX = Mathf.Min(export_info.ObjectInfoList[i].Bounds.min.x, minX);
            minY = Mathf.Min(export_info.ObjectInfoList[i].Bounds.min.y, minY);
            minZ = Mathf.Min(export_info.ObjectInfoList[i].Bounds.min.z, minZ);
        }
        Vector3 size = new Vector3(maxX - minX, maxY - minY, maxZ - minZ);
        Vector3 center = new Vector3(minX + size.x/2, minY + size.y/2, minZ + size.z/2);
        export_info.Bounds = new Bounds(center, size);

        SaveLightInfo(export_info);

        // DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(SceneInfo));
        // MemoryStream msObj = new MemoryStream();
        // //将序列化之后的Json格式数据写入流中
        // js.WriteObject(msObj, export_info);
        // msObj.Position = 0;
        // StreamReader sr = new StreamReader(msObj, Encoding.UTF8);
        // string json = sr.ReadToEnd();
        // File.WriteAllText(SavePath+Selection.activeGameObject.name+"/scene_info.json", json);
        // sr.Close();
        // msObj.Close();
        string json = JsonUtility.ToJson(export_info);
        File.WriteAllText(SavePath+Selection.activeGameObject.name+"/scene_info.json", json);
        Debug.Log("export : "+json);
        // Debug.Log("export : "+json);
    }

    private static void SaveLightInfo(SceneInfo export_info)
    {
        export_info.LightmapMode = LightmapSettings.lightmapsMode;
        export_info.LightColorResPath = new List<string>();
        export_info.LightDirResPath = new List<string>();
        if (LightmapSettings.lightmaps != null && LightmapSettings.lightmaps.Length > 0)
        {
            int l = LightmapSettings.lightmaps.Length;
            for (int i = 0; i < l; i++)
            {
                string path = AssetDatabase.GetAssetPath(LightmapSettings.lightmaps[i].lightmapColor);
                export_info.LightColorResPath.Add(path);
                path = AssetDatabase.GetAssetPath(LightmapSettings.lightmaps[i].lightmapDir);
                export_info.LightDirResPath.Add(path);
            }
        }
    }

    private static void PickChild(Transform transform, List<SceneStaticObject> sceneObjectList)
    {
        var obj = PrefabUtility.GetCorrespondingObjectFromSource(transform);
        if (obj != null)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            // path = path.Replace(SavePath, "");
            // string ext = Path.GetExtension(path);
            // path = path.Replace(ext, "");
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

    private static SceneStaticObject GetChildInfo(Transform transform, string resPath)
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
        int lightmapIndex = renderers[0].lightmapIndex;
        Vector4 lightmapScaleOffset = renderers[0].lightmapScaleOffset;
        SceneStaticObject obj = new SceneStaticObject(bounds, transform.position, transform.eulerAngles, transform.localScale, resPath, lightmapIndex, lightmapScaleOffset);
        return obj;
        
    }
}
}