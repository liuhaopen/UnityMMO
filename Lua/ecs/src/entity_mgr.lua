local class = require "ecs.common.class"

local mt = class("entity_mgr")

function mt:on_new()
	self.archetypes = {}
	self.e_to_arche_map = {}
	self.eid = 0
end

function mt:foreach(filter, cb)
	local chunks = self:get_chunks_by_filter(filter)
	for _,chunk in ipairs(chunks) do
		for _,e_data in pairs(chunk) do
			--CAT_TODO:应该要返回一个中间table，防止在cb里访问不存在的component
			cb(e_data)
		end
	end
end

function mt:get_chunks_by_filter(filter)
	local ret = {}
	for archetype_name,v in pairs(self.archetypes) do
		local archetype = self.archetypes[archetype_name]
		if self:is_match(archetype.com_names_map, filter) then
			ret[#ret + 1] = v.chunk
		end
	end 
	return ret
end

function mt:is_match(com_names_map, filter)
	if type(filter) == "string" then
		return com_names_map[filter]
	else
		if filter.type == "all" then
			for _,com_name_or_filter in ipairs(filter.com_name_or_filters) do
				if not self:is_match(com_names_map, com_name_or_filter) then
					return false
				end
			end
			return true
		elseif filter.type == "any" then
			for _,com_name_or_filter in ipairs(filter.com_name_or_filters) do
				if self:is_match(com_names_map, com_name_or_filter) then
					return true
				end
			end
			return false
		elseif filter.type == "no" then
			for _,com_name_or_filter in ipairs(filter.com_name_or_filters) do
				if self:is_match(com_names_map, com_name_or_filter) then
					return false
				end
			end
			return true
		end
	end
	return false
end

function mt:create_entity(...)
	local com_names = {...}
	local archetype = self:get_archetype_by_list(com_names)
	return self:create_entity_by_archetype(archetype)
end

function mt:create_entity_by_archetype(archetype)
	self.eid = self.eid + 1
	local e = self.eid
	self:__add_to_archetype(e, archetype)
	return e
end

function mt:is_entity_exist(e)
	return self.e_to_arche_map[e] ~= nil
end

function mt:destroy_entity(e)
	local archetype = self.e_to_arche_map[e]
	archetype.chunk[e] = nil
	self.e_to_arche_map[e] = nil
end

function mt:__add_to_archetype( e, archetype )
	local last_archetype = self.e_to_arche_map[e]
	if last_archetype then
		archetype.chunk[e] = last_archetype.chunk[e]
		last_archetype.chunk[e] = nil
	else
		archetype.chunk[e] = {
			entity = e,
		}
	end
	self.e_to_arche_map[e] = archetype
end

function mt:get_component(e, com_name)
	local archetype = self.e_to_arche_map[e]
	if archetype then
		return archetype.chunk[e][com_name]
	end
	return nil
end

function mt:set_component(e, com_name, com_val)
	local already_has_com = self:has_component(e, com_name)
	local archetype = self.e_to_arche_map[e]
	if already_has_com then
		archetype.chunk[e][com_name] = com_val
	else
		local new_com_names = {}
		for name,v in pairs(archetype.com_names_map) do
			new_com_names[#new_com_names + 1] = name
		end
		new_com_names[#new_com_names + 1] = com_name
		local new_archetype = self:get_archetype_by_list(new_com_names)
		self:__add_to_archetype(e, new_archetype)
		new_archetype.chunk[e][com_name] = com_val
	end
end

function mt:remove_component(e, com_name)
	local already_has_com = self:has_component(e, com_name)
	if already_has_com then
		local archetype = self.e_to_arche_map[e]
		local new_com_names = {}
		for name,v in pairs(archetype.com_names_map) do
			if name ~= com_name then
				new_com_names[#new_com_names + 1] = name
			end
		end
		local new_archetype = self:get_archetype_by_list(new_com_names)
		self:__add_to_archetype(e, new_archetype)
		new_archetype.chunk[e][com_name] = nil
	end
end

function mt:has_component(e, com_name)
	local archetype = self.e_to_arche_map[e]
	if archetype.com_names_map[com_name] then
		return true
	end
	return false
end

function mt:get_archetype_by_list(com_names)
	table.sort(com_names)
	local str = table.concat(com_names, "$")
	
	if not self.archetypes[str] then
		self.archetypes[str] = {
			com_names_map = nil,
			type_name = str,
			chunk = {}
		}
		local com_names_map = {}
		for _,name in ipairs(com_names) do
			com_names_map[name] = true
		end
		self.archetypes[str].com_names_map = com_names_map
	end
	return self.archetypes[str]
end

function mt:get_archetype(...)
	return self:get_archetype_by_list({...})
end

return mt