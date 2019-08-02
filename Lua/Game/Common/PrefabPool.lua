local PrefabPool = {}

function PrefabPool:Init( hide_container )
	self.pools = {}
	self.prefab_map = {}
	self.hide_container = GameObject.Find(hide_container)
	assert(self.hide_container, "PrefabPool:Init failed, cannot find hide container name :".. hide_container)
	self.hide_container = self.hide_container.transform
end

function PrefabPool:Register( names )
	for i,v in ipairs(names) do
		local on_load = function ( objs )
			assert(objs and objs[0], "PrefabPool:Register load prefab failed in debug mode!")
			local prefab = objs[0]
			--只保留文件名作key值
			local key = string.gsub(v,".+/","")
			local start_index = string.find(key, "%.")
			assert(start_index, "PrefabPool:Register failed! load a wrong file : "..v)
			key = string.sub(key, 1, start_index-1)
			self.prefab_map[key] = prefab
		end
		ResMgr:LoadPrefab(v, on_load)
	end
end

--创建一个ui通用控件
function PrefabPool:Get( name )
	name = name or ""
	local prefab = self.prefab_map[name]
	assert(prefab, "PrefabPool:create widget failed, cannot find widget prefab name :"..name)

	local pool = self.pools[name]
	if pool and #pool > 0 then
		return table.remove(pool, #pool)
	else
		local gameObject = Util.InstantiateObject(self.prefab_map[name])
		gameObject.name = name
		local widget = {prefab_name=name, gameObject=gameObject, transform=gameObject.transform}
		return widget
	end
end

function PrefabPool:Recycle( widget )
	assert(widget and widget.prefab_name, "PrefabPool:recycle widget failed,are you sure recycle an widget which came from PrefabPool:Get?")
	widget.transform:SetParent(self.hide_container)
	local pool = self.pools[widget.prefab_name]
	pool = pool or {}
	pool[#pool+1] = widget
	self.pools[widget.prefab_name] = pool
end

return PrefabPool