using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityMMO;

// A behaviour that is attached to a playable
public class JumpCtrlBehaviour : PlayableBehaviour
{
    public AnimationCurve Curve;
    public Entity Owner;
    public EntityManager EntityMgr;
    private float curTime;
    private float maxTime;
    private int jumpCount;
    private float startYPos;
    bool isMainRole = false;

    public void Init(Entity owner, EntityManager entityMgr)
    {
        Owner = owner;
        EntityMgr = entityMgr;
        isMainRole = RoleMgr.GetInstance().IsMainRoleEntity(Owner);
    }
    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        if (isMainRole)
        {
            // var curInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            // if (curInput.magnitude <= 0)
                GameInput.GetInstance().JoystickDir = Vector2.zero;
        }
        // if (EntityMgr.HasComponent<JumpState>(Owner))
        // {
        //     var jumpState = EntityMgr.GetComponentData<JumpState>(Owner);
        //     // jumpState.JumpCount = 0;
        //     jumpState.JumpStatus = JumpState.State.None;
        //     EntityMgr.SetComponentData<JumpState>(Owner, jumpState);
        // }
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        // maxTime= (float)PlayableExtensions.GetDuration(playable);
        // var trans = EntityMgr.GetComponentObject<Transform>(Owner);
        // if (EntityMgr.HasComponent<JumpState>(Owner) && trans!=null)
        // {
        //     var jumpState = EntityMgr.GetComponentData<JumpState>(Owner);
        //     jumpCount = math.clamp(jumpState.JumpCount+1, 1, GameConst.MaxJumpCount);
        //     startYPos = trans.localPosition.y;
        //     if (jumpState.JumpCount == 0)
        //     {
        //         jumpState.OriginYPos = trans.localPosition.y;
        //         jumpState.AscentHeight = 4;
        //     }
        //     else if (jumpState.JumpCount == 1)
        //     {
        //         jumpState.AscentHeight = 2.6f;
                
        //     }
        //     else if (jumpState.JumpCount == 2)
        //     {
        //         //第三段跳只是前进
        //         jumpState.AscentHeight = 0;
        //     }
        //     else
        //     {   
        //     }
        //     jumpState.JumpCount = jumpCount;
        //     jumpState.JumpStatus = JumpState.State.StartJump;
        //     // Debug.Log("jumpState.JumpCount : "+jumpState.JumpCount);
        //     EntityMgr.SetComponentData<JumpState>(Owner, jumpState);
        // }
        // Debug.Log("start jump");
        // EntityMgr.SetComponentData<TimelineState>(Owner, new TimelineState{NewStatus=newState, InterruptStatus=interruptState});
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        var trans = EntityMgr.GetComponentObject<Transform>(Owner);
        if (isMainRole)
        {
            GameInput.GetInstance().JoystickDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            // if (GameInput.GetInstance().JoystickDir.magnitude<=0)
            // {
            //     // var inputDir = new Vector3(trans.forward.x, 0, trans.forward.z);
            //     var inputDir = trans.TransformDirection(trans.forward);
            //     // var inputDir = new Vector3(Vector3.forward.x, 0, Vector3.forward.z);
            //     // trans.Translate(inputDir, Space.Self);
            //     // trans.Translate(inputDir, Space.World);
            //     GameInput.GetInstance().JoystickDir = new Vector2(inputDir.x, inputDir.z);
            //     Debug.Log("GameInput.GetInstance().JoystickDir : "+GameInput.GetInstance().JoystickDir.x+" "+GameInput.GetInstance().JoystickDir.y+" forward:"+trans.forward.x+" "+trans.forward.z);
            // }
        }
        // curTime += info.deltaTime;
        // if (EntityMgr.HasComponent<JumpState>(Owner) && trans!=null)
        // {
        //     var jumpState = EntityMgr.GetComponentData<JumpState>(Owner);
        //     var percent = Curve.Evaluate(curTime/ maxTime);
        //     var targetY = jumpState.OriginYPos + jumpState.AscentHeight*percent;
        //     Debug.Log("jump percent "+percent+" targetY:"+targetY);
        //     if (EntityMgr.HasComponent<PosOffset>(Owner))
        //     {
        //         var gravity = GameConst.Gravity*Time.deltaTime;//需要抵消重力
        //         var offsetY = targetY-trans.localPosition.y-gravity;
        //         var posOffset = new PosOffset{Value=new float3(0, offsetY, 0)};
        //         EntityMgr.SetComponentData<PosOffset>(Owner, posOffset);
        //     }
        // }
    }
}
