package.path = [[../../Lua/Common/?.lua;../../Lua/?.lua]]
local sprotoparser = require "sprotoparser"
local param = {...}
local proto_c2s_tb = {}
local proto_s2c_tb = {}
for k,v in pairs(param or {}) do
    local proto_str = require("Proto/"..v)
    if proto_str[1] then
        table.insert(proto_c2s_tb, proto_str[1])
    end
    if proto_str[2] then
        table.insert(proto_s2c_tb, proto_str[2])
    end
end

local c2s_spb = sprotoparser.parse(table.concat(proto_c2s_tb))
local s2c_spb = sprotoparser.parse(table.concat(proto_s2c_tb))
-- print("c2s_spb : "..c2s_spb)
-- print("s2c_spb : "..s2c_spb)
local file = io.open ("sproto_c2s.spb", "wb")
-- print("file : "..file)
file:write(c2s_spb)
file:close()

file = io.open ("sproto_s2c.spb", "wb")
file:write(s2c_spb)
file:close()

if c2s_spb then
	print("generate c2s spb succeed!")
else
	print("generate c2s spb failed!")
end

if s2c_spb then
	print("generate s2c spb succeed!")
else
	print("generate s2c spb failed!")
end