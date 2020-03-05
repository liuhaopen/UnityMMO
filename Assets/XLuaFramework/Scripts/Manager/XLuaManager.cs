using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;
using XLuaFramework;

[Hotfix]
[LuaCallCSharp]
public class XLuaManager : MonoBehaviour
{
    Action<float, float> luaUpdate = null;
    Action luaLateUpdate = null;
    Action<float> luaFixedUpdate = null;
    LuaEnv luaEnv = null;
    public static XLuaManager Instance = null;
    Action onLoginOk = null;

    protected void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void InitExternal()
    {
        luaEnv.AddBuildin("sproto.core", XLua.LuaDLL.Lua.LoadSproto);
        luaEnv.AddBuildin("crypt", XLua.LuaDLL.Lua.LoadCrypt);
        luaEnv.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);
    }

    public void InitLuaEnv()
    {
        // XLua.LuaEnv.AddIniter(XLua.CSObjectWrap.XLua_Gen_Manual_Register__.Init);
        luaEnv = new LuaEnv();
        if (luaEnv != null)
        {
            luaEnv.AddLoader(CustomLoader);

            InitExternal();
            LoadScript("BaseRequire");

            luaUpdate = luaEnv.Global.Get<Action<float, float>>("Update");
            luaLateUpdate = luaEnv.Global.Get<Action>("LateUpdate");
            luaFixedUpdate = luaEnv.Global.Get<Action<float>>("FixedUpdate");

            if (NetworkManager.GetInstance())
            {
                SafeDoString("require 'NetDispatcher'");
                NetworkManager.GetInstance().OnConnectCallBack += luaEnv.Global.Get<Action<byte[]>>("OnConnectServer");
                NetworkManager.GetInstance().OnDisConnectCallBack += luaEnv.Global.Get<Action<byte[]>>("OnDisConnectFromServer");
                NetworkManager.GetInstance().OnReceiveLineCallBack += luaEnv.Global.Get<Action<byte[]>>("OnReceiveLineFromServer");
                NetworkManager.GetInstance().OnReceiveMsgCallBack += luaEnv.Global.Get<Action<byte[]>>("OnReceiveMsgFromServer");
            }
            else
                Debug.LogError("must init network manager before init xlua manager!");
        }
        else
        {
            Debug.LogError("InitLuaEnv failed!");
        }
    }

    public void StartLogin(Action login_ok)
    {
        onLoginOk = login_ok;
        LoadScript("LuaMain");
        SafeDoString("LuaMain()");
    }

    //登录成功后，在lua端的LoginController会调用本方法的
    public void OnLoginOk()
    {
        Debug.Log("XLuaManager onLoginOk");
        onLoginOk();
        onLoginOk = null;
    }

    public LuaEnv GetLuaEnv()
    {
        return luaEnv;
    }

    public void SafeDoString(string scriptContent, string chunkName="chunk")
    {
        if (luaEnv != null)
        {
            try
            {
                luaEnv.DoString(scriptContent, chunkName);
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

    public void LoadOutsideFile(string file_path)
    {
        SafeDoString(File.ReadAllText(file_path));
    }

    static bool IsUseLuaFileWithoutBundle = false;//just for debug 
    public static byte[] CustomLoader(ref string filepath)
    {
        string scriptPath = string.Empty;
        filepath = filepath.Replace(".", "/") + ".lua";

        if (!IsUseLuaFileWithoutBundle)
        {
            scriptPath = Path.Combine(AppConfig.LuaAssetsDir, filepath);
            // Debug.Log("Load lua script : " + scriptPath);
            return Util.GetFileBytes(scriptPath);
        }
        else
        {
            string dataPath = Application.dataPath;
            dataPath = dataPath.Replace("/Assets", "");
            dataPath = dataPath.Replace(AppConfig.AppName+"/App/PC/"+AppConfig.AppName+"_Data", AppConfig.AppName);
            scriptPath = Path.Combine(dataPath + "/Lua/", filepath);
            // Debug.Log("Load lua script : " + scriptPath);
            return Util.GetFileBytes(scriptPath);
        }
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
        NetworkManager.GetInstance().OnConnectCallBack = null;
        NetworkManager.GetInstance().OnDisConnectCallBack = null;
        NetworkManager.GetInstance().OnReceiveLineCallBack = null;
        NetworkManager.GetInstance().OnReceiveMsgCallBack = null;
        CSLuaBridge.GetInstance().ClearDelegate();
        LoadScript("LuaMain");
        SafeDoString("ExitGame()");
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
        typeof(Action<byte[]>),
        typeof(Action<float>),
        typeof(Action<float, float>),
    };
}
#endif

    
}
