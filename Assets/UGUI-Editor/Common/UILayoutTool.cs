#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

namespace U3DExtends
{
    public class UILayoutTool : MonoBehaviour
    {
        public static void ResortAllLayout()
        {
            GameObject testUI = GameObject.Find(Configure.UITestNodeName);
            if (testUI != null)
            {
                LayoutInfo[] layouts = testUI.GetComponentsInChildren<LayoutInfo>();
                if (layouts.Length > 0)
                {
                    SceneView.lastActiveSceneView.MoveToView(layouts[0].transform);
                    
                    RectTransform first_trans = layouts[0].transform as RectTransform;
                    Vector2 startPos = new Vector2(first_trans.sizeDelta.x*first_trans.localScale.x/2, -first_trans.sizeDelta.y * first_trans.localScale.y/2);
                    int index = 0;
                    foreach (var item in layouts)
                    {
                        int row = index / 5;
                        int col = index % 5;
                        RectTransform rectTrans = item.transform as RectTransform;
                        Vector3 pos = new Vector3((rectTrans.sizeDelta.x*rectTrans.localScale.x) * col + startPos.x, (-rectTrans.sizeDelta.y * rectTrans.localScale.y) * row + startPos.y, 0);
                        item.transform.localPosition = pos;
                        index++;
                    }
                }
            }
        }

        public static void OptimizeBatchForMenu()
        {
            OptimizeBatch(Selection.activeTransform);
        }

