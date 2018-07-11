-- require "3rd/pblua/login_pb"
-- require "3rd/pbc/protobuf"
require "Common/ui/UIHelper"

-- local lpeg = require "lpeg"

-- local json = require "cjson"
-- local util = require "3rd/cjson/util"

-- local sproto = require "3rd/sproto/sproto"
-- local core = require "sproto.core"
-- local print_r = require "3rd/sproto/print_r"

require "Logic/LuaClass"
-- require "Logic/CtrlManager"
require "Common/functions"
-- require "Controller/PromptCtrl"

--管理器--
Game = {};
local this = Game;

local game; 
local transform;
local gameObject;
local WWW = UnityEngine.WWW;

function Game.InitViewPanels()
    local loginCtrl = require("UI/Login/LoginController")
    loginCtrl.Init()
    
    this.loginView = require("UI/Login/LoginView")
    this.loginView:Open()
end

--初始化完成，发送链接服务器信息--
function Game.OnInitOK()
    print('Cat:Game.lua[Game.OnInitOK()]')
    --注册LuaView--
    this.InitViewPanels();

    logWarn('LuaFramework InitOK--->>>');
end

--测试sproto--
function Game.test_sproto_func()
    logWarn("test_sproto_func-------->>");
    local sp = sproto.parse [[
    .Person {
        name 0 : string
        id 1 : integer
        email 2 : string

        .PhoneNumber {
            number 0 : string
            type 1 : integer
        }

        phone 3 : *PhoneNumber
    }

    .AddressBook {
        person 0 : *Person(id)
        others 1 : *Person
    }
    ]]

    local ab = {
        person = {
            [10000] = {
                name = "Alice",
                id = 10000,
                phone = {
                    { number = "123456789" , type = 1 },
                    { number = "87654321" , type = 2 },
                }
            },
            [20000] = {
                name = "Bob",
                id = 20000,
                phone = {
                    { number = "01234567890" , type = 3 },
                }
            }
        },
        others = {
            {
                name = "Carol",
                id = 30000,
                phone = {
                    { number = "9876543210" },
                }
            },
        }
    }
    local code = sp:encode("AddressBook", ab)
    local addr = sp:decode("AddressBook", code)
    print_r(addr)
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
