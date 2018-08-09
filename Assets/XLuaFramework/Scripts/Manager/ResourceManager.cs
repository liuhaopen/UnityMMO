using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UObject = UnityEngine.Object;
using XLua;

namespace XLuaFramework
{
public class AssetBundleInfo {
    public AssetBundle m_AssetBundle;
    public int m_ReferencedCount;

    public AssetBundleInfo(AssetBundle assetBundle) {
        m_AssetBundle = assetBundle;
        m_ReferencedCount = 0;
    }
}

    [LuaCallCSharp]
    public class ResourceManager : MonoBehaviour
    {
        static ResourceManager _instance;
        string m_BaseDownloadingURL = "";
        string[] m_AllManifest = null;
        AssetBundleManifest m_AssetBundleManifest = null;
        Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();
        Dictionary<string, AssetBundleInfo> m_LoadedAssetBundles = new Dictionary<string, AssetBundleInfo>();
        Dictionary<string, List<LoadAssetRequest>> m_LoadRequests = new Dictionary<string, List<LoadAssetRequest>>();

        class LoadAssetRequest {
            public Type assetType;
            public string[] assetNames;
            public LuaFunction luaFunc;
            public Action<UObject[]> sharpFunc;
        }

        public static ResourceManager GetInstance()
        {
            return _instance;
        }

        private void Awake() {
            Debug.Log("resource manager start");
            _instance = this;
        }

        // Load AssetBundleManifest.
        public void Initialize(string manifestName, Action initOK) {
            m_BaseDownloadingURL = AppConfig.GetRelativePath();
            Debug.Log("ResourceManager:Initialize() m_BaseDownloadingURL:" + m_BaseDownloadingURL);
            LoadAsset<AssetBundleManifest>(manifestName, new string[] { "AssetBundleManifest" }, delegate(UObject[] objs) {
                if (objs.Length > 0) {
                    m_AssetBundleManifest = objs[0] as AssetBundleManifest;
                    m_AllManifest = m_AssetBundleManifest.GetAllAssetBundles();
                }
                if (initOK != null)
                    initOK();
                else
                    Debug.Log("ResourceManager:Initialize failed!");
            });
        }

        public void LoadPrefab(string abName, string assetName, Action<UObject[]> func) {
            LoadAsset<GameObject>(abName, new string[] { assetName }, func);
        }

        public void LoadPrefab(string abName, string[] assetNames, Action<UObject[]> func) {
            LoadAsset<GameObject>(abName, assetNames, func);
        }

        public void LoadPrefab(string abName, string[] assetNames, LuaFunction func) {
            LoadAsset<GameObject>(abName, assetNames, null, func);
        }

        public void LoadPrefabGameObject(string name, LuaFunction func = null) {
            string assetName = System.IO.Path.GetFileNameWithoutExtension(name);
            string abName = PackRule.PathToAssetBundleName(name);

#if UNITY_EDITOR
            if (AppConfig.DebugMode)
            {
                //这个接口只在编辑器模式下生效
                UnityEngine.Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath(name, typeof(UnityEngine.Object));
                GameObject prefab = obj as GameObject;
                if (prefab == null) return;

                GameObject go = Instantiate(prefab) as GameObject;
                go.name = assetName;
                if (func != null) func.Call(go);
                return;
            }
#endif

            // if (!is_sync_load)
            // {
                this.LoadPrefab(abName, assetName, delegate(UnityEngine.Object[] objs) {
                    if (objs.Length == 0) return;
                    GameObject prefab = objs[0] as GameObject;
                    if (prefab == null) return;

                    GameObject go = Instantiate(prefab) as GameObject;
                    go.name = assetName;

                    if (func != null) func.Call(go);
                    Debug.LogWarning("CreatePanel::>> " + name + " " + prefab);
                });
            // }
            // else
            // {
                //Cat_Todo : luaframework资源想同步加载的话好像只能通过宏?有空处理下
                //GameObject prefab = ResManager.LoadAsset<GameObject>(name, assetName);
                //if (prefab == null) return;

                //GameObject go = Instantiate(prefab) as GameObject;
                //go.name = assetName;

                //if (func != null) func.Call(go);
                //Debug.LogWarning("CreatePanel::>> " + name + " " + prefab);
            // }
        }

        string GetRealAssetPath(string abName) {
            if (abName.Equals(AppConfig.AssetDir)) {
                return abName;
            }
            abName = abName.ToLower();
            if (!abName.EndsWith(AppConfig.ExtName)) {
                abName += AppConfig.ExtName;
            }
            if (abName.Contains("/")) {
                return abName;
            }
            //string[] paths = m_AssetBundleManifest.GetAllAssetBundles();  产生GC，需要缓存结果
            for (int i = 0; i < m_AllManifest.Length; i++) {
                int index = m_AllManifest[i].LastIndexOf('/');  
                string path = m_AllManifest[i].Remove(0, index + 1);    //字符串操作函数都会产生GC
                if (path.Equals(abName)) {
                    return m_AllManifest[i];
                }
            }
            Debug.LogError("GetRealAssetPath Error:>>" + abName);
            return null;
        }

        /// <summary>
        /// 载入素材
        /// </summary>
        void LoadAsset<T>(string abName, string[] assetNames, Action<UObject[]> action = null, LuaFunction func = null) where T : UObject {
            abName = GetRealAssetPath(abName);

            LoadAssetRequest request = new LoadAssetRequest();
            request.assetType = typeof(T);
            request.assetNames = assetNames;
            request.luaFunc = func;
            request.sharpFunc = action;

            List<LoadAssetRequest> requests = null;
            if (!m_LoadRequests.TryGetValue(abName, out requests)) {
                requests = new List<LoadAssetRequest>();
                requests.Add(request);
                m_LoadRequests.Add(abName, requests);
                StartCoroutine(OnLoadAsset<T>(abName));
            } else {
                requests.Add(request);
            }
        }

