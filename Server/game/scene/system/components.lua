local ECS = require "ECS"

ECS.TypeManager.RegisterType("umo.position", {x=0, y=0, z=0})
ECS.TypeManager.RegisterType("umo.target_pos", {x=0, y=0, z=0})
ECS.TypeManager.RegisterType("umo.uid", {value=0})
ECS.TypeManager.RegisterType("umo.type_id", {value=0})--Role类型的话对应RoleID，Monster和NPC类型对应TypeID
ECS.TypeManager.RegisterType("umo.scene_obj_type", {value=0})
ECS.TypeManager.RegisterType("umo.hp", {cur=0, max=0})
ECS.TypeManager.RegisterType("umo.move_speed", {value=0})
ECS.TypeManager.RegisterType("umo.patrol_info", {x=0, y=0, z=0, radius=0})--怪物有一个中心点，以此点划圆巡逻
ECS.TypeManager.RegisterType("umo.aoi_handle", {value=0})
ECS.TypeManager.RegisterType("umo.damage_event", {id=0})
ECS.TypeManager.RegisterType("umo.monster_ai", {ai_id=0})
ECS.TypeManager.RegisterType("umo.monster_state", {state=0, sub_state=0})