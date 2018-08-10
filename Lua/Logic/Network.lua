-- require "Common/protocal"
-- require "Common/functions"
-- Event = require 'events'
-- local sproto = require "Common.util.sproto"
-- local sprotoparser = require "Common.util.sprotoparser"
-- local crypt = require "crypt"
-- local print_r = require "print_r"

Network = {};
local this = Network;

local print_net = function() end
-- print_net = print --注释掉就不打印网络信息了
print("load network lua file")
function Network.Start() 
    print("Network.Start!!");
    this.session = 0
    this.response_call_back = {}

    -- this.InitSpb()

    -- Event.AddListener(Protocal.Connect, this.OnConnect)
    -- Event.AddListener(Protocal.Message, this.OnMessage)
    -- Event.AddListener(Protocal.Exception, this.OnException)
end

function Network.InitSpb()
    print_net('Cat:Network.lua[27] AppConfig.SprotoBinMode', AppConfig.SprotoBinMode)
    if AppConfig.SprotoBinMode then
        local c2s_path = Util.DataPath.."sproto_c2s.spb";
        local c2s_file = io.open(c2s_path,'r')
        print_net('Cat:Network.lua[29] c2s_file', c2s_file, c2s_path)
        local c2s_data
        if c2s_file then
            c2s_data = c2s_file:read("*a")
        else
            print_net('Cat:Network.lua[had not find spb file : ', c2s_path)
        end
        this.sproto_c2s = sproto.new(c2s_data)
    else
        local fileNames = Util.GetFileNamesInFolder(AppConfig.LuaAssetsDir.."/Common/Proto")
        fileNames = Split(fileNames, ",")
        local proto_c2s_tb = {}
        for k,v in pairs(fileNames or {}) do
            local proto_str = require("Proto."..v)
            -- print_net('Cat:Network.lua[35] c2s : ', proto_str)
            if proto_str then
                table.insert(proto_c2s_tb, proto_str)
            end
        end
        local sprotoparser = require "sprotoparser"
        local c2s_spb = sprotoparser.parse(table.concat(proto_c2s_tb))
        this.sproto_c2s = sproto.new(c2s_spb)
    end
end

--Socket消息--
-- function Network.OnSocket(key, data)
--     Event.Broadcast(tostring(key), data);
-- end

--当连接建立时--
function OnConnectServer() 

    print("Game Server connected!!");
end

--异常断线--
function Network.OnException() 
    -- NetManager:SendConnect();
   	logError("OnException------->>>>");
end

--连接中断，或者被踢掉--
-- function Network.OnDisconnect() 
--     logError("OnDisconnect------->>>>");
-- end

function Network.SendMessage( req_name, req_arg, response_call_back )
    print_net('Cat:Network.lua[57] req_name, req_arg, response_call_back', req_name, req_arg, response_call_back)
    this.session = this.session + 1
    local buffer = ByteBuffer.New();
    local code, tag = this.sproto_c2s:request_encode(req_name, req_arg)
    print_net('Cat:Network.lua[129] tag', tag)
    print_net('Cat:Network.lua[117] code', code)
    if response_call_back then
        this.response_call_back[this.session] = {req_name, response_call_back}
    else
    end
    local pack_str = string.pack(">IA>I", tag, code, this.session)
    print_net('Cat:LoginController.lua[82] pack_str', pack_str)
    print_net('Cat:Network.lua[139] len :', #pack_str)
    -- for i=1,#pack_str do
    --     print_net(pack_str:byte(i))
    -- end
    buffer:WriteBuffer(pack_str);
    NetMgr:SendMessage(buffer);
end

function Network.Listen( req_name, req_arg, response_call_back )
    assert(response_call_back, "Network.Listen call back empty : "..req_name)
    local on_ack
    on_ack = function ( ack_data )
        response_call_back(ack_data)
        --因为用了skynet的loginserver那一套,是不支持后端主推协议的,所以只好每次收到回复后再请求
        Network.SendMessage(req_name, req_arg, on_ack)
    end
    Network.SendMessage(req_name, req_arg, on_ack)
end

function Network.SwitchToWaitForGameServerHandshake() 
    --因为连接上游戏服务器后收到的第一条数据是握手校验,其数据结构不一样,所以要临时换下接收网络数据的函数
    Event.RemoveListener(Protocal.Message)
    Event.AddListener(Protocal.Message, Network.OnMessageForGameServerHandshake)
end

function Network.OnMessageForGameServerHandshake(buffer) 
    local code = buffer:ToLuaString()
    print_net('Cat:Network.lua[handshake] code', code)
    Event.RemoveListener(Protocal.Message)
    Event.AddListener(Protocal.Message, Network.OnMessage)
    local result = string.sub(code, 1, 3)
    print_net('Cat:Network.lua[handshake] result code', result, tonumber(result))
    if tonumber(result) == 200 then
        Event.Broadcast(LoginConst.Event.LoginSucceed)

        local on_server_time_ack = function ( server_time_info )
            print_net('Cat:Network.lua[118] server_time_info:', server_time_info.server_time)
            this.server_time = server_time_info.server_time
        end
        this.SendMessage("account_get_server_time", nil, on_server_time_ack)
    else
        --Cat_Todo : 处理握手失败
    end
end

function Network.OnMessage(buffer) 
    local code = buffer:ToLuaString()
    print_net('Cat:Network.lua[OnMessage] code:|'..code.."|", #code)
    local content_size = #code - 5
    assert(content_size >= 0)
    local _, content, result, session = string.unpack(code, "A"..content_size.."b>I")
    print_net('Cat:Network.lua[149] content|'..content.."|")
    print_net('Cat:Network.lua[179] result, session', result, session)
    if session and this.response_call_back[session] and #this.response_call_back[session]==2 then
        print_net('Cat:Network.lua[168] this.sproto_c2s', this.sproto_c2s, this.response_call_back[session][1])
        for i=1,#code do
            print_net(code:byte(i))
        end
        local encode = this.sproto_c2s:response_decode(this.response_call_back[session][1], content)
        print_net('Cat:Network.lua[168] encode', encode)
        this.response_call_back[session][2](encode)
        this.response_call_back[session] = nil
    end
end

--卸载网络监听--
function Network.Unload()
    Event.RemoveListener(Protocal.Message)
    Event.RemoveListener(Protocal.Exception)
    print('Unload Network...')
end
