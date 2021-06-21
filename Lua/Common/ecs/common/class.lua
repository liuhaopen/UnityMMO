local call_construc
call_construc = function(obj, cls, on_new, ...)
	local parent = cls.__parent_cls__
	if parent then
		call_construc(obj, parent, parent.on_new, ...)
	end
	if on_new then
		on_new(obj, ...)
	end
end

local function class(name, parent)
	local cls = {
		__cls_name__ = name,
		__parent_cls__ = parent,
	}
	cls.__index = cls
	
	cls.new = function(arge1, ...)
		local obj = setmetatable({
			__cls_name__ = name,
			__cls__ = cls,
			__parent_cls__ = parent,
		}, cls)
		call_construc(obj, cls, obj.on_new, ...)
		return obj
    end

    if parent then
		local meta = {}
		meta.__index = function(t, k)
			return parent[k]
	    end
		setmetatable(cls, meta)
	end

	return cls
end

return class