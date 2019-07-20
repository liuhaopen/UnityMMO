local skynet = require "skynet"
local BagConst = require "game.gm.GMConst"

local this = {
	user_info = nil,
	gmList = {
		{gmName="增加道具", defaultGMStr="goods,100000,1"},
		{gmName="清空背包", defaultGMStr="clearAllGoods"},
	},
}

local gmHandler = {}
function gmHandler.goods( gmParts )
	if #gmParts ~= 3 then
		return 1
	end
	local func = this.dispatcher:GetPublicFunc("Bag", "AddBagGoods")
	print('Cat:GM.lua[14] func', func)
	if func then
		func(tonumber(gmParts[2]), tonumber(gmParts[3]))
		return 0
	end
	return 1
end

function gmHandler.clearAllGoods( gmParts )

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
	print("Cat:GM [start:18] gmParts: ", gmParts)
	PrintTable(gmParts)
	print("Cat:GM [end]")
	if gmParts then
		local handleFunc = gmHandler[(gmParts[1] or "")]
		if handleFunc then
			local code = handleFunc(gmParts)
			return {errorCode = code}
		end
	end
	return {errorCode = 1}
end

local PublicFuncs = {}
function PublicFuncs.Init( user_info, dispatcher )
	this.user_info = user_info
	this.dispatcher = dispatcher
end

SprotoHandlers.PublicClassName = "GM"
SprotoHandlers.PublicFuncs = PublicFuncs
return SprotoHandlers