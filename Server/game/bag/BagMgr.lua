local skynet = require "skynet"
local BagConst = require "game.bag.BagConst"
local BagMgr = {
	bagLists = {}
}

function BagMgr:Init( user_info )
	self.user_info = user_info
end

function BagMgr:InitBagList( pos )
	self.gameDBServer = self.gameDBServer or skynet.localname(".GameDBServer")
	local condition = string.format("role_id=%s and pos=%s", self.user_info.cur_role_id, pos)
	local hasBagList, goodsList = skynet.call(self.gameDBServer, "lua", "select_by_condition", "Bag", condition)
	print('Cat:BagMgr.lua[15] hasBagList', hasBagList)
	print("Cat:BagMgr [start:16] goodsList:", goodsList)
	PrintTable(goodsList)
	print("Cat:BagMgr [end]")
	local bagInfo = {cellNum=200, pos=pos}
	if hasBagList then
		local sort_func = function ( a, b )
			return a.cell < b.cell
		end
		table.sort(goodsList, sort_func)
		bagInfo.goodsList = goodsList
	else
		bagInfo.goodsList = {}
	end
	return bagInfo
end

function BagMgr:GetBagInfo( reqData )
	print("Cat:BagMgr [start:29] reqData:", reqData, self.user_info.cur_role_id)
	PrintTable(reqData)
	print("Cat:BagMgr [end]")
	local bagList = self.bagLists[reqData.pos]
	if not bagList then
		bagList = self:InitBagList(reqData.pos)
		self.bagLists[reqData.pos] = bagList
	end
	return bagList
end

local findEmptyCell = function ( bagInfo )
	local cell = 0
	if bagInfo and bagInfo.goodsList then
		for i,v in ipairs(bagInfo.goodsList) do
			
		end
	end
	return cell
end

function BagMgr:AddGoods( goodsTypeID, num, pos )
	local bagInfo = self.bagLists[pos]
	if bagInfo and bagInfo.goodsList then
		local emptyCell, index = findEmptyCell(bagInfo)
		local newGoods = {

		}
		table.insert(bagInfo.goodsList, index, newGoods)
	else
		--Cat_Todo : uninit?
	end
end

function BagMgr:GetChangeList(  )
	if not self.co_change_list then
		local changeList = self.cacheChangeList and self.cacheChangeList
		if not changeList then
			self.co_change_list = coroutine.running()
			skynet.wait(self.co_change_list)
		end
		changeList = self.cacheChangeList and self.cacheChangeList
		if changeList then
			table.remove(self.cacheChangeList, 1)
			return {goodsList=changeList}
		else
		end
	else
		--shouldn't be here,the client requested it again before replying
	end
	return {}
end

local SprotoHandlers = {}
function SprotoHandlers.Bag_GetInfo( reqData )
	return BagMgr:GetBagInfo(reqData)
end

function SprotoHandlers.Bag_GetChangeList( reqData )
	return BagMgr:GetChangeList()
end

local PublicFuncs = {}
function PublicFuncs.Init( user_info )
	BagMgr:Init(user_info)
end
function PublicFuncs.AddBagGoods( goodsTypeID, num )
	BagMgr:AddGoods(goodsTypeID, num, BagConst.Pos.Bag)
end

SprotoHandlers.PublicClassName = "Bag"
SprotoHandlers.PublicFuncs = PublicFuncs
return SprotoHandlers