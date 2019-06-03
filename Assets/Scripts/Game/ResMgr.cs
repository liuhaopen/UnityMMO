using System.Collections.Generic;
using UnityEngine;

namespace UnityMMO
{
public class ResMgr
{
	static ResMgr Instance;
    Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();

    public static ResMgr GetInstance()
    {
        if (Instance!=null)
            return Instance;
        Instance = new ResMgr();
        return Instance;
    }

    public void Init()
	{
        LoadPrefab("Assets/AssetBundleRes/role/prefab/MainRole.prefab", "MainRole");    
        LoadPrefab("Assets/AssetBundleRes/role/prefab/Role.prefab", "Role");
        LoadPrefab("Assets/AssetBundleRes/monster/prefab/Monster.prefab", "Monster");
        LoadPrefab("Assets/AssetBundleRes/npc/prefab/NPC.prefab", "NPC");
        LoadPrefab("Assets/AssetBundleRes/ui/common/Nameboard.prefab", "Nameboard");
        LoadPrefab("Assets/AssetBundleRes/ui/common/FightFlyWord.prefab", "FightFlyWord");
	}

    public void LoadPrefab(string path, string storePrefabName)
    {
        XLuaFramework.ResourceManager.GetInstance().LoadAsset<GameObject>(path, delegate(UnityEngine.Object[] objs) {
            if (objs.Length > 0 && (objs[0] as GameObject)!=null)
            {
                GameObject prefab = objs[0] as GameObject;
                if (prefab != null) 
                {
                    this.prefabDic[storePrefabName] = prefab;
                    return;
                }
            }
            Debug.LogError("cannot find prefab in "+path);
        });
    }

    public GameObject GetPrefab(string name)
    {
        return this.prefabDic[name];
    }

    public GameObject SpawnGameObject(string prefabName, Vector3 position=default(Vector3), Quaternion rotation=default(Quaternion))
    {
        //Cat:TODO pool this
        return GameObject.Instantiate(this.prefabDic[prefabName], position, rotation);
    }

    public void OnDestroy()
	{
		Instance = null;
	}

   
}

}