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

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            group = GetEntityQuery(typeof(UserCommand), typeof(TargetPosition), typeof(MoveSpeed));
        }

        protected override void OnUpdate()
        {
            // Debug.Log("on OnUpdate player input system");
            float dt = Time.deltaTime;
            var userCommandArray = group.ToComponentDataArray<UserCommand>(Allocator.TempJob);
            var targetPosArray = group.ToComponentDataArray<TargetPosition>(Allocator.TempJob);
            var moveSpeedArray = group.ToComponentDataArray<MoveSpeed>(Allocator.TempJob);
            if (userCommandArray.Length==0)
                return;
            var userCommand = userCommandArray[0];
            SampleInput(ref userCommand, dt);
            // userCommandArray[0] = userCommand;
            userCommandArray.Dispose();
            targetPosArray.Dispose();
            moveSpeedArray.Dispose();
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
                CastSkill(-1);
            else if (GameInput.GetInstance().GetKeyUp(KeyCode.I))
                CastSkill(0);
            else if (GameInput.GetInstance().GetKeyUp(KeyCode.O))
                CastSkill(1);
            else if (GameInput.GetInstance().GetKeyUp(KeyCode.K))
                CastSkill(2);
            else if (GameInput.GetInstance().GetKeyUp(KeyCode.L))
                CastSkill(3);
            else if (GameInput.GetInstance().GetKeyUp(KeyCode.Space))
                DoJump();
        }

        void DoJump()
        {
            var roleGameOE = RoleMgr.GetInstance().GetMainRole();
            var jumpState = EntityManager.GetComponentData<LocomotionState>(roleGameOE.Entity);
            var isMaxJump = jumpState.JumpCount >= GameConst.MaxJumpCount;
            if (isMaxJump)
            {
                //已经最高跳段了，就不能再跳
                return;
            }
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

        void CastSkill(int skillIndex=-1)
        {
            var roleGameOE = RoleMgr.GetInstance().GetMainRole();
            var roleInfo = roleGameOE.GetComponent<RoleInfo>();
            var skillID = SkillManager.GetInstance().GetSkillIDByIndex(skillIndex);
          
            string assetPath = ResPath.GetRoleSkillResPath(skillID);
            bool isNormalAttack = skillIndex == -1;//普通攻击
            if (!isNormalAttack)
                SkillManager.GetInstance().ResetCombo();//使用非普攻技能时就重置连击索引
            var uid = EntityManager.GetComponentData<UID>(roleGameOE.Entity);
            Action<TimelineInfo.Event> afterAdd = null;
            if (isNormalAttack)
            {
                //普攻的话增加连击索引
                afterAdd = (TimelineInfo.Event e)=>
                {
                    if (e == TimelineInfo.Event.AfterAdd)
                        SkillManager.GetInstance().IncreaseCombo();
                };
            }
            var timelineInfo = new TimelineInfo{ResPath=assetPath, Owner=roleGameOE.Entity,  StateChange=afterAdd};
            TimelineManager.GetInstance().AddTimeline(uid.Value, timelineInfo, EntityManager);
        }

    }
}
