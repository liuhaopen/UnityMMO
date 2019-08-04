local Ac = Ac or {}

local importer = require("Action.Common.Importer")
importer.enable()

--让本框架里的文件都有Ac这个全局变量
local AcEnv = {
	Ac = Ac
}
setmetatable(AcEnv, {
	__index = _ENV,	
	__newindex = function (t,k,v)
		--本框架内不允许新增和修改全局变量，实在想要的也可以使用_ENV.xx = yy这种形式，但我像是这种没节操的人吗？！
		error("attempt to set a global value", 2)
	end,
})

Ac.OO = importer.require("Action.Common.LuaOO", AcEnv)
Ac.ActionInterval = importer.require("Action.Src.ActionInterval", AcEnv)

--为了不影响全局，这里要还原一下package.searchers
importer.disable()

return Ac