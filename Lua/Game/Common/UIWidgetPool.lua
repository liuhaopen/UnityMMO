UIWidgetPool = {}

function UIWidgetPool:Init( hide_container )
	self.pools = {}
	self.prefab_map = {}
	self.hide_container = GameObject.Find(hide_container)
	assert(self.hide_container, "UIWidgetPool:Init failed, cannot find hide container name :".. hide_container)
	self.hide_container = self.hide_container.transform
end

function UIWidgetPool:RegisterWidgets( names, call_back )
	print('Cat:UIWidgetPool.lua[16] AppConfig.DebugMode', AppConfig.DebugMode, names, call_back)
	-- if AppConfig.DebugMode then
		for i,v in ipairs(names) do
			local on_load = function ( objs )
				assert(objs and objs[0], "UIWidgetPool:RegisterWidgets load prefab failed in debug mode!")
				local prefab = objs[0]
				--只保留文件名作key值
				local key = string.gsub(v,".+/","")
				local start_index = string.find(key, "%.")
				assert(start_index, "UIWidgetPool:RegisterWidgets failed! load a wrong file : "..v)
				key = string.sub(key, 1, start_index-1)
				print('Cat:UIWidgetPool.lua[23] key', key)
				self.prefab_map[key] = prefab
			end
			ResMgr:LoadPrefab(v, on_load)
		end
	-- else
	-- 	local on_load_succeed = function ( prefabs )
	-- 		assert(#prefabs ~= #names, "UIWidgetPool:RegisterWidgets load prefab failed!")
	-- 		for i,v in ipairs(names) do
	-- 			assert(self.prefab_map[v]==nil, "UIWidgetPool:RegisterWidgets already register widget name :"..v)
	-- 			self.prefab_map[v] = prefabs[i-1]
	-- 		end
	-- 		if call_back then
	-- 			call_back()
	-- 		end
	-- 	end
	-- 	assert(false, "not support assets bundle yet")
	-- 	--Cat_Todo : 有空再把Luaframework原来的资源管理改成用全路径的
	-- 	ResMgr:LoadPrefab(names, on_load_succeed)
	-- end
end

--创建一个ui通用控件
function UIWidgetPool:CreateWidget( name )
	name = name or ""
	local prefab = self.prefab_map[name]
	assert(prefab, "UIWidgetPool:create widget failed, cannot find widget prefab name :"..name)

	local pool = self.pools[name]
	if pool and #pool > 0 then
		return table.remove(pool, #pool)
	else
		local gameObject = Util.InstantiateObject(self.prefab_map[name])
		local widget = {prefab_name=name, gameObject=gameObject, transform=gameObject.transform}
		return widget
	end
end

function UIWidgetPool:RecycleWidget( widget )
	assert(widget and widget.prefab_name, "UIWidgetPool:recycle widget failed,are you sure recycle an widget which came from UIWidgetPool:CreateWidget?")
	widget.transform:SetParent(self.hide_container)
	local pool = self.pools[widget.prefab_name]
	pool = pool or {}
	pool[#pool+1] = widget
	self.pools[widget.prefab_name] = pool
end