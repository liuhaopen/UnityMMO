local ECS = require "ECS"

ECS.TypeManager.RegisterType("UMO.Position", {x=0, y=0, z=0})
ECS.TypeManager.RegisterType("UMO.TargetPos", {x=0, y=0, z=0})
ECS.TypeManager.RegisterType("UMO.UID", {value=0})
ECS.TypeManager.RegisterType("UMO.TypeID", {value=0})--Role类型的话对应RoleID，Monster和NPC类型对应TypeID
ECS.TypeManager.RegisterType("UMO.SceneObjType", {value=0})
ECS.TypeManager.RegisterType("UMO.HP", {cur=0, max=0})
ECS.TypeManager.RegisterType("UMO.MoveSpeed", {value=0})
ECS.TypeManager.RegisterType("UMO.PatrolInfo", {x=0, y=0, z=0, radius=0})--怪物有一个中心点，以此点划圆巡逻
ECS.TypeManager.RegisterType("UMO.AOIHandle", {value=0})
ECS.TypeManager.RegisterType("UMO.DamageEvents", {events=nil})
ECS.TypeManager.RegisterType("UMO.MonsterAI", {ai_id=0})
ECS.TypeManager.RegisterType("UMO.RoleInfo", {name="", base_info=nil})
