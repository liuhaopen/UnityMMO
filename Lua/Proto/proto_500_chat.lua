return [[

.chatInfo {
	roleID 0 : integer
	roleName 1 : string
	content 2 : string
	career 3 : integer
	headID 4 : integer
	headUrl 5 : string
	bubbleID 6 : integer
	time 7 : integer
	extra  8 : string
}

#获取某频道的历史聊天记录
Chat_GetHistory 500 {
	request {
		channel 0 : integer
	}
	response {
		channel 0 : integer
		list 1 : *chatInfo
	}
}

#发送聊天
Chat_Send 501 {
	request {
		channel 0 : integer
		content 1 : string
		extra 2 : string
	}
	response {
		ret 0 : integer
	}
}

#监听新聊天信息
Chat_GetNew 502 {
	response {
		list 0 : *chatInfo
	}
}
]]

