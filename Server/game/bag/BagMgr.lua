local skynet = require "skynet"
local BagConst = require "game.bag.BagConst"
local this = {
	bagLists = {},
	user_info = nil,
	id_service = nil,
	gameDBServer = nil,
}

local function initBagList( pos )
	this.gameDBServer = this.gameDBServer or skynet.localname(".GameDBServer")
	local condition = string.format("role_id=%s and pos=%s", this.user_info.cur_role_id, pos)
	local hasBagList, goodsList = skynet.call(this.gameDBServer, "lua", "select_by_condition", "Bag", condition)
	print('Cat:this.lua[15] hasBagList', hasBagList, pos)
	print("Cat:this [start:16] goodsList:", goodsList)
	PrintTable(goodsList)
	print("Cat:this [end]")
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

local findEmptyCell = function ( bagInfo )
	local cell = 1
	if bagInfo and bagInfo.goodsList then
		cell = #bagInfo.goodsList + 1
		for i,v in ipairs(bagInfo.goodsList) do
			if v.cell > i then
				return i
			end
		end
	end
	return cell
end

local findGoodsInList = function ( goodsList, goodsTypeID )
	if not goodsList then return end
	for i,v in ipairs(goodsList) do
		if v.goods_type_id == goodsTypeID then
			return v, i
		end
	end
	return nil
end

local notifyBagChange = function (  )
	if this.cacheChangeList and #this.cacheChangeList > 0 and this.coForGoodsChangeList then
		local co = this.coForGoodsChangeList
		this.coForGoodsChangeList = nil
		skynet.wakeup(co)
	end
end

local addNewGoodsToNotifyCache = function ( goodsInfo, notify )
	print("Cat:BagMgr [start:60] goodsInfo: ", goodsInfo)
	PrintTable(goodsInfo)
	print("Cat:BagMgr [end]")
	this.cacheChangeList = this.cacheChangeList or {}
	table.insert(this.cacheChangeList, goodsInfo)
	if notify then
		notifyBagChange()
	end
end

local changeGoodsNum = function( goodsTypeID, num, pos, notify )
	this.gameDBServer = this.gameDBServer or skynet.localname(".GameDBServer")

	local bagInfo = this.bagLists[pos]
	if bagInfo and bagInfo.goodsList then
		local goodsInfo, goodsIndex = findGoodsInList(bagInfo.goodsList)
		local overlapNum = 10
		local newGoods
		if goodsInfo and goodsInfo.num < overlapNum then
			goodsInfo.num = goodsInfo.num + num
			if goodsInfo.num <= 0 then
				table.remove(bagInfo.goodsList, goodsIndex)
				if goodsInfo.num < 0 then
					skynet.error("bag change goods num less than 0")
				end
				goodsInfo.num = 0
				skynet.call(this.gameDBServer, "lua", "delete", "Bag", "goods_uid", goodsInfo.goods_uid)
			else
				skynet.call(this.gameDBServer, "lua", "update", "Bag", "goods_uid", goodsInfo.goods_uid, goodsInfo)
			end
			newGoods = goodsInfo
		else
			local emptyCell = findEmptyCell(bagInfo)
			print('Cat:BagMgr.lua[81] emptyCell', emptyCell)
			this.id_service = this.id_service or skynet.localname(".id_service")
			local goods_uid = skynet.call(this.id_service, "lua", "gen_uid", "goods")
			local addNum = math.min(overlapNum, num)
			newGoods = {
				goods_uid = goods_uid,
				goods_type_id = goodsTypeID,
				num = addNum,
				pos = pos,
				cell = emptyCell,
				role_id = this.user_info.cur_role_id,
			}
			table.insert(bagInfo.goodsList, emptyCell, newGoods)
			skynet.call(this.gameDBServer, "lua", "insert", "Bag", newGoods)
			if num > overlapNum then
				changeGoodsNum(goodsTypeID, num - overlapNum, pos, notify)
			end
		end
		addNewGoodsToNotifyCache(newGoods, notify)
	else
		--Cat_Todo : uninit?
		skynet.error("bag:add goods uninit bag info")
	end
end

local SprotoHandlers = {}
function SprotoHandlers.Bag_GetInfo( reqData )
	print("Cat:this [start:29] reqData:", reqData, this.user_info.cur_role_id)
	PrintTable(reqData)
	print("Cat:this [end]")
	local bagList = this.bagLists[reqData.pos]
	if not bagList then
		bagList = initBagList(reqData.pos)
		this.bagLists[reqData.pos] = bagList
	end
	return bagList
end

function SprotoHandlers.Bag_GetChangeList( reqData )
	print('Cat:BagMgr.lua[131  req get change list')
	if not this.coForGoodsChangeList then
		if not this.cacheChangeList or #this.cacheChangeList <= 0 then
			this.coForGoodsChangeList = coroutine.running()
			print('Cat:BagMgr.lua[134] this.coForGoodsChangeList', this.coForGoodsChangeList)
			skynet.wait(this.coForGoodsChangeList)
		end
		local changeList = this.cacheChangeList
		if changeList then
			this.cacheChangeList = nil
			return {goodsList=changeList}
		else
		end
	else
		--shouldn't be here,the client requested it again before replying
	end
	return {}
end

local PublicFuncs = {}
function PublicFuncs.Init( user_info )
	this.user_info = user_info
	
end
function PublicFuncs.AddBagGoods( goodsTypeID, num )
	print('Cat:BagMgr.lua[137] goodsTypeID, num', goodsTypeID, num)
	changeGoodsNum(goodsTypeID, num, BagConst.Pos.Bag, true)
end

SprotoHandlers.PublicClassName = "Bag"
SprotoHandlers.PublicFuncs = PublicFuncs
return SprotoHandlers