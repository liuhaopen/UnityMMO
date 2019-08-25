using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using XLua;

namespace XLuaFramework {
    [LuaCallCSharp]
    public class AppConfig {
        //调试模式-开启后直接加载本地资源,不需要打包,如果是手机平台的话将自动设为false
        public static bool DebugMode = true;                       
        /// <summary>
        /// 如果开启更新模式，前提必须启动框架自带服务器端。
        /// 否则就需要自己将StreamingAssets里面的所有内容
        /// 复制到自己的Webserver上面，并修改下面的WebUrl。
        /// 更新模式-默认关闭,如果是手机平台的话将自动设为true
        /// </summary>
        public static bool UpdateMode = false;                       
         //Lua字节码模式-默认关闭 
        public const bool LuaByteMode = false;                      
        //Lua代码AssetBundle模式
        public static bool LuaBundleMode = false;                    
        //把sproto协议文件编译成二进制码
        public static bool SprotoBinMode = false;                    
        //应用程序名称
        public const string AppName = "UnityMMO";        
        //临时目录       
        public const string LuaTempDir = "LuaTemp/";    
        //应用程序前缀
        public const string AppPrefix = AppName + "_";          
        //Asset Bundle的目录 
        public const string AssetDir = "StreamingAssets";          

        public static void Init()
        {
            //从配置文件里读取
            if (Application.isMobilePlatform)
            {
                DebugMode = false;
                UpdateMode = true;
            }
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                DebugMode = false;
                UpdateMode = false;
            }
        }                

        
        /// <summary>
        /// 取得数据存放目录
        /// </summary>
        private static string data_path = null;
        public static string DataPath {
            get {
                string game = AppConfig.AppName.ToLower();
                if (Application.isMobilePlatform) {
                    if (data_path == null)
                        data_path = Application.persistentDataPath + "/" + game + "/";
                }
                else if (AppConfig.DebugMode) {
                    if (data_path == null)
                        data_path = AppConfig.AppDataPath + "/" + AppConfig.AssetDir + "/";
                }
                else if (Application.platform == RuntimePlatform.OSXEditor) {
                    if (data_path == null)
                    {
                        int i = Application.dataPath.LastIndexOf('/');
                        data_path = Application.dataPath.Substring(0, i + 1) + game + "/";
                    }
                }
                else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform ==   RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WebGLPlayer) {
                    data_path = AppConfig.AppDataPath+"/"+AppConfig.AssetDir+"/";
                }
                else
                    if (data_path == null)
                        data_path = "c:/" + game + "/";
                return data_path;
            }
        }
        private static string relative_Path = null;
        public static string GetRelativePath() {
            if (!AppConfig.UpdateMode && !AppConfig.DebugMode)
            {
                if (relative_Path == null)
                {
                    if (Application.platform ==   RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WebGLPlayer)
                        relative_Path = DataPath;
                    else
                        relative_Path = "jar:file://" + System.Environment.CurrentDirectory.Replace("\\", "/") + "/" + AppConfig.AssetDir + "/";
                }
                return relative_Path;
            }
            else
            {
                if (relative_Path == null)
                {
                    relative_Path = "file:///" + DataPath.Replace("\\", "/");
                }
                return relative_Path;
            }
        }

        static string luaAssetsDir = string.Empty;
        public static string LuaAssetsDir
        {
            get
            {
                if (luaAssetsDir != string.Empty)
                    return luaAssetsDir;

                if (DebugMode)
                {
                    luaAssetsDir = AppDataPath + "/Lua/";
                }
                else
                {
                    luaAssetsDir = AppConfig.DataPath + "/lua/";
                }
                return luaAssetsDir;
            }
        }

        public static string FrameworkRoot {
            get {
                return Application.dataPath + "/XLuaFramework";
            }
        }

        //打包后的资源路径,注意不是运行时使用的!
        private static string streamingAssetsTargetPath = string.Empty;
        public static string StreamingAssetsTargetPath
        {
            get
            {
                if (streamingAssetsTargetPath != string.Empty)
                    return streamingAssetsTargetPath;
                streamingAssetsTargetPath = GetStreamingAssetsTargetPathByPlatform(Application.platform);
                if (streamingAssetsTargetPath == string.Empty)
                    Debug.Log("Unspport System!");
                return streamingAssetsTargetPath;
            }
        }
        
        public static string GetStreamingAssetsTargetPathByPlatform(RuntimePlatform platform)
        {
            string dataPath = Application.dataPath.Replace("/Assets", "");
            if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.WebGLPlayer)
                return dataPath + "/" + AppConfig.AssetDir;
            else if (platform == RuntimePlatform.Android)
                return dataPath + "/StreamingAssetsAndroid/" + AppConfig.AssetDir;
            else if (platform == RuntimePlatform.IPhonePlayer || platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.OSXPlayer)
                return dataPath + "/StreamingAssetsIOS/" + AppConfig.AssetDir;
            else
                Debug.Log("Unspport System!");

            return string.Empty;
        }

        //热更新用的文件服务器地址,可以自己用IIS或Apache搭建
        private static string webUrl = string.Empty;
        public static string WebUrl
        {
            get
            {
                if (webUrl != string.Empty)
                    return webUrl;
                var fileUrl = UnityMMO.ConfigGame.GetInstance().Data.FileServerURL;
                Debug.Log("fileUrl : "+fileUrl);
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WebGLPlayer)
                    webUrl = fileUrl+"/WindowsStreamingAssets/";
                else if (Application.platform == RuntimePlatform.Android)
                    webUrl = fileUrl+"/AndroidStreamingAssets/";
                else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
                    webUrl = fileUrl+"/IOSStreamingAssets/";
                else
                    Debug.Log("Unspport System!");
                return webUrl;
            }
        }

        public static string AppDataPath
        {
            get
            {
                string dataPath = Application.dataPath;
                bool isWindows = Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
                if (AppConfig.DebugMode || isWindows)
                    dataPath = dataPath.Replace("/Assets", "");
                return isWindows ? dataPath.Replace(AppName+"/App/PC/"+AppName+"_Data", AppName) : dataPath;
            }
        }
    }
}