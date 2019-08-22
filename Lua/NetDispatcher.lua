local sproto = require "Common.Util.sproto"
local sprotoparser = require "Common.util.sprotoparser"
-- local print_r = require "print_r"
NetDispatcher = {}

NetDispatcher.Event = {
    OnConnect = "NetDispatcher.Event.OnConnect",
    OnDisConnect = "NetDispatcher.Event.OnDisConnect",
    OnReceiveLine = "NetDispatcher.Event.OnReceiveLine",
}
--当连接建立时
function OnConnectServer() 
    print("Game Server connected!!")
    GlobalEventSystem:Fire(NetDispatcher.Event.OnConnect)
end

function OnDisConnectFromServer() 
    print("Game Server disconnected!!")
    GlobalEventSystem:Fire(NetDispatcher.Event.OnDisConnect)
end

function OnReceiveLineFromServer(bytes) 
    -- print("OnReceiveLineFromServer!", bytes)
    GlobalEventSystem:Fire(NetDispatcher.Event.OnReceiveLine, bytes)
end

function OnReceiveMsgFromServer(bytes) 
    -- print("OnReceiveMsgFromServer!", bytes)
    NetDispatcher:OnMessage(bytes)
end

local print_net = function() end
-- print_net = print --注释掉就不打印网络信息了

function NetDispatcher:Start() 
    print("NetDispatcher.Start!!", GameConst.MaxLuaNetSessionID)
    self.min_lua_net_session_id = GameConst.MinLuaNetSessionID
    self.max_lua_net_session_id = GameConst.MaxLuaNetSessionID
    self.session = self.max_lua_net_session_id
    
    self.response_call_back = {}
    --一开始是不分发sproto协议的,要等游戏验证正式的服务器后才开始
    self:SetEnable(true)
    self:InitSpb()
end

function NetDispatcher:SetEnable( is_enable )
    if is_enable then
        NetDispatcher.OnMessage = NetDispatcher.OriginOnMessage
    else
        NetDispatcher.OnMessage = function(bytes) 
            LoginController:OnReceiveMsg(bytes)
        end
    end
end

function NetDispatcher:InitSpb()
    print_net('Cat:NetDispatcher.lua[27] AppConfig.SprotoBinMode', AppConfig.SprotoBinMode)
    if AppConfig.SprotoBinMode then
        local c2s_path = Util.DataPath.."sproto_c2s.spb";
        local c2s_file = io.open(c2s_path,'r')
        print_net('Cat:NetDispatcher.lua[29] c2s_file', c2s_file, c2s_path)
        local c2s_data
        if c2s_file then
            c2s_data = c2s_file:read("*a")
        else
            print_net('Cat:NetDispatcher.lua[had not find spb file : ', c2s_path)
        end
        self.sproto_c2s = sproto.new(c2s_data)
    else
        local fileNames = Util.GetFileNamesInFolder(AppConfig.LuaAssetsDir.."/Proto")
        fileNames = Split(fileNames, ",")
        local proto_c2s_tb = {}
        for k,v in pairs(fileNames or {}) do
            local proto_str = require("Proto."..v)
            print_net('Cat:NetDispatcher.lua[35] c2s : ', proto_str)
            if proto_str then
                table.insert(proto_c2s_tb, proto_str)
            end
        end
        local c2s_spb = sprotoparser.parse(table.concat(proto_c2s_tb))
        self.sproto_c2s = sproto.new(c2s_spb)
    end
end

function NetDispatcher:SendMessage( req_name, req_arg, response_call_back )
    print_net('Cat:NetDispatcher.lua[57] req_name, req_arg, response_call_back', req_name, req_arg, response_call_back)
    if CS.UnityMMO.GameVariable.IsSingleMode then return end
    self.session = self.session + 1
    -- if self.session >= self.max_lua_net_session_id then
    --     self.session = self.min_lua_net_session_id
    -- end
    local code, tag = self.sproto_c2s:request_encode(req_name, req_arg)
    print_net('Cat:NetDispatcher.lua[129] tag', tag)
    print_net('Cat:NetDispatcher.lua[117] code', code)
    if response_call_back then
        self.response_call_back[self.session] = {req_name, response_call_back}
    end
    -- local pack_str = string.pack(">I4c"..#code..">I4", tag, code, self.session)
    local pack_str = string.pack(">I4", tag)..string.pack("c"..#code, code)..string.pack(">I4", self.session)
    print_net('Cat:LoginController.lua[82] pack_str', pack_str)
    print_net('Cat:NetDispatcher.lua[139] len :', #pack_str)
    -- for i=1,#pack_str do
    --     print_net(pack_str:byte(i))
    -- end
    NetMgr:SendBytes(pack_str)
end

function NetDispatcher:Listen( req_name, req_arg, response_call_back )
    assert(response_call_back, "NetDispatcher.Listen call back empty : "..req_name)
    local on_ack
    on_ack = function ( ack_data )
        response_call_back(ack_data)
        --因为用了skynet的loginserver那一套,是不支持后端主推协议的,所以只好每次收到回复后再请求
        NetDispatcher:SendMessage(req_name, req_arg, on_ack)
    end
    NetDispatcher:SendMessage(req_name, req_arg, on_ack)
end

function NetDispatcher:OriginOnMessage(bytes) 
    -- local code = bytes:ToLuaString()
    local code = tostring(bytes)
    print_net('Cat:NetDispatcher.lua[OnMessage] code:|'..code.."|", #code)
    local content_size = #code - 5
    assert(content_size >= 0)
    local content, result, session = string.unpack("c"..content_size.."B>I4", code)
    -- string.unpack(code, "s"..content_size.."B>I4")
    -- print_net('Cat:NetDispatcher.lua[149] content|'..content.."|")
    -- print_net('Cat:NetDispatcher.lua[179] result, session', result, session)
    if session and self.response_call_back[session] and #self.response_call_back[session]==2 then
        print_net('Cat:NetDispatcher.lua[168] self.sproto_c2s', self.sproto_c2s, self.response_call_back[session][1])
        for i=1,#code do
            print_net(code:byte(i))
        end
        local encode = self.sproto_c2s:response_decode(self.response_call_back[session][1], content)
        print_net('Cat:NetDispatcher.lua[168] encode', encode)
        self.response_call_back[session][2](encode)
        self.response_call_back[session] = nil
    end
end

function NetDispatcher:OnMessage( bytes )
    --一开始只需要发网络信息转发给LoginController,等正式登录后才会把本函数切为OriginOnMessage分发sproto协议
    LoginController:OnReceiveMsg(bytes)
end

--卸载网络监听--
function NetDispatcher:Unload()
    -- Event.RemoveListener(Protocal.Message)
    -- Event.RemoveListener(Protocal.Exception)
    print('Unload NetDispatcher...')
end
