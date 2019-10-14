require("Game/Login/LoginConst")
require("Game/Login/LoginModel")
local crypt = require "crypt"

LoginController = {}

function LoginController:Init(  )
	self:InitEvents()

    self.loginView = require("Game/Login/LoginView").New()
    self.loginView:Load()
end

function LoginController:InitEvents(  )
    GlobalEventSystem:Bind(LoginConst.Event.StartLogin, LoginController.StartLogin, self)
    GlobalEventSystem:Bind(NetDispatcher.Event.OnConnect, LoginController.Connect, self)
    GlobalEventSystem:Bind(NetDispatcher.Event.OnDisConnect, LoginController.Disconnect, self)
    GlobalEventSystem:Bind(NetDispatcher.Event.OnReceiveLine, LoginController.OnReceiveLine, self)

    local LoginSucceed = function (  )
        --登录成功后就请求角色列表
        local on_ack = function ( ack_data )
            print("Cat:LoginController [start:27] ack_data:", ack_data)
            PrintTable(ack_data)
            print("Cat:LoginController [end]")
            local role_list = ack_data.role_list
            LoginModel:GetInstance():SetRoleList(role_list)
            
            if self.loginView then
                -- UIMgr:Close(self.loginView)
                self.loginView:Unload()
                self.loginView = nil
            end
            if role_list and #role_list > 0 then
                --已有角色就先进入选择角色界面
                local view = require("Game/Login/LoginSelectRoleView").New()
                UIMgr:Show(view)
            else
                --还没有角色就先进入创建角色界面
                local view = require("Game/Login/LoginCreateRoleView").New()
                UIMgr:Show(view)
            end
        end
        NetDispatcher:SendMessage("account_get_role_list", nil, on_ack)
    end
    self.login_succeed_handler = GlobalEventSystem:Bind(LoginConst.Event.LoginSucceed, LoginSucceed)

    local SelectRoleEnterGame = function ( role_id )
        CS.UnityMMO.LoadingView.Instance:SetActive(true)
        CS.UnityMMO.LoadingView.Instance:ResetData()
        local on_ack = function ( ack_data )
            if ack_data.result == 1 then
                --进入游戏成功,先关掉所有界面
                UIMgr:CloseAllView()
                --请求角色信息和场景信息
                self:ReqMainRole()
            else
                --进入游戏失败
            end
        end
        NetDispatcher:SendMessage("account_select_role_enter_game", {role_id = role_id}, on_ack)
    end
    self.select_role_enter_game_handler = GlobalEventSystem:Bind(LoginConst.Event.SelectRoleEnterGame, SelectRoleEnterGame)
end

function LoginController:ReqMainRole(  )
    local on_ack_main_role = function ( ack_data )
        --加载其它系统的controller
        print("Cat:LoginController [start:76] ack_data:", ack_data)
        PrintTable(ack_data)
        print("Cat:LoginController [end]")
        local role_info = ack_data.role_info
        local pos = Vector3.New(role_info.pos_x/GameConst.RealToLogic, role_info.pos_y/GameConst.RealToLogic, role_info.pos_z/GameConst.RealToLogic)
        SceneMgr.Instance:AddMainRole(role_info.scene_uid, role_info.role_id, role_info.name, role_info.career, pos, role_info.cur_hp, role_info.max_hp)
        
        MainRole:GetInstance():SetBaseInfo(role_info)
        GameVariable.IsNeedSynchSceneInfo = true

        GlobalEventSystem:Fire(GlobalEvents.GameStart)
    end
    NetDispatcher:SendMessage("scene_get_main_role_info", nil, on_ack_main_role)
end

function LoginController:StartLogin(login_info)
    print('Cat:LoginController.lua[StartLogin]')
    PrintTable(login_info)
	--[[登录流程:
	1:由第三方平台(如九游,腾讯游戏平台等)提供的SDK里的界面进行注册或登录,其登录将给我们一个token
	2:请求最新的服务器列表,可通过http取下来.(优化:服务器列表可以分成若干文件提高用户体验)
	3:前端向登录服务器请求连接,然后一连接成功就会收到一个登录服务器的随机值(challenge)
	4:前端发一个经过dhexchange的随机值(handshake_client_key)给登录服务器
    5:登录服务器收到handshake_client_key后生成并发送handshake_server_key给前端,此时双方可算出密钥secret了
    6:前端利用之前收到的challenge和算出的secret生成并发送hmac和token信息(包含帐号密码和游戏服务器标识)给登录服务器
    7:登录服务器验证用户信息后告诉前端是否验证成功(200 subid或者400 Bad Request等)
	8:开始请求连接游戏服务器M,连接上后前端利用secret和subid生成并发送握手校验给M
    9:收到游戏服务器M的校验结果(成功为200)
    10:可以正常向游戏服务器收发协议了
	--]]
    self.login_info = login_info
    --向登录服务器请求连接,一连接上就等待收到其发过来的随机值了(challenge)
    self.login_state = LoginConst.Status.WaitForLoginServerChanllenge

	NetMgr:SendConnect(self.login_info.account_ip, self.login_info.account_port, CS.XLuaFramework.NetPackageType.BaseLine)
end

function LoginController:OnReceiveLine(bytes) 
    -- print('Cat:LoginController.lua[114] bytes', bytes)
    local code = tostring(bytes)
    -- print('Cat:LoginController.lua[145] code|'..code.."|login state:"..self.login_state)
    if self.login_state == LoginConst.Status.WaitForLoginServerChanllenge then
        self.challenge = crypt.base64decode(code)
        self.clientkey = crypt.randomkey()
        local handshake_client_key = crypt.base64encode(crypt.dhexchange(self.clientkey))
        local buffer = handshake_client_key.."\n"
        NetMgr:SendBytes(buffer)
        self.login_state = LoginConst.Status.WaitForLoginServerHandshakeKey
    elseif self.login_state == LoginConst.Status.WaitForLoginServerHandshakeKey then
        self.secret = crypt.dhsecret(crypt.base64decode(code), self.clientkey)
        local hmac = crypt.hmac64(self.challenge, self.secret)
        local hmac_base = crypt.base64encode(hmac)
        NetMgr:SendBytes(hmac_base.."\n")

        local token = {
            server = "DevelopServer",
            user = self.login_info.account,
            pass = self.login_info.password or "password",
        }
        self.token = token
        local function encode_token(token)
            return string.format("%s@%s:%s",
                crypt.base64encode(token.user),
                crypt.base64encode(token.server),
                crypt.base64encode(token.pass))
        end
        local etoken = crypt.desencode(self.secret, encode_token(token))
        local etoken_base = crypt.base64encode(etoken)
        NetMgr:SendBytes(etoken_base.."\n")

        self.login_state = LoginConst.Status.WaitForLoginServerAuthorResult
    elseif self.login_state == LoginConst.Status.WaitForLoginServerAuthorResult then
        local result = tonumber(string.sub(code, 1, 3))
        -- print('Cat:LoginController.lua[194] result', result)
        if result == 200 then
            print('Cat:LoginController.lua login succeed!')
            self.subid = crypt.base64decode(string.sub(code, 5))
            -- print('Cat:LoginController.lua[login ok] subid', self.subid)
            self:StartConnectGameServer()
        else
            self.error_map = self.error_map or {
                [400] = "握手失败",
                [401] = "自定义的 auth_handler 不认可 token",
                [403] = "自定义的 login_handler 执行失败",
                [406] = "该用户已经在登陆中",
            }
            local error_str = self.error_map[result] or "未知错误"
            -- print('Cat:LoginController.lua[147] self.error_map[result]', error_str)
            Message:Show(error_str)
        end
    end
end

function LoginController:Connect()
	-- print('Cat:LoginController.lua[Connect] self.login_state : ', self.login_state)
	if self.login_state == LoginConst.Status.WaitForGameServerConnect then
		--刚连接上游戏服务器时需要进行一次握手校验
		local handshake = string.format("%s@%s#%s:%d", crypt.base64encode(self.token.user), crypt.base64encode(self.token.server),crypt.base64encode(self.subid) , 1)
		local hmac = crypt.hmac64(crypt.hashkey(handshake), self.secret)
		local handshake_str = handshake .. ":" .. crypt.base64encode(hmac)
		-- print('Cat:LoginController.lua[132] handshake_str', handshake_str)
        NetMgr:SendBytes(handshake_str)
        --接下来的处理就在OnReceiveMsg函数里
        self.login_state = LoginConst.Status.WaitForGameServerHandshake
	end
    if self.reconnectView then
        UIMgr:Close(self.reconnectView)
        self.reconnectView = nil
    end
end

function LoginController:StartConnectGameServer(  )
    --正式向游戏服务器请求连接,注意此时的协议已经不是基于行解析的了,换成了根据协议头两字节作为内容大小去解析的,所以接收数据的事件换成了NetDispatcher.Event.OnReceiveMsg(具体处理函数是本类)
    NetMgr:SendConnect(self.login_info.game_ip, self.login_info.game_port, CS.XLuaFramework.NetPackageType.BaseHead)
    self.login_state = LoginConst.Status.WaitForGameServerConnect
end

function LoginController:OnReceiveMsg( bytes )
    local code = tostring(bytes)
    local result = string.sub(code, 1, 3)
    if tonumber(result) == 200 then
        --接收完一次就把网络控制权交给NetDispatcher了,开始使用sproto协议 
        NetDispatcher:Start()

        GlobalEventSystem:Fire(LoginConst.Event.LoginSucceed)
        CS.XLuaManager.Instance:OnLoginOk()

        Time:StartSynchServerTime()
    else
        Message:Show("与游戏服务器握手失败:"..result)
    end
end

function LoginController:Disconnect()
	print('Cat:LoginController.lua[Disconnect]', self.login_state)
    if not self.login_info.had_disconnect_with_account_server and (self.login_state == LoginConst.Status.WaitForGameServerConnect or self.login_state == LoginConst.Status.WaitForGameServerHandshake) then
        --每次登录流程中，进入游戏服务器时都会从帐号服务器断开，所以首次断开时可忽略，不需要弹断网的窗口
        self.login_info.had_disconnect_with_account_server = true
        return
    end
    if self.login_state == LoginConst.Status.WaitForLoginServerChanllenge then
        Message:Show("连接登录服务器失败")
    end
    if self.reconnectView then return end
    local showData = {
        content = "网络已断开连接",
        ok_btn_text = "重连",
        on_ok = function()
            -- Message:Show("重连")
            --Cat_Todo : 判断帐号服务器是否也断了，是的话也是要先连帐号服务器的
            -- self:StartConnectGameServer()
            self:StartLogin(self.login_info)
            -- UIMgr:Close(self.reconnectView)
        end,
        cancel_btn_text = "重新登录",
        on_cancel = function()
            --Cat_Todo : 清理场景啊
            --显示登录界面
            if self.loginView then
                UIMgr:Close(self.reconnectView)
                self.reconnectView = nil
            else
                self.loginView = require("Game/Login/LoginView").New()
                self.loginView:Load()
                -- UIMgr:Show(self.loginView)
            end
        end,
    }
    self.reconnectView = UI.AlertView.Show(showData)
end

return LoginController