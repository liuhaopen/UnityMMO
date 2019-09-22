using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UObject = UnityEngine.Object;
using XLua;
using UnityEngine.Networking;

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
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.05f);

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
            // m_BaseDownloadingURL = AppConfig.DataPath;
            Debug.Log("ResourceManager:Initialize() m_BaseDownloadingURL:" + m_BaseDownloadingURL);
            if (AppConfig.DebugMode)
            {
                initOK();
                return;
            }
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

        public void LoadSprite(string file_path, LuaFunction func = null)
        {
            LoadAsset<Sprite>(file_path, null, func);
        }

        public void LoadTexture(string file_path, LuaFunction func = null)
        {
            LoadAsset<Texture>(file_path, null, func);
        }

        public void LoadPrefab(string file_path, LuaFunction func = null)
        {
            LoadAsset<UnityEngine.GameObject>(file_path, null, func);
        }

        public void LoadMaterial(string file_path, LuaFunction func = null)
        {
            LoadAsset<Material>(file_path, null, func);
        }

        public void LoadNavMesh(string file_name, LuaFunction func = null)
        {
            string url = m_BaseDownloadingURL + "/" + file_name;
            WWW bundle = new WWW(url);
            if (bundle.error == null)
            {
                AssetBundle ab = bundle.assetBundle; //将场景通过AssetBundle方式加载到内存中  
            }
            else
            {
                Debug.LogError(bundle.error);
            }
        }

        public void LoadPrefabGameObject(string file_path, LuaFunction func = null) {
            this.LoadAsset<GameObject>(file_path, delegate(UnityEngine.Object[] objs) {
                if (objs==null || objs.Length == 0) return;
                GameObject prefab = objs[0] as GameObject;
                if (prefab == null) return;

                GameObject go = Instantiate(prefab) as GameObject;
                string assetName = System.IO.Path.GetFileNameWithoutExtension(file_path);
                go.name = assetName;

                if (func != null) func.Call(go);
                // Debug.LogWarning("CreatePanel::>> " + file_path + " " + prefab);
            });
        }

        public void LoadPrefabGameObjectWithAction(string file_path, Action<UObject> action = null) {
            this.LoadAsset<GameObject>(file_path, delegate(UnityEngine.Object[] objs) {
                if (objs.Length == 0) return;
                GameObject prefab = objs[0] as GameObject;
                if (prefab == null) return;

                GameObject go = Instantiate(prefab) as GameObject;
                string assetName = System.IO.Path.GetFileNameWithoutExtension(file_path);
                go.name = assetName;

                if (action != null) action(go);
            });
        }

        string GetRealAssetPath(string abName) {
            if (abName.Equals(AppConfig.AssetDir)) {
                return abName;
            }
            abName = abName.ToLower();
            if (abName.Contains("/")) {
                return abName;
            }
            for (int i = 0; i < m_AllManifest.Length; i++) {
                int index = m_AllManifest[i].LastIndexOf('/');  
                string path = m_AllManifest[i].Remove(0, index + 1);    //字符串操作函数都会产生GC
                if (path.Equals(abName)) {
                    return m_AllManifest[i];
                }
            }
            Debug.LogError("GetRealAssetPath Error:>>" + abName);
            return "";
        }

        public void LoadAsset<T>(string file_path, Action<UObject[]> action = null, LuaFunction func = null) where T : UObject 
        {
#if UNITY_EDITOR
            if (AppConfig.DebugMode)
            {
                StartCoroutine(LoadAssetInLocal<T>(file_path, action, func));
                return;
            }
#endif
            // string assetName = System.IO.Path.GetFileNameWithoutExtension(file_path);
            string abName = PackRule.PathToAssetBundleName(file_path);
            string resName = file_path.ToLower();
            this.LoadAsset<T>(abName, new string[] {resName}, action, func);
        }

        IEnumerator LoadAssetInLocal<T>(string file_path, Action<UObject[]> action = null, LuaFunction func = null) where T : UObject
        {
            yield return waitForSeconds;
#if UNITY_EDITOR
            T res = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(file_path);
            if (res != null)
            {
                if (func != null)
                {
                    List<T> list = new List<T>();
                    list.Add(res);
                    object[] args = new object[] { list.ToArray() };
                    func.Call(args);
                    func.Dispose();
                    func = null;
                }
                else if (action != null)
                {
                    List<UObject> list = new List<UObject>();
                    list.Add(res);
                    UObject[] args = list.ToArray();
                    action(args);
                }
            }
            else
            {
                Debug.Log("ResourceManager:LoadAsset Error:cannot find file:" + file_path);
            }
#endif
        }

        /// <summary>
        /// 载入素材
        /// </summary>
        void LoadAsset<T>(string abName, string[] assetNames, Action<UObject[]> action = null, LuaFunction func = null) where T : UObject {
            // Debug.Log("ResourceManager:LoadAsset() abName : "+abName+" assetNames:"+assetNames[0]);
            abName = GetRealAssetPath(abName);
            if (abName=="")
            {
                if (action != null)
                    action(null);
                if (func != null)
                {
                    func.Call(null);
                    func.Dispose();
                    func = null;
                }
                return;
            }
            // Debug.Log("ResourceManager:LoadAsset() real abName : "+abName);

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
                // Debug.Log("OnLoadAsset abName : "+abName+" bundleInfo:"+(bundleInfo!=null));
                if (bundleInfo == null) {
                    m_LoadRequests.Remove(abName);
                    Debug.LogError("OnLoadAsset failed!--->>>" + abName);
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
            // Debug.Log("OnLoadAssetBundle : url:"+url+" m_AssetBundleManifest:"+(m_AssetBundleManifest!=null) + new System.Diagnostics.StackTrace().ToString());
            UnityWebRequest webRequest = null;
            if (type == typeof(AssetBundleManifest))
            {
                webRequest = UnityWebRequestAssetBundle.GetAssetBundle(url);   
            }
            else 
            {
                Hash128 hash = new Hash128();
                if (m_AssetBundleManifest != null)
                {
                    string[] dependencies = m_AssetBundleManifest.GetAllDependencies(abName);
                    if (dependencies.Length > 0) {
                        m_Dependencies.Add(abName, dependencies);
                        for (int i = 0; i < dependencies.Length; i++) {
                            string depName = dependencies[i];
                            AssetBundleInfo bundleInfo = null;
                            if (m_LoadedAssetBundles.TryGetValue(depName, out bundleInfo)) 
                            {
                                bundleInfo.m_ReferencedCount++;
                            } 
                            else if (m_LoadRequests.ContainsKey(depName)) 
                            {
                                yield return WaitForAssetBundleLoaded(depName);
                            }
                            else
                            {
                                m_LoadRequests.Add(depName, null);
                                yield return StartCoroutine(OnLoadAssetBundle(depName, type));
                                m_LoadRequests.Remove(depName);
                            }
                        }
                    }
                    hash = m_AssetBundleManifest.GetAssetBundleHash(abName);
                }
                webRequest = UnityWebRequestAssetBundle.GetAssetBundle(url, hash, 0);
            }
            yield return webRequest.SendWebRequest();

            AssetBundle assetObj = DownloadHandlerAssetBundle.GetContent(webRequest);
            // Debug.Log("assetObj : "+(assetObj!=null)+" abName:"+abName+" isDone:"+webRequest.isDone);
            if (assetObj != null) {
                m_LoadedAssetBundles.Add(abName, new AssetBundleInfo(assetObj));
            }
        }

        IEnumerator WaitForAssetBundleLoaded(string abName)
        {
            float tryTime = 30.0f;
            float tryDuration = 0.03f;
            while (true)
            {
                yield return new WaitForSeconds(tryDuration);
                if (!m_LoadRequests.ContainsKey(abName))
                    break;
                tryTime -= tryDuration;
                if (tryTime <= 0)
                    break;
            }
        }

        AssetBundleInfo GetLoadedAssetBundle(string abName) {
            AssetBundleInfo bundle;
            m_LoadedAssetBundles.TryGetValue(abName, out bundle);
            // Debug.Log("GetLoadedAssetBundle abName:"+abName+" isNotNil:"+(bundle==null)+" isContain:"+(m_LoadedAssetBundles.ContainsKey(abName)));
            if (bundle == null)
            {
                return null;
            }

            // No dependencies are recorded, only the bundle itself is required.
            string[] dependencies = null;
            if (!m_Dependencies.TryGetValue(abName, out dependencies))
            {
                return bundle;
            }

            // Make sure all dependencies are loaded
            foreach (var dependency in dependencies) {
                AssetBundleInfo dependentBundle;
                m_LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
                if (dependentBundle == null) 
                {
                    return null;
                }
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
            if (abName=="")
                return;
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
}
}