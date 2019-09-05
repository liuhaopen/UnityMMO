using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using XLuaFramework;
using System;
using Unity.Mathematics;
using Unity.Collections;
using UnityMMO.Component;

namespace UnityMMO
{
    [DisableAutoCreation]
    public class PlayerInputSystem : ComponentSystem
    {
        public PlayerInputSystem()
        {
        }
        EntityQuery group;

        protected override void OnCreate()
        {
            base.OnCreate();
            group = GetEntityQuery(typeof(UserCommand), typeof(TargetPosition));
        }

        protected override void OnUpdate()
        {
            // Debug.Log("on OnUpdate player input system");
            float dt = Time.deltaTime;
            var userCommandArray = group.ToComponentDataArray<UserCommand>(Allocator.TempJob);
            var targetPosArray = group.ToComponentDataArray<TargetPosition>(Allocator.TempJob);
            if (userCommandArray.Length > 0)
            {
                var userCommand = userCommandArray[0];
                SampleInput(ref userCommand, dt);
            }
            userCommandArray.Dispose();
            targetPosArray.Dispose();
        }

        public void SampleInput(ref UserCommand command, float deltaTime)
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            // GameInput.GetInstance().JoystickDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            float angle = Vector2.Angle(Vector2.up, moveInput);
            if (moveInput.x < 0)
                angle = 360 - angle;
            float magnitude = Mathf.Clamp(moveInput.magnitude, 0, 1);       
            command.moveYaw = angle;
            command.moveMagnitude = magnitude;

            var roleGameOE = RoleMgr.GetInstance().GetMainRole();
            EntityManager.SetComponentData<ActionData>(roleGameOE.Entity, ActionData.Empty);

            float invertY = 1.0f;

            Vector2 deltaMousePos = new Vector2(0, 0);
            if(deltaTime > 0.0f)
                deltaMousePos += new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y") * invertY);

            const float configMouseSensitivity = 1.5f;
            command.lookYaw += deltaMousePos.x * configMouseSensitivity;
            command.lookYaw = command.lookYaw % 360;
            while (command.lookYaw < 0.0f) command.lookYaw += 360.0f;

            command.lookPitch += deltaMousePos.y * configMouseSensitivity;
            command.lookPitch = Mathf.Clamp(command.lookPitch, 0, 180);
            
            command.jump = (command.jump!=0 || Input.GetKeyDown(KeyCode.Space))?1:0; 
            command.sprint = (command.sprint!=0 || Input.GetKey(KeyCode.LeftShift))?1:0;

            if (GameInput.GetInstance().GetKeyUp(KeyCode.J))
                SkillManager.GetInstance().CastSkillByIndex(-1);
            else if (GameInput.GetInstance().GetKeyUp(KeyCode.I))
                SkillManager.GetInstance().CastSkillByIndex(0);
            else if (GameInput.GetInstance().GetKeyUp(KeyCode.O))
                SkillManager.GetInstance().CastSkillByIndex(1);
            else if (GameInput.GetInstance().GetKeyUp(KeyCode.K))
                SkillManager.GetInstance().CastSkillByIndex(2);
            else if (GameInput.GetInstance().GetKeyUp(KeyCode.L))
                SkillManager.GetInstance().CastSkillByIndex(3);
            else if (GameInput.GetInstance().GetKeyUp(KeyCode.Space))
                DoJump();
        }

        void DoJump()
        {
            var roleGameOE = RoleMgr.GetInstance().GetMainRole();
            var jumpState = EntityManager.GetComponentData<JumpData>(roleGameOE.Entity);
            var isMaxJump = jumpState.JumpCount >= GameConst.MaxJumpCount;
            if (isMaxJump)
            {
                //已经最高跳段了，就不能再跳
                return;
            }
            RoleMgr.GetInstance().StopMainRoleRunning();
            var actionData = EntityManager.GetComponentData<ActionData>(roleGameOE.Entity);
            actionData.Jump = 1;
            EntityManager.SetComponentData<ActionData>(roleGameOE.Entity, actionData);
            //这里的timeline只作跳跃中的表现，如加粒子加女主尾巴等，状态和高度控制还是放在MovementUpdateSystem里，因为跳跃这个动作什么时候结束是未知的，你可能跳崖了，这时跳跃状态会一直维持至到碰到地面，所以不方便放在timeline里。
            var newJumpCount = math.clamp(jumpState.JumpCount+1, 0, GameConst.MaxJumpCount);
            var roleInfo = roleGameOE.GetComponent<RoleInfo>();
            var assetPath = ResPath.GetRoleJumpResPath(roleInfo.Career, newJumpCount);
            var timelineInfo = new TimelineInfo{ResPath=assetPath, Owner=roleGameOE.Entity, StateChange=null};
            var uid = EntityManager.GetComponentData<UID>(roleGameOE.Entity);
            TimelineManager.GetInstance().AddTimeline(uid.Value, timelineInfo, EntityManager);
        }

    }
}
