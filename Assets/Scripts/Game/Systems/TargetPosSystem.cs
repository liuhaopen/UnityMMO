using Unity.Entities;
using Unity.Mathematics;
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
        group = GetComponentGroup(typeof(TargetPosition), typeof(Transform), typeof(MoveSpeed), typeof(RoleMoveQuery));
    }

    protected override void OnUpdate()
    {
        float dt = Time.deltaTime;
        var entities = group.GetEntityArray();
        var targetPositions = group.GetComponentDataArray<TargetPosition>();
        var speeds = group.GetComponentDataArray<MoveSpeed>();
        var transforms = group.GetComponentArray<Transform>();
        var moveQuerys = group.GetComponentArray<RoleMoveQuery>();
        for (int i=0; i<targetPositions.Length; i++)
        {
            // if (m_world.GetEntityManager().HasComponent<MainRoleTag>(entities[i]))
                // continue;
            var targetPos = targetPositions[i].Value;
            var speed = speeds[i].Value;
            var curTrans = transforms[i];
            float3 startPos = curTrans.localPosition;
            var moveDir = targetPos-startPos;
            moveDir.y = -20;
            var newPos = startPos+moveDir*speed/GameConst.SpeedFactor*dt;
            // curTrans.localPosition = newPos;
            var moveQuery = moveQuerys[i];
            moveQuery.moveQueryStart = startPos;
            moveQuery.moveQueryEnd = newPos;

            Vector3 targetDirection = new Vector3(moveDir.x, moveDir.y, moveDir.z);
            Vector3 lookDirection = targetDirection.normalized;
            Quaternion freeRotation = Quaternion.LookRotation(lookDirection, curTrans.up);
            var diferenceRotation = freeRotation.eulerAngles.y - curTrans.eulerAngles.y;
            var eulerY = curTrans.eulerAngles.y;

            if (diferenceRotation < 0 || diferenceRotation > 0) eulerY = freeRotation.eulerAngles.y;
            var euler = new Vector3(0, eulerY, 0);
            curTrans.rotation = Quaternion.Slerp(curTrans.rotation, Quaternion.Euler(euler), Time.deltaTime);
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
        group = GetComponentGroup(typeof(UserCommand), typeof(TargetPosition), typeof(Transform), typeof(MoveSpeed));
    }

    protected override void OnUpdate()
    {
        float dt = Time.deltaTime;
        var userCommandArray = group.GetComponentDataArray<UserCommand>();
        var targetPosArray = group.GetComponentDataArray<TargetPosition>();
        var posArray = group.GetComponentArray<Transform>();
        var moveSpeedArray = group.GetComponentDataArray<MoveSpeed>();
        if (userCommandArray.Length==0)
            return;
        var userCommand = userCommandArray[0];
        // if (userCommand.moveMagnitude > 0)
        // {
            Vector2 input = new Vector2();
            input.x = Input.GetAxis("Horizontal");
	        input.y = Input.GetAxis("Vertical");

            var forward = SceneMgr.Instance.FreeLookCameraTrans.TransformDirection(Vector3.forward);
            forward.y = 0;
            //get the right-facing direction of the referenceTransform
            var right = SceneMgr.Instance.FreeLookCameraTrans.TransformDirection(Vector3.right);
            // determine the direction the player will face based on input and the referenceTransform's right and forward directions
            float3 targetDirection = input.x * right + input.y * forward;
            // Debug.Log("targetDirection : "+targetDirection.x + " "+targetDirection.y+" "+targetDirection.z);
            // var moveYawRotation = Quaternion.Euler(0, userCommand.lookYaw + userCommand.moveYaw, 0);
            // var moveVec = moveYawRotation * Vector3.forward * userCommand.moveMagnitude;
            // var lastTargetPos = targetPosArray[0].Value;
            float3 curPos = posArray[0].localPosition;
            var speed = moveSpeedArray[0].Value;
            var newTargetPos = new TargetPosition();
            // newTargetPos.Value = lastTargetPos+userCommand.moveYaw*(speed/GameConst.SpeedFactor*dt);
            newTargetPos.Value = curPos+targetDirection*(speed/GameConst.SpeedFactor*1);//延着方向前进1秒为目标坐标
            //TODO:通过navmesh判断是否障碍区，是的话取得最近点
            targetPosArray[0] = newTargetPos;
            // Debug.Log("curPos : "+curPos.x+" "+curPos.y+" "+curPos.z+" dir:"+targetDirection.x+" "+targetDirection.z);
        // }
    }
}


// [DisableAutoCreation]
// class MovementHandleCollision : BaseComponentDataSystem<RoleState, RoleMoveQuery>
// {
//     public MovementHandleCollision(GameWorld world) : base(world)
//     {
//         // ExtraComponentRequirements = new ComponentType[] { typeof(ServerEntity) } ;
//     }
    
//     protected override void Update(Entity entity, RoleState roleState, RoleMoveQuery query)
//     {
//         var time = m_world.worldTime;
//         var predictedState = EntityManager.GetComponentData<CharacterPredictedData>(charAbility.character);
    
//         // Manually calculate resulting velocity as characterController.velocity is linked to Time.deltaTime
//         var newPos = query.moveQueryResult;
//         // var oldPos = query.moveQueryStart;
//         // var velocity = (newPos - oldPos) / time.tickDuration;
    
//         // predictedState.velocity = velocity;
//         roleState.transform.position = query.moveQueryResult;
        
//         // EntityManager.SetComponentData(charAbility.character, predictedState);
//     }
// }
