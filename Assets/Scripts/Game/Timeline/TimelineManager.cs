using System.Collections.Generic;
using Unity.Entities;
using System;
using UnityMMO.Component;

namespace UnityMMO
{
public class TimelineInfo
{
    public Entity Owner;
    public string ResPath;
    public enum Event
    {
        AfterAdd,
        StartPlay
    }
    public Action<Event> StateChange;
    public Dictionary<string, object> Param;
}
public class TimelineManager
{
    static TimelineManager instance;
    Dictionary<long, List<TimelineInfo>> pool;
    
    public static TimelineManager GetInstance()
    {
        if (instance != null)
            return instance;
        instance = new TimelineManager();
        return instance;
    }

    public void Init()
    {
        pool = new Dictionary<long, List<TimelineInfo>>();
    }

    public void AddTimeline(long id, TimelineInfo info, EntityManager entityMgr)
    {
        if (info.Owner == Entity.Null || !entityMgr.HasComponent<TimelineState>(info.Owner))
            return;
        var state = entityMgr.GetComponentData<TimelineState>(info.Owner);
        bool isNeedIgnore = state.NewStatus == TimelineState.NewState.Forbid;
        if (isNeedIgnore)
            return;
        if (!pool.ContainsKey(id))
            pool[id] = new List<TimelineInfo>();
        if (pool[id].Count >= 3)
            return;
        pool[id].Add(info);
        if (info.StateChange != null)
        {
            info.StateChange(TimelineInfo.Event.AfterAdd);
        }
    }

    public Dictionary<long, List<TimelineInfo>> GetTimelineDic()
    {
        return pool;
    }

    private TimelineManager()
    {

    }
}
}