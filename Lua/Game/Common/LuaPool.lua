local LuaPool = BaseClass()

--[[
objInfos:类型为数组，其中元素包含每个对象池的信息：name字符串类型,createFunc创建函数,maxNum允许最大数量
--]]
function LuaPool:Init( objInfos )
	self.pool = {}
	self.objInfos = objInfos
end

function LuaPool:Register( objInfo )
	if not objInfo then return end
	self.objInfos[objInfo.name] = objInfo
end

function LuaPool:Get( name )
	-- print('Cat:LuaPool.lua[Get] name', name)
	if not name then
		LogError("LuaPool:Get error : name nil")
		return
	end
	local objs = self.pool[name]
	if objs and #objs > 0 then
		if name=="GoodsInfoView" then
			print('Cat:LuaPool.lua[24] #objs', #objs)
		end
		return table.remove(objs, #objs)
	end
	local info = self.objInfos[name]
	if not info then
		LogError("LuaPool:Get error : register info nil : name "..name)
	end
	if info.prototype then
		return info.prototype.New()
	end
	if info.createFunc then
		return info.createFunc()
	end
	LogError("LuaPool:Get error : register info has not assign prototype or createFunc : name "..name)
	return nil
end

function LuaPool:Recycle( name, obj )
	-- print('Cat:LuaPool.lua[Recycle] name, obj', name, obj)
	if name then
		if obj then
			if obj.Recycle then
				obj:Recycle()
			end
			local objs = self.pool[name]
			objs = objs or {}
			objs[#objs+1] = obj
			self.pool[name] = objs
		else
			LogError("LuaPool:Recycle error : obj nil")
		end
	else
		LogError("LuaPool:Recycle error : name nil")
	end
end

return LuaPool