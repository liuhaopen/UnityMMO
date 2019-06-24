local function destroyFunc(self)
	if self.is_destroyed then  --是否已经调过一次DeleteMe
		return
	end
	self.is_destroyed = true

	local now_super = self.__class_type 
	while now_super ~= nil do	
		local ondestroy = rawget(now_super, "OnDestroy")
		print('Cat:BaseClass.lua[10] ondestroy', ondestroy)
		if ondestroy then  --每一个类调OnDestroy方法
			ondestroy(self)
		end
		now_super = now_super.super
	end
end

function BaseClass(super)
	local class_type={}
	class_type.Constructor=false
	class_type.DefaultVar=false
	class_type.super=super
	class_type.New=function(...) 
		local obj=nil
		local create
		create = function(c, obj, ...)
			if c.super then
				create(c.super, obj, ...)
			end
			if c.Constructor then
				c.Constructor(obj,...)
			end
		end
		if class_type.DefaultVar then
			obj = class_type.DefaultVar(obj)
		else
			obj = {}
		end
		local function meta_func(t, k)
			local ret = class_type[k]
			obj[k] = ret
			return ret
		end
		setmetatable(obj, { __index=meta_func })
		create(class_type, obj, ...)
		obj.__class_type = class_type
		obj.Destroy = destroyFunc
		return obj
	end

	if super then
		setmetatable(class_type,{__index=
			function(t,k)
				local ret=super[k]
				class_type[k]=ret
				return ret
			end
		})
	end
 
	return class_type
end

return BaseClass