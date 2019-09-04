local ECS = require "ECS"

ECS.TypeManager.RegisterType("UMO.Position", {x=0, y=0, z=0})
ECS.TypeManager.RegisterType("UMO.TargetPos", {x=0, y=0, z=0})
ECS.TypeManager.RegisterType("UMO.UID", 0)
ECS.TypeManager.RegisterType("UMO.TypeID", 0)--Role类型的话对应RoleID，Monster和NPC类型对应TypeID
ECS.TypeManager.RegisterType("UMO.SceneObjType", {value=0})
ECS.TypeManager.RegisterType("UMO.HP", {cur=0, max=0, deathTime=0, killedBy})
ECS.TypeManager.RegisterType("UMO.MoveSpeed", nil)
ECS.TypeManager.RegisterType("UMO.PatrolInfo", {x=0, y=0, z=0, radius=0})--怪物有一个中心点，以此点划圆巡逻
ECS.TypeManager.RegisterType("UMO.AOIHandle", {value=0})
--CD数组，元素结构为{[skill_id]=end_time}
ECS.TypeManager.RegisterType("UMO.CD", {})
ECS.TypeManager.RegisterType("UMO.Skill", {caster_uid=0, cast_time=0, skill_id=0, skill_lv=0, targets={}, max_target_num=0})
ECS.TypeManager.RegisterType("UMO.Buff", {})
ECS.TypeManager.RegisterType("UMO.BaseAttr", nil)
ECS.TypeManager.RegisterType("UMO.FightAttr", nil)

--里面是个数组，元素结构：attacker攻击者，damage伤害值，direction攻击方向，impulse推力
ECS.TypeManager.RegisterType("UMO.DamageEvents", {})

ECS.TypeManager.RegisterType("UMO.MonsterAI", 0)
ECS.TypeManager.RegisterType("UMO.RoleInfo", {name="", base_info=nil})
--0活着的，1死了，2幽灵
-- ECS.TypeManager.RegisterType("UMO.DeadState", 0)
ECS.TypeManager.RegisterType("UMO.Beatable", true)--是否可被打的
ECS.TypeManager.RegisterType("UMO.Ability", {NormalAtk=BOD.New(), CastSkill=BOD.New(), Jump=BOD.New()})--各种能力
ECS.TypeManager.RegisterType("UMO.MsgAgent", nil)
