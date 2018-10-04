using UnityEngine;
using UnityEditor;
using System.IO;

class SaveAsPrefab
{
    [MenuItem("Terrain/Save SelectObjs As Prefab")]
    private static void OnClickSaveAsPrefab()
    {
        string save_path = "Assets/Temp/";
        if (!Directory.Exists(save_path))
            Directory.CreateDirectory(save_path);

        foreach (var item in Selection.gameObjects)
        {
            string new_file_path = save_path+item.name+".prefab";
            PrefabUtility.CreatePrefab(new_file_path, item);
        }
    }
}