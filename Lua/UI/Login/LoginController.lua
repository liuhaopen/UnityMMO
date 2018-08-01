require("UI/Login/LoginConst")
require("UI/Login/LoginModel")

local LoginController = {}

local this = LoginController

function LoginController.Init(  )
	print('Cat:LoginController.lua[Init]')

	this.InitEvents()

    local loginView = require("UI/Login/LoginView")
    UIMgr:Show(loginView)
end

function LoginController.InitEvents(  )

	local StartLogin = function ( login_info )
        this.StartLogin(login_info)
	end
    Event.AddListener(LoginConst.Event.StartLogin, StartLogin); 
    Event.AddListener(Protocal.Connect, LoginController.Connect); 
    Event.AddListener(Protocal.Disconnect, LoginController.Disconnect); 
    Event.AddListener(Protocal.MessageLine, LoginController.MessageLine)

    local LoginSucceed = function (  )
        print('Cat:LoginController.lua[LoginSucceed]')
        --登录成功后就请求角色列表
        local on_ack = function ( ack_data )
            print("Cat:LoginController [start:27] ack_data:", ack_data)
            PrintTable(ack_data)
            print("Cat:LoginController [end]")
            local role_list = ack_data.role_list
            LoginModel:GetInstance():SetRoleList(role_list)
            if role_list and #role_list > 0 then
                --已有角色就先进入选择角色界面
                local view = require("UI/Login/LoginSelectRoleView").New()
                UIMgr:Show(view)
            else
                --还没有角色就先进入创建角色界面
                local view = require("UI/Login/LoginCreateRoleView").New()
                UIMgr:Show(view)
            end
        end
        Network.SendMessage("account_get_role_list", nil, on_ack)
    end
    Event.AddListener(LoginConst.Event.LoginSucceed, LoginSucceed); 

    local SelectRoleEnterGame = function ( role_id )
        local on_ack = function ( ack_data )
            print("Cat:LoginController [start:54] ack_data:", ack_data)
            PrintTable(ack_data)
            print("Cat:LoginController [end]")
            if ack_data.result == 1 then
                --进入游戏成功,先关掉所有界面
                UIMgr:CloseAllView()
                --显示加载界面

                --请求角色信息和场景信息
                this.ReqMainRole()
            else
                --进入游戏失败
            end
        end
        Network.SendMessage("account_select_role_enter_game", {role_id = role_id}, on_ack)
    end
    Event.AddListener(LoginConst.Event.SelectRoleEnterGame, SelectRoleEnterGame); 
end

function LoginController.ReqMainRole(  )
    local on_ack_main_role = function ( ack_role_data )
        --请求场景信息
        local on_ack_scene_info = function ( ack_scene_data )
            --加载场景

            --关闭加载界面
            
        end
        Network.SendMessage("scene_get_cur_scene_info", nil, on_ack_scene_info)
        --加载其它系统的controller
        
    end
    Network.SendMessage("scene_get_main_role_info", nil, on_ack)
end

function LoginController.StartLogin(login_info)
	print('Cat:LoginController.lua[StartLogin]')
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
    this.login_info = login_info

    --向登录服务器请求连接,一连接上就等待收到其发过来的随机值了(challenge)
    this.login_state = LoginConst.Status.WaitForLoginServerChanllenge
	NetMgr:SendConnect("192.168.5.142", 8001, NetPackageType.BaseLine)
end

function LoginController.MessageLine(buffer) 
    local code = buffer:ToLuaString()
    print('Cat:LoginController.lua[145] code|'..code.."|", this.login_state)
    if this.login_state == LoginConst.Status.WaitForLoginServerChanllenge then
        this.challenge = crypt.base64decode(code)
        this.clientkey = crypt.randomkey()
        local handshake_client_key = crypt.base64encode(crypt.dhexchange(this.clientkey))
        local buffer = ByteBuffer.New()
        buffer:WriteBuffer(handshake_client_key.."\n")
        NetMgr:SendMessage(buffer)
        this.login_state = LoginConst.Status.WaitForLoginServerHandshakeKey
    elseif this.login_state == LoginConst.Status.WaitForLoginServerHandshakeKey then
        this.secret = crypt.dhsecret(crypt.base64decode(code), this.clientkey)
        local hmac = crypt.hmac64(this.challenge, this.secret)
        local hmac_base = crypt.base64encode(hmac)
        
        local buffer = ByteBuffer.New()
        buffer:WriteBuffer(hmac_base.."\n")
        NetMgr:SendMessage(buffer)

        local token = {
            server = "DevelopServer",
            user = this.login_info.account,
            pass = this.login_info.password or "password",
        }
        this.token = token
        local function encode_token(token)
            return string.format("%s@%s:%s",
                crypt.base64encode(token.user),
                crypt.base64encode(token.server),
                crypt.base64encode(token.pass))
        end
        local etoken = crypt.desencode(this.secret, encode_token(token))
        local etoken_base = crypt.base64encode(etoken)
        local buffer = ByteBuffer.New()
        buffer:WriteBuffer(etoken_base.."\n")
        NetMgr:SendMessage(buffer)

        this.login_state = LoginConst.Status.WaitForLoginServerAuthorResult
    elseif this.login_state == LoginConst.Status.WaitForLoginServerAuthorResult then
        local result = tonumber(string.sub(code, 1, 3))
        print('Cat:LoginController.lua[194] result', result)
        if result == 200 then
            print('Cat:LoginController.lua login succeed!')
            this.subid = crypt.base64decode(string.sub(code, 5))
            print('Cat:LoginController.lua[login ok] subid', this.subid)

            --正式向游戏服务器请求连接
            NetMgr:SendConnect("192.168.5.142", 8888, NetPackageType.BaseHead)
            this.login_state = LoginConst.Status.WaitForGameServerConnect
        else
            this.error_map = this.error_map or {
                [400] = "握手失败",
                [401] = "自定义的 auth_handler 不认可 token",
                [403] = "自定义的 login_handler 执行失败",
                [406] = "该用户已经在登陆中",
            }
            print('Cat:LoginController.lua[147] this.error_map[result]', this.error_map[result] or "未知错误")
        end
    end
end

function LoginController.Connect()
	print('Cat:LoginController.lua[Connect] this.login_state : ', this.login_state)
	if this.login_state == LoginConst.Status.WaitForGameServerConnect then
		--刚连接上游戏服务器时需要进行一次握手校验
		local handshake = string.format("%s@%s#%s:%d", crypt.base64encode(this.token.user), crypt.base64encode(this.token.server),crypt.base64encode(this.subid) , 1)
		local hmac = crypt.hmac64(crypt.hashkey(handshake), this.secret)
		local handshake_str = handshake .. ":" .. crypt.base64encode(hmac)
		print('Cat:LoginController.lua[132] handshake_str', handshake_str)
		Network.SwitchToWaitForGameServerHandshake()
		local buffer = ByteBuffer.New()
        buffer:WriteBuffer(handshake_str)
        NetMgr:SendMessage(buffer)
        --接下来的处理放在Network了,详情看Network.SwitchToWaitForGameServerHandshake函数
        this.login_state = LoginConst.Status.WaitForGameServerHandshake
	end
end

function LoginController.Disconnect()
	print('Cat:LoginController.lua[Disconnect]')
    --Cat_Todo : 重新向游戏服务器请求连接
	-- if this.login_state == 4 then
 --    	NetMgr:SendConnect("192.168.5.142", 8888, NetPackageType.BaseHead)
 --    end
end


return LoginController