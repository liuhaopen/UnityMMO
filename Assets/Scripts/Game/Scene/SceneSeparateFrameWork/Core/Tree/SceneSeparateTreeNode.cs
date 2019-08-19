using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneSeparateTreeNode<T> where T : ISceneObject, ISOLinkedListNode
{

    /// <summary>
    /// 节点包围盒
    /// </summary>
    public Bounds Bounds
    {
        get { return m_Bounds; }
    }

    /// <summary>
    /// 节点当前深度
    /// </summary>
    public int CurrentDepth
    {
        get { return m_CurrentDepth; }
    }

    /// <summary>
    /// 节点数据列表
    /// </summary>
    public LinkedList<T> ObjectList
    {
        get { return m_ObjectList; }
    }

    private int m_CurrentDepth;

    private Bounds m_Bounds;

    private Vector3 m_HalfSize;

    private LinkedList<T> m_ObjectList;

    protected SceneSeparateTreeNode<T>[] m_ChildNodes;

    public SceneSeparateTreeNode(Bounds bounds, int depth, int childCount)
    {
        m_Bounds = bounds;
        m_CurrentDepth = depth;
        m_ObjectList = new LinkedList<T>();
        m_ChildNodes = new SceneSeparateTreeNode<T>[childCount];

        if (childCount == 8)
            m_HalfSize = new Vector3(m_Bounds.size.x/2, m_Bounds.size.y/2, m_Bounds.size.z/2);
        else
            m_HalfSize = new Vector3(m_Bounds.size.x/2, m_Bounds.size.y, m_Bounds.size.z/2);
    }

    public void Clear()
    {
        for (int i = 0; i < m_ChildNodes.Length; i++)
        {
            if (m_ChildNodes[i] != null)
                m_ChildNodes[i].Clear();
        }
        if (m_ObjectList != null)
            m_ObjectList.Clear();
    }

    public bool Contains(T obj)
    {
        for (int i = 0; i < m_ChildNodes.Length; i++)
        {
            if (m_ChildNodes[i] != null && m_ChildNodes[i].Contains(obj))
                return true;
        }

        if (m_ObjectList != null && m_ObjectList.Contains(obj))
            return true;
        return false;
    }

    public SceneSeparateTreeNode<T> Insert(T obj, int depth, int maxDepth)
    {
        if (m_ObjectList.Contains(obj))
            return this;
        if (depth < maxDepth)
        {
            SceneSeparateTreeNode<T> node = GetContainerNode(obj, depth);
            if (node != null)
                return node.Insert(obj, depth + 1, maxDepth);
        }
        var n = m_ObjectList.AddFirst(obj);
        obj.SetLinkedListNode(n);
        return this;
    }

    public void Remove(T obj)
    {
        m_ObjectList.Remove(obj.GetLinkedListNode<T>());
    }

    public void Trigger(IDetector detector, TriggerHandle<T> handle)
    {
        if (handle == null)
            return;

        for (int i = 0; i < m_ChildNodes.Length; i++)
        {
            var node = m_ChildNodes[i];
            if (node != null)
                node.Trigger(detector, handle);
        }

        if (detector.IsDetected(m_Bounds))
        {
            var node = m_ObjectList.First;
            while (node != null)
            {
                if (detector.IsDetected(node.Value.Bounds))
                    handle(node.Value);
                node = node.Next;
            }
        }
    }

    protected SceneSeparateTreeNode<T> GetContainerNode(T obj, int depth)
    {
        SceneSeparateTreeNode<T> result = null;
        int ix = -1;
        int iz = -1;
        int iy = m_ChildNodes.Length == 4 ? 0 : -1;

        int nodeIndex = 0;
        for (int i = ix; i <= 1; i += 2)
        {
            for (int j = iz; j <= 1; j += 2)
            {
                for (int k = iy; k <= 1; k += 2)
                {
                    result = CreateNode(ref m_ChildNodes[nodeIndex], depth, m_Bounds.center + new Vector3(i* m_HalfSize.x / 2, k*m_HalfSize.y/2, j* m_HalfSize.z / 2),
            m_HalfSize, obj);
                    if (result != null)
                        return result;
                    nodeIndex += 1;
                }
            }
        }
        return null;
    }

    protected SceneSeparateTreeNode<T> CreateNode(ref SceneSeparateTreeNode<T> node, int depth, Vector3 centerPos, Vector3 size, T obj) 
    {
        SceneSeparateTreeNode<T> result = null;
        if (node == null)
        {
            Bounds bounds = new Bounds(centerPos, size);
            if (bounds.IsBoundsContainsAnotherBounds(obj.Bounds))
            {
                SceneSeparateTreeNode<T> newNode = new SceneSeparateTreeNode<T>(bounds, depth + 1, m_ChildNodes.Length);
                node = newNode;
                result = node;
            }
        }
        else if (node.Bounds.IsBoundsContainsAnotherBounds(obj.Bounds))
        {
            result = node;
        }
        return result;
    }

#if UNITY_EDITOR
    public void DrawNode(Color treeMinDepthColor, Color treeMaxDepthColor, Color objColor, Color hitObjColor, int drawMinDepth, int drawMaxDepth, bool drawObj, int maxDepth)
    {
        if (m_ChildNodes != null)
        {
            for (int i = 0; i < m_ChildNodes.Length; i++)
            {
                var node = m_ChildNodes[i];
                if (node != null)
                    node.DrawNode(treeMinDepthColor, treeMaxDepthColor, objColor, hitObjColor, drawMinDepth, drawMaxDepth, drawObj, maxDepth);
            }
        }

        if (m_CurrentDepth >= drawMinDepth && m_CurrentDepth <= drawMaxDepth)
        {
            float d = ((float)m_CurrentDepth) / maxDepth;
            Color color = Color.Lerp(treeMinDepthColor, treeMaxDepthColor, d);

            m_Bounds.DrawBounds(color);
        }
        if (drawObj)
        {
            var node = m_ObjectList.First;
            while (node != null)
            {
                var sceneobj = node.Value as SceneObject;
                if (sceneobj != null)
                    sceneobj.DrawArea(objColor, hitObjColor);
                node = node.Next;
            }
        }

    }

#endif
}
