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
    Dictionary<string, List<GameObject>> gameObjectPool;

    public static ResMgr GetInstance()
    {
        if (Instance!=null)
            return Instance;
        Instance = new ResMgr();
        return Instance;
    }

    public void Init()
	{
        gameObjectPool = new Dictionary<string, List<GameObject>>();
        LoadPrefab("Assets/AssetBundleRes/role/prefab/MainRole.prefab", "MainRole");    
        LoadPrefab("Assets/AssetBundleRes/role/prefab/Role.prefab", "Role");
        LoadPrefab("Assets/AssetBundleRes/monster/prefab/Monster.prefab", "Monster");
        LoadPrefab("Assets/AssetBundleRes/npc/prefab/NPC.prefab", "NPC");
        LoadPrefab("Assets/AssetBundleRes/ui/common/Nameboard.prefab", "Nameboard");
        LoadPrefab("Assets/AssetBundleRes/ui/common/FightFlyWord.prefab", "FightFlyWord");
	}

    public void LoadPrefab(string path, string storePrefabName, Action<GameObject> callBack=null)
    {
        XLuaFramework.ResourceManager.GetInstance().LoadAsset<GameObject>(path, delegate(UnityEngine.Object[] objs) {
            if (objs.Length > 0 && (objs[0] as GameObject)!=null)
            {
                GameObject prefab = objs[0] as GameObject;
                if (prefab != null) 
                {
                    this.prefabDic[storePrefabName] = prefab;
                    if (callBack != null)
                        callBack(prefab);
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

    public GameObject GetGameObject(string name)
    {
        GameObject obj = null;
        if (gameObjectPool.ContainsKey(name))
        {
            var pool = gameObjectPool[name];
            if (pool.Count > 0)
            {
                obj = pool[pool.Count-1];
                obj.SetActive(true);
                pool.RemoveAt(pool.Count-1);
                return obj;
            }
        }
        if (this.prefabDic.ContainsKey(name))
        {
            var prefab = this.prefabDic[name];
            if (prefab != null)
                return GameObject.Instantiate(prefab);
        }
        Debug.LogError("ResMgr.GetGameObject cannot find prefab name :"+name);
        return null;
    }

    public bool HasLoadedPrefab(string name)
    {
        return this.prefabDic.ContainsKey(name);
    }

    public void UnuseGameObject(string name, GameObject obj)
    {
        // obj.SetActive(false);//交给使用者控制显示隐藏,因为 entities 的 gameObject 一隐藏就会清掉
        if (gameObjectPool.ContainsKey(name))
        {
            gameObjectPool[name].Add(obj);
        }
        else
        {
            var pool = new List<GameObject>();
            pool.Add(obj);
            gameObjectPool.Add(name, pool);
        }
    }

    public void LoadMonsterResList(List<int> list, Action<bool> callBack)
    {
        if (list.Count <= 0 && callBack != null)
            callBack(true);
        int count = 0;
        for (int i = 0; i < list.Count; i++)
        {
            var typeID = list[i];
            if (this.prefabDic.ContainsKey("MonsterRes_"+typeID))
            {
                count++;
                if (callBack != null && count==list.Count)
                    callBack(true);
                continue;
            }
            string bodyPath = ResPath.GetMonsterBodyResPath(typeID);
            if (bodyPath == string.Empty)
            {
                Debug.LogError("ResMgr:LoadMonsterResList monster body res id 0, typeID:"+typeID);
                if (callBack!=null)
                    callBack(false);
                return;
            }
            XLuaFramework.ResourceManager.GetInstance().LoadAsset<GameObject>(bodyPath, delegate(UnityEngine.Object[] objs) {
                if (objs.Length > 0 && (objs[0] as GameObject)!=null)
                {
                    GameObject prefab = objs[0] as GameObject;
                    if (prefab != null) 
                    {
                        Debug.Log("load monster ok : "+"MonsterRes_"+typeID);
                        this.prefabDic["MonsterRes_"+typeID] = prefab;
                        count++;
                        if (callBack != null && count==list.Count)
                        {
                            callBack(true);
                        }
                        return;
                    }
                }
                Debug.LogError("ResMgr:LoadMonsterResList cannot find prefab in "+bodyPath);
                if (callBack!=null)
                    callBack(false);
            });
        }
        
    }

    public void LoadSceneRes(List<string> list, Action<bool> callBack)
    {
        if (list.Count <= 0 && callBack != null)
            callBack(true);
        scenePrefabList = new List<GameObject>(list.Count);
        sceneObjectPool = new Dictionary<int, List<GameObject>>();
        UnloadAllPooledSceneObjects();
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
                        LoadingView.Instance.SetData((float)(0.4+(0.2*count/list.Count)), "加载场景资源文件:"+fileName);
                        if (callBack != null && count==list.Count)
                        {
                            callBack(true);
                        }
                        return;
                    }
                }
                Debug.LogError("cannot find scene prefab in "+list[resID]);
                if (callBack!=null)
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

    private void UnloadAllPooledSceneObjects()
    {
        foreach (var prefab in scenePrefabList)
        {
            GameObject.Destroy(prefab);
        }
        foreach (var pool in sceneObjectPool)
        {
            foreach (var obj in pool.Value)
            {
                GameObject.Destroy(obj);
            }
        }
    }

    public void OnDestroy()
	{
		Instance = null;
	}
   
}
}