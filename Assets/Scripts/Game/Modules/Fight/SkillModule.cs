using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace UnityMMO
{
public class SkillManager
{
    static SkillManager instance;
    int curComboIndex;
    int career;
    int lastRandomSkillID=0;
    int[] skillIDs = new int[4];//主界面里的四个技能id，因为人物有多于4技能可以选择，所以需要后端记录下来哪四个常用的放在主界面
    Dictionary<int,long> skillCDEndTime = new Dictionary<int,long>();
    public static SkillManager GetInstance()
    {
        if (instance != null)
            return instance;
        instance = new SkillManager();
        return instance;
    }

    public void Init(int career)
    {
        this.career = career;
        this.curComboIndex = 0;
        //just for test
        for (int i = 0; i < 4; i++)
        {
            skillIDs[i] = 100000+career*10000+10+i;
        }
    }

    public int GetCurAttackID()
    {
        return GetAttackID(career, curComboIndex);
    }

    public void ResetCombo()
    {
        curComboIndex = 0;
    }

    public void IncreaseCombo()
    {
        //普攻有四个
        curComboIndex++;
        if (curComboIndex>=4)
            curComboIndex = 0;
    }

    public int GetSkillIDByIndex(int skillIndex)
    {
        if (skillIndex == -1)
            return GetCurAttackID();
        else
            return skillIDs[skillIndex];
    }

    public void SetSkillCD(int skillID, long endTime)
    {
        // Debug.Log("set cd : "+skillID+" endTime:"+endTime);
        if (skillCDEndTime.ContainsKey(skillID))
            skillCDEndTime[skillID] = endTime;
        else
            skillCDEndTime.Add(skillID, endTime);
        XLuaFramework.CSLuaBridge.GetInstance().CallLuaFunc2Num(GlobalEvents.SkillCDChanged, skillID, endTime);
    }

    public long GetSkillCD(int skillID)
    {
        long endTime;
        skillCDEndTime.TryGetValue(skillID, out endTime);
        return endTime;
    }

    public string GetSkillResPath(int skillID)
    {
        string assetPath;
        int scene_obj_type = GetSceneObjTypeBySkillID(skillID);
        if (scene_obj_type == (int)SceneObjectType.Role)
            assetPath = ResPath.GetRoleSkillResPath(skillID);
        else if(scene_obj_type == (int)SceneObjectType.Monster)
            assetPath = ResPath.GetMonsterSkillResPath(skillID);
        else
            assetPath = "";
        return assetPath;
    }

    public int GetCareerBySkillID(int skillID)
    {
        int scene_obj_type = GetSceneObjTypeBySkillID(skillID);
        if (scene_obj_type == (int)SceneObjectType.Role)
            return (int)math.floor((skillID%100000)/10000);
        return 1;
    }

    public int GetSceneObjTypeBySkillID(int skillID)
    {
        return (int)math.floor((skillID/100000));
    }

    private static int GetAttackID(int career, int comboIndex)
    {
        //技能id：十万位是类型1角色，2怪物，3NPC，万位为职业，个十百位随便用
        return 100000+career*10000+comboIndex;
    }

    public bool IsSkillInCD(int skillID)
    {
        long endTime = GetSkillCD(skillID);
        return endTime >= TimeEx.ServerTime;
    }

    public bool IsNormalAttack(int skillID)
    {
        var combatID = ((skillID%100000)%10000);
        // Debug.Log("combatID : "+combatID);
        //普攻的技能id都是个位数没有十位数的
        return combatID<10;
    }

    public int CastSkillByIndex(int skillIndex=-1)
    {
        var skillID = GetSkillIDByIndex(skillIndex);
        CastSkill(skillID);
        return skillID;
    }

    public int CastRandomSkill()
    {
        for (int i = 0; i < skillIDs.Length; i++)
        {
            var skillID = skillIDs[i];
            if (lastRandomSkillID == skillID)
                continue;
            var isInCD = IsSkillInCD(skillID);
            if (!isInCD)
            {
                CastSkill(skillID);
                lastRandomSkillID = skillID;
                return skillID;
            }
        }
        return CastSkillByIndex(-1);
    }

    public void CastSkill(int skillID)
    {
        // var skillID = GetSkillIDByIndex(skillIndex);
        var isInCD = IsSkillInCD(skillID);
        if (isInCD)
        {
            XLuaFramework.CSLuaBridge.GetInstance().CallLuaFuncStr(GlobalEvents.MessageShow, "技能冷却中...");
            return;
        }
        var roleGameOE = RoleMgr.GetInstance().GetMainRole();
        var roleInfo = roleGameOE.GetComponent<RoleInfo>();
        string assetPath = ResPath.GetRoleSkillResPath(skillID);
        bool isNormalAttack = IsNormalAttack(skillID);//普通攻击
        // Debug.Log("isNormalAttack : "+isNormalAttack);
        if (!isNormalAttack)
            ResetCombo();//使用非普攻技能时就重置连击索引
        var uid = SceneMgr.Instance.EntityManager.GetComponentData<UID>(roleGameOE.Entity);
        Action<TimelineInfo.Event> afterAdd = null;
        if (isNormalAttack)
        {
            //普攻的话增加连击索引
            afterAdd = (TimelineInfo.Event e)=>
            {
                if (e == TimelineInfo.Event.AfterAdd)
                    IncreaseCombo();
            };
        }
        var timelineInfo = new TimelineInfo{ResPath=assetPath, Owner=roleGameOE.Entity,  StateChange=afterAdd};
        TimelineManager.GetInstance().AddTimeline(uid.Value, timelineInfo, SceneMgr.Instance.EntityManager);
    }

    private SkillManager()
    {
    }
}

}
