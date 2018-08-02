#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace U3DExtends {
static public class ContextMenu
{
    static List<string> mEntries = new List<string>();
    static GenericMenu mMenu;

    static public void AddItem(string item, bool isChecked, GenericMenu.MenuFunction callback)
    {
        if (callback != null)
        {
            if (mMenu == null) mMenu = new GenericMenu();
            int count = 0;

            for (int i = 0; i < mEntries.Count; ++i)
            {
                string str = mEntries[i];
                if (str == item) ++count;
            }
            mEntries.Add(item);

            if (count > 0) item += " [" + count + "]";
            mMenu.AddItem(new GUIContent(item), isChecked, callback);
        }
        else AddDisabledItem(item);
    }

    static public void AddItemWithArge(string item, bool isChecked, GenericMenu.MenuFunction2 callback, object arge)
    {
        if (callback != null)
        {
            if (mMenu == null) mMenu = new GenericMenu();
            int count = 0;

            for (int i = 0; i < mEntries.Count; ++i)
            {
                string str = mEntries[i];
                if (str == item) ++count;
            }
            mEntries.Add(item);

            if (count > 0) item += " [" + count + "]";
            mMenu.AddItem(new GUIContent(item), isChecked, callback, arge);
        }
        else AddDisabledItem(item);
    }

        static public void Show()
    {
        if (mMenu != null)
        {
            mMenu.ShowAsContext();
            mMenu = null;
            mEntries.Clear();
        }
    }

    //增加几种对齐菜单
    static public void AddAlignMenu()
    {
        AddItem("对齐/左对齐 ←", false, AlignTool.AlignInHorziontalLeft);
        AddItem("对齐/右对齐 →", false, AlignTool.AlignInHorziontalRight);
        AddItem("对齐/上对齐 ↑", false, AlignTool.AlignInVerticalUp);
        AddItem("对齐/下对齐 ↓", false, AlignTool.AlignInVerticalDown);
        AddItem("对齐/水平均匀 |||", false, AlignTool.UniformDistributionInHorziontal);
        AddItem("对齐/垂直均匀 ☰", false, AlignTool.UniformDistributionInVertical);
        AddItem("对齐/一样大 ■", false, AlignTool.ResizeMax);
        AddItem("对齐/一样小 ●", false, AlignTool.ResizeMin);
    }

    //增加层次菜单
    static public void AddPriorityMenu()
    {
        AddItem("层次/最里层 ↑↑↑", false, PriorityTool.MoveToTopWidget);
        AddItem("层次/最外层 ↓↓↓", false, PriorityTool.MoveToBottomWidget);
        AddItem("层次/往里挤 ↑", false, PriorityTool.MoveUpWidget);
        AddItem("层次/往外挤 ↓", false, PriorityTool.MoveDownWidget);
    }

    //增加UI控件菜单
    static public void AddUIMenu()
    {
        AddItem("添加控件/Empty", false, UIEditorHelper.CreateEmptyObj);
        AddItem("添加控件/Image", false, UIEditorHelper.CreateImageObj);
        AddItem("添加控件/RawImage", false, UIEditorHelper.CreateRawImageObj);
        AddItem("添加控件/Button", false, UIEditorHelper.CreateButtonObj);
        AddItem("添加控件/Text", false, UIEditorHelper.CreateTextObj);
        AddItem("添加控件/HScroll", false, UIEditorHelper.CreateHScrollViewObj);
        AddItem("添加控件/VScroll", false, UIEditorHelper.CreateVScrollViewObj);
    }

    //增加UI组件菜单
    static public void AddUIComponentMenu()
    {
        AddItem("添加组件/Image", false, UIEditorHelper.AddImageComponent);
        //AddItem("添加组件/RawImage", false, UIEditorHelper.CreateRawImageObj);
        //AddItem("添加组件/Button", false, UIEditorHelper.CreateButtonObj);
        //AddItem("添加组件/Text", false, UIEditorHelper.CreateTextObj);
        AddItem("添加组件/HLayout", false, UIEditorHelper.AddHorizontalLayoutComponent);
        AddItem("添加组件/VLayout", false, UIEditorHelper.AddVerticalLayoutComponent);
        AddItem("添加组件/GridLayout", false, UIEditorHelper.AddGridLayoutGroupComponent);
            
    }

    //增加显示隐藏菜单
    static public void AddShowOrHideMenu()
    {
        bool hasHideWidget = false;
        foreach (var item in Selection.gameObjects)
        {
            if (!item.activeSelf)
            {
                hasHideWidget = true;
                break;
            }
        }
        if (hasHideWidget)
            AddItem("显示", false, UILayoutTool.ShowAllSelectedWidgets);
        else
            AddItem("隐藏", false, UILayoutTool.HideAllSelectedWidgets);
    }

    static public void AddCommonItems(GameObject[] targets)
    {
            if (targets == null || targets.Length <= 0)
            {
                AddItem("新建", false, UIEditorHelper.CreatNewLayoutForMenu);
                AddItem("打开界面", false, UIEditorHelper.LoadLayout);
                AddItem("打开文件夹", false, UIEditorHelper.LoadLayoutWithFolder);
            }
            if (targets != null && targets.Length > 0)
            {
                AddItem("保存", false, UIEditorHelper.SaveLayoutForMenu);
                AddItem("另存为", false, UIEditorHelper.SaveAnotherLayoutContextMenu);
                AddItem("重新加载", false, UIEditorHelper.ReLoadLayoutForMenu);

                AddSeparator("///");
                AddItem("复制选中控件名", false, UIEditorHelper.CopySelectWidgetName);

                //如果选中超过1个节点的话
                if (targets.Length > 1)
                {
                    AddAlignMenu();
                    AddItem("同流合污", false, UILayoutTool.MakeGroup);
                }
                AddSeparator("///");
                if (targets.Length == 1)
                {
                    AddUIMenu();
                    AddUIComponentMenu();
                    AddPriorityMenu();

                    if (UIEditorHelper.IsNodeCanDivide(targets[0]))
                        AddItem("分道扬镖", false, UILayoutTool.UnGroup);
                    Decorate uiBase = targets[0].GetComponent<Decorate>();
                    if (uiBase != null)
                    {
                        if (uiBase.gameObject.hideFlags == HideFlags.NotEditable)
                        {
                            AddItem("解锁", false, UIEditorHelper.UnLockWidget);
                        }
                        else
                        {
                            AddItem("锁定", false, UIEditorHelper.LockWidget);
                        }
                    }
                }

                AddShowOrHideMenu();

                AddSeparator("///");

                AddItem("添加参考图", false, UIEditorHelper.CreateDecorate);
                if (targets.Length == 1 && targets[0].transform.childCount > 0)
                    AddItem("优化层级", false, UILayoutTool.OptimizeBatchForMenu);

            }
            AddItem("排序所有界面", false, UILayoutTool.ResortAllLayout);
            AddItem("清空界面", false, UIEditorHelper.ClearAllCanvas);
        }

    static public void AddSeparator(string path)
    {
        if (mMenu == null) mMenu = new GenericMenu();

        if (Application.platform != RuntimePlatform.OSXEditor)
            mMenu.AddSeparator(path);
    }

    static public void AddDisabledItem(string item)
    {
        if (mMenu == null) mMenu = new GenericMenu();
        mMenu.AddDisabledItem(new GUIContent(item));
    }
}
}
#endif