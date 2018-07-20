require "Common/ui/UIHelper"
require "Logic/LuaClass"
require "Common/functions"
require "UI/Common/UIManager"
-- local lpeg = require "lpeg"
-- local json = require "cjson"
-- local util = require "3rd/cjson/util"

--管理器--
Game = {}
local this = Game

local game
local transform
local gameObject
-- local WWW = UnityEngine.WWW

Ctrls = {}

--初始化完成，发送链接服务器信息--
function Game.OnInitOK()
    print('Cat:Game.lua[Game.OnInitOK()]')

    local ctrl_paths = {
        -- "UI/Error/ErrorController", 
        "UI/Test/TestController",
        "UI/Login/LoginController", 

    }
    for i,v in ipairs(ctrl_paths) do
        local ctrl = require(v)
        if type(ctrl) ~= "boolean" then
            --调用每个Controller的Init函数
            ctrl.Init()
            table.insert(Ctrls, ctrl)
        else
            --Controller类忘记了在最后return
            assert(false, 'Cat:Main.lua error : you must forgot write a return in you controller file :'..v)
        end
    end

    logWarn('LuaFramework InitOK--->>>');
end

--测试lpeg--
function Game.test_lpeg_func()
	logWarn("test_lpeg_func-------->>");
	-- matches a word followed by end-of-string
	local p = lpeg.R"az"^1 * -1

	print(p:match("hello"))        --> 6
	print(lpeg.match(p, "hello"))  --> 6
	print(p:match("1 hello"))      --> nil
end

--测试cjson--
function Game.test_cjson_func()
    local path = Util.DataPath.."lua/3rd/cjson/example2.json";
    local text = util.file_load(path);
    LuaHelper.OnJsonCallFunc(text, this.OnJsonCall);
end

--cjson callback--
function Game.OnJsonCall(data)
    local obj = json.decode(data);
    print(obj['menu']['id']);
end

--销毁--
function Game.OnDestroy()
	--logWarn('OnDestroy--->>>');
end
