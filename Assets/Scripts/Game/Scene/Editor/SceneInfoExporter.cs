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
    static Dictionary<string, int> ResPathDic;
    static List<string> ResPathList;
    static int CurResID;

    [MenuItem("SceneEditor/Export Scene Info")]
    private static void Export()
    {
        var sceneLoadCtl = Selection.activeGameObject.GetComponent<SceneObjectLoadController>();
        // SceneInfoForServer scene_info = Selection.activeTransform.GetComponent<SceneInfoForServer>();
        if (sceneLoadCtl == null)
        {
            EditorUtility.DisplayDialog("Warning", "you must select a GameObject with SceneObjectLoadController component", "Ok");
            return;
        }
        sceneLoadCtl.ResetAllData();
        string defaultFolder = SavePath+Selection.activeGameObject.name;
        string save_path = EditorUtility.SaveFilePanel("Save Scene Info File", defaultFolder, "scene_info", "json");
        if (save_path=="")
            return;
        ResPathDic = new Dictionary<string, int>();
        ResPathList = new List<string>();
        CurResID = 0;
        SceneInfo export_info = new SceneInfo();
        //先把所有选中的场景节点信息导出来
        export_info.ObjectInfoList = new List<SceneStaticObject>();
        PickChild(Selection.activeTransform, export_info.ObjectInfoList, Vector3.one);
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

        BornInfo[] born_list = Selection.activeTransform.GetComponentsInChildren<BornInfo>();
        export_info.BornList = new List<BornInfoData>();
        foreach (var item in born_list)
        {
            export_info.BornList.Add(new BornInfoData(item.GetUnityPos(), item.born_id));
        }
        MonsterInfo[] mons_list = Selection.activeTransform.GetComponentsInChildren<MonsterInfo>();
        export_info.MonsterList = new List<int>();
        foreach (var item in mons_list)
        {
            if (!export_info.MonsterList.Exists(x => x==item.monster_type_id))
                export_info.MonsterList.Add(item.monster_type_id);
        }

        export_info.ResPathList = ResPathList;
        string json = JsonUtility.ToJson(export_info, true);
        File.WriteAllText(save_path, json);
        Debug.Log("export succeed : "+save_path+" content : "+json);

        int max_create_num = 19;
        int min_create_num = 0;
        sceneLoadCtl.Init(export_info.Bounds.center, export_info.Bounds.size, true, max_create_num, min_create_num, SceneSeparateTreeType.QuadTree);
        Debug.Log("export_info.ObjectInfoList.Count : "+export_info.ObjectInfoList.Count.ToString());
        for (int i = 0; i < export_info.ObjectInfoList.Count; i++)
        {
            sceneLoadCtl.AddSceneBlockObject(export_info.ObjectInfoList[i]);
        }
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

    private static void PickChild(Transform transform, List<SceneStaticObject> sceneObjectList, Vector3 boundScale)
    {
        var obj = PrefabUtility.GetCorrespondingObjectFromSource(transform);
        var curScale = boundScale;
        var scaleInfo = transform.GetComponent<BoundScaleInfo>();
        if (scaleInfo != null)
            curScale = scaleInfo.BoundScale;
        if (obj != null)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            var o = GetChildInfo(transform, path, curScale);
            if (o != null)
                sceneObjectList.Add(o);
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.activeSelf)
                    PickChild(transform.GetChild(i), sceneObjectList, curScale);
            }
        }
    }

    private static int GetResID(string resPath)
    {
        int resID = -1;
        bool isOk = ResPathDic.TryGetValue(resPath, out resID);
        if (!isOk)
        {
            resID = CurResID;
            CurResID++;
            ResPathDic.Add(resPath, resID);
            ResPathList.Add(resPath);
        }
        return resID;
    }

    private static SceneStaticObject GetChildInfo(Transform transform, string resPath, Vector3 boundScale)
    {
        int resID = GetResID(resPath);
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
        var expandAmount = size;
        if (boundScale.x != 1)
            expandAmount.x *= boundScale.x;
        if (boundScale.y != 1)
            expandAmount.y *= boundScale.y;
        if (boundScale.z != 1)
            expandAmount.z *= boundScale.z;
        if (expandAmount != size)
            bounds.Expand(expandAmount);
        bool useLightProbe = renderers[0].lightProbeUsage != UnityEngine.Rendering.LightProbeUsage.Off;
        int lightmapIndex = useLightProbe?-1:renderers[0].lightmapIndex;
        Vector4 lightmapScaleOffset = renderers[0].lightmapScaleOffset;
        SceneStaticObject obj = new SceneStaticObject(bounds, transform.position, transform.eulerAngles, transform.localScale, resID, lightmapIndex, lightmapScaleOffset);
        return obj;
    }
}
}