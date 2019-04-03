local monster_mgr = {}

function monster_mgr:init( entity_mgr, cfg )
	self.entity_mgr = entity_mgr
	self.cfg = cfg

	self:init_archetype()
end

function monster_mgr:init_archetype(  )
	self.monster_archetype = self.entity_mgr:CreateArchetype({
		"UMO.Position", "UMO.UID", "UMO.TypeID"
	})
	self.monster_nest_archetype = self.entity_mgr:CreateArchetype({
		"UMO.Position", "umo.monster_nest",
	})
end

function monster_mgr:create_monster( type_id, pos )
	local monster = self.entity_mgr:CreateEntityByArcheType(self.monster_archetype)
	self.entity_mgr:SetComponentData(monster, "UMO.Position", {x=pos.x, y=pos.y, z=pos.z})
	self.entity_mgr:SetComponentData(monster, "UMO.UID", {value=new_scene_uid(SceneObjectType.Monster)})
	self.entity_mgr:SetComponentData(monster, "UMO.TypeID", {value=type_id})
end

return monster_mgr