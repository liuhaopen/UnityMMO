using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace UnityMMO
{
public class ResMgr
{
	static ResMgr Instance;
    Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
    List<GameObject> scenePrefabList;
    Dictionary<int, List<GameObject>> sceneObjectPool;
    Dictionary<string, List<GameObject>> gameObjectPool;
    Dictionary<string, Material> materialDic;
    List<KeyValuePair<string, string>> preloadRes;
    List<KeyValuePair<string, string>> preloadTimelineRes;
    List<KeyValuePair<string, string>> preloadMaterialRes;
    Dictionary<string, PlayableAsset> playablePool;
    
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
        playablePool = new Dictionary<string, PlayableAsset>();
        materialDic = new Dictionary<string, Material>();
        preloadRes = new List<KeyValuePair<string, string>>();
        preloadRes.Add(new KeyValuePair<string,string>("MainRole", "Assets/AssetBundleRes/role/prefab/MainRole.prefab"));
        preloadRes.Add(new KeyValuePair<string,string>("Role", "Assets/AssetBundleRes/role/prefab/Role.prefab"));
        preloadRes.Add(new KeyValuePair<string,string>("Monster", "Assets/AssetBundleRes/monster/prefab/Monster.prefab"));
        preloadRes.Add(new KeyValuePair<string,string>("NPC", "Assets/AssetBundleRes/npc/prefab/NPC.prefab"));
        preloadRes.Add(new KeyValuePair<string,string>("Nameboard", "Assets/AssetBundleRes/ui/common/Nameboard.prefab"));
        preloadRes.Add(new KeyValuePair<string,string>("FightFlyWord", "Assets/AssetBundleRes/ui/common/FightFlyWord.prefab"));
        preloadRes.Add(new KeyValuePair<string,string>("CameraForLogin", "Assets/AssetBundleRes/scene/login/objs/other_effect/chuangjue/camera_for_create_role.prefab"));
        preloadRes.Add(new KeyValuePair<string,string>("SceneForLogin", "Assets/AssetBundleRes/scene/login/objs/other_effect/chuangjue/scene_for_login.prefab"));
        preloadRes.Add(new KeyValuePair<string,string>("UIBagView", "Assets/AssetBundleRes/ui/bag/BagView.prefab"));
        preloadRes.Add(new KeyValuePair<string,string>("UITaskDialogView", "Assets/AssetBundleRes/ui/task/TaskDialogView.prefab"));
        preloadTimelineRes = new List<KeyValuePair<string, string>>();
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/jump_career1_1.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/jump_career1_2.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/jump_career1_3.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/jump_career2_1.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/jump_career2_2.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/jump_career2_3.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/skill_110000.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/skill_110001.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/skill_110002.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/skill_110003.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/skill_110010.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/skill_110011.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/skill_110012.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/skill_110013.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/skill_120000.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/skill_120001.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/skill_120002.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/skill_120003.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/skill_120010.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/skill_120011.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/skill_120012.playable"));
        preloadTimelineRes.Add(new KeyValuePair<string,string>("Playable", "Assets/AssetBundleRes/role/timeline/skill_120013.playable"));
        preloadMaterialRes =  new List<KeyValuePair<string, string>>();
        preloadMaterialRes.Add(new KeyValuePair<string,string>("function_hitfresnel", "Assets/AssetBundleRes/effect/materials/function_hitfresnel.mat"));
	}

    public void StartPreLoadRes(Action<bool> onOk)
    {
        int count = 0;
        for (int i = 0; i < preloadRes.Count; i++)
        {
            LoadPrefab(preloadRes[i].Value, preloadRes[i].Key, (GameObject obj)=>{
                if (obj != null)
                {
                    count++;
                    if (count >= preloadRes.Count)
                    {
                        if (onOk != null)
                        {
                            onOk(true);
                            onOk = null;
                        }
                    }
                }
                else
                {
                    if (onOk != null)
                    {
                        onOk(false);
                        onOk = null;
                    }
                }
            });
        }
        for (int i = 0; i < preloadTimelineRes.Count; i++)
        {
            LoadPlayable(preloadTimelineRes[i].Value, preloadTimelineRes[i].Key);
        }
        for (int i = 0; i < preloadMaterialRes.Count; i++)
        {
            LoadMaterial(preloadMaterialRes[i].Value, preloadMaterialRes[i].Key);
        }
    }

    public void LoadMaterial(string path, string storeName, Action<Material> callBack=null)
    {
        XLuaFramework.ResourceManager.GetInstance().LoadAsset<Material>(path, delegate(UnityEngine.Object[] objs) {
            if (objs.Length > 0 && (objs[0] as Material)!=null)
            {
                var material = objs[0] as Material;
                if (material != null) 
                {
                    this.materialDic[storeName] = material;
                    if (callBack != null)
                        callBack(material);
                    return;
                }
            }
            Debug.LogError("cannot find material in "+path);
            if (callBack != null)
                callBack(null);
        });
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
            if (callBack != null)
                callBack(null);
        });
    }

    public void LoadPlayable(string path, string storePlayablebName, Action<PlayableAsset> callBack=null)
    {
        XLuaFramework.ResourceManager.GetInstance().LoadAsset<PlayableAsset>(path, delegate(UnityEngine.Object[] objs) {
            if (objs.Length > 0 && (objs[0] as PlayableAsset)!=null)
            {
                PlayableAsset palyable = objs[0] as PlayableAsset;
                // Debug.Log("palyable ；"+path+" name:"+storePlayablebName);
                if (palyable != null) 
                {
                    this.playablePool[storePlayablebName] = palyable;
                    if (callBack != null)
                        callBack(palyable);
                    return;
                }
            }
            Debug.LogError("cannot find palyable in "+path);
            if (callBack != null)
                callBack(null);
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

    public Material GetMeterial(string name)
    {
        var material = this.materialDic[name];
        if (material != null)
            return new Material(material);
        return null;
    }

    public void ClearGameObjectPool(string name)
    {
        if (gameObjectPool.ContainsKey(name))
        {
            gameObjectPool[name].Clear();
        }
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
                        // Debug.Log("load monster ok : "+"MonsterRes_"+typeID);
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