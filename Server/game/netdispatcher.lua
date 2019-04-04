--每个系统负责一个协议段(以百为单位),比如帐号系统:1~99,聊天:200~299
local netdispatcher = {
	--监听收到前端协议请求的处理函数,min_proto_tag为proto起始号
	handler_list = {
		{min_proto_tag=1, max_proto_tag=99, handler=require("game.service.account")},
		{min_proto_tag=100, max_proto_tag=199, handler=require("game.service.scene")},
	},
}
netdispatcher.handler_len = #netdispatcher.handler_list

function netdispatcher:dispatch( tag )
	local index = self:tag2index(tag, 1, self.handler_len)
	local handler = self.handler_list[index] and self.handler_list[index].handler
	return handler
end

--二分查找该协议号在self.handler_list的索引
function netdispatcher:tag2index( tag, start_index, end_index )
	if start_index < 1 or end_index > self.handler_len or end_index < start_index then
		return -1
	end
	local half = math.floor(start_index+(end_index-start_index)/2)
	local half_handler = self.handler_list[half]
	if tag < half_handler.min_proto_tag then
		return self:tag2index(tag, start_index, half-1)
	elseif tag > half_handler.max_proto_tag then
		return self:tag2index(tag, half+1, end_index)
	else
		return half
	end
end

return netdispatcher