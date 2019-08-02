using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using XLuaFramework;

public class Packager {
    public static string platform = string.Empty;
    static List<string> paths = new List<string>();
    static List<string> files = new List<string>();
    static List<AssetBundleBuild> maps = new List<AssetBundleBuild>();
    
    const string ApachePathForAndroid = "E:/Apache24/htdocs/AndroidStreamingAssets";

    static string[] exts = { ".txt", ".xml", ".lua", ".assetbundle", ".json" };
    static bool CanCopy(string ext) {   //能不能复制
        foreach (string e in exts) {
            if (ext.Equals(e)) return true;
        }
        return false;
    }

    [MenuItem("LuaFramework/Build iPhone Resource", false, 100)]
    public static void BuildiPhoneResource() {
        BuildTarget target;
#if UNITY_5
        target = BuildTarget.iOS;
#else
        target = BuildTarget.iOS;
#endif
        BuildAssetResource(target);
    }

    [MenuItem("LuaFramework/Build Android Resource", false, 101)]
    public static void BuildAndroidResource() {
        BuildAssetResource(BuildTarget.Android);
    }

    [MenuItem("LuaFramework/Build Android Resource And Copy", false, 102)]
    public static void BuildAndroidResourceAndCopy()
    {
        BuildAndroidResource();
        CopyToServerFolder(AppConfig.GetStreamingAssetsTargetPathByPlatform(RuntimePlatform.Android), ApachePathForAndroid);
        UnityEngine.Debug.Log("Copy Succeed!");
    }

    public static void CopyFolder(string strFromPath, string strToPath, bool isCopySelf=true)
    {
        strFromPath = strFromPath.Replace('\\', '/');
        strToPath = strToPath.Replace('\\', '/');
        //如果源文件夹不存在，则创建
        if (!Directory.Exists(strFromPath))
        {
            Directory.CreateDirectory(strFromPath);
        }
        //取得要拷贝的文件夹名
        string strFolderName = string.Empty;
        if (isCopySelf)
        {
            strFolderName = strFromPath.Substring(strFromPath.LastIndexOf("/") + 1, strFromPath.Length - strFromPath.LastIndexOf("/") - 1);
            //如果目标文件夹中没有源文件夹则在目标文件夹中创建源文件夹
            if (!Directory.Exists(strToPath + "/" + strFolderName))
            {
                Directory.CreateDirectory(strToPath + "/" + strFolderName);
            }
        }
        else
        {
            if (!Directory.Exists(strToPath))
            {
                Directory.CreateDirectory(strToPath);
            }
        }
        //创建数组保存源文件夹下的文件名
        string[] strFiles = Directory.GetFiles(strFromPath);
        //循环拷贝文件
        for (int i = 0; i < strFiles.Length; i++)
        {
            //取得拷贝的文件名，只取文件名，地址截掉。
            string strFileName = Path.GetFileName(strFiles[i]);
            //开始拷贝文件,true表示覆盖同名文件
            if (isCopySelf)
                File.Copy(strFiles[i], strToPath + "/" + strFolderName + "/" + strFileName, true);
            else
                File.Copy(strFiles[i], strToPath + "/" + strFileName, true);

        }
        //创建DirectoryInfo实例
        DirectoryInfo dirInfo = new DirectoryInfo(strFromPath);
        //取得源文件夹下的所有子文件夹名称
        DirectoryInfo[] ZiPath = dirInfo.GetDirectories();
        for (int j = 0; j < ZiPath.Length; j++)
        {
            //把得到的子文件夹当成新的源文件夹，从头开始新一轮的拷贝
            CopyFolder(ZiPath[j].FullName, strToPath + "/" + ZiPath[j].Name, false);
        }
    }

    private static void CopyToServerFolder(string fromPath, string toPath)
    {
        if (Directory.Exists(toPath)) 
            Directory.Delete(toPath, true);
        CopyFolder(fromPath, toPath, false);
    }

    [MenuItem("LuaFramework/Build Windows Resource", false, 103)]
    public static void BuildWindowsResource() {
        BuildAssetResource(BuildTarget.StandaloneWindows64);
    }

    public static RuntimePlatform BuildTargetToPlatform(BuildTarget target)
    {
        if (target == BuildTarget.StandaloneWindows64 || target == BuildTarget.StandaloneWindows)
            return RuntimePlatform.WindowsEditor;
        else if (target == BuildTarget.Android)
            return RuntimePlatform.Android;
        else if (target == BuildTarget.iOS)
            return RuntimePlatform.IPhonePlayer;
        else
            return RuntimePlatform.WindowsEditor;
    }

    /// <summary>
    /// 生成绑定素材
    /// </summary>
    public static void BuildAssetResource(BuildTarget target) {
        string streamPath = AppConfig.GetStreamingAssetsTargetPathByPlatform(BuildTargetToPlatform(target));
        if (Directory.Exists(streamPath))
        {
            Directory.Delete(streamPath, true);
        }

        if (Directory.Exists(streamPath))
        {
            Directory.Delete(streamPath, true);
        }
        Directory.CreateDirectory(streamPath);
        AssetDatabase.Refresh();

        maps.Clear();

        //把Lua文件先存放在一个临时文件夹,然后针对此文件夹打包
        string dataPath = Application.dataPath.Replace("/Assets", "");
        string tempLuaDir = dataPath + "/" + AppConfig.LuaTempDir;
        if (AppConfig.LuaBundleMode)
            HandleLuaBundle(tempLuaDir);
        else
            HandleLuaFile(streamPath);
        if (AppConfig.SprotoBinMode)
            HandleSprotoBundle(streamPath);


        HandleSceneBundles();
        HandleNormalBundles("effect");
        HandleNormalBundles("sound");
        HandleNormalBundles("role");
        HandleNormalBundles("npc");
        HandleNormalBundles("monster");
        HandleUIBundles();

        BuildPipeline.BuildAssetBundles(streamPath, maps.ToArray(), BuildAssetBundleOptions.None, target);

        // BuildSceneBundles("Assets/AssetBundleRes/scene/navmesh", streamPath, target);
        BuildSceneBundles("Assets/AssetBundleRes/scene/base_world", streamPath, target);

        BuildFileIndex(streamPath);

        if (Directory.Exists(tempLuaDir)) Directory.Delete(tempLuaDir, true);
        AssetDatabase.Refresh();
    }

    static void AddBuildMap(string bundleName, string pattern, string path) {
        string[] files = Directory.GetFiles(path, pattern);
        if (files.Length == 0) return;

        for (int i = 0; i < files.Length; i++) {
            files[i] = files[i].Replace('\\', '/');
        }
        AssetBundleBuild build = new AssetBundleBuild();
        build.assetBundleName = bundleName;
        build.assetNames = files;
        maps.Add(build);
    }

    /// <summary>
    /// 处理Lua代码包
    /// </summary>
    static void HandleLuaBundle(string tempLuaDir) {
        //string tempLuaDir = AppConfig.AppDataPath.ToLower() + "/"+AppConfig.LuaTempDir+"/";
        if (!Directory.Exists(tempLuaDir)) Directory.CreateDirectory(tempLuaDir);

        string[] srcDirs = { AppConfig.LuaAssetsDir };
        for (int i = 0; i < srcDirs.Length; i++) 
        {
            if (AppConfig.LuaByteMode) 
            {
                string sourceDir = srcDirs[i];
                string[] files = Directory.GetFiles(sourceDir, "*.lua", SearchOption.AllDirectories);
                int len = sourceDir.Length;

                if (sourceDir[len - 1] == '/' || sourceDir[len - 1] == '\\') 
                {
                    --len;
                }
                for (int j = 0; j < files.Length; j++) 
                {
                    string str = files[j].Remove(0, len);
                    string dest = tempLuaDir + str + ".bytes";
                    string dir = Path.GetDirectoryName(dest);
                    Directory.CreateDirectory(dir);
                    EncodeLuaFile(files[j], dest);
                }    
            } 
            else 
            {
                CopyLuaBytesFiles(srcDirs[i], tempLuaDir);
            }
        }
        string[] dirs = Directory.GetDirectories(tempLuaDir, "*", SearchOption.AllDirectories);
        for (int i = 0; i < dirs.Length; i++) {
            string name = dirs[i].Replace(tempLuaDir, string.Empty);
            name = name.Replace('\\', '_').Replace('/', '_');
            name = "lua/lua_" + name.ToLower();

            string path = "Assets" + dirs[i].Replace(Application.dataPath, "");
            AddBuildMap(name, "*.bytes", path);
        }
        AddBuildMap("lua/lua", "*.bytes", AppConfig.LuaTempDir);

        //-------------------------------处理非Lua文件----------------------------------
        string luaPath = AppConfig.AppDataPath.ToLower() + "/StreamingAssets/lua/";
        for (int i = 0; i < srcDirs.Length; i++) {
            paths.Clear(); files.Clear();
            string luaDataPath = srcDirs[i].ToLower();
            Recursive(luaDataPath);
            foreach (string f in files) {
                if (f.EndsWith(".meta") || f.EndsWith(".lua")) continue;
                string newfile = f.Replace(luaDataPath, "");
                string path = Path.GetDirectoryName(luaPath + newfile);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                string destfile = path + "/" + Path.GetFileName(f);
                File.Copy(f, destfile, true);
            }
        }
        AssetDatabase.Refresh();
    }
    public static void CopyLuaBytesFiles(string sourceDir, string destDir, bool appendext = true, string searchPattern = "*.lua", SearchOption option = SearchOption.AllDirectories)
    {
        if (!Directory.Exists(sourceDir))
        {
            return;
        }

        string[] files = Directory.GetFiles(sourceDir, searchPattern, option);
        int len = sourceDir.Length;

        if (sourceDir[len - 1] == '/' || sourceDir[len - 1] == '\\')
        {
            --len;
        }         

        for (int i = 0; i < files.Length; i++)
        {
            string str = files[i].Remove(0, len);
            string dest = destDir + "/" + str;
            if (appendext) dest += ".bytes";
            string dir = Path.GetDirectoryName(dest);
            Directory.CreateDirectory(dir);
            File.Copy(files[i], dest, true);
        }
    }

    public static void BuildSceneBundles(string path, string streamPath, BuildTarget target)
    {
        // string path = "Assets/AssetBundleRes/scene/navmesh";
        string[] file_paths = Directory.GetFiles(path);
        // List<string> levels = new List<string>();
        foreach(string file_path in file_paths)
        {
            string ext = Path.GetExtension(file_path);
            if (ext.Equals(".unity"))
            {
                string navmesh_scene_name = Path.GetFileNameWithoutExtension(file_path);
                // levels.Add(file_path);
                string[] levels = new string[]{file_path};
                Debug.Log("file : "+file_path+" save : "+streamPath+"/"+navmesh_scene_name+" target:"+target.ToString());

                BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
                buildPlayerOptions.scenes = new string[]{file_path};
                buildPlayerOptions.locationPathName = streamPath+"/"+navmesh_scene_name;
                buildPlayerOptions.target = target;
                buildPlayerOptions.options = BuildOptions.BuildAdditionalStreamedScenes;

                UnityEditor.Build.Reporting.BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
                UnityEditor.Build.Reporting.BuildSummary summary = report.summary;
                if (summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
                {
                    Debug.Log("Build scene succeeded: " + summary.totalSize + " bytes");
                }

                if (summary.result == UnityEditor.Build.Reporting.BuildResult.Failed)
                {
                    Debug.Log("Build scene failed : "+file_path);
                }
                // BuildPipeline.BuildPlayer(levels, streamPath+"/"+navmesh_scene_name, target, BuildOptions.BuildAdditionalStreamedScenes);
            }
        }
    }

    public static void HandleSceneBundles()
    {
        string path = "Assets/AssetBundleRes/scene/";
        string[] dirs = Directory.GetDirectories(path);
        if (dirs.Length == 0)
            return;
        for (int i = 0; i < dirs.Length; i++)
        {
            string folder_name = Path.GetFileName(dirs[i]);
            if (folder_name == "base_world")
                continue;
            string asset_name = "scene_" + folder_name;
            List<string> file_list = new List<string>();//文件列表
            paths.Clear(); files.Clear(); Recursive(dirs[i], false);
            UnityEngine.Debug.Log("scene asset_name : "+asset_name+" filenum:"+files.Count.ToString());
            // foreach (string f in files)
            // {
            //     string name = Path.GetFileName(f);
            //     string ext = Path.GetExtension(f);
            //     file_list.Add(f);
            //     file_list.Add(f + ".meta");
            // }
            if (files.Count > 0)
            {
                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = asset_name;
                //DeleteUICache(dataPath, build.assetBundleName);
                build.assetNames = files.ToArray();
                maps.Add(build);
            }
        }
    }

    public static void HandleRoleBundles()
    {
        string path = "Assets/AssetBundleRes/role/";
        string[] dirs = Directory.GetDirectories(path);
        if (dirs.Length == 0)
            return;
        for (int i = 0; i < dirs.Length; i++)
        {
            string asset_name = "role_" + Path.GetFileName(dirs[i]);
            List<string> file_list = new List<string>();//文件列表
            paths.Clear(); files.Clear(); Recursive(dirs[i], false);
            UnityEngine.Debug.Log("role asset_name : "+asset_name+" filenum:"+files.Count.ToString());
            if (files.Count > 0)
            {
                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = asset_name;
                build.assetNames = files.ToArray();
                maps.Add(build);
            }
        }
    }

    public static void HandleNormalBundles(string prefix)
    {
        string path = "Assets/AssetBundleRes/"+prefix+"/";
        string[] dirs = Directory.GetDirectories(path);
        if (dirs.Length == 0)
            return;
        for (int i = 0; i < dirs.Length; i++)
        {
            string asset_name = prefix+"_" + Path.GetFileName(dirs[i]);
            List<string> file_list = new List<string>();//文件列表
            paths.Clear(); files.Clear(); Recursive(dirs[i], false);
            UnityEngine.Debug.Log(prefix+" asset_name : "+asset_name+" filenum:"+files.Count.ToString());
            if (files.Count > 0)
            {
                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = asset_name;
                build.assetNames = files.ToArray();
                maps.Add(build);
            }
        }
    }

    public static void HandleUIBundles()
    {
        string path = "Assets/AssetBundleRes/ui/";
        string[] dirs = Directory.GetDirectories(path);
        Debug.Log("dirs.Length : "+dirs.Length.ToString());
        if (dirs.Length == 0)
            return;
        for (int i = 0; i < dirs.Length; i++)
        {
            string asset_name = "ui_" + Path.GetFileName(dirs[i]);
            List<string> file_list = new List<string>();//文件列表
            paths.Clear(); files.Clear(); Recursive(dirs[i], false);
            UnityEngine.Debug.Log("ui asset_name : "+asset_name+" filenum:"+files.Count.ToString());
            if (files.Count > 0)
            {
                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = asset_name;
                build.assetNames = files.ToArray();
                maps.Add(build);
            }
        }
    }

    // public static void HandleUIBundles(string dataPath="")
    // {
        // string ui_path = "Assets/AssetBundleRes/ui/";
        // string ui_prefab_path = ui_path + "prefab";
        // string ui_texture_path = ui_path + "texutre";
        // string[] ui_dirs = Directory.GetDirectories(ui_prefab_path);
        // if (ui_dirs.Length == 0)
        //     return;
        // for (int i = 0; i < ui_dirs.Length; i++)
        // {
        //     string asset_name = "ui_" + Path.GetFileName(ui_dirs[i]);

        //     List<string> prefab_list = new List<string>();//预制文件列表
        //     paths.Clear(); files.Clear(); Recursive(ui_dirs[i]);
        //     foreach (string f in files)
        //     {
        //         string name = Path.GetFileName(f);
        //         string ext = Path.GetExtension(f);
        //         if (ext.Equals(".prefab"))
        //         {
        //             prefab_list.Add(f);
        //             prefab_list.Add(f + ".meta");
        //         }
        //     }
        //     if (prefab_list.Count > 0)
        //     {
        //         AssetBundleBuild build = new AssetBundleBuild();
        //         build.assetBundleName = asset_name;
        //         //DeleteUICache(dataPath, build.assetBundleName);
        //         build.assetNames = prefab_list.ToArray();
        //         maps.Add(build);
        //         //string temp = asset_name.ToLower();
        //         //assets_list.Add(build.assetBundleName);
        //     }
        // }
        // ui_dirs = Directory.GetDirectories(ui_texture_path);
        // if (ui_dirs.Length == 0)
        //     return;
        // for (int i = 0; i < ui_dirs.Length; i++)
        // {
        //     string asset_name = "ui_" + Path.GetFileName(ui_dirs[i]);

        //     List<string> asset_list = new List<string>();//资源文件列表
        //     paths.Clear(); files.Clear(); Recursive(ui_dirs[i]);
        //     foreach (string f in files)
        //     {
        //         string name = Path.GetFileName(f);
        //         string ext = Path.GetExtension(f);
        //         if (ext.Equals(".png"))
        //         {
        //             asset_list.Add(f);
        //             asset_list.Add(f + ".meta");
        //         }
        //     }
        //     if (asset_list.Count > 0)
        //     {
        //         AssetBundleBuild build = new AssetBundleBuild();
        //         build.assetBundleName = asset_name;
        //         //DeleteUICache(dataPath, build.assetBundleName);
        //         build.assetNames = asset_list.ToArray();
        //         maps.Add(build);
        //         //string temp = asset_name.ToLower();
        //         //assets_list.Add(build.assetBundleName);
        //     }
        // }
    // }

    //[MenuItem("Test/Build Sproto BinFile")]
    public static void TestHandleSprotoBundle()
    {
        string streamPath = AppConfig.StreamingAssetsTargetPath;
        HandleSprotoBundle(streamPath);
    }

    public static void HandleSprotoBundle(string streamPath)
    {
        string tool_path = Application.dataPath.Replace("/Assets", "") + "/Tools/sprotodumper/";
        string names = Util.GetFileNamesInFolder(AppConfig.LuaAssetsDir + "/Common/Proto", " ");

        System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo.FileName = "cmd.exe";
        p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
        p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
        p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
        p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
        p.StartInfo.CreateNoWindow = true;//不显示程序窗口
        p.Start();//启动程序
        //向cmd窗口发送输入信息
        p.StandardInput.WriteLine("cd \\");
        p.StandardInput.WriteLine("cd I:");
        p.StandardInput.WriteLine(@"cd "+ tool_path);
        p.StandardInput.WriteLine("lua sprotodumper.lua " + names+ "&exit");
        
        p.StandardInput.AutoFlush = true;
        //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
        //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令
        //获取cmd窗口的输出信息
        string output = p.StandardOutput.ReadToEnd();
        p.WaitForExit();//等待程序执行完退出进程
        p.Close();

        if (output.Contains("succeed!"))
        {
            UnityEngine.Debug.Log("generate sproto spb succeed!");
            File.Copy(tool_path + "/sproto_c2s.spb", streamPath + "/sproto_c2s.spb");
            File.Copy(tool_path + "/sproto_s2c.spb", streamPath + "/sproto_s2c.spb");
        }
        else
        {
            UnityEngine.Debug.Log(output);
            UnityEngine.Debug.Log("generate sproto spb failed! please check up line for detail");
        }
    }

    /// <summary>
    /// 处理Lua文件
    /// </summary>
    static void HandleLuaFile(string toPath)
    {
        string resPath = toPath;
        string luaPath = resPath + "/lua/";

        //----------复制Lua文件----------------
        if (!Directory.Exists(luaPath)) {
            Directory.CreateDirectory(luaPath); 
        }
        string rootPath = Application.dataPath.Replace("/Assets", "");
        string[] luaPaths = { rootPath + "/Lua/",};

        for (int i = 0; i < luaPaths.Length; i++) {
            paths.Clear();
            files.Clear();
            string luaDataPath = luaPaths[i].ToLower();
            Recursive(luaDataPath);
            int n = 0;
            foreach (string f in files) {
                if (f.EndsWith(".meta")) continue;
                string newfile = f.Replace(luaDataPath, "");
                string newpath = luaPath + newfile;
                string path = Path.GetDirectoryName(newpath);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                if (File.Exists(newpath)) {
                    File.Delete(newpath);
                }
                if (AppConfig.LuaByteMode) {
                    EncodeLuaFile(f, newpath);
                } else {
                    File.Copy(f, newpath, true);
                }
                UpdateProgress(n++, files.Count, newpath);
            } 
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    static void BuildFileIndex(string toPath) {
        string resPath = toPath;
        ///----------------------创建文件列表-----------------------
        string newFilePath = resPath + "/files.txt";
        if (File.Exists(newFilePath)) File.Delete(newFilePath);

        paths.Clear(); files.Clear();
        Recursive(resPath);

        FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        for (int i = 0; i < files.Count; i++) {
            string file = files[i];
            string ext = Path.GetExtension(file);
            if (file.EndsWith(".meta") || file.Contains(".DS_Store")) continue;

            string md5 = Util.md5file(file);
            string value = file.Replace(resPath, string.Empty);
            sw.WriteLine(value + "|" + md5);
        }
        sw.Close(); fs.Close();
    }

    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    static void Recursive(string path, bool ignore_meta=true) {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names) {
            string ext = Path.GetExtension(filename);
            if (ignore_meta && ext.Equals(".meta")) continue;
            files.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs) {
            paths.Add(dir.Replace('\\', '/'));
            Recursive(dir, ignore_meta);
        }
    }

    static void UpdateProgress(int progress, int progressMax, string desc) {
        string title = "Processing...[" + progress + " - " + progressMax + "]";
        float value = (float)progress / (float)progressMax;
        EditorUtility.DisplayProgressBar(title, desc, value);
    }

    public static void EncodeLuaFile(string srcFile, string outFile) {
        if (!srcFile.ToLower().EndsWith(".lua")) {
            File.Copy(srcFile, outFile, true);
            return;
        }
        bool isWin = true; 
        string luaexe = string.Empty;
        string args = string.Empty;
        string exedir = string.Empty;
        string currDir = Directory.GetCurrentDirectory();
        string dataPath = Application.dataPath.Replace("/Assets", "");
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            isWin = true;
            luaexe = "luajit.exe";
            args = "-b -g " + srcFile + " " + outFile;
            exedir = dataPath + "Tools/LuaEncoder/luajit/";
        } else if (Application.platform == RuntimePlatform.OSXEditor) {
            isWin = false;
            luaexe = "./luajit";
            args = "-b -g " + srcFile + " " + outFile;
            exedir = dataPath + "Tools/LuaEncoder/luajit_mac/";
        }
        Directory.SetCurrentDirectory(exedir);
        System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
        info.FileName = luaexe;
        info.Arguments = args;
        info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        info.UseShellExecute = isWin;
        info.ErrorDialog = true;
        Debug.Log(info.FileName + " " + info.Arguments);

        System.Diagnostics.Process pro = System.Diagnostics.Process.Start(info);
        pro.WaitForExit();
        Directory.SetCurrentDirectory(currDir);
    }

}