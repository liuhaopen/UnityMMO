using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace U3DExtends { 
public class SceneEditor {

    static Object LastSelectObj = null;//用来记录上次选中的GameObject，只有它带有Image组件时才把图片赋值给它
    static Object CurSelectObj = null;
    [InitializeOnLoadMethod]
    static void Init()
    {
        SceneView.onSceneGUIDelegate += OnSceneGUI;

        //选中Image节点并点击图片后即帮它赋上图片
        if (Configure.IsEnableFastSelectImage)
            Selection.selectionChanged += OnSelectChange;
    }

    static void OnSelectChange()
    {
        LastSelectObj = CurSelectObj;
        CurSelectObj = Selection.activeObject;
        //如果要遍历目录，修改为SelectionMode.DeepAssets
        UnityEngine.Object[] arr = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.TopLevel);
        if (arr != null && arr.Length > 0)
        {
            GameObject selectObj = LastSelectObj as GameObject;
            if (selectObj != null && (arr[0] is Sprite || arr[0] is Texture2D))
            {
                string assetPath = AssetDatabase.GetAssetPath(arr[0]);
                Image image = selectObj.GetComponent<Image>();
                bool isImgWidget = false;
                if (image != null)
                {
                    isImgWidget = true;
                    UIEditorHelper.SetImageByPath(assetPath, image, Configure.IsAutoSizeOnFastSelectImg);
                }
                if (isImgWidget)
                {
                    //赋完图后把焦点还给Image节点
                    EditorApplication.delayCall = delegate
                    {
                        Selection.activeGameObject = LastSelectObj as GameObject;
                    };
                }
            }
        }
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        bool is_handled = false;
        if (Configure.IsEnableDragUIToScene && (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform))
        {
            //拉UI prefab或者图片入scene界面时帮它找到鼠标下的Canvas并挂在其上，若鼠标下没有画布就创建一个
            Object handleObj = DragAndDrop.objectReferences[0];
            if (!IsNeedHandleAsset(handleObj))
            {
                //让系统自己处理
                return;
            }
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            //当松开鼠标时
            if (Event.current.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                foreach (var item in DragAndDrop.objectReferences)
                {
                    HandleDragAsset(sceneView, item);
                }
            }
            is_handled = true;
        }
        else if (e.type == EventType.KeyDown && Configure.IsMoveNodeByArrowKey)
        {
            //按上按下要移动节点，因为默认情况下只是移动Scene界面而已
            foreach (var item in Selection.transforms)
            {
                Transform trans = item;
                if (trans != null)
                {
                    if (e.keyCode == KeyCode.UpArrow)
                    {
                        Vector3 newPos = new Vector3(trans.localPosition.x, trans.localPosition.y + 1, trans.localPosition.z);
                        trans.localPosition = newPos;
                        is_handled = true;
                    }
                    else if (e.keyCode == KeyCode.DownArrow)
                    {
                        Vector3 newPos = new Vector3(trans.localPosition.x, trans.localPosition.y - 1, trans.localPosition.z);
                        trans.localPosition = newPos;
                        is_handled = true;
                    }
                    else if (e.keyCode == KeyCode.LeftArrow)
                    {
                        Vector3 newPos = new Vector3(trans.localPosition.x - 1, trans.localPosition.y, trans.localPosition.z);
                        trans.localPosition = newPos;
                        is_handled = true;
                    }
                    else if (e.keyCode == KeyCode.RightArrow)
                    {
                        Vector3 newPos = new Vector3(trans.localPosition.x + 1, trans.localPosition.y, trans.localPosition.z);
                        trans.localPosition = newPos;
                        is_handled = true;
                    }
                }
            }
        }
        else if (Event.current != null && Event.current.button == 1 && Event.current.type == EventType.MouseUp && Configure.IsShowSceneMenu)
        {
            if (Selection.gameObjects == null || Selection.gameObjects.Length==0 || Selection.gameObjects[0].transform is RectTransform)
            {
                ContextMenu.AddCommonItems(Selection.gameObjects);
                ContextMenu.Show();
                is_handled = true;
            }
        }
        //else if (e.type == EventType.MouseMove)//show cur mouse pos
        //{
        //    Camera cam = sceneView.camera;
        //    Vector3 mouse_abs_pos = e.mousePosition;
        //    mouse_abs_pos.y = cam.pixelHeight - mouse_abs_pos.y;
        //    mouse_abs_pos = sceneView.camera.ScreenToWorldPoint(mouse_abs_pos);
        //    Debug.Log("mouse_abs_pos : " + mouse_abs_pos.ToString());
        //}
        if (e!=null && Event.current.type == EventType.KeyUp && e.control && e.keyCode==KeyCode.E)
            LayoutInfo.IsShowLayoutName = !LayoutInfo.IsShowLayoutName;
        if (is_handled)
            Event.current.Use();
    }

    static bool HandleDragAsset(SceneView sceneView, Object handleObj)
    {
        Event e = Event.current;
        Camera cam = sceneView.camera;
        Vector3 mouse_abs_pos = e.mousePosition;
        mouse_abs_pos.y = cam.pixelHeight - mouse_abs_pos.y;
        mouse_abs_pos = sceneView.camera.ScreenToWorldPoint(mouse_abs_pos);
        if (handleObj.GetType() == typeof(Sprite) || handleObj.GetType() == typeof(Texture2D))
        {
            GameObject box = new GameObject("Image_1", typeof(Image));
            Undo.RegisterCreatedObjectUndo(box, "create image on drag pic");
            box.transform.position = mouse_abs_pos;
            Transform container_trans = UIEditorHelper.GetContainerUnderMouse(mouse_abs_pos, box);
            if (container_trans == null)
            {
                //没有容器的话就创建一个
                container_trans = NewLayoutAndEventSys(mouse_abs_pos);
            }
            box.transform.SetParent(container_trans);
            mouse_abs_pos.z = container_trans.position.z;
            box.transform.position = mouse_abs_pos;
            box.transform.localScale = Vector3.one;
            Selection.activeGameObject = box;
                
            //生成唯一的节点名字
            box.name = CommonHelper.GenerateUniqueName(container_trans.gameObject, handleObj.name);
            //赋上图片
            Image imageBoxCom = box.GetComponent<Image>();
            if (imageBoxCom != null)
            {
                imageBoxCom.raycastTarget = false;
                string assetPath = AssetDatabase.GetAssetPath(handleObj);
                UIEditorHelper.SetImageByPath(assetPath, imageBoxCom);
                return true;
            }
        }
        else
        {
            GameObject new_obj = GameObject.Instantiate(handleObj) as GameObject;
            if (new_obj != null)
            {
                Undo.RegisterCreatedObjectUndo(new_obj, "create obj on drag prefab");
                new_obj.transform.position = mouse_abs_pos;
                GameObject ignore_obj = new_obj;
               
                Transform container_trans = UIEditorHelper.GetContainerUnderMouse(mouse_abs_pos, ignore_obj);
                if (container_trans == null)
                {
                    container_trans = NewLayoutAndEventSys(mouse_abs_pos);
                }
                new_obj.transform.SetParent(container_trans);
                mouse_abs_pos.z = container_trans.position.z;
                new_obj.transform.position = mouse_abs_pos;
                new_obj.transform.localScale = Vector3.one;
                Selection.activeGameObject = new_obj;
                //生成唯一的节点名字
                new_obj.name = CommonHelper.GenerateUniqueName(container_trans.gameObject, handleObj.name);
                return true;
            }
        }
        return false;
    }

    private static Transform NewLayoutAndEventSys(Vector3 pos)
    {
        GameObject layout = UIEditorHelper.CreatNewLayout();
        pos.z = 0;
        layout.transform.position = pos;
        Vector3 last_pos = layout.transform.localPosition;
        last_pos.z = 0;
        layout.transform.localPosition = last_pos;
        return UIEditorHelper.GetRootLayout(layout.transform);
    }

    static bool IsNeedHandleAsset(Object obj)
    {
        if (obj.GetType() == typeof(Sprite) || obj.GetType() == typeof(Texture2D))
            return true;
        else
        {
            GameObject gameObj = obj as GameObject;
            if (gameObj != null)
            {
                RectTransform uiBase = gameObj.GetComponent<RectTransform>();
                if (uiBase != null)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
    
}