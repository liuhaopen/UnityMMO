return [[

#gmName:在前端显示的按钮名字
#defaultGMStr:默认的gm字符串
.gmInfo {
	gmName 0 : string
	defaultGMStr 1 : string
}
#获取可用的GM列表
GM_GetList 400 {
	response {
		gmList 0 : *gmInfo
	}
}

#获取可用的GM列表
GM_Excute 401 {
	request {
		gmStr 0 : string
	}
	response {
		ret 0 : integer
		gmStr 1 : string
	}
}

]]

