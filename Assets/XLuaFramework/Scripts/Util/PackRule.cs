
using UnityEngine;

public class PackRule
{
    static string UIPath = "Assets/AssetBundleRes/ui/";
    static string ScenePath = "Assets/AssetBundleRes/scene/";
    static string RolePath = "Assets/AssetBundleRes/role/";
    static string EffectPath = "Assets/AssetBundleRes/effect/";
    static string NPCPath = "Assets/AssetBundleRes/npc/";
    static string MonsterPath = "Assets/AssetBundleRes/monster/";
    public static string PathToAssetBundleName(string path)
    {
        path = path.Replace('\\', '/');
        // Debug.Log("path : "+path);
        // string ab_name = "";
        if (path.StartsWith(UIPath))
        {
            string sub_path = path.Substring(UIPath.Length);
            string[] path_parts = sub_path.Split('/');
            if (path_parts.Length>0)
                return  "ui_"+path_parts[0];
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
        else if (path.StartsWith(EffectPath))
        {
            string sub_path = path.Substring(EffectPath.Length);
            string[] path_parts = sub_path.Split('/');
            if (path_parts.Length>0)
                return "effect_"+path_parts[0];
        }
        else if (path.StartsWith(NPCPath))
        {
            string sub_path = path.Substring(NPCPath.Length);
            string[] path_parts = sub_path.Split('/');
            if (path_parts.Length>0)
                return "npc_"+path_parts[0];
        }
        else if (path.StartsWith(MonsterPath))
        {
            string sub_path = path.Substring(MonsterPath.Length);
            string[] path_parts = sub_path.Split('/');
            if (path_parts.Length>0)
                return "monster_"+path_parts[0];
        }
        Debug.LogError("PackRule:PathToAssetBundleName : cannot find ab name : " + path);
        return "";
    } 
}