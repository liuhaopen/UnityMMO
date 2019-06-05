LoginConst = LoginConst or {
	--LoginConst.Status.WaitForLoginServerChanllenge
	Status = {
		WaitForLoginServerChanllenge = 1,--刚连接上登录服务器后就等收challenge了
		WaitForLoginServerHandshakeKey = 2,--等待接收登录服务器的handshake key
		WaitForLoginServerAuthorResult = 3,--等待登录服务器的验证结果
		WaitForGameServerConnect = 4,--等待连接上游戏服务器
		WaitForGameServerHandshake = 5,--等待游戏服务器的握手验证
	},
	-- LoginConst.Event.SelectRoleEnterGame
	Event = {
		StartLogin = "LoginConst.Event.StartLogin",
		LoginSucceed = "LoginConst.Event.LoginSucceed",
		CreateRole = "LoginConst.Event.CreateRole",
		DeleteRole = "LoginConst.Event.DeleteRole",
		SelectRoleEnterGame = "LoginConst.Event.SelectRoleEnterGame",
	},
}

return LoginConst