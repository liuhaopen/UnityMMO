using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMMO
{
public class ResMgr
{
	static ResMgr Instance;
    Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
    List<GameObject> scenePrefabList;
    Dictionary<int, List<GameObject>> sceneObjectPool;

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

    public void LoadSceneRes(List<string> list, Action<bool> callBack)
    {
        scenePrefabList = new List<GameObject>(list.Count);
        sceneObjectPool = new Dictionary<int, List<GameObject>>();
        for (int i = 0; i < list.Count; i++)
        {
            scenePrefabList.Add(null);
        }
        int count = 0;
        for (int i = 0; i < list.Count; i++)
        {
            int resID = i;
            XLuaFramework.ResourceManager.GetInstance().LoadAsset<GameObject>(list[i], delegate(UnityEngine.Object[] objs) 
            {
                if (objs.Length > 0 && (objs[0] as GameObject)!=null)
                {
                    GameObject prefab = objs[0] as GameObject;
                    if (prefab != null) 
                    {
                        scenePrefabList[resID] = prefab;
                        count++;
                        var fileName = System.IO.Path.GetFileName(list[resID]);
                        LoadingView.Instance.SetData((float)(0.4+(0.2*count/list.Count)), "加载场景资源文件："+fileName);
                        if (callBack != null && count==list.Count)
                        {
                            callBack(true);
                        }
                        return;
                    }
                }
                Debug.LogError("cannot find scene prefab in "+list[resID]);
                callBack(false);
            });
        }
        
    }

    public GameObject GetSceneRes(int resID)
    {
        GameObject obj = null;
        if (sceneObjectPool.ContainsKey(resID))
        {
            var pool = sceneObjectPool[resID];
            if (pool.Count > 0)
            {
                obj = pool[pool.Count-1];
                obj.SetActive(true);
                pool.RemoveAt(pool.Count-1);
                return obj;
            }
        }
        if (resID >= 0 && resID < scenePrefabList.Count)
            return GameObject.Instantiate(scenePrefabList[resID]);
        return null;
    }

    public void UnuseSceneObject(int resID, GameObject obj)
    {
        obj.SetActive(false);
        if (sceneObjectPool.ContainsKey(resID))
        {
            sceneObjectPool[resID].Add(obj);
        }
        else
        {
            var pool = new List<GameObject>();
            pool.Add(obj);
            sceneObjectPool.Add(resID, pool);
        }
    }

    public void OnDestroy()
	{
		Instance = null;
	}
   
}
}