LuaGameObj = LuaGameObj or {}

function LuaGameObj.Create(obj_info)
	self.name = obj_info and obj_info.name
	self.components = {}
end

function LuaGameObj.AddComponent( component )
	self.components[]
end

function LuaGameObj.Update(  )
	for k,v in pairs(self.components) do
		if v.Update then
			v:Update()
		end
	end
end
