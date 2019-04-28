local BP = BP or {}

local package = package
local debug = debug
package.upath = "./?.lua"
local function load_env(filename)
	local f,err = loadfile(filename)
	if f == nil then
		return err
	end
	return function()
		return function(env)
			if env then
				debug.setupvalue(f, 1, env)
			end
			-- local _ENV = env
			print('Cat:Blueprint.lua[17] filename', filename)
			return f(filename)
		end
	end
end
local function searcher_env(name)
	local filename, err = package.searchpath(name, package.upath)
	if filename == nil then
		return err
	else
		return load_env(filename)
	end
end
local searchers = package.searchers
-- table.insert(package.searchers, 1, searcher_env)

-- print('Cat:Blueprint.lua[3] _ENV', _ENV)
-- local old_require = require
local require = require("Blueprint.Core.BPImport")
-- require = new_require and new_require or require

local BPEnv = {
	BP = BP
}
setmetatable(BPEnv, {
	__index = function( t, k )
		if t[k] then
			return t[k]
		else
			return _ENV[k]
		end
	end,	
	__newindex = function (t,k,v)
		error("attempt to update a read-only table", 2)
	end,	
})
print('Cat:Blueprint.lua[51] BPEnv', BPEnv)
BP.BlueprintGraph = require "Core.BlueprintGraph"(BPEnv)
print('Cat:Blueprint.lua[52] BP.BlueprintGraph', BP.BlueprintGraph)

-- require = old_require
-- package.searchers = searchers
table.remove(package.searchers, 1)

local ret = BP
--不要全局变量
BP = nil 
return ret