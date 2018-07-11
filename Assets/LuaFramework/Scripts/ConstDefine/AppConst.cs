using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LuaFramework {
    public class AppConst {
        public static bool DebugMode = true;                       //调试模式-用于内部测试,如果是手机平台的话将自动设为false
        /// <summary>
        /// 如果开启更新模式，前提必须启动框架自带服务器端。
        /// 否则就需要自己将StreamingAssets里面的所有内容
        /// 复制到自己的Webserver上面，并修改下面的WebUrl。
        /// </summary>
        public static bool UpdateMode = false;                       //更新模式-默认关闭,如果是手机平台的话将自动设为true
        public const bool LuaByteMode = false;                       //Lua字节码模式-默认关闭 
        public static bool LuaBundleMode = false;                    //Lua代码AssetBundle模式
        public static bool SprotoBinMode = false;                    //把sproto协议文件编译成二进制码

        public const int TimerInterval = 1;
        public const int GameFrameRate = 60;                        //游戏帧频

        public const string AppName = "LuaFramework";               //应用程序名称
        public const string LuaTempDir = "LuaTemp/";                    //临时目录
        public const string AppPrefix = AppName + "_";              //应用程序前缀
        public const string ExtName = ".unity3d";                   //素材扩展名
        public const string AssetDir = "StreamingAssets";           //素材目录 
        public static string UserId = string.Empty;                 //用户ID
        public static int SocketPort = 8888;                           //Socket服务器端口
        public static string SocketAddress = "192.168.5.142";          //Socket服务器地址
        public static bool IsUseLocalResource = true;              //是否使用本地非打包的资源

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
                    luaAssetsDir = Util.DataPath + "/lua/";
                }
                return luaAssetsDir;
            }
        }

        public static string FrameworkRoot {
            get {
                return Application.dataPath + "/LuaFramework";
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
                return dataPath + "/" + AppConst.AssetDir;
            else if (platform == RuntimePlatform.Android)
                return dataPath + "/StreamingAssetsAndroid/" + AppConst.AssetDir;
            else if (platform == RuntimePlatform.IPhonePlayer || platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.OSXPlayer)
                return dataPath + "/StreamingAssetsIOS/" + AppConst.AssetDir;
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
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WebGLPlayer)
                    webUrl = "http://192.168.5.123/WindowsStreamingAssets/";
                else if (Application.platform == RuntimePlatform.Android)
                    webUrl = "http://192.168.5.123/AndroidStreamingAssets/";
                else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
                    webUrl = "http://192.168.5.123/IOSStreamingAssets/";
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
                if (AppConst.DebugMode)
                    dataPath = dataPath.Replace("/Assets", "");

                return DebugMode ? dataPath.Replace("UnityMMO/App/PC/PC_Data", "UnityMMO") : dataPath;
            }
        }
    }
}