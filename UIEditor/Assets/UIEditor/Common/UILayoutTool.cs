#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace U3DExtends
{
    public class UILayoutTool : MonoBehaviour
    {
        [MenuItem("UIEditor/排序所有界面 " + Configure.ShortCut.SortAllCanvas)]
        public static void ResortAllLayout(object o)
        {
            GameObject testUI = GameObject.Find(Configure.UITestNodeName);
            if (testUI != null)
            {
                Canvas[] layouts = testUI.GetComponentsInChildren<Canvas>();
                if (layouts[0] != null)
                {
                    SceneView.lastActiveSceneView.MoveToView(layouts[0].transform);
                }
                Vector2 startPos = new Vector2(layouts[0].transform.position.x - 1280 * 1, layouts[0].transform.position.y + 720 * 1);
                int index = 0;
                foreach (var item in layouts)
                {
                    int row = index / 5;
                    int col = index % 5;
                    Vector2 pos = new Vector2(1280 * col + startPos.x, -720 * row + startPos.y);
                    item.transform.position = pos;
                    index++;
                }
            }
        }

        //[MenuItem("UIEditor/显示 " + Configure.ShortCut.SortAllCanvas)]
        public static void ShowAllSelectedWidgets(object o)
        {
            foreach (var item in Selection.gameObjects)
            {
                item.SetActive(true);
            }
        }
        //[MenuItem("UIEditor/隐藏 " + Configure.ShortCut.SortAllCanvas)]
        public static void HideAllSelectedWidgets(object o)
        {
            foreach (var item in Selection.gameObjects)
            {
                item.SetActive(false);
            }
        }

        //[MenuItem("UIEditor/Operate/解除")]
        public static void UnGroup(object o)
        {
            if (Selection.gameObjects == null || Selection.gameObjects.Length <= 0)
            {
                EditorUtility.DisplayDialog("Error", "当前没有选中节点", "Ok");
                return;
            }
            if (Selection.gameObjects.Length > 1)
            {
                EditorUtility.DisplayDialog("Error", "只能同时解除一个Box", "Ok");
                return;
            }
            GameObject target = Selection.activeGameObject;
            Transform new_parent = target.transform.parent;
            if (target.transform.childCount > 0)
            {
                Transform[] child_ui = target.transform.GetComponentsInChildren<Transform>(true);
                foreach (var item in child_ui)
                {
                    //不是自己的子节点或是自己的话就跳过
                    if (item.transform.parent != target.transform || item.transform == target.transform)
                        continue;

                    item.transform.SetParent(new_parent, true);
                }
                Undo.DestroyObjectImmediate(target);
                //GameObject.DestroyImmediate(target);
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "选择对象容器控件", "Ok");
            }
        }

        //[MenuItem("UIEditor/Operate/组合")]
        public static void MakeGroup(object o)
        {
            if (Selection.gameObjects == null || Selection.gameObjects.Length <= 0)
            {
                EditorUtility.DisplayDialog("Error", "当前没有选中节点", "Ok");
                return;
            }
            //先判断选中的节点是不是挂在同个父节点上的
            Transform parent = Selection.gameObjects[0].transform.parent;
            foreach (var item in Selection.gameObjects)
            {
                Debug.Log("item name :" + item.name);
                if (item.transform.parent != parent)
                {
                    EditorUtility.DisplayDialog("Error", "不能跨容器组合", "Ok");
                    return;
                }
            }
            GameObject box = new GameObject("container", typeof(RectTransform));
            RectTransform rectTrans = box.GetComponent<RectTransform>();
            if (rectTrans != null)
            {
                Vector2 left_top_pos = new Vector2(99999, -99999);
                Vector2 right_bottom_pos = new Vector2(-99999, 99999);
                foreach (var item in Selection.gameObjects)
                {
                    Bounds bound = UIEditorHelper.GetBounds(item);
                    Vector3 boundMin = item.transform.parent.InverseTransformPoint(bound.min);
                    Vector3 boundMax = item.transform.parent.InverseTransformPoint(bound.max);
                    Debug.Log("bound : " + boundMin.ToString() + " max:" + boundMax.ToString());
                    if (boundMin.x < left_top_pos.x)
                        left_top_pos.x = boundMin.x;
                    if (boundMax.y > left_top_pos.y)
                        left_top_pos.y = boundMax.y;
                    if (boundMax.x > right_bottom_pos.x)
                        right_bottom_pos.x = boundMax.x;
                    if (boundMin.y < right_bottom_pos.y)
                        right_bottom_pos.y = boundMin.y;
                }
                rectTrans.SetParent(parent);
                rectTrans.sizeDelta = new Vector2(right_bottom_pos.x - left_top_pos.x,  left_top_pos.y - right_bottom_pos.y);
                left_top_pos.x += rectTrans.sizeDelta.x/2;
                left_top_pos.y -= rectTrans.sizeDelta.y/2;
                rectTrans.localPosition = left_top_pos;

                //需要先生成好Box和设置好它的坐标和大小才可以把选中的节点挂进来，注意要先排好序，不然层次就乱了
                GameObject[] sorted_objs = Selection.gameObjects.OrderBy(x => x.transform.GetSiblingIndex()).ToArray();
                for (int i = 0; i < sorted_objs.Length; i++)
                {
                    sorted_objs[i].transform.SetParent(rectTrans, true);
                }
            }
            Selection.activeGameObject = box;
        }

        
    }


    public class PriorityTool
    {
        [MenuItem("UIEditor/层次/最里层 " + Configure.ShortCut.MoveNodeTop)]
        public static void MoveToTopWidget(object o)
        {
            Transform curSelect = Selection.activeTransform;
            if (curSelect != null)
            {
                curSelect.SetAsFirstSibling();
            }
        }
        [MenuItem("UIEditor/层次/最外层 " + Configure.ShortCut.MoveNodeBottom)]
        public static void MoveToBottomWidget(object o)
        {
            Transform curSelect = Selection.activeTransform;
            if (curSelect != null)
            {
                curSelect.SetAsLastSibling();
            }
        }

        [MenuItem("UIEditor/层次/往里挤 " + Configure.ShortCut.MoveNodeUp)]
        public static void MoveUpWidget(object o)
        {
            Transform curSelect = Selection.activeTransform;
            if (curSelect != null)
            {
                int curIndex = curSelect.GetSiblingIndex();
                if (curIndex > 0)
                    curSelect.SetSiblingIndex(curIndex - 1);
            }
        }

        [MenuItem("UIEditor/层次/往外挤 " + Configure.ShortCut.MoveNodeDown)]
        public static void MoveDownWidget(object o)
        {
            Transform curSelect = Selection.activeTransform;
            if (curSelect != null)
            {
                int curIndex = curSelect.GetSiblingIndex();
                int child_num = curSelect.parent.childCount;
                if (curIndex < child_num - 1)
                    curSelect.SetSiblingIndex(curIndex + 1);
            }
        }
    }

    public class AlignTool
    {
        [MenuItem("UIEditor/对齐/左对齐 ←")]
        internal static void AlignInHorziontalLeft(object o)
        {
            float x = Mathf.Min(Selection.gameObjects.Select(obj => obj.transform.localPosition.x).ToArray());

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                gameObject.transform.localPosition = new Vector2(x,
                    gameObject.transform.localPosition.y);
            }
        }

        [MenuItem("UIEditor/对齐/右对齐 →")]
        public static void AlignInHorziontalRight(object o)
        {
            float x = Mathf.Max(Selection.gameObjects.Select(obj => obj.transform.localPosition.x +
            ((RectTransform)obj.transform).sizeDelta.x).ToArray());
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                gameObject.transform.localPosition = new Vector3(x -
            ((RectTransform)gameObject.transform).sizeDelta.x, gameObject.transform.localPosition.y);
            }
        }

        [MenuItem("UIEditor/对齐/上对齐 ↑")]
        public static void AlignInVerticalUp(object o)
        {
            float y = Mathf.Max(Selection.gameObjects.Select(obj => obj.transform.localPosition.y).ToArray());
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, y);
            }
        }

        [MenuItem("UIEditor/对齐/下对齐 ↓")]
        public static void AlignInVerticalDown(object o)
        {
            float y = Mathf.Min(Selection.gameObjects.Select(obj => obj.transform.localPosition.y -
            ((RectTransform)obj.transform).sizeDelta.y).ToArray());

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, y + ((RectTransform)gameObject.transform).sizeDelta.y);
            }
        }


        [MenuItem("UIEditor/对齐/水平均匀 |||")]
        public static void UniformDistributionInHorziontal(object o)
        {
            int count = Selection.gameObjects.Length;
            float firstX = Mathf.Min(Selection.gameObjects.Select(obj => obj.transform.localPosition.x).ToArray());
            float lastX = Mathf.Max(Selection.gameObjects.Select(obj => obj.transform.localPosition.x).ToArray());
            float distance = (lastX - firstX) / (count - 1);
            var objects = Selection.gameObjects.ToList();
            objects.Sort((x, y) => (int)(x.transform.localPosition.x - y.transform.localPosition.x));
            for (int i = 0; i < count; i++)
            {
                objects[i].transform.localPosition = new Vector3(firstX + i * distance, objects[i].transform.localPosition.y);
            }
        }

        [MenuItem("UIEditor/对齐/垂直均匀 ☰")]
        public static void UniformDistributionInVertical(object o)
        {
            int count = Selection.gameObjects.Length;
            float firstY = Mathf.Min(Selection.gameObjects.Select(obj => obj.transform.localPosition.y).ToArray());
            float lastY = Mathf.Max(Selection.gameObjects.Select(obj => obj.transform.localPosition.y).ToArray());
            float distance = (lastY - firstY) / (count - 1);
            var objects = Selection.gameObjects.ToList();
            objects.Sort((x, y) => (int)(x.transform.localPosition.y - y.transform.localPosition.y));
            for (int i = 0; i < count; i++)
            {
                objects[i].transform.localPosition = new Vector3(objects[i].transform.localPosition.x, firstY + i * distance);
            }
        }

        [MenuItem("UIEditor/对齐/一样大 ■")]
        public static void ResizeMax(object o)
        {
            var height = Mathf.Max(Selection.gameObjects.Select(obj => ((RectTransform)obj.transform).sizeDelta.y).ToArray());
            var width = Mathf.Max(Selection.gameObjects.Select(obj => ((RectTransform)obj.transform).sizeDelta.x).ToArray());
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                ((RectTransform)gameObject.transform).sizeDelta = new Vector2(width, height);
            }
        }

        [MenuItem("UIEditor/对齐/一样小 ●")]
        public static void ResizeMin(object o)
        {
            var height = Mathf.Min(Selection.gameObjects.Select(obj => ((RectTransform)obj.transform).sizeDelta.y).ToArray());
            var width = Mathf.Min(Selection.gameObjects.Select(obj => ((RectTransform)obj.transform).sizeDelta.x).ToArray());
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                ((RectTransform)gameObject.transform).sizeDelta = new Vector2(width, height);
            }
        }

    }
}
#endif