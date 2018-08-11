using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;
using XLuaFramework;

/// <summary>
/// 说明：xLua管理类
/// 注意：
/// 1、整个Lua虚拟机执行的脚本分成3个模块：热修复、公共模块、逻辑模块
/// 2、公共模块：提供Lua语言级别的工具类支持，和游戏逻辑无关，最先被启动
/// 3、热修复模块：脚本全部放Lua/XLua目录下，随着游戏的启动而启动
/// 4、逻辑模块：资源热更完毕后启动
/// 5、资源热更以后，理论上所有被加载的Lua脚本都要重新执行加载，如果热更某个模块被删除，则可能导致Lua加载异常，这里的方案是释放掉旧的虚拟器另起一个
/// @by wsh 2017-12-28
/// </summary>

[Hotfix]
[LuaCallCSharp]
public class XLuaManager : MonoBehaviour
{
    Action<float, float> luaUpdate = null;
    Action luaLateUpdate = null;
    Action<float> luaFixedUpdate = null;
    LuaEnv luaEnv = null;

    protected void Awake()
    {
        InitLuaEnv();
    }

    public LuaEnv GetLuaEnv()
    {
        return luaEnv;
    }

    private void InitExternal()
    {
        luaEnv.AddBuildin("sproto.core", XLua.LuaDLL.Lua.LoadSproto);
        luaEnv.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);
        
    }

    void InitLuaEnv()
    {
        luaEnv = new LuaEnv();
        if (luaEnv != null)
        {
            luaEnv.AddLoader(CustomLoader);

            InitExternal();
            SafeDoString("require('BaseRequire')");

            luaUpdate = luaEnv.Global.Get<Action<float, float>>("Update");
            luaLateUpdate = luaEnv.Global.Get<Action>("LuaUpdate.LateUpdate");
            luaFixedUpdate = luaEnv.Global.Get<Action<float>>("LuaUpdate.FixedUpdate");

            LoadScript("Main");
            SafeDoString("Main()");

            if (NetworkManager.GetInstance())
            {
                SafeDoString("require 'Logic.Network'; Network.Start() ");
                NetworkManager.GetInstance().OnConnectCallBack = luaEnv.Global.Get<Action>("OnConnectServer");
                Debug.Log("luamanager onConnectCallBack : "+(NetworkManager.GetInstance().OnConnectCallBack!=null).ToString());
            }
            else
                Debug.LogError("must init network manager before init xlua manager!");
        }
        else
        {
            Debug.LogError("InitLuaEnv failed!");
        }
    }

    public void StartGame()
    {

    }

    public void SafeDoString(string scriptContent)
    {
        if (luaEnv != null)
        {
            try
            {
                luaEnv.DoString(scriptContent);
            }
            catch (System.Exception ex)
            {
                string msg = string.Format("xLua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                Debug.LogError(msg, null);
            }
        }
    }
    
    void LoadScript(string scriptName)
    {
        SafeDoString(string.Format("require('{0}')", scriptName));
    }

    public static byte[] CustomLoader(ref string filepath)
    {
        string scriptPath = string.Empty;
        filepath = filepath.Replace(".", "/") + ".lua";
#if UNITY_EDITOR
        if (AppConfig.DebugMode)
        {
            // scriptPath = Path.Combine(Application.dataPath, luaScriptsFolder);
            scriptPath = Path.Combine(AppConfig.LuaAssetsDir, filepath);
            // Debug.Log("Load lua script : " + scriptPath);
            return Util.GetFileBytes(scriptPath);
        }
#endif

        // scriptPath = string.Format("{0}/{1}.bytes", luaAssetbundleAssetName, filepath);
        // string assetbundleName = null;
        // string assetName = null;
        // bool status = AssetBundleManager.Instance.MapAssetPath(scriptPath, out assetbundleName, out assetName);
        // if (!status)
        // {
        //     Debug.LogError("MapAssetPath failed : " + scriptPath);
        //     return null;
        // }
        // var asset = AssetBundleManager.Instance.GetAssetCache(assetName) as TextAsset;
        // if (asset != null)
        // {
        //     //Logger.Log("Load lua script : " + scriptPath);
        //     return asset.bytes;
        // }
        // Debug.LogError("Load lua script failed : " + scriptPath + ", You should preload lua assetbundle first!!!");
        return null;
    }

    private void Update()
    {
        if (luaEnv != null)
        {
            luaEnv.Tick();
            if (luaUpdate != null)
            {
                try
                {
                    luaUpdate(Time.deltaTime, Time.unscaledDeltaTime);
                }
                catch (Exception ex)
                {
                    Debug.LogError("luaUpdate err : " + ex.Message + "\n" + ex.StackTrace);
                }
            }
            // if (Time.frameCount % 6000 == 0)
            // {
            //     luaEnv.FullGc();
            // }
        }
    }

    private void LateUpdate() {
        if (luaLateUpdate != null)
        {
            try
            {
                luaLateUpdate();
            }
            catch (Exception ex)
            {
                Debug.LogError("luaLateUpdate err : " + ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
    
    private void FixedUpdate() {
        if (luaFixedUpdate != null)
        {
            try
            {
                luaFixedUpdate(Time.fixedDeltaTime);
            }
            catch (Exception ex)
            {
                Debug.LogError("luaFixedUpdate err : " + ex.Message + "\n" + ex.StackTrace);
            }
        }
    }

    private void OnDestroy()
    {
        luaUpdate = null;
        luaLateUpdate = null;
        luaFixedUpdate = null;
        if (luaEnv != null)
        {
            try
            {
                luaEnv.Dispose();
                luaEnv = null;
            }
            catch (System.Exception ex)
            {
                string msg = string.Format("xLua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                Debug.LogError(msg, null);
            }
        }
    }
    
#if UNITY_EDITOR
public static class LuaUpdaterExporter
{
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>()
    {
        typeof(Action),
        typeof(Action<float>),
        typeof(Action<float, float>),
    };
}
#endif

    
}
