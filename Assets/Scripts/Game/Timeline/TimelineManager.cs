using System.Collections.Generic;

namespace UnityMMO
{
public class TimelineInfo
{
    public string ResPath;
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

    public void AddTimeline(long id, TimelineInfo info)
    {
        if (!pool.ContainsKey(id))
            pool[id] = new List<TimelineInfo>();
        pool[id].Add(info);
    }

    // public TimelineInfo GetTimeline(long id, int index)
    // {
    //     TimelineInfo result = null;
    //     if (pool.ContainsKey(id) && pool[id].Count>index)
    //         result = pool[id][index];
    //     return result;
    // }

    public TimelineInfo PopTimeline(long id)
    {
        TimelineInfo result = null;
        if (pool.ContainsKey(id) && pool[id].Count>0)
        {
            result = pool[id][0];
            pool[id].Remove(result);
        }
        return result;
    }

    private TimelineManager()
    {

    }
}
}