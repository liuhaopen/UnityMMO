using System;
using Cinemachine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityMMO;
using UnityMMO.Component;
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
    // ComponentGroup RequestGroup;
    protected override void OnCreate()
    {
        base.OnCreate();
        // RequestGroup = GetComponentGroup(typeof(TimelineSpawnRequest));
    }

    protected override void OnUpdate()
    {
        float dt = Time.deltaTime;
        var dic = TimelineManager.GetInstance().GetTimelineDic();
        foreach (var item in dic)
        {
            var itemlineList = item.Value;
            if (itemlineList.Count > 0)
            {
                var timelineInfo = item.Value[0];
                var isOk = Process(timelineInfo);
                if (isOk)
                    itemlineList.RemoveAt(0);
            }
        }
    }

    bool Process(TimelineInfo timelineInfo)
    {
        var timelineState = EntityManager.GetComponentData<TimelineState>(timelineInfo.Owner);
        bool isCanInterrupt = timelineState.InterruptStatus == TimelineState.InterruptState.Allow;
        if (!isCanInterrupt)
            return false;
        ResourceManager.GetInstance().LoadAsset<PlayableAsset>(timelineInfo.ResPath, delegate(UnityEngine.Object[] objs)
        {
            if (objs==null || objs.Length<=0)
                return;
            var entity = timelineInfo.Owner;
            var playerDirector = EntityManager.GetComponentObject<PlayableDirector>(entity);
            playerDirector.Stop();
            playerDirector.playableAsset = objs[0] as PlayableAsset;
            var looksInfo = EntityManager.GetComponentData<LooksInfo>(entity);
            if (looksInfo.CurState != LooksInfo.State.Loaded)
                return;
            var looksEntity = looksInfo.LooksEntity;
            var animator = EntityManager.GetComponentObject<Animator>(looksEntity);
            foreach (var at in playerDirector.playableAsset.outputs)
            {
                if (at.sourceObject == null)
                {
                    // Debug.Log("detect nil track in : "+timelineInfo.ResPath);
                    continue;
                }
                if (at.sourceObject.GetType() == typeof(AnimationTrack))
                {
                    playerDirector.SetGenericBinding(at.sourceObject, animator);
                }
                else if (at.sourceObject.GetType() == typeof(CinemachineTrack))
                {
                    CinemachineBrain mainCamBrian = SceneMgr.Instance.MainCameraTrans.GetComponent<Cinemachine.CinemachineBrain>();//将主摄像机传入
                    playerDirector.SetGenericBinding(at.sourceObject, mainCamBrian);
                    CinemachineTrack cinemachineTrack = (CinemachineTrack)at.sourceObject;
                    try
                    {
                        foreach (var c in cinemachineTrack.GetClips())
                        {
                            CinemachineShot shot = (CinemachineShot)c.asset;
                            // playerDirector.SetReferenceValue(shot.VirtualCamera.exposedName, vCams[idx++]);
                        }
                    }
                    catch (Exception)
                    {
                        Debug.LogError("Clip Num InEqual Cam Num");
                    }
                }
                else if (at.streamName.StartsWith("ParticleTrack"))
                {
                    var nameParts = at.streamName.Split('_');
                    var hangPointName = "root";
                    if (nameParts.Length > 1)
                    {
                        hangPointName = nameParts[1];
                    }
                    var ct = at.sourceObject as ControlTrack;
                    var looksTrans = EntityManager.GetComponentObject<Transform>(looksEntity);
                    var particleParent = looksTrans.Find(hangPointName);
                    foreach (var info in ct.GetClips())
                    {
                        var cpa = info.asset as ControlPlayableAsset;
                        playerDirector.SetReferenceValue(cpa.sourceGameObject.exposedName, particleParent.gameObject);
                    }
                }
                // Debug.Log("timelineInfo.Param != null : "+(timelineInfo.Param != null).ToString());
                if (timelineInfo.Param != null)
                {
                    var nameParts = at.streamName.Split('_');
                    // Debug.Log("nameParts : "+nameParts[0]);
                    if (timelineInfo.Param.ContainsKey(nameParts[0]))
                    {
                        var ta = at.sourceObject as TrackAsset;
                        foreach (var info in ta.GetClips())
                        {
                            // var cpa = info.asset as ScriptPlayable<>;
                            var cpa = info.asset as ParamPlayableAsset;
                            if (cpa != null)
                            {
                                cpa.Param = timelineInfo.Param[nameParts[0]];
                            }
                            else
                            {
                                Debug.LogError("error : trying to set param with no param playable asset name : "+at.streamName);
                            }
                        }
                        
                    }
                }
            }
            playerDirector.Play();
        });
        return true;
    }
}