using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityMMO;

[DisableAutoCreation]
public class MovementUpdateSystem : BaseComponentSystem
{
    public MovementUpdateSystem(GameWorld world) : base(world) {}

    ComponentGroup group;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        group = GetComponentGroup(typeof(TargetPosition), typeof(Transform), typeof(MoveSpeed), typeof(MoveQuery), typeof(LocomotionState));
    }

    protected override void OnUpdate()
    {
        float dt = Time.deltaTime;
        var entities = group.GetEntityArray();
        var targetPositions = group.GetComponentDataArray<TargetPosition>();
        var speeds = group.GetComponentDataArray<MoveSpeed>();
        var transforms = group.GetComponentArray<Transform>();
        var moveQuerys = group.GetComponentArray<MoveQuery>();
        var locoStates = group.GetComponentDataArray<LocomotionState>();
        for (int i=0; i<targetPositions.Length; i++)
        {
            var targetPos = targetPositions[i].Value;
            var speed = speeds[i].Value;
            if (speed<=0)
                continue;
            var curTrans = transforms[i];
            float3 startPos = curTrans.localPosition;
            var moveDir = targetPos-startPos;
            var groundDir = moveDir;
            groundDir.y = 0;
            groundDir = Vector3.Normalize(groundDir);
            float moveDistance = Vector3.Magnitude(groundDir);
            bool isMoveWanted = moveDistance>0.01f;
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
            //垂直方向有两因素，1是VerticalSpeed，比如跳跃时会设置；2是模仿重力，人物需要贴着地面走，有碰撞检测的所以不怕
            newPos.y = startPos.y + (speeds[i].VerticalSpeed/GameConst.SpeedFactor)*dt;
            newPos.y += GameConst.Gravity*dt;

            var moveQuery = moveQuerys[i];
            moveQuery.moveQueryStart = startPos;
            //不能直接设置新坐标，因为需要和地形做碰撞处理什么的，所以利用CharacterController走路，在HandleMovementQueries才设置新坐标
            moveQuery.moveQueryEnd = newPos;

            //new locomotion state
            var newLocoState = LocomotionState.State.StateNum;
            var curLocoStateObj = locoStates[i];
            var curLocoState = curLocoStateObj.Value;
            bool isOnGround = curLocoState == LocomotionState.State.Idle || curLocoState == LocomotionState.State.Run || curLocoState == LocomotionState.State.Sprint;
            // Debug.Log("isOnGround : "+isOnGround.ToString()+" movewanted:"+isMoveWanted.ToString());
            if (isOnGround)
            {
                if (isMoveWanted)
                    newLocoState = LocomotionState.State.Run;
                else
                    newLocoState = LocomotionState.State.Idle;
            }
            //jump
            if (isOnGround)
            {

            }
            if (newLocoState != LocomotionState.State.StateNum && newLocoState != curLocoState)
            {
                curLocoStateObj.Value = newLocoState;
                locoStates[i] = curLocoStateObj;
            }
            
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
        var input = GameInput.GetInstance().JoystickDir;
        bool isJump = Input.GetKeyDown(KeyCode.Space);
        var forward = SceneMgr.Instance.MainCameraTrans.TransformDirection(Vector3.forward);
        forward.y = 0;
        var right = SceneMgr.Instance.MainCameraTrans.TransformDirection(Vector3.right);
        float3 targetDirection = input.x * right + input.y * forward;
        targetDirection.y = 0;
        float3 curPos = posArray[0].localPosition;
        var speed = moveSpeedArray[0].Value;
        var newTargetPos = new TargetPosition();
        if (speed > 0)
            newTargetPos.Value = curPos+targetDirection*(speed/GameConst.SpeedFactor*0.5f);//延着方向前进0.5秒为目标坐标
        else
            newTargetPos.Value = curPos;
        newTargetPos.Value.y = targetPosArray[0].Value.y;
        targetPosArray[0] = newTargetPos;
    }
}
