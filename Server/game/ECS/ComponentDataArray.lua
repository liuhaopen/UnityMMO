local ComponentDataArray = {}
ECS.ComponentDataArray = ComponentDataArray

function ComponentDataArray:Constructor( iterator, length )
	self.m_Iterator = iterator
	self.m_Cache = nil
	self.m_Length = length
end

local get_fun = function ( t, k )
	
end

local set_fun = function ( t, k )
	
end

local _index_fun_map_ = {
	["get"] = get_fun,
	["get"] = get_fun,
}

local _index_fun = function(t, k)
	return _index_fun_map_[k](t, k)
end

setmetatable(ComponentDataArray, { __index=_index_fun })

