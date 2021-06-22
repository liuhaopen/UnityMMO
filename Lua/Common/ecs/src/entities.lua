local mt = {}

function mt:on_new()
	self.archetypes = {}
	self.e_to_arche_map = {}
	self.euid = 0
end

function mt:foreach(filter, cb)
	local chunks = self:get_chunks_by_filter(filter)
	for _,chunk in ipairs(chunks) do
		for _,e_data in pairs(chunk) do
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
				if not self:is_match(com_name_or_filter) then
					return false
				end
			end
			return true
		elseif filter.type == "any" then
			for _,com_name_or_filter in ipairs(filter.com_name_or_filters) do
				if self:is_match(com_name_or_filter) then
					return true
				end
			end
			return false
		elseif filter.type == "no" then
			for _,com_name_or_filter in ipairs(filter.com_name_or_filters) do
				if self:is_match(com_name_or_filter) then
					return false
				end
			end
			return true
		end
	end
	return true
end

function mt:create_entity(...)
	self.euid = self.euid + 1
	local e = self.euid
	local com_names = {...}
	local archetype = self:get_archetype(com_names)
	self:__add_to_archetype(e, archetype)
	return e
end

function mt:destroy_entity(e)
	local archetype = self.e_to_arche_map[e]
	archetype.chunk[e] = nil
	self.e_to_arche_map[e] = nil
end

function mt:__add_to_archetype( e, archetype )
	archetype.chunk[e] = {
		entity = e,
	}
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
	local is_same_archetype = self:has_component(e, com_name)
	local archetype = self.e_to_arche_map[e]
	if is_same_archetype then
		archetype.chunk[e][com_name] = com_val
	else
		local new_com_names = {}
		for name,v in pairs(archetype.com_names_map) do
			new_com_names[#new_com_names + 1] = name
		end
		local new_archetype = self:get_archetype(new_com_names)
		self:destroy_entity(e)
		self:__add_to_archetype(e, new_archetype)
	end
end

function mt:has_component(e, com_name)
	local archetype = self.e_to_arche_map[e]
	return archetype.com_names_map[com_name]
end

function mt:get_archetype(com_names)
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

return mt