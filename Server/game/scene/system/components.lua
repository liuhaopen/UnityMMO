local ECS = require "ECS"

ECS.TypeManager.RegisterType("umo.position", {x="integer", y="integer", z="integer"})
ECS.TypeManager.RegisterType("umo.target_pos", {x="integer", y="integer", z="integer"})
ECS.TypeManager.RegisterType("umo.uid", {value="integer"})
ECS.TypeManager.RegisterType("umo.type_id", {value="integer"})--Role类型的话对应RoleID，Monster和NPC类型对应TypeID
ECS.TypeManager.RegisterType("umo.scene_obj_type", {value="integer"})
ECS.TypeManager.RegisterType("umo.hp", {cur="integer", max="integer"})
ECS.TypeManager.RegisterType("umo.move_speed", {value="integer"})
ECS.TypeManager.RegisterType("umo.patrol_info", {x="integer", y="integer", z="integer", radius="integer"})--怪物有一个中心点，以此点划圆巡逻
ECS.TypeManager.RegisterType("umo.aoi_handle", {value="integer"})
ECS.TypeManager.RegisterType("umo.damage_event", {id="integer"})
ECS.TypeManager.RegisterType("umo.monster_ai", {ai_id="integer"})
ECS.TypeManager.RegisterType("umo.monster_state", {state="integer", sub_state="integer"})