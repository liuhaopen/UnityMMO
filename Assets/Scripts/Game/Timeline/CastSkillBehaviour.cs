using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityMMO;

// A behaviour that is attached to a playable
public class CastSkillBehaviour : PlayableBehaviour
{
    public Entity Owner;
    public EntityManager EntityMgr;
    private int skillID;
    
    public void Init(Entity owner, EntityManager entityMgr, int skillID)
    {
        Owner = owner;
        EntityMgr = entityMgr;
        skillID = skillID;
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
        Debug.Log("cast skill behav isMainRole :"+isMainRole.ToString());
        if (isMainRole)
        {
            var trans = EntityMgr.GetComponentObject<Transform>(Owner);
            if (trans != null)
            {
                SprotoType.scene_cast_skill.request req = new SprotoType.scene_cast_skill.request();
                req.skill_id = skillID;
                req.cur_pos_x = (long)(trans.localPosition.x * GameConst.RealToLogic);
                req.cur_pos_y = (long)(trans.localPosition.y * GameConst.RealToLogic);
                req.cur_pos_z = (long)(trans.localPosition.z * GameConst.RealToLogic);
                req.target_pos_x = (long)(trans.localPosition.x * GameConst.RealToLogic);
                req.target_pos_y = (long)(trans.localPosition.y * GameConst.RealToLogic);
                req.target_pos_z = (long)(trans.localPosition.z * GameConst.RealToLogic);
                req.direction = (long)(trans.eulerAngles.y * GameConst.RealToLogic);
                Debug.Log("req.direction : "+req.direction);
                NetMsgDispatcher.GetInstance().SendMessage<Protocol.scene_cast_skill>(req);
            }
        }
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
