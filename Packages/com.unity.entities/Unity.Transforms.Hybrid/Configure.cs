using UnityEngine;

namespace U3DExtends
{
    //功能和快捷键的配置
    public static class Configure
    {
        //是否开启场景中的右键菜单
        public static bool IsShowSceneMenu = true;

        //选中图片节点再选图片时，即帮节点赋上该图
        public static bool IsEnableFastSelectImage = false;
        //选中图片节点再选图片时，帮节点赋上该图时自动设为原图大小
        public static bool IsAutoSizeOnFastSelectImg = false;

        //拉UI prefab或者图片入scene界面时帮它找到鼠标下的Canvas并挂在其上，若鼠标下没有画布就创建一个
        public static bool IsEnableDragUIToScene = true;

        //是否开启用箭头按键移动UI节点
        public static bool IsMoveNodeByArrowKey = true;

        //保存界面时是否需要显示保存成功的提示框
        public static bool IsShowDialogWhenSaveLayout = true;

        //结束游戏运行时是否重新加载运行期间修改过的界面
        public static bool ReloadLayoutOnExitGame = true;
        
        //一添加参考图就打开选择图片框
        public static bool OpenSelectPicDialogWhenAddDecorate = true;

        //此路径可以为空，设置后首次导入本插件时就会加载该目录下的所有prefab
        //public const string PrefabWinFirstSearchPath = "Assets/LuaFramework/AssetBundleRes/ui/uiComponent/prefab";
        public const string PrefabWinFirstSearchPath = "";

        //快捷键配置  菜单项快捷键：%#&1 代表的就是：Ctrl + Shift + Alt + 1
        public static class ShortCut
        { 
            //复制选中节点全名的字符串到系统剪切板
            public const string CopyNodesName = "%#c";
        }

        //所有编辑界面的Canvas都放到此节点上，可定制节点名
        public static string UITestNodeName = "UITestNode";
        public static Vector3 UITestNodePos = new Vector3(0, 0, 500);
        public static Vector2 UITestNodeSize = new Vector2(4, 4);
        public const string FolderName = "UGUI-Editor";
        public static string ResAssetsPath = Application.dataPath + "/" + FolderName + "/Res";

        static string projectUUID = string.Empty;
        public static string ProjectUUID
        {
            get
            {
#if UNITY_EDITOR
                if (projectUUID == string.Empty)
                {
                    projectUUID = UIEditorHelper.GenMD5String(Application.dataPath);
                }
#endif
                return projectUUID;
            }
        }
    }
}
