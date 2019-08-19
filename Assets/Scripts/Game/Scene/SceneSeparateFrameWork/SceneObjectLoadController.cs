using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景物件加载控制器
/// </summary>
public class SceneObjectLoadController : MonoBehaviour
{

    private WaitForEndOfFrame m_WaitForFrame;

    /// <summary>
    /// 当前场景资源四叉树
    /// </summary>
    private SceneSeparateTree<SceneObject> m_QuadTree;

    /// <summary>
    /// 刷新时间
    /// </summary>
    private float m_RefreshTime;
    /// <summary>
    /// 销毁时间
    /// </summary>
    private float m_DestroyRefreshTime;
    
    private Vector3 m_OldRefreshPosition;
    private Vector3 m_OldDestroyRefreshPosition;

    /// <summary>
    /// 异步任务队列
    /// </summary>
    private Queue<SceneObject> m_ProcessTaskQueue;

    /// <summary>
    /// 已加载的物体列表
    /// </summary>
    private List<SceneObject> m_LoadedObjectList;

    /// <summary>
    /// 待销毁物体列表
    /// </summary>
    //private Queue<SceneObject> m_PreDestroyObjectQueue;
    private PriorityQueue<SceneObject> m_PreDestroyObjectQueue;

    private TriggerHandle<SceneObject> m_TriggerHandle;

    private bool m_IsTaskRunning;

    private bool m_IsInitialized;

    private int m_MaxCreateCount;
    private int m_MinCreateCount;
    private float m_MaxRefreshTime;
    private float m_MaxDestroyTime;
    private bool m_Asyn;

    private IDetector m_CurrentDetector;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="center">场景区域中心</param>
    /// <param name="size">场景区域大小</param>
    /// <param name="asyn">是否异步</param>
    /// <param name="maxCreateCount">最大创建数量</param>
    /// <param name="minCreateCount">最小创建数量</param>
    /// <param name="maxRefreshTime">更新区域时间间隔</param>
    /// <param name="maxDestroyTime">检查销毁时间间隔</param>
    /// <param name="quadTreeDepth">四叉树深度</param>
    public void Init(Vector3 center, Vector3 size, bool asyn, int maxCreateCount, int minCreateCount, float maxRefreshTime, float maxDestroyTime, SceneSeparateTreeType treeType , int quadTreeDepth = 5)
    {
        if (m_IsInitialized)
            return;
        m_QuadTree = new SceneSeparateTree<SceneObject>(treeType, center, size, quadTreeDepth);
        m_LoadedObjectList = new List<SceneObject>();
        //m_PreDestroyObjectQueue = new Queue<SceneObject>();
        m_PreDestroyObjectQueue = new PriorityQueue<SceneObject>(new SceneObjectWeightComparer());
        m_TriggerHandle = new TriggerHandle<SceneObject>(this.TriggerHandle); 

        m_MaxCreateCount = Mathf.Max(0, maxCreateCount);
        m_MinCreateCount = Mathf.Clamp(minCreateCount, 0, m_MaxCreateCount);
        m_MaxRefreshTime = maxRefreshTime;
        m_MaxDestroyTime = maxDestroyTime;
        m_Asyn = asyn;

        m_IsInitialized = true;

        m_RefreshTime = maxRefreshTime;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="center">场景区域中心</param>
    /// <param name="size">场景区域大小</param>
    /// <param name="asyn">是否异步</param>
    public void Init(Vector3 center, Vector3 size, bool asyn, SceneSeparateTreeType treeType)
    {
        Init(center, size, asyn, 25, 15, 1, 5, treeType);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="center">场景区域中心</param>
    /// <param name="size">场景区域大小</param>
    /// <param name="asyn">是否异步</param>
    /// <param name="maxCreateCount">更新区域时间间隔</param>
    /// <param name="minCreateCount">检查销毁时间间隔</param>
    public void Init(Vector3 center, Vector3 size, bool asyn, int maxCreateCount, int minCreateCount, SceneSeparateTreeType treeType)
    {
        Init(center, size, asyn, maxCreateCount, minCreateCount, 0.1f, 2, treeType);
    }

    public void ResetAllData() {
        StopAllCoroutines();
        if (m_QuadTree)
            m_QuadTree.Clear();
        m_QuadTree = null;
        if (m_ProcessTaskQueue != null)
            m_ProcessTaskQueue.Clear();
        if (m_LoadedObjectList != null)
        {
            foreach (var item in m_LoadedObjectList)
            {
                DestroyObject(item, false);
            }
            m_LoadedObjectList.Clear();
        }
        m_ProcessTaskQueue = null;
        m_LoadedObjectList = null;
        m_TriggerHandle = null;
        m_IsInitialized = false;
    }

    void OnDestroy()
    {
        ResetAllData();
    }

    /// <summary>
    /// 添加场景物体
    /// </summary>
    /// <param name="obj"></param>
    public void AddSceneBlockObject(ISceneObject obj)
    {
        if (!m_IsInitialized)
            return;
        if (m_QuadTree == null)
            return;
        if (obj == null)
            return;
        //使用SceneObject包装
        SceneObject sbobj = new SceneObject(obj);
        m_QuadTree.Add(sbobj);
        //如果当前触发器存在，直接物体是否可触发，如果可触发，则创建物体
        if (m_CurrentDetector != null && m_CurrentDetector.IsDetected(sbobj.Bounds))
        {
            DoCreateInternal(sbobj);
        }
    }

    /// <summary>
    /// 刷新触发器
    /// </summary>
    /// <param name="detector">触发器</param>
    public void RefreshDetector(IDetector detector)
    {
        if (!m_IsInitialized)
            return;
        m_RefreshTime += Time.deltaTime;
        //达到刷新时间才刷新，避免区域更新频繁
        if (m_RefreshTime > m_MaxRefreshTime)
        {
            m_OldRefreshPosition = detector.Position;
            m_RefreshTime = 0;
            m_CurrentDetector = detector;
            //进行触发检测
            m_QuadTree.Trigger(detector, m_TriggerHandle);
            //标记超出区域的物体
            MarkOutofBoundsObjs();
            //m_IsInitLoadComplete = true;
        }
        if(m_PreDestroyObjectQueue != null && m_PreDestroyObjectQueue.Count > m_MinCreateCount)
        {
            m_DestroyRefreshTime += Time.deltaTime;
            if (m_DestroyRefreshTime > m_MaxDestroyTime)
            {
                // m_OldDestroyRefreshPosition = detector.Position;
                m_DestroyRefreshTime = 0;
                //删除超出区域的物体
                DestroyOutOfBoundsObjs();
            }
        }
    }

    /// <summary>
    /// 四叉树触发处理函数
    /// </summary>
    /// <param name="data">与当前包围盒发生触发的场景物体</param>
    void TriggerHandle(SceneObject data)
    {
        if (data == null)
            return;
        if (data.Flag == SceneObject.CreateFlag.Old) //如果发生触发的物体已经被创建则标记为新物体，以确保不会被删掉
        {
            data.Weight ++;
            data.Flag = SceneObject.CreateFlag.New;
        }
        else if (data.Flag == SceneObject.CreateFlag.OutofBounds)//如果发生触发的物体已经被标记为超出区域，则从待删除列表移除该物体，并标记为新物体
        {
            data.Flag = SceneObject.CreateFlag.New;
            //if (m_PreDestroyObjectList.Remove(data))
            {
                m_LoadedObjectList.Add(data);
            }
        }
        else if (data.Flag == SceneObject.CreateFlag.None) //如果发生触发的物体未创建则创建该物体并加入已加载的物体列表
        {
            DoCreateInternal(data);
        }
    }

    //执行创建物体
    private void DoCreateInternal(SceneObject data)
    {
        //加入已加载列表
        m_LoadedObjectList.Add(data);
        //创建物体
        CreateObject(data, m_Asyn);
    }

    /// <summary>
    /// 标记离开视野的物体
    /// </summary>
    void MarkOutofBoundsObjs()
    {
        if (m_LoadedObjectList == null)
            return;
        int i = 0;
        while (i < m_LoadedObjectList.Count)
        {
            if (m_LoadedObjectList[i].Flag == SceneObject.CreateFlag.Old)//已加载物体标记仍然为Old，说明该物体没有进入触发区域，即该物体在区域外
            {
                m_LoadedObjectList[i].Flag = SceneObject.CreateFlag.OutofBounds;
                //m_PreDestroyObjectList.Add(m_LoadedObjectList[i]);
                if (m_MinCreateCount == 0)//如果最小创建数为0直接删除
                {
                    DestroyObject(m_LoadedObjectList[i], m_Asyn);
                }
                else
                {
                    //m_PreDestroyObjectQueue.Enqueue(m_LoadedObjectList[i]);
                    m_PreDestroyObjectQueue.Push(m_LoadedObjectList[i]);//加入待删除队列
                }
                m_LoadedObjectList.RemoveAt(i);

            }
            else
            {
                m_LoadedObjectList[i].Flag = SceneObject.CreateFlag.Old;//其它物体标记为旧
                i++;
            }
        }
    }

    /// <summary>
    /// 删除超出区域外的物体
    /// </summary>
    void DestroyOutOfBoundsObjs()
    {
        while(m_PreDestroyObjectQueue.Count>m_MinCreateCount)
        {

            //var obj = m_PreDestroyObjectQueue.Dequeue();
            var obj = m_PreDestroyObjectQueue.Pop();
            if (obj == null)
                continue;
            if (obj.Flag == SceneObject.CreateFlag.OutofBounds)
            {
                DestroyObject(obj, m_Asyn);
            }
        }
    }

    /// <summary>
    /// 创建物体
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="asyn"></param>
    private void CreateObject(SceneObject obj, bool asyn)
    {
        if (obj == null)
            return;
        if (obj.TargetObj == null)
            return;
        if (obj.Flag == SceneObject.CreateFlag.None)
        {
            if (!asyn)
                CreateObjectSync(obj);
            else
                ProcessObjectAsyn(obj, true);
            obj.Flag = SceneObject.CreateFlag.New;//被创建的物体标记为New
        }
    }

    /// <summary>
    /// 删除物体
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="asyn"></param>
    private void DestroyObject(SceneObject obj, bool asyn)
    {
        if (obj == null)
            return;
        if (obj.Flag == SceneObject.CreateFlag.None)
            return;
        if (obj.TargetObj == null)
            return;
        if (!asyn)
            DestroyObjectSync(obj);
        else
            ProcessObjectAsyn(obj, false);
        obj.Flag = SceneObject.CreateFlag.None;//被删除的物体标记为None
    }

    /// <summary>
    /// 同步方式创建物体
    /// </summary>
    /// <param name="obj"></param>
    private void CreateObjectSync(SceneObject obj)
    {
        if (obj.ProcessFlag == SceneObject.CreatingProcessFlag.IsPrepareDestroy)//如果标记为IsPrepareDestroy表示物体已经创建并正在等待删除，则直接设为None并返回
        {
            obj.ProcessFlag = SceneObject.CreatingProcessFlag.None;
            return;
        }
        obj.OnShow(transform);//执行OnShow
    }

    /// <summary>
    /// 同步方式销毁物体
    /// </summary>
    /// <param name="obj"></param>
    private void DestroyObjectSync(SceneObject obj)
    {
        if (obj.ProcessFlag == SceneObject.CreatingProcessFlag.IsPrepareCreate)//如果物体标记为IsPrepareCreate表示物体未创建并正在等待创建，则直接设为None并放回
        {
            obj.ProcessFlag = SceneObject.CreatingProcessFlag.None;
            return;
        }
        obj.OnHide();//执行OnHide
    }

    /// <summary>
    /// 异步处理
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="create"></param>
    private void ProcessObjectAsyn(SceneObject obj, bool create)
    {
        if (create)
        {
            if (obj.ProcessFlag == SceneObject.CreatingProcessFlag.IsPrepareDestroy)//表示物体已经创建并等待销毁，则设置为None并跳过
            {
                obj.ProcessFlag = SceneObject.CreatingProcessFlag.None;
                return;
            }
            if (obj.ProcessFlag == SceneObject.CreatingProcessFlag.IsPrepareCreate)//已经开始等待创建，则跳过
                return;
            obj.ProcessFlag = SceneObject.CreatingProcessFlag.IsPrepareCreate;//设置为等待开始创建
        }
        else
        {
            if (obj.ProcessFlag == SceneObject.CreatingProcessFlag.IsPrepareCreate)//表示物体未创建并等待创建，则设置为None并跳过
            {
                obj.ProcessFlag = SceneObject.CreatingProcessFlag.None;
                return;
            }
            if (obj.ProcessFlag == SceneObject.CreatingProcessFlag.IsPrepareDestroy)//已经开始等待销毁，则跳过
                return;
            obj.ProcessFlag = SceneObject.CreatingProcessFlag.IsPrepareDestroy;//设置为等待开始销毁
        }
        if (m_ProcessTaskQueue == null)
            m_ProcessTaskQueue = new Queue<SceneObject>();
        m_ProcessTaskQueue.Enqueue(obj);//加入
        if (!m_IsTaskRunning)
        {
            StartCoroutine(AsynTaskProcess());//开始协程执行异步任务
        }
    }

    /// <summary>
    /// 异步任务
    /// </summary>
    /// <returns></returns>
    private IEnumerator AsynTaskProcess()
    {
        if (m_ProcessTaskQueue == null)
            yield return 0;
        m_IsTaskRunning = true;
        while (m_ProcessTaskQueue.Count > 0)
        {
            var obj = m_ProcessTaskQueue.Dequeue();
            if (obj != null)
            {
                if (obj.ProcessFlag == SceneObject.CreatingProcessFlag.IsPrepareCreate)//等待创建
                {
                    obj.ProcessFlag = SceneObject.CreatingProcessFlag.None;
                    if (obj.OnShow(transform))
                    {
                        if (m_WaitForFrame == null)
                            m_WaitForFrame = new WaitForEndOfFrame();
                        yield return m_WaitForFrame;
                    }
                }
                else if (obj.ProcessFlag == SceneObject.CreatingProcessFlag.IsPrepareDestroy)//等待销毁
                {
                    obj.ProcessFlag = SceneObject.CreatingProcessFlag.None;
                    obj.OnHide();
                    if (m_WaitForFrame == null)
                        m_WaitForFrame = new WaitForEndOfFrame();
                    yield return m_WaitForFrame;
                }
            }
        }
        m_IsTaskRunning = false;
    }

    private class SceneObjectWeightComparer : IComparer<SceneObject>
    {

        public int Compare(SceneObject x, SceneObject y)
        {
            if (y.Weight < x.Weight)
                return 1;
            else if (y.Weight == x.Weight)
                return 0;
            return -1;
        }
    }

#if UNITY_EDITOR
    public int debug_DrawMinDepth = 0;
    public int debug_DrawMaxDepth = 5;
    public bool debug_DrawObj = true;
    void OnDrawGizmosSelected()
    {
        Color mindcolor = new Color32(0, 66, 255, 255);
        Color maxdcolor = new Color32(133, 165, 255, 255);
        Color objcolor = new Color32(0, 210, 255, 255);
        Color hitcolor = new Color32(255, 216, 0, 255);
        if (m_QuadTree != null)
            m_QuadTree.DrawTree(mindcolor, maxdcolor, objcolor, hitcolor, debug_DrawMinDepth, debug_DrawMaxDepth, debug_DrawObj);
    }
#endif
}
