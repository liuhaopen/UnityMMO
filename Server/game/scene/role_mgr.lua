local role_mgr = {}

function role_mgr:init( scene )
	self.scene_mgr = scene
	self.entity_mgr = scene.entity_mgr
	self.aoi = scene.aoi

	self:init_archetype()
end

function role_mgr:init_archetype(  )
	self.role_archetype = self.entity_mgr:CreateArchetype({
		"umo.position", "umo.target_pos", "umo.uid", "umo.type_id", "umo.hp", "umo.scene_obj_type",
	})
end

function role_mgr:create_role( uid, role_id, pos_x, pos_y, pos_z )
	local role = self.entity_mgr:CreateEntityByArcheType(self.role_archetype)
	self.entity_mgr:SetComponentData(role, "umo.position", {x=pos_x, y=pos_y, z=pos_z})
	self.entity_mgr:SetComponentData(role, "umo.target_pos", {x=pos_x, y=pos_y, z=pos_z})
	self.entity_mgr:SetComponentData(role, "umo.uid", {value=uid})
	self.entity_mgr:SetComponentData(role, "umo.type_id", {value=role_id})
	self.entity_mgr:SetComponentData(role, "umo.hp", {cur=1000, max=1000})
	self.entity_mgr:SetComponentData(role, "umo.scene_obj_type", {value=SceneObjectType.Role})
	return role
end

return role_mgr