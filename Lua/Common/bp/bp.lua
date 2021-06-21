local bp = bp or {}

--[[
词汇表：
oo：object-oriented面向对象
sequence：队伍，按顺序一个一个地执行
parallel：并行队伍，同时执行
slot：管理孔，一个节点有输入孔和输出孔，用来节点间传输信息
graph：管理一颗树的数据，包含用户自定义数据，黑板，孔的数据
bb：blackboard黑板的意思，用于节点间共享数据，每颗行为树都有一块黑板
set_bb：设置黑板变量
--]]
bp.oo = require("bp.common.lua_oo")
bp.sequence = require("bp.src.sequence")
bp.parallel = require("bp.src.parallel")
bp.call_func = require("bp.src.call_func")
bp.wait = require("bp.src.wait")
bp.random = require("bp.src.random")
bp.if_else = require("bp.src.if_else")
bp.all_true = require("bp.src.all_true")
bp.any_true = require("bp.src.any_true")
bp.loop = require("bp.src.loop")
bp.slot_mgr = require("bp.src.slot_mgr")
bp.graph = require("bp.src.graph")
bp.set_bb = require("bp.src.set_bb")
bp.get_bb = require("bp.src.get_bb")
bp.operator = require("bp.src.operator")
bp.op = bp.operator
bp.log = require("bp.src.log")

local interval_actions = require("bp.src.action.action_interval")
for k,v in pairs(interval_actions) do
	assert(not bp[k], "already has class : "..(k))
	bp[k] = v
end

return bp