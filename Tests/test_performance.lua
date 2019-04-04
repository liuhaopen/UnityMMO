package.path = package.path ..';../?.lua;../../?.lua;Tests/?.lua';

local ECSCore = require("ECSCore")

local test_times = 1000000
local core_cost = 0
local lua_cost = 0

local chunk = ECSCore.CreateChunk(test_times*8)
local test_tbl = {}
for i=1,test_times  do
    test_tbl[i] = i
end

local bt = os.clock()
for i = 1, test_times do
    ECSCore.WriteNumber(chunk, (i-1)*8, i)
    local vi = ECSCore.ReadNumber(chunk, (i-1)*8)
end
core_cost = core_cost +  os.clock() - bt

bt = os.clock()
for i = 1, test_times do
    test_tbl[i] = i
    local v1 = test_tbl[i]
end
lua_cost = lua_cost +  os.clock() - bt
print("ecs core cost", core_cost)
print("lua cost", lua_cost)