using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityMMO;
using SprotoType;

[DisableAutoCreation]
public class UploadMainRolePosSystem : BaseComponentSystem
{
    float lastSynchTime = 0;
    ComponentGroup group;
    public UploadMainRolePosSystem(GameWorld world) : base(world) {}

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        group = GetComponentGroup(typeof(Position), typeof(TargetPosition), typeof(MainRoleTag));
    }

    protected override void OnUpdate()
    {
        if (Time.time - lastSynchTime < 0.2 || !GameVariable.IsNeedSynchSceneInfo)
            return;
        lastSynchTime = Time.time;
        var positions = group.GetComponentDataArray<Position>();
        var targetPositions = group.GetComponentDataArray<TargetPosition>();
        long synchTime = System.DateTime.Now.Millisecond;
        for (int i=0; i<targetPositions.Length; i++)
        {
            var targetPos = targetPositions[i].Value;
            var pos = positions[i].Value;
            var distance = Vector3.Distance(targetPos, pos);
            if (distance <= 0.5)
                continue;
            scene_walk.request walk = new scene_walk.request();
            Debug.Log("upload pos"+pos.ToString());
            walk.start_x = (int)(pos.x*GameConst.RealToLogic);
            walk.start_y = (int)(pos.y*GameConst.RealToLogic);
            walk.start_z = (int)(pos.z*GameConst.RealToLogic);
            walk.end_x = (int)(targetPos.x*GameConst.RealToLogic);
            walk.end_y = (int)(targetPos.y*GameConst.RealToLogic);
            walk.end_z = (int)(targetPos.z*GameConst.RealToLogic);
            walk.time = synchTime;
            NetMsgDispatcher.GetInstance().SendMessage<Protocol.scene_walk>(walk);
        }
    }
}