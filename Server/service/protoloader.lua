-- module proto as examples/proto.lua
package.path = "./examples/?.lua;" .. package.path

local skynet = require "skynet"
local sprotoparser = require "sprotoparser"
local sprotoloader = require "sprotoloader"
local proto = require "proto"
require "util.util"

skynet.start(function()
	--从协议目录里读取所有的lua文件,拼接其字符串生成sproto协议对象
 	local s = io.popen("ls ../Lua/Common/Proto")
	local fileNames = s:read("*all")
	fileNames = Split(fileNames, "\n")
	local proto_c2s_tb = {}
    for k,v in pairs(fileNames or {}) do
    	local dot_index = string.find(v, ".", 1, true)
    	local is_lua_file = string.find(v, ".lua", -4, true)
    	if Trim(v) ~= "" and dot_index ~= nil and is_lua_file then
	    	local name_without_ex = string.sub(v, 1, dot_index-1)
	        local proto_str = require("Proto."..name_without_ex)
	        if proto_str then
	            table.insert(proto_c2s_tb, proto_str)
	        end
        end
    end
    local c2s_spb = sprotoparser.parse(table.concat(proto_c2s_tb))
	sprotoloader.save(c2s_spb, 1)
	-- don't call skynet.exit() , because sproto.core may unload and the global slot become invalid
end)
