using System.Collections.Generic;
using System.IO;
// using AssetBundleBrowser.AssetBundleDataSource;
using UnityEditor;
using UnityEngine;

namespace XLuaFramework
{
    public class MD5ListsFileCreator// : AssetBundleBrowser.AssetBundleDataSource.ABDataSource
    {
        // [MenuItem("XLuaFramework/Build Windows Resource", false, 101)]
        // public static void BuildWindowsResource() 
        // {
        //     BuildAssetResource(BuildTarget.StandaloneWindows);
        // }

        // public static void BuildAssetResource(BuildTarget target) 
        // {
        //     string streamPath = AppConfig.GetStreamingAssetsTargetPathByPlatform(BuildTargetToPlatform(target));
        //     if (Directory.Exists(streamPath))
        //     {
        //         Directory.Delete(streamPath, true);
        //     }

        //     if (Directory.Exists(streamPath))
        //     {
        //         Directory.Delete(streamPath, true);
        //     }
        //     Directory.CreateDirectory(streamPath);
        //     AssetDatabase.Refresh();

        //     BuildFileIndex(streamPath);

        // }

        // static void BuildFileIndex(string toPath) {
        //     string resPath = toPath;
        //     ///----------------------创建文件列表-----------------------
        //     string newFilePath = resPath + "/files.txt";
        //     if (File.Exists(newFilePath)) File.Delete(newFilePath);

        //     paths.Clear(); files.Clear();
        //     Recursive(resPath);

        //     FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        //     StreamWriter sw = new StreamWriter(fs);
        //     for (int i = 0; i < files.Count; i++) {
        //         string file = files[i];
        //         string ext = Path.GetExtension(file);
        //         if (file.EndsWith(".meta") || file.Contains(".DS_Store")) continue;

        //         string md5 = Util.md5file(file);
        //         string value = file.Replace(resPath, string.Empty);
        //         sw.WriteLine(value + "|" + md5);
        //     }
        //     sw.Close(); fs.Close();
        // }

        // /// <summary>
        // /// 遍历目录及其子目录
        // /// </summary>
        // static void Recursive(string path) {
        //     string[] names = Directory.GetFiles(path);
        //     string[] dirs = Directory.GetDirectories(path);
        //     foreach (string filename in names) {
        //         string ext = Path.GetExtension(filename);
        //         if (ext.Equals(".meta")) continue;
        //         files.Add(filename.Replace('\\', '/'));
        //     }
        //     foreach (string dir in dirs) {
        //         paths.Add(dir.Replace('\\', '/'));
        //         Recursive(dir);
        //     }
        // }

        // public static RuntimePlatform BuildTargetToPlatform(BuildTarget target)
        // {
        //     if (target == BuildTarget.StandaloneWindows)
        //         return RuntimePlatform.WindowsEditor;
        //     else if (target == BuildTarget.Android)
        //         return RuntimePlatform.Android;
        //     else if (target == BuildTarget.iOS)
        //         return RuntimePlatform.IPhonePlayer;
        //     else
        //         return RuntimePlatform.WindowsEditor;
        // }

//extend to AssetBundle-Browser some day
#if false
        public static List<AssetBundleBrowser.AssetBundleDataSource.ABDataSource> CreateDataSources()
        {
            var op = new MD5ListsFileCreator();
            var retList = new List<AssetBundleBrowser.AssetBundleDataSource.ABDataSource>();
            // retList.Add(op);
            return retList;
        }

        public string Name 
        {
            get {return "MD5ListsFileCreator";}
        }

        public string ProviderName
        {
            get {return "Cat";}
        }

        public bool CanSpecifyBuildTarget 
        {
            get {return false;}
        }

        public bool CanSpecifyBuildOutputDirectory
        {
            get {return false;}
        }

        public bool CanSpecifyBuildOptions
        {
            get {return false;}
        }
        MD5ListsFileCreator()
        {
            
        }
        public bool BuildAssetBundles(ABBuildInfo info)
        {
            Debug.Log("hahhshiehf");
            // throw new System.NotImplementedException();
            return true;
        }

        public string[] GetAllAssetBundleNames()
        {
            return new string[]{};
        }

        public string GetAssetBundleName(string assetPath)
        {
            return "";
        }

        public string[] GetAssetPathsFromAssetBundle(string assetBundleName)
        {
            return new string[]{};
        }

        public string GetImplicitAssetBundleName(string assetPath)
        {
            return "";
        }

        public bool IsReadOnly()
        {
            return true;
        }

        public void RemoveUnusedAssetBundleNames()
        {
        }

        public void SetAssetBundleNameAndVariant(string assetPath, string bundleName, string variantName)
        {
        }
#endif
    }

}