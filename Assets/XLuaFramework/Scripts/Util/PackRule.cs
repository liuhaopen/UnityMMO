
using UnityEngine;

public class PackRule
{
    static string UIPath = "Assets/AssetBundleRes/ui/";
    static string ScenePath = "Assets/AssetBundleRes/scene/";
    static string RolePath = "Assets/AssetBundleRes/role/";
    public static string PathToAssetBundleName(string path)
    {
        path = path.Replace('\\', '/');
        // Debug.Log("path : "+path);
        string ab_name = "";
        if (path.StartsWith(UIPath))
        {
            // Debug.Log("UIPath : "+UIPath);
            ab_name = "ui_";
            string sub_path = path.Substring(UIPath.Length);
            string[] path_parts = sub_path.Split('/');
            for (int i = 1; i < path_parts.Length-1; i++)
            {
                ab_name += path_parts[i];
                if (i < path_parts.Length - 2)
                    ab_name += "_";
            }
            return ab_name;
        }
        else if (path.StartsWith(ScenePath))
        {
            string sub_path = path.Substring(ScenePath.Length);
            string[] path_parts = sub_path.Split('/');
            if (path_parts.Length>0)
                return  "scene_"+path_parts[0];
        }
        else if (path.StartsWith(RolePath))
        {
            string sub_path = path.Substring(RolePath.Length);
            string[] path_parts = sub_path.Split('/');
            if (path_parts.Length>0)
                return "role_"+path_parts[0];
        }
        Debug.Log("PackRule:PathToAssetBundleName : cannot find ab name : " + path);
        return "";
    } 
}