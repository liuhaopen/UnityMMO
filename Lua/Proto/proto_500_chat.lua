return [[

.chatInfo {
	roleID 0 : integer
	name 1 : string
	content 2 : string
	career 3 : integer
	lv 4 : integer
	vip 5 : integer
	headID 6 : integer
	headUrl 7 : string
	bubbleID 8 : integer
	time 9 : integer
	channel 10 : integer
	extra  11 : string
	id 12 : integer
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