        public static void OptimizeBatch(Transform trans)
        {
            if (trans == null)
                return;
            Dictionary<string, List<Transform>> imageGroup = new Dictionary<string, List<Transform>>();
            Dictionary<string, List<Transform>> textGroup = new Dictionary<string, List<Transform>>();
            List<List<Transform>> sortedImgageGroup = new List<List<Transform>>();
            List<List<Transform>> sortedTextGroup = new List<List<Transform>>();
            for (int i = 0; i < trans.childCount; i++)
            {
                Transform child = trans.GetChild(i);
                Texture cur_texture = null;
                Image img = child.GetComponent<Image>();
                if (img != null)
                {
                    cur_texture = img.mainTexture;
                }
                else
                {
                    RawImage rimg = child.GetComponent<RawImage>();
                    if (rimg != null)
                        cur_texture = rimg.mainTexture;
                }
                if (cur_texture != null)
                {
                    string cur_path = AssetDatabase.GetAssetPath(cur_texture);
                    TextureImporter importer = AssetImporter.GetAtPath(cur_path) as TextureImporter;
                    //Debug.Log("cur_path : " + cur_path + " importer:"+(importer!=null).ToString());
                    if (importer != null)
                    {
                        string atlas = importer.spritePackingTag;
                        //Debug.Log("atlas : " + atlas);
                        if (atlas != "")
                        {
                            if (!imageGroup.ContainsKey(atlas))
                            {
                                List<Transform> list = new List<Transform>();
                                sortedImgageGroup.Add(list);
                                imageGroup.Add(atlas, list);
                            }
                            imageGroup[atlas].Add(child);
                        }
                    }
                }
                else
                {
                    Text text = child.GetComponent<Text>();
                    if (text != null)
                    {
                        string fontName = text.font.name;
                        //Debug.Log("fontName : " + fontName);
                        if (!textGroup.ContainsKey(fontName))
                        {
                            List<Transform> list = new List<Transform>();
                            sortedTextGroup.Add(list);
                            textGroup.Add(fontName, list);
                        }
                        textGroup[fontName].Add(child);
                    }
                }
                OptimizeBatch(child);
            }
            //同一图集的Image间层级顺序继续保留,不同图集的顺序就按每组第一张的来
            for (int i = sortedImgageGroup.Count - 1; i >= 0; i--)
            {
                List<Transform> children = sortedImgageGroup[i];
                for (int j = children.Count - 1; j >= 0; j--)
                {
                    children[j].SetAsFirstSibling();
                }

            }
            foreach (var item in sortedTextGroup)
            {
                List<Transform> children = item;
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].SetAsLastSibling();
                }
            }
        }

        public static void ShowAllSelectedWidgets()
        {
            foreach (var item in Selection.gameObjects)
            {
                item.SetActive(true);
            }
        }
        public static void HideAllSelectedWidgets()
        {
            foreach (var item in Selection.gameObjects)
            {
                item.SetActive(false);
            }
        }

        //[MenuItem("UIEditor/Operate/解除")]
        public static void UnGroup()
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
        public static void MakeGroup()
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
                if (item.transform.parent != parent)
                {
                    EditorUtility.DisplayDialog("Error", "不能跨容器组合", "Ok");
                    return;
                }
            }
            GameObject box = new GameObject("container", typeof(RectTransform));
            Undo.IncrementCurrentGroup();
            int group_index = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName("Make Group");
            Undo.RegisterCreatedObjectUndo(box, "create group object");
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
                    //Debug.Log("bound : " + boundMin.ToString() + " max:" + boundMax.ToString());
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
                rectTrans.localScale = Vector3.one;

                //需要先生成好Box和设置好它的坐标和大小才可以把选中的节点挂进来，注意要先排好序，不然层次就乱了
                GameObject[] sorted_objs = Selection.gameObjects.OrderBy(x => x.transform.GetSiblingIndex()).ToArray();
                for (int i = 0; i < sorted_objs.Length; i++)
                {
                    Undo.SetTransformParent(sorted_objs[i].transform, rectTrans, "move item to group");
                }
            }
            Selection.activeGameObject = box;
            Undo.CollapseUndoOperations(group_index);
        }

        
    }


    public class PriorityTool
    {
        // [MenuItem("UIEditor/层次/最里层 " + Configure.ShortCut.MoveNodeTop)]
        public static void MoveToTopWidget()
        {
            Transform curSelect = Selection.activeTransform;
            if (curSelect != null)
            {
                curSelect.SetAsFirstSibling();
            }
        }
        // [MenuItem("UIEditor/层次/最外层 " + Configure.ShortCut.MoveNodeBottom)]
        public static void MoveToBottomWidget()
        {
            Transform curSelect = Selection.activeTransform;
            if (curSelect != null)
            {
                curSelect.SetAsLastSibling();
            }
        }

        // [MenuItem("UIEditor/层次/往里挤 " + Configure.ShortCut.MoveNodeUp)]
        public static void MoveUpWidget()
        {
            Transform curSelect = Selection.activeTransform;
            if (curSelect != null)
            {
                int curIndex = curSelect.GetSiblingIndex();
                if (curIndex > 0)
                    curSelect.SetSiblingIndex(curIndex - 1);
            }
        }

        // [MenuItem("UIEditor/层次/往外挤 " + Configure.ShortCut.MoveNodeDown)]
        public static void MoveDownWidget()
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
        // [MenuItem("UIEditor/对齐/左对齐 ←")]
        internal static void AlignInHorziontalLeft()
        {
            float x = Mathf.Min(Selection.gameObjects.Select(obj => obj.transform.localPosition.x).ToArray());

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                gameObject.transform.localPosition = new Vector2(x,
                    gameObject.transform.localPosition.y);
            }
        }

        // [MenuItem("UIEditor/对齐/右对齐 →")]
        public static void AlignInHorziontalRight()
        {
            float x = Mathf.Max(Selection.gameObjects.Select(obj => obj.transform.localPosition.x +
            ((RectTransform)obj.transform).sizeDelta.x).ToArray());
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                gameObject.transform.localPosition = new Vector3(x -
            ((RectTransform)gameObject.transform).sizeDelta.x, gameObject.transform.localPosition.y);
            }
        }

        // [MenuItem("UIEditor/对齐/上对齐 ↑")]
        public static void AlignInVerticalUp()
        {
            float y = Mathf.Max(Selection.gameObjects.Select(obj => obj.transform.localPosition.y).ToArray());
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, y);
            }
        }

        // [MenuItem("UIEditor/对齐/下对齐 ↓")]
        public static void AlignInVerticalDown()
        {
            float y = Mathf.Min(Selection.gameObjects.Select(obj => obj.transform.localPosition.y -
            ((RectTransform)obj.transform).sizeDelta.y).ToArray());

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, y + ((RectTransform)gameObject.transform).sizeDelta.y);
            }
        }


        // [MenuItem("UIEditor/对齐/水平均匀 |||")]
        public static void UniformDistributionInHorziontal()
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

        // [MenuItem("UIEditor/对齐/垂直均匀 ☰")]
        public static void UniformDistributionInVertical()
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

        // [MenuItem("UIEditor/对齐/一样大 ■")]
        public static void ResizeMax()
        {
            var height = Mathf.Max(Selection.gameObjects.Select(obj => ((RectTransform)obj.transform).sizeDelta.y).ToArray());
            var width = Mathf.Max(Selection.gameObjects.Select(obj => ((RectTransform)obj.transform).sizeDelta.x).ToArray());
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                ((RectTransform)gameObject.transform).sizeDelta = new Vector2(width, height);
            }
        }

        // [MenuItem("UIEditor/对齐/一样小 ●")]
        public static void ResizeMin()
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