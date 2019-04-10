function RequireAllLuaFileInFolder( folder, ignore_list )
	-- print('Cat:helper.lua[2] folder', folder)
	local s = io.popen("ls "..folder)--for linux
	-- local s = io.popen("dir /b Tests")--for windows
	local fileNames = s:read("*all")
	fileNames = Split(fileNames, "\n")
	for k,v in pairs(fileNames or {}) do
		local is_ignore = false
		if ignore_list then
			for ii,vv in ipairs(ignore_list) do
				if v == vv then
					is_ignore = true
					break
				end
			end
		end
		if v~="" and not is_ignore then
			local dot_index = string.find(v, ".", 1, true)
	    	local is_lua_file = string.find(v, ".lua", -4, true)
	    	if dot_index ~= nil and is_lua_file then
		    	local name_without_ex = string.sub(v, 1, dot_index-1)
		    	-- print('test_all.lua init test file name : ', name_without_ex)
				require(name_without_ex)
			end
		end
	end
end