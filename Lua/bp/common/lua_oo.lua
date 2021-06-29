-- See details for additional information : http://angg.twu.net/LATEX/dednat6/eoo.lua.html
local OO = {}

OO.class = {
    type   = "class",
    __call = function (class, o) return setmetatable(o, class) end,
}
setmetatable(OO.class, OO.class)

-- OO.class_by_name = {
--     type   = "class_by_name",
--     __call = function (class, o) 
--     	local new_cls = {type="action_interval"}
--     	setmetatable(new_cls, class)
--     	return new_cls
--     end,
-- }
-- setmetatable(OO.class_by_name, OO.class_by_name)

OO.type = function (o)
	local  mt = getmetatable(o)
	return mt and mt.type or type(o)
end

-- OO.over = function (lowertable)
-- 	return function (uppertable)
-- 	    setmetatable(uppertable, {__index=lowertable})
-- 	    return uppertable
--   	end
-- end

OO.class_over = function (parent)
	return function (child)
	    setmetatable(child.__index, {__index=parent.__index})
	    return OO.class(child)
  	end
end

OO.has_func = function (o, func_name)
	-- if o and type(o)=="table" and o.__index and o.__index[func_name] then
	if o and type(o)=="table" and o[func_name] then
		return true
	end 
	return false
end

OO.callable = function (o)
	local o_type = type(o)
	if o_type =="table" then
		local meta_tbl = getmetatable(o)
		return meta_tbl.__call ~= nil
	elseif o_type == "function" then
		return true
	end
	return false
end

return OO