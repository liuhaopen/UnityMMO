#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace U3DExtends {
static public class ContextMenu
{
    static List<string> mEntries = new List<string>();
    static GenericMenu mMenu;

    static public void AddItem(string item, bool isChecked, GenericMenu.MenuFunction2 callback, object param)
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
            mMenu.AddItem(new GUIContent(item), isChecked, callback, param);
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
        AddItem("对齐/左对齐 ←", false, AlignTool.AlignInHorziontalLeft, null);
        AddItem("对齐/右对齐 →", false, AlignTool.AlignInHorziontalRight, null);
        AddItem("对齐/上对齐 ↑", false, AlignTool.AlignInVerticalUp, null);
        AddItem("对齐/下对齐 ↓", false, AlignTool.AlignInVerticalDown, null);
        AddItem("对齐/水平均匀 |||", false, AlignTool.UniformDistributionInHorziontal, null);
        AddItem("对齐/垂直均匀 ☰", false, AlignTool.UniformDistributionInVertical, null);
        AddItem("对齐/一样大 ■", false, AlignTool.ResizeMax, null);
        AddItem("对齐/一样小 ●", false, AlignTool.ResizeMin, null);
    }

    //增加层次菜单
    static public void AddPriorityMenu()
    {
        AddItem("层次/最里层 ↑↑↑", false, PriorityTool.MoveToTopWidget, null);
        AddItem("层次/最外层 ↓↓↓", false, PriorityTool.MoveToBottomWidget, null);
        AddItem("层次/往里挤 ↑", false, PriorityTool.MoveUpWidget, null);
        AddItem("层次/往外挤 ↓", false, PriorityTool.MoveDownWidget, null);
    }

    //增加UI控件菜单
    static public void AddUIMenu()
    {
        AddItem("添加控件/Empty", false, UIEditorHelper.CreateEmptyObj, null);
        AddItem("添加控件/Image", false, UIEditorHelper.CreateImageObj, null);
        AddItem("添加控件/RawImage", false, UIEditorHelper.CreateRawImageObj, null);
        AddItem("添加控件/Button", false, UIEditorHelper.CreateButtonObj, null);
        AddItem("添加控件/Text", false, UIEditorHelper.CreateTextObj, null);
    }

    //增加UI组件菜单
    static public void AddUIComponentMenu()
    {
        AddItem("添加组件/Image", false, UIEditorHelper.AddImageComponent, null);
        //AddItem("添加组件/RawImage", false, UIEditorHelper.CreateRawImageObj, null);
        //AddItem("添加组件/Button", false, UIEditorHelper.CreateButtonObj, null);
        //AddItem("添加组件/Text", false, UIEditorHelper.CreateTextObj, null);
        AddItem("添加组件/HLayout", false, UIEditorHelper.AddHorizontalLayoutComponent, null);
        AddItem("添加组件/VLayout", false, UIEditorHelper.AddVerticalLayoutComponent, null);
        AddItem("添加组件/GridLayout", false, UIEditorHelper.AddGridLayoutGroupComponent, null);
            
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
            AddItem("显示", false, UILayoutTool.ShowAllSelectedWidgets, null);
        else
            AddItem("隐藏", false, UILayoutTool.HideAllSelectedWidgets, null);
    }

    static public void AddCommonItems(GameObject[] targets)
    {
            if (targets == null || targets.Length <= 0)
            {
                AddItem("新建", false, UIEditorHelper.CreatNewLayoutForMenu, null);
                AddItem("打开界面", false, UIEditorHelper.LoadLayout, null);
            }
            if (targets != null && targets.Length > 0)
            {
                AddItem("保存", false, UIEditorHelper.SaveLayoutForMenu, null);
                AddItem("另存为", false, UIEditorHelper.SaveAnotherLayoutContextMenu, null);
                AddItem("重新加载", false, UIEditorHelper.ReLoadLayoutForMenu, null);

                AddSeparator("///");
                AddItem("复制选中控件名", false, UIEditorHelper.CopySelectWidgetName, null);

                //如果选中超过1个节点的话
                if (targets.Length > 1)
                {
                    AddAlignMenu();
                    AddItem("同流合污", false, UILayoutTool.MakeGroup, null);
                }
                AddSeparator("///");
                if (targets.Length == 1)
                {
                    AddUIMenu();
                    AddUIComponentMenu();
                    AddPriorityMenu();

                    if (UIEditorHelper.IsNodeCanDivide(targets[0]))
                        AddItem("分道扬镖", false, UILayoutTool.UnGroup, null);
                    Decorate uiBase = targets[0].GetComponent<Decorate>();
                    if (uiBase != null)
                    {
                        if (uiBase.gameObject.hideFlags == HideFlags.NotEditable)
                        {
                            AddItem("解锁", false, UIEditorHelper.UnLockWidget, null);
                        }
                        else
                        {
                            AddItem("锁定", false, UIEditorHelper.LockWidget, null);
                        }
                    }
                }

                AddShowOrHideMenu();

                AddSeparator("///");

                AddItem("添加参考图", false, UIEditorHelper.CreateDecorate, null);

            }
            AddItem("排序所有界面", false, UILayoutTool.ResortAllLayout, null);
            AddItem("清空界面", false, UIEditorHelper.ClearAllCanvas, null);
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