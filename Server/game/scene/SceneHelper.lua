local SceneHelper = {scene_uid_counter={0,0,0}}

function SceneHelper:new_scene_uid( scene_obj_type )
	self.scene_uid_counter[scene_obj_type] = self.scene_uid_counter[scene_obj_type] + 1
	return scene_obj_type*10000000000 + self.scene_uid_counter[scene_obj_type]
end

--返回SceneConst.ObjectType枚举类型
function SceneHelper:get_type_by_uid( scene_uid )
    local type = math.floor(scene_uid/10000000000)
    return type
end

SceneHelper.add_info_item = function ( change_obj_infos, scene_uid, info_item )
	change_obj_infos = change_obj_infos or {obj_infos={}}
	local cur_info = nil
	for i,v in ipairs(change_obj_infos.obj_infos) do
		if v.scene_obj_uid == scene_uid then
			cur_info = v
		end
	end
	if not cur_info then
		cur_info = {scene_obj_uid=scene_uid, info_list={}}
		table.insert(change_obj_infos.obj_infos, cur_info)
	end
	table.insert(cur_info.info_list, info_item)
	return change_obj_infos
end

return SceneHelper