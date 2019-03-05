#if UNITY_EDITOR
using UnityEditor;

//路径保存器，记录上次打开的路径，不同项目的不同用处路径都分开保存
namespace U3DExtends
{
    public enum PathType {
        //OpenLayout,//打开布局时默认打开的文件夹路径//和Save用同个会方便点
        SaveLayout,//保存布局时默认打开的文件夹路径
        OpenDecorate,//选择参考图时默认打开的文件夹路径
        PrefabTool,//Prefab界面用的
    }
    public class PathSaver {
        private volatile static PathSaver _instance = null;
        private static readonly object lockHelper = new object();
        private PathSaver() { }
        public static PathSaver GetInstance()
        {
            if(_instance == null)
            {
                lock(lockHelper)
                {
                    if(_instance == null)
                        _instance = new PathSaver();
                }
            }
            return _instance;
        }

        public string GeDefaultPath(PathType type)
        {
            return "";
        }

        public string GetLastPath(PathType type)
        {
            return EditorPrefs.GetString("PathSaver_" + U3DExtends.Configure.ProjectUUID + "_" + type.ToString(), GeDefaultPath(type));
        }

        public void SetLastPath(PathType type, string path)
        {
            if (path == "")
                return;
            path = System.IO.Path.GetDirectoryName(path);
            EditorPrefs.SetString("PathSaver_" + U3DExtends.Configure.ProjectUUID + "_" + type.ToString(), path);
        }
	  
    }
}
#endif