using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using XLuaFramework;
using System;


namespace UnityMMO
{
    [DisableAutoCreation]
    public class PlayerInputSystem : ComponentSystem
    {
        public PlayerInputSystem()
        {
        }
        ComponentGroup group;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            group = GetComponentGroup(typeof(UserCommand), typeof(TargetPosition), typeof(MoveSpeed));
        }

        protected override void OnUpdate()
        {
            // Debug.Log("on OnUpdate player input system");
            float dt = Time.deltaTime;
            var userCommandArray = group.GetComponentDataArray<UserCommand>();
            var targetPosArray = group.GetComponentDataArray<TargetPosition>();
            var moveSpeedArray = group.GetComponentDataArray<MoveSpeed>();
            if (userCommandArray.Length==0)
                return;
            var userCommand = userCommandArray[0];
            SampleInput(ref userCommand, dt);
            // userCommandArray[0] = userCommand;
        }

        public void SampleInput(ref UserCommand command, float deltaTime)
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            float angle = Vector2.Angle(Vector2.up, moveInput);
            if (moveInput.x < 0)
                angle = 360 - angle;
            float magnitude = Mathf.Clamp(moveInput.magnitude, 0, 1);       
            // if (magnitude > maxMoveMagnitude)
            // {
            //     maxMoveYaw = angle;
            //     maxMoveMagnitude = magnitude;
            // }
            command.moveYaw = angle;
            command.moveMagnitude = magnitude;

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
            Debug.Log("do jump");
            var roleGameOE = RoleMgr.GetInstance().GetMainRole();
            // var speed = EntityManager.GetComponentData<MoveSpeed>(roleGameOE.Entity);
            // speed.VerticalSpeed = 2200;
            // EntityManager.SetComponentData<MoveSpeed>(roleGameOE.Entity, speed);

            var roleInfo = roleGameOE.GetComponent<RoleInfo>();
            var assetPath = GameConst.GetRoleJumpResPath(roleInfo.Career, 1);
            var timelineInfo = new TimelineInfo{ResPath=assetPath, Owner=roleGameOE.Entity, StateChange=null};
            var uid = EntityManager.GetComponentData<UID>(roleGameOE.Entity);
            TimelineManager.GetInstance().AddTimeline(uid.Value, timelineInfo, EntityManager);

            // var trans = EntityManager.GetComponentObject<Transform>(roleGameOE.Entity);
            // var targetPos = EntityManager.GetComponentData<TargetPosition>(roleGameOE.Entity);
            // targetPos.Value.y = trans.localPosition.y + 10;
            // EntityManager.SetComponentData<TargetPosition>(roleGameOE.Entity, targetPos);
        }

        void CastSkill(int skillIndex=-1)
        {
            var roleGameOE = RoleMgr.GetInstance().GetMainRole();
            var roleInfo = roleGameOE.GetComponent<RoleInfo>();
            var skillID = SkillManager.GetInstance().GetSkillIDByIndex(skillIndex);
            string assetPath = GameConst.GetRoleSkillResPath(roleInfo.Career, skillID);
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
