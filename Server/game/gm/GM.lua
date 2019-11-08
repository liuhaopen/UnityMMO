local skynet = require "skynet"
local BagConst = require "game.gm.GMConst"

local this = {
	user_info = nil,
	gmList = {
		{gmName="增加道具", defaultGMStr="Goods,100000,1"},
		{gmName="清空背包", defaultGMStr="ClearAllGoods"},
		{gmName="切换场景", defaultGMStr="Scene,1001"},
		{gmName="改变属性", defaultGMStr="Attr,1,100"},
		{gmName="设置金钱", defaultGMStr="Money,100000"},
		{gmName="升级", defaultGMStr="LvUp,10"},
		{gmName="改变速度", defaultGMStr="Speed,10"},
		{gmName="设置等级", defaultGMStr="Lv,10"},
		{gmName="增加经验", defaultGMStr="Exp,10000"},
		{gmName="发送协议", defaultGMStr="Proto,Bag_Change,123456"},
		{gmName="完成任务", defaultGMStr="Task,1000000"},
		{gmName="重置任务", defaultGMStr="TaskReset"},
	},
}

local gmHandler = {}
function gmHandler.Goods( gmParts )
	if #gmParts ~= 3 then
		return ErrorCode.GMArgeWrong
	end
	local func = this.dispatcher:GetPublicFunc("Bag", "ChangeBagGoods")
	print('Cat:GM.lua[14] func', func)
	if func then
		func(tonumber(gmParts[2]), tonumber(gmParts[3]))
		return ErrorCode.Succeed
	end
	return ErrorCode.GMUnknow
end

function gmHandler.ClearAllGoods( gmParts )
	local func = this.dispatcher:GetPublicFunc("Bag", "ClearAllGoods")
	print('Cat:GM.lua[clearAllGoods] func', func)
	if func then
		func()
		return ErrorCode.Succeed
	end
	return ErrorCode.GMUnknow
end

function gmHandler.Attr( gmParts )
	local world = skynet.uniqueservice("world")
	local scene_service = skynet.call(world, "lua", "get_role_scene_service", this.user_info.cur_role_id)
	skynet.error("gmHandler.Attr scene_service : "+scene_service)
	if scene_service then
		local attrList = {}
		for i = 2, #gmParts, 2 do
			table.insert(attrList, {gmParts[i], gmParts[i+1]})
		end
		skynet.send(scene_service, "lua", "change_attr", this.user_info.cur_role_id, attrList)
	end
end

local SprotoHandlers = {}
function SprotoHandlers.GM_GetList( reqData )
	return {gmList=this.gmList}
end

function SprotoHandlers.GM_Excute( reqData )
	print("Cat:GM [start:31] reqData: ,", reqData)
	PrintTable(reqData)
	print("Cat:GM [end]")
	local gmParts = Split(reqData.gmStr, ",")
	local code = ErrorCode.GMUnknow
	if gmParts then
		local handleFunc = gmHandler[(gmParts[1] or "")]
		if handleFunc then
			code = handleFunc(gmParts)
		end
	end
	return {ret=code, gmStr=reqData.gmStr}
end

local PublicFuncs = {}
function PublicFuncs.Init( user_info, dispatcher )
	this.user_info = user_info
	this.dispatcher = dispatcher
end

SprotoHandlers.PublicClassName = "GM"
SprotoHandlers.PublicFuncs = PublicFuncs
return SprotoHandlers