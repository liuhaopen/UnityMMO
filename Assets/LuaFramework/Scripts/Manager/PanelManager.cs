using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LuaInterface;

namespace LuaFramework {
    public class PanelManager : Manager {
        private Transform parent;

        //Transform Parent {
        //    get {
        //        if (parent == null) {
        //            GameObject go = GameObject.Find("Canvas");
        //            if (go != null) parent = go.transform;
        //        }
        //        return parent;
        //    }
        //}

        public void CreatePanel(string name, LuaFunction func = null, bool is_sync_load = false) {
            string assetName = System.IO.Path.GetFileNameWithoutExtension(name);
            string abName = PackRule.PathToAssetBundleName(name);

#if UNITY_EDITOR
            if (AppConst.IsUseLocalResource)
            {
                //这个接口只在编辑器模式下生效
                Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath(name, typeof(UnityEngine.Object));
                GameObject prefab = obj as GameObject;
                if (prefab == null) return;

                GameObject go = Instantiate(prefab) as GameObject;
                go.name = assetName;
                if (func != null) func.Call(go);
                return;
            }
#endif

            if (!is_sync_load)
            {
                ResManager.LoadPrefab(abName, assetName, delegate(UnityEngine.Object[] objs) {
                    if (objs.Length == 0) return;
                    GameObject prefab = objs[0] as GameObject;
                    if (prefab == null) return;

                    GameObject go = Instantiate(prefab) as GameObject;
                    go.name = assetName;

                    if (func != null) func.Call(go);
                    Debug.LogWarning("CreatePanel::>> " + name + " " + prefab);
                });
            }
            else
            {
                //Cat_Todo : luaframework资源想同步加载的话好像只能通过宏?有空处理下
                //GameObject prefab = ResManager.LoadAsset<GameObject>(name, assetName);
                //if (prefab == null) return;

                //GameObject go = Instantiate(prefab) as GameObject;
                //go.name = assetName;

                //if (func != null) func.Call(go);
                //Debug.LogWarning("CreatePanel::>> " + name + " " + prefab);
            }
        }

    }
}