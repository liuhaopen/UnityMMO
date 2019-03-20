using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using XLuaFramework;

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
            userCommandArray[0] = userCommand;
            // for (int i = 0; i < inputDataArray.Length; ++i)
            // {
            //     PlayerInput pi;
            //     pi.Move.x = Input.GetAxis("Horizontal");
            //     pi.Move.y = Input.GetAxis("Vertical");
            //     inputDataArray[i] = pi;
            // }
        }

        public void SampleInput(ref UserCommand command, float deltaTime)
        {
            // To accumulate move we store the input with max magnitude and uses that
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
            if (GameInput.GetInstance().GetKeyUp(KeyCode.U))
                TestSkill1();

            command.jump = (command.jump!=0 || Input.GetKeyDown(KeyCode.Space))?1:0; 
            command.sprint = (command.sprint!=0 || Input.GetKey(KeyCode.LeftShift))?1:0;
            // command.skill = 
        }

        void TestSkill1()
        {
            string assetPath = "Assets/AssetBundleRes/role/career_2/skill/timeline/Attack.playable";
            ResourceManager.GetInstance().LoadAsset<PlayableAsset>(assetPath, delegate(UnityEngine.Object[] objs)
            {
                if (objs==null || objs.Length<=0)
                    return;
                var mainRole = RoleMgr.GetInstance().GetMainRole();
                var playerDirector = mainRole.GetComponent<PlayableDirector>();
                playerDirector.playableAsset = objs[0] as PlayableAsset;
                var animator = mainRole.GetComponentInChildren<Animator>();
                // Dictionary<string, PlayableBinding> bindingDict = new Dictionary<string, PlayableBinding>();
                foreach (var at in playerDirector.playableAsset.outputs)
                {
                    // if (!bindingDict.ContainsKey(at.streamName))
                    // {
                    //     Debug.Log("at.streamName : "+at.streamName);
                    //     bindingDict.Add(at.streamName, at);
                    // }
                    if (at.streamName.StartsWith("AnimationTrack"))
                    {
                        playerDirector.SetGenericBinding(at.sourceObject, animator);
                    }
                    else if (at.streamName.StartsWith("ParticleTrack"))
                    {
                        var ct = at.sourceObject as ControlTrack;
                        var looksInfo = EntityManager.GetComponentData<LooksInfo>(mainRole.Entity);
                        var looksEntity = looksInfo.LooksEntity;
                        var looksTrans = EntityManager.GetComponentObject<Transform>(looksEntity);
                        var particleParent = looksTrans.Find("root");
                        foreach (var info in ct.GetClips())
                        {
                            if (info.displayName == "particle")
                            {
                                var cpa = info.asset as ControlPlayableAsset;
                                playerDirector.SetReferenceValue(cpa.sourceGameObject.exposedName, particleParent.gameObject);
                               
                            }
                        }
                    }
                }
                playerDirector.Play();
            });
        }
      
    }
}