        IEnumerator OnLoadAsset<T>(string abName) where T : UObject {
            AssetBundleInfo bundleInfo = GetLoadedAssetBundle(abName);
            if (bundleInfo == null) {
                yield return StartCoroutine(OnLoadAssetBundle(abName, typeof(T)));

                bundleInfo = GetLoadedAssetBundle(abName);
                if (bundleInfo == null) {
                    m_LoadRequests.Remove(abName);
                    Debug.LogError("OnLoadAsset--->>>" + abName);
                    yield break;
                }
            }
            List<LoadAssetRequest> list = null;
            if (!m_LoadRequests.TryGetValue(abName, out list)) {
                m_LoadRequests.Remove(abName);
                yield break;
            }
            for (int i = 0; i < list.Count; i++) {
                string[] assetNames = list[i].assetNames;
                List<UObject> result = new List<UObject>();

                AssetBundle ab = bundleInfo.m_AssetBundle;
                for (int j = 0; j < assetNames.Length; j++) {
                    string assetPath = assetNames[j];
                    AssetBundleRequest request = ab.LoadAssetAsync(assetPath, list[i].assetType);
                    yield return request;
                    result.Add(request.asset);

                    //T assetObj = ab.LoadAsset<T>(assetPath);
                    //result.Add(assetObj);
                }
                if (list[i].sharpFunc != null) {
                    list[i].sharpFunc(result.ToArray());
                    list[i].sharpFunc = null;
                }
                if (list[i].luaFunc != null) {
                    list[i].luaFunc.Call((object)result.ToArray());
                    list[i].luaFunc.Dispose();
                    list[i].luaFunc = null;
                }
                bundleInfo.m_ReferencedCount++;
            }
            m_LoadRequests.Remove(abName);
        }

        IEnumerator OnLoadAssetBundle(string abName, Type type) {
            string url = m_BaseDownloadingURL + abName;

            WWW download = null;
            if (type == typeof(AssetBundleManifest))
                download = new WWW(url);
            else {
                string[] dependencies = m_AssetBundleManifest.GetAllDependencies(abName);
                if (dependencies.Length > 0) {
                    m_Dependencies.Add(abName, dependencies);
                    for (int i = 0; i < dependencies.Length; i++) {
                        string depName = dependencies[i];
                        AssetBundleInfo bundleInfo = null;
                        if (m_LoadedAssetBundles.TryGetValue(depName, out bundleInfo)) {
                            bundleInfo.m_ReferencedCount++;
                        } else if (!m_LoadRequests.ContainsKey(depName)) {
                            yield return StartCoroutine(OnLoadAssetBundle(depName, type));
                        }
                    }
                }
                download = WWW.LoadFromCacheOrDownload(url, m_AssetBundleManifest.GetAssetBundleHash(abName), 0);
            }
            yield return download;

            AssetBundle assetObj = download.assetBundle;
            if (assetObj != null) {
                m_LoadedAssetBundles.Add(abName, new AssetBundleInfo(assetObj));
            }
        }

        AssetBundleInfo GetLoadedAssetBundle(string abName) {
            AssetBundleInfo bundle = null;
            m_LoadedAssetBundles.TryGetValue(abName, out bundle);
            if (bundle == null) return null;

            // No dependencies are recorded, only the bundle itself is required.
            string[] dependencies = null;
            if (!m_Dependencies.TryGetValue(abName, out dependencies))
                return bundle;

            // Make sure all dependencies are loaded
            foreach (var dependency in dependencies) {
                AssetBundleInfo dependentBundle;
                m_LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
                if (dependentBundle == null) return null;
            }
            return bundle;
        }

        /// <summary>
        /// 此函数交给外部卸载专用，自己调整是否需要彻底清除AB
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="isThorough"></param>
        public void UnloadAssetBundle(string abName, bool isThorough = false) {
            abName = GetRealAssetPath(abName);
            Debug.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory before unloading " + abName);
            UnloadAssetBundleInternal(abName, isThorough);
            UnloadDependencies(abName, isThorough);
            Debug.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory after unloading " + abName);
        }

        void UnloadDependencies(string abName, bool isThorough) {
            string[] dependencies = null;
            if (!m_Dependencies.TryGetValue(abName, out dependencies))
                return;

            // Loop dependencies.
            foreach (var dependency in dependencies) {
                UnloadAssetBundleInternal(dependency, isThorough);
            }
            m_Dependencies.Remove(abName);
        }

        void UnloadAssetBundleInternal(string abName, bool isThorough) {
            AssetBundleInfo bundle = GetLoadedAssetBundle(abName);
            if (bundle == null) return;

            if (--bundle.m_ReferencedCount <= 0) {
                if (m_LoadRequests.ContainsKey(abName)) {
                    return;     //如果当前AB处于Async Loading过程中，卸载会崩溃，只减去引用计数即可
                }
                bundle.m_AssetBundle.Unload(isThorough);
                m_LoadedAssetBundles.Remove(abName);
                Debug.Log(abName + " has been unloaded successfully");
            }
        }

        //////////新增
        public UnityEngine.Object LoadPrefabInLocalByFile(string file_path)
        {
            UnityEngine.Object new_obj = null;
#if UNITY_EDITOR
            //这个接口只在编辑器模式下生效
            new_obj = UnityEditor.AssetDatabase.LoadAssetAtPath(file_path, typeof(UnityEngine.Object));
#endif
            return new_obj;
        }
}
}