using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityMMO;
using XLuaFramework;

public struct TimelineSpawnRequest : IComponentData
{
    public Entity Owner;
    public long TimelineID;

    public static void Create(EntityManager entityMgr, Entity Owner, long TimelineID)
    {
        var data = new TimelineSpawnRequest();
        data.Owner = Owner;
        data.TimelineID = TimelineID;
        Entity entity = entityMgr.CreateEntity(typeof(TimelineSpawnRequest));
        entityMgr.SetComponentData(entity, data);
    }
}


[DisableAutoCreation]
public class TimelineSpawnSystem : BaseComponentSystem
{
    public TimelineSpawnSystem(GameWorld world) : base(world) {}

    ComponentGroup RequestGroup;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        RequestGroup = GetComponentGroup(typeof(TimelineSpawnRequest));
    }

    protected override void OnUpdate()
    {
        float dt = Time.deltaTime;
        var requestArray = RequestGroup.GetComponentDataArray<TimelineSpawnRequest>();
        if (requestArray.Length == 0)
            return;

        var requestEntityArray = RequestGroup.GetEntityArray();
        
        // Copy requests as spawning will invalidate Group
        var requests = new TimelineSpawnRequest[requestArray.Length];
        for (var i = 0; i < requestArray.Length; i++)
        {
            requests[i] = requestArray[i];
            PostUpdateCommands.DestroyEntity(requestEntityArray[i]);
        }

        for(var i = 0; i < requests.Length; i++)
        {
            Process(requests[i]);
        }
    }

    void Process(TimelineSpawnRequest req)
    {
        var _playerDirector = EntityManager.GetComponentObject<PlayableDirector>(req.Owner);
        if (_playerDirector.state == PlayState.Playing)
            return;
        var timelineInfo = TimelineManager.GetInstance().PopTimeline(req.TimelineID);
        ResourceManager.GetInstance().LoadAsset<PlayableAsset>(timelineInfo.ResPath, delegate(UnityEngine.Object[] objs)
        {
            if (objs==null || objs.Length<=0)
                return;
            var entity = req.Owner;
            // var mainRole = RoleMgr.GetInstance().GetMainRole();
            // var playerDirector = mainRole.GetComponent<PlayableDirector>();
            var playerDirector = EntityManager.GetComponentObject<PlayableDirector>(entity);
            playerDirector.playableAsset = objs[0] as PlayableAsset;
            var looksInfo = EntityManager.GetComponentData<LooksInfo>(entity);
            if (looksInfo.CurState != LooksInfo.State.Loaded)
                return;
            var looksEntity = looksInfo.LooksEntity;
            // var animator = mainRole.GetComponentInChildren<Animator>();
            var animator = EntityManager.GetComponentObject<Animator>(looksEntity);
            foreach (var at in playerDirector.playableAsset.outputs)
            {
                if (at.streamName.StartsWith("AnimationTrack"))
                {
                    playerDirector.SetGenericBinding(at.sourceObject, animator);
                }
                else if (at.streamName.StartsWith("ParticleTrack"))
                {
                    var ct = at.sourceObject as ControlTrack;
                    var looksTrans = EntityManager.GetComponentObject<Transform>(looksEntity);
                    var particleParent = looksTrans.Find("root");
                    foreach (var info in ct.GetClips())
                    {
                        if (info.displayName.StartsWith("particle"))
                        {
                            var cpa = info.asset as ControlPlayableAsset;
                            playerDirector.SetReferenceValue(cpa.sourceGameObject.exposedName, particleParent.gameObject);
                        
                        }
                    }
                }
            }
            playerDirector.Play();
        });
    }
}