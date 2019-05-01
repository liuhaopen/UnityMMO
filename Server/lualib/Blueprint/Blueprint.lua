local BP = BP or {}

local importer = require("Blueprint.Core.Importer")
importer.enable()

--让本框架里的文件都有BP这个全局变量
local BPEnv = {
	BP = BP
}
setmetatable(BPEnv, {
	__index = _ENV,	
	__newindex = function (t,k,v)
		--本框架内不允许新增和修改全局变量，实在想要的也可以使用_ENV.xx = yy这种形式，但我像是这种没节操的人吗？！
		error("attempt to set a global value", 2)
	end,
})


BP.BaseClass = importer.require("Blueprint.Core.BaseClass", BPEnv)
BP.Node = importer.require("Blueprint.Core.Node", BPEnv)
BP.Graph = importer.require("Blueprint.Core.Graph", BPEnv)

BP.UpdateEvent = importer.require("Blueprint.Event.UpdateEvent", BPEnv)


BP.TypeManager = importer.require("Blueprint.Core.TypeManager", BPEnv)
BP.TypeManager:InitDefaultTypes()

--为了不影响全局，这里要还原一下package.searchers
importer.disable()

return BP