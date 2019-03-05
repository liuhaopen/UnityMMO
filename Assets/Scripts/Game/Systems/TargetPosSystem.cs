using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityMMO;

[DisableAutoCreation]
public class TargetPosSystem : BaseComponentSystem
{
    public TargetPosSystem(GameWorld world) : base(world) {}

    ComponentGroup group;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        group = GetComponentGroup(typeof(TargetPosition), typeof(Position), typeof(MoveSpeed));
    }

    protected override void OnUpdate()
    {
        float dt = Time.deltaTime;
        var entities = group.GetEntityArray();
        var targetPositions = group.GetComponentDataArray<TargetPosition>();
        var speeds = group.GetComponentDataArray<MoveSpeed>();
        var curPositions = group.GetComponentDataArray<Position>();
        for (int i=0; i<targetPositions.Length; i++)
        {
            if (m_world.GetEntityManager().HasComponent<MainRoleTag>(entities[i]))
                continue;
            
            var targetPos = targetPositions[i].Value;
            var speed = speeds[i].Value;
            var curPos = curPositions[i];
            var startPos = curPos.Value;
            var newPos = startPos+(targetPos-startPos)*speed/GameConst.SpeedFactor*dt;
            curPos.Value = newPos;
            curPositions[i] = curPos;
        }
    }
}


[DisableAutoCreation]
public class CreateTargetPosFromUserInputSystem : BaseComponentSystem
{
    public CreateTargetPosFromUserInputSystem(GameWorld world) : base(world) {}

    ComponentGroup group;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        group = GetComponentGroup(typeof(UserCommand), typeof(TargetPosition), typeof(Position), typeof(MoveSpeed));
    }

    protected override void OnUpdate()
    {
        float dt = Time.deltaTime;
        var userCommandArray = group.GetComponentDataArray<UserCommand>();
        var targetPosArray = group.GetComponentDataArray<TargetPosition>();
        var moveSpeedArray = group.GetComponentDataArray<MoveSpeed>();
        if (userCommandArray.Length==0)
            return;
        var userCommand = userCommandArray[0];
        if (userCommand.moveMagnitude > 0)
        {
            var moveYawRotation = Quaternion.Euler(0, userCommand.lookYaw + userCommand.moveYaw, 0);
            var moveVec = moveYawRotation * Vector3.forward * userCommand.moveMagnitude;

            var lastTargetPos = targetPosArray[0].Value;
            var speed = moveSpeedArray[0].Value;
            var newTargetPos = new TargetPosition();
            newTargetPos.Value = lastTargetPos+userCommand.moveYaw*(speed/GameConst.SpeedFactor*dt);
            targetPosArray[0] = newTargetPos;
        }
    }
}