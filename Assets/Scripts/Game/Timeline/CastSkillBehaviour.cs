using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityMMO;

// A behaviour that is attached to a playable
public class CastSkillBehaviour : PlayableBehaviour
{
    public Entity Owner;
    public EntityManager EntityMgr;
    private int SkillID;
    
    public void Init(Entity owner, EntityManager entityMgr, int skillID)
    {
        Owner = owner;
        EntityMgr = entityMgr;
        this.SkillID = skillID;
    }
    
    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        var isMainRole = RoleMgr.GetInstance().IsMainRoleEntity(Owner);
        // Debug.Log("cast skill behav isMainRole :"+isMainRole.ToString());
        if (isMainRole)
        {
            var trans = EntityMgr.GetComponentObject<Transform>(Owner);
            if (trans != null)
            {
                SprotoType.scene_cast_skill.request req = new SprotoType.scene_cast_skill.request();
                req.skill_id = SkillID;
                req.cur_pos_x = (long)(trans.localPosition.x * GameConst.RealToLogic);
                req.cur_pos_y = (long)(trans.localPosition.y * GameConst.RealToLogic);
                req.cur_pos_z = (long)(trans.localPosition.z * GameConst.RealToLogic);
                req.target_pos_x = (long)(trans.localPosition.x * GameConst.RealToLogic);
                req.target_pos_y = (long)(trans.localPosition.y * GameConst.RealToLogic);
                req.target_pos_z = (long)(trans.localPosition.z * GameConst.RealToLogic);
                req.direction = (long)(trans.eulerAngles.y * GameConst.RealToLogic);
                // Debug.Log("req.direction : "+req.direction+" skillID "+SkillID);
                NetMsgDispatcher.GetInstance().SendMessage<Protocol.scene_cast_skill>(req, delegate(Sproto.SprotoTypeBase result){
                    SprotoType.scene_cast_skill.response ack = result as SprotoType.scene_cast_skill.response;
                    // Debug.Log("ack : "+(ack!=null).ToString());
                    if (ack==null)
                        return;
                    // Debug.Log("scene_cast_skill ack.result : "+ack.result);
                    if (ack.result == 1)
                    {
                        XLuaFramework.CSLuaBridge.GetInstance().CallLuaFuncStr(GlobalEvents.MessageShow, "技能冷却中...");
                        return;
                    }
                    SkillManager.GetInstance().SetSkillCD((int)ack.skill_id, ack.cd_end_time);
                    // // Debug.Log("playable : "+(Playable.Equals(playable, Playable.Null)).ToString());
                    // var graph = PlayableExtensions.GetGraph(playable);
                    // // Debug.Log("graph.IsDone() : "+graph.IsDone().ToString());
                    // if (!graph.IsDone())
                    // {
                    //     var playableNum = graph.GetRootPlayableCount();
                    //     // Debug.Log("playableNum : "+playableNum);
                    //     for (int i = 0; i < playableNum; i++)
                    //     {
                    //         var rootPlayable = graph.GetRootPlayable(i);
                    //         FindFlyWord(rootPlayable, ack.fight_event.defenders, 0);
                    //     }
                    // }
                });
            }
        }
    }

    void FindFlyWord(Playable playable, object obj, int level=0)
    {
        // var inputCount = playable.GetInputCount();
        // for (int i = 0; i < inputCount; i++)
        // {
        //     Type playableType = playable.GetInput(i).GetPlayableType();
        //     var isFlyWord = playableType == typeof(ApplyDamageBehaviour);
        //     if (isFlyWord)
        //     {
        //         var flyWordPlayable = (ScriptPlayable<ApplyDamageBehaviour>)(playable.GetInput(i));
        //         var behaviour = flyWordPlayable.GetBehaviour();
        //         if (behaviour != null)
        //             behaviour.Defenders = obj as List<SprotoType.scene_fight_defender_info>;
        //         // Debug.Log("isFlyWord : "+isFlyWord.ToString()+" flyWordPlayable:"+flyWordPlayable.ToString()+" behaviour:"+(behaviour!=null).ToString()+" level:"+level+" i:"+i+" inputCount:"+inputCount+" playableType:"+playableType.FullName);
        //     }
        //     FindFlyWord(playable.GetInput(i), obj, level+1);
        // }
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
    }
}
