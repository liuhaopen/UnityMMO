local call_construc
call_construc = function(obj, on_new, arge1, ...)
	local parent = obj.__parent_cls__
	if parent then
		call_construc(parent, parent.on_new, arge1, ...)
	end
	if on_new then
		if obj.__cls__ == arge1 then
			on_new(obj, ...)
		else
			on_new(obj, arge1, ...)
		end
	end
end

local function class(name, parent)
	local cls = {
		__cls_name__ = name,
	}
	cls.__index = cls
	
	cls.new = function(arge1, ...)
		local obj = setmetatable({
			__cls_name__ = name,
			__cls__ = cls,
			__parent_cls__ = parent,
		}, cls)
		call_construc(obj, obj.on_new, arge1, ...)
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