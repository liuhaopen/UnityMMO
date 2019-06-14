using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityMMO;

[DisableAutoCreation]
public class MovementUpdateSystem : BaseComponentSystem
{
    public MovementUpdateSystem(GameWorld world) : base(world) {}

    EntityQuery group;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        // group = GetComponentGroup(typeof(TargetPosition), typeof(Transform), typeof(MoveSpeed), typeof(MoveQuery), typeof(LocomotionState), typeof(PosOffset), typeof(PosSynchInfo));
        group = GetEntityQuery(typeof(TargetPosition), typeof(Transform), typeof(MoveSpeed), typeof(MoveQuery), typeof(LocomotionState), typeof(PosOffset));
    }

    protected override void OnUpdate()
    {
        float dt = Time.deltaTime;
        var entities = group.ToEntityArray(Allocator.TempJob);
        var targetPositions = group.ToComponentDataArray<TargetPosition>(Allocator.TempJob);
        var speeds = group.ToComponentDataArray<MoveSpeed>(Allocator.TempJob);
        var transforms = group.ToComponentArray<Transform>();
        var moveQuerys = group.ToComponentArray<MoveQuery>();
        var locoStates = group.ToComponentDataArray<LocomotionState>(Allocator.TempJob);
        var posOffsets = group.ToComponentDataArray<PosOffset>(Allocator.TempJob);
        for (int i=0; i<targetPositions.Length; i++)
        {
            var targetPos = targetPositions[i].Value;
            var speed = speeds[i].Value;
            var posOffset = posOffsets[i].Value;
            var curLocoStateObj = locoStates[i];
            // if (speed <= 0 || curLocoStateObj.LocoState==LocomotionState.State.BeHit|| curLocoStateObj.LocoState==LocomotionState.State.Dead)
            if (speed <= 0)
                continue;
            var curTrans = transforms[i];
            float3 startPos = curTrans.localPosition;
            var moveDir = targetPos-startPos;
            var groundDir = moveDir;
            groundDir.y = 0;
            float moveDistance = Vector3.Magnitude(groundDir);
            groundDir = Vector3.Normalize(groundDir);
            bool isMoveWanted = moveDistance>0.01f;
            
            var newLocoState = LocomotionState.State.StateNum;
            var phaseDuration = Time.time - curLocoStateObj.StartTime;
            var curLocoState = curLocoStateObj.LocoState;
            // bool isOnGround = curLocoState == LocomotionState.State.Idle || curLocoState == LocomotionState.State.Run || curLocoState == LocomotionState.State.Sprint;
            bool isOnGround = curLocoStateObj.IsOnGround();
            // Debug.Log("isOnGround : "+isOnGround.ToString()+" movewanted:"+isMoveWanted.ToString());
            if (isOnGround)
            {
                if (isMoveWanted)
                    newLocoState = LocomotionState.State.Run;
                else
                    newLocoState = LocomotionState.State.Idle;
            }
            float ySpeed = 0;
            bool isClickJump = false;
            if (EntityManager.HasComponent<ActionData>(entities[i]))
            {
                var actionData = EntityManager.GetComponentData<ActionData>(entities[i]);
                isClickJump = actionData.Jump == 1;
            }
            // Jump
            if (isOnGround)
                curLocoStateObj.JumpCount = 0;

            if (isClickJump && isOnGround)
            {
                curLocoStateObj.JumpCount = 1;
                newLocoState = LocomotionState.State.Jump;
            }

            if (isClickJump && curLocoStateObj.IsInJump() && curLocoStateObj.JumpCount < 3)
            {
                curLocoStateObj.JumpCount = curLocoStateObj.JumpCount + 1;
                newLocoState = curLocoStateObj.JumpCount==2?LocomotionState.State.DoubleJump:LocomotionState.State.TrebleJump;
            }

            if (curLocoStateObj.LocoState == LocomotionState.State.Jump || curLocoStateObj.LocoState == LocomotionState.State.DoubleJump || curLocoStateObj.LocoState == LocomotionState.State.TrebleJump)
            {
                if (phaseDuration >= GameConst.JumpAscentDuration[curLocoStateObj.LocoState-LocomotionState.State.Jump])
                    newLocoState = LocomotionState.State.InAir;
            }
            // Debug.Log("newLocoState ; "+newLocoState.ToString()+" curLocoStateObj:"+curLocoStateObj.LocoState.ToString()+" jumpcount:"+curLocoStateObj.JumpCount);
            if (newLocoState != LocomotionState.State.StateNum && newLocoState != curLocoState)
            {
                curLocoStateObj.LocoState = newLocoState;
                curLocoStateObj.StartTime = Time.time;
            }
            if (curLocoStateObj.LocoState == LocomotionState.State.Jump || curLocoStateObj.LocoState == LocomotionState.State.DoubleJump || curLocoStateObj.LocoState == LocomotionState.State.TrebleJump)
            {
                ySpeed = GameConst.JumpAscentHeight[curLocoStateObj.LocoState-LocomotionState.State.Jump] / GameConst.JumpAscentDuration[curLocoStateObj.LocoState-LocomotionState.State.Jump] - GameConst.Gravity;
            }
            locoStates[i] = curLocoStateObj;

            // Debug.Log("ySpeed : "+ySpeed+" state :"+newLocoState.ToString());
            float3 newPos;
            if (moveDistance < speed/GameConst.SpeedFactor*dt)
            {
                //目标已经离得很近了
                newPos = targetPos;
            }
            else
            {
                newPos = startPos+groundDir*speed/GameConst.SpeedFactor*dt;
            }
            newPos.y = startPos.y;
            //模仿重力，人物需要贴着地面走，有碰撞检测的所以不怕
            newPos.y += (GameConst.Gravity+ySpeed) * dt;
            newPos += posOffset;
            var moveQuery = moveQuerys[i];
            moveQuery.moveQueryStart = startPos;
            //不能直接设置新坐标，因为需要和地形做碰撞处理什么的，所以利用CharacterController走路，在HandleMovementQueries才设置新坐标
            moveQuery.moveQueryEnd = newPos;

            //change role rotation
            if (isMoveWanted)
            {
                Vector3 targetDirection = new Vector3(groundDir.x, groundDir.y, groundDir.z);
                Vector3 lookDirection = targetDirection.normalized;
                Quaternion freeRotation = Quaternion.LookRotation(lookDirection, curTrans.up);
                var diferenceRotation = freeRotation.eulerAngles.y - curTrans.eulerAngles.y;
                var eulerY = curTrans.eulerAngles.y;

                if (diferenceRotation < 0 || diferenceRotation > 0) eulerY = freeRotation.eulerAngles.y;
                var euler = new Vector3(0, eulerY, 0);
                curTrans.rotation = Quaternion.Slerp(curTrans.rotation, Quaternion.Euler(euler), Time.deltaTime*50);
            }
        }
        entities.Dispose();
        targetPositions.Dispose();
        speeds.Dispose();
        locoStates.Dispose();
        posOffsets.Dispose();
    }
}


[DisableAutoCreation]
class MovementHandleGroundCollision : BaseComponentSystem
{
    public MovementHandleGroundCollision(GameWorld world) : base(world)
    {
    }

    EntityQuery group;
    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        group = GetEntityQuery(typeof(Transform), typeof(MoveQuery), typeof(LocomotionState), typeof(TargetPosition));
    }

    protected override void OnUpdate()
    {
        var locoStates = group.ToComponentDataArray<LocomotionState>(Allocator.TempJob);
        var targets = group.ToComponentDataArray<TargetPosition>(Allocator.TempJob);
        var querys = group.ToComponentArray<MoveQuery>();
        var transforms = group.ToComponentArray<Transform>();
        for (int i = 0; i < locoStates.Length; i++)
        {
            var locoState = locoStates[i];
            if (locoState.LocoState == LocomotionState.State.Dead)
                continue;
            var query = querys[i];
            // Check for ground change (hitting ground or leaving ground)  
            var isOnGround = locoState.IsOnGround();
            // Debug.Log("isOnGround : "+isOnGround.ToString()+" query:"+query.isGrounded);
            if (isOnGround != query.isGrounded)
            {
                if (query.isGrounded)
                {
                    Vector3 startPos = transforms[i].localPosition;
                    Vector3 targetPos = targets[i].Value;
                    var groundDir = targetPos-startPos;
                    bool isMoveWanted = Vector3.Magnitude(groundDir)>0.01f;
                    if (isMoveWanted)
                    {
                        locoState.LocoState = LocomotionState.State.Run;  
                    }
                    else
                    {
                        locoState.LocoState = LocomotionState.State.Idle;    
                    }
                }
                else
                {
                    locoState.LocoState = LocomotionState.State.InAir;                    
                }
                
                locoState.StartTime = Time.time;
                locoStates[i] = locoState;
            }
            // Manually calculate resulting velocity as characterController.velocity is linked to Time.deltaTime
            // var newPos = query.moveQueryResult;
            // var oldPos = query.moveQueryStart;
            // var velocity = (newPos - oldPos) / Time.deltaTime;
            // locoState.velocity = velocity;
            // locoState.position = query.moveQueryResult;
            // EntityManager.SetComponentData(charAbility.character, locoState);
        }
        locoStates.Dispose();
        targets.Dispose();
    }
}


[DisableAutoCreation]
public class CreateTargetPosFromUserInputSystem : BaseComponentSystem
{
    public CreateTargetPosFromUserInputSystem(GameWorld world) : base(world) {}

    EntityQuery group;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        group = GetEntityQuery(typeof(TargetPosition), typeof(Transform), typeof(MoveSpeed), typeof(PosSynchInfo), typeof(LocomotionState));
    }

    protected override void OnUpdate()
    {
        float dt = Time.deltaTime;
        var targetPosArray = group.ToComponentDataArray<TargetPosition>(Allocator.TempJob);
        var posArray = group.ToComponentArray<Transform>();
        var moveSpeedArray = group.ToComponentDataArray<MoveSpeed>(Allocator.TempJob);
        var locoStates = group.ToComponentDataArray<LocomotionState>(Allocator.TempJob);
        var curLocoStateObj = locoStates[0];
        if (curLocoStateObj.LocoState==LocomotionState.State.BeHit|| curLocoStateObj.LocoState==LocomotionState.State.Dead)
            return;
        var input = GameInput.GetInstance().JoystickDir;
        if (input.sqrMagnitude > 0)
        {
            var forward = SceneMgr.Instance.MainCameraTrans.TransformDirection(Vector3.forward);
            forward.y = 0;
            var right = SceneMgr.Instance.MainCameraTrans.TransformDirection(Vector3.right);
            float3 targetDirection = input.x * right + input.y * forward;
            targetDirection.y = 0;
            targetDirection = Vector3.Normalize(targetDirection);
            float3 curPos = posArray[0].localPosition;
            var speed = moveSpeedArray[0].Value;
            var newTargetPos = new TargetPosition();
            if (speed > 0)
                newTargetPos.Value = curPos+targetDirection*(speed/GameConst.SpeedFactor*0.10f);//延着方向前进0.10秒为目标坐标
            else
                newTargetPos.Value = curPos;
            newTargetPos.Value.y = targetPosArray[0].Value.y;
            // Debug.Log("targetDirection : "+newTargetPos.Value.x+" "+newTargetPos.Value.y+" "+newTargetPos.Value.z);
            targetPosArray[0] = newTargetPos;
        }
        else
        {
            var newTargetPos = new TargetPosition{Value=posArray[0].localPosition};
            targetPosArray[0] = newTargetPos;
        }
        targetPosArray.Dispose();
        moveSpeedArray.Dispose();
        locoStates.Dispose();
    }
}
