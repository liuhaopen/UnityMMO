local skynet = require "skynet"
local BagConst = require "game.bag.BagConst"
local GoodsCfg = require "Config.ConfigGoods"
local this = {
	bagLists = {},
	user_info = nil,
	id_service = nil,
	gameDBServer = nil,
}

local function initBagList( pos )
	this.gameDBServer = this.gameDBServer or skynet.localname(".GameDBServer")
	local condition = string.format("roleID=%s and pos=%s", this.user_info.cur_role_id, pos)
	local hasBagList, goodsList = skynet.call(this.gameDBServer, "lua", "select_by_condition", "Bag", condition)
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

local findEmptyCellIndex = function ( goodsList )
	local cell = 1
	if goodsList then
		cell = #goodsList + 1
		for i,v in ipairs(goodsList) do
			if v.cell > i then
				return i
			end
		end
	else
		cell = BagConst.MaxCell+9999
	end
	return cell
end

--ignoreFullOverlap：是否忽略数量已满重叠数的道具
local findGoodsInList = function ( goodsList, goodsTypeID, ignoreFullOverlap )
	if not goodsList then return end
	local cfg = ignoreFullOverlap and GoodsCfg[goodsTypeID]
	for i,v in ipairs(goodsList) do
		if v.typeID == goodsTypeID then
			if not ignoreFullOverlap or v.num < cfg.overlap then
				return v, i
			end
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
	this.cacheChangeList = this.cacheChangeList or {}
	local hasFind = false
	for i,v in ipairs(this.cacheChangeList) do
		if v.uid == goodsInfo.uid then
			hasFind = true
			this.cacheChangeList[i] = goodsInfo
			break
		end
	end
	if not hasFind then
		table.insert(this.cacheChangeList, goodsInfo)
	end
	if notify then
		notifyBagChange()
	end
end

local changeGoodsNum = nil
changeGoodsNum = function( goodsTypeID, diffNum, pos, notify )
	-- print('Cat:BagMgr.lua[81] goodsTypeID, diffNum, pos, notify', goodsTypeID, diffNum, pos, notify)
	this.gameDBServer = this.gameDBServer or skynet.localname(".GameDBServer")
	local bagInfo = this.bagLists[pos]
	if bagInfo and bagInfo.goodsList then
		local goodsInfo, goodsIndex = findGoodsInList(bagInfo.goodsList, goodsTypeID, true)
		local cfg = GoodsCfg[goodsTypeID]
		local overlapNum = cfg and cfg.overlap or 1
		-- print('Cat:BagMgr.lua[90] overlapNum', goodsInfo, goodsIndex, overlapNum)
		local newGoods
		if goodsInfo and goodsInfo.num < overlapNum then
			local goodsLastNum = goodsInfo.num
			goodsInfo.num = math.min(overlapNum, goodsInfo.num + diffNum)
			-- print('Cat:BagMgr.lua[90] goodsInfo.num, diffNum', goodsInfo.num, diffNum)
			if goodsInfo.num <= 0 then
				--diffNum可以为负数，道具数为0，该清除掉该道具了
				table.remove(bagInfo.goodsList, goodsIndex)
				if goodsInfo.num < 0 then
					skynet.error("bag change goods num less than 0")
				end
				goodsInfo.num = 0
				skynet.call(this.gameDBServer, "lua", "delete", "Bag", "uid", goodsInfo.uid)
			else
				--更新道具数量
				skynet.call(this.gameDBServer, "lua", "update", "Bag", "uid", goodsInfo.uid, goodsInfo)
				diffNum = diffNum-(goodsInfo.num-goodsLastNum)
				-- print('Cat:BagMgr.lua[113] diffNum', diffNum)
				if diffNum > 0 then
					changeGoodsNum(goodsTypeID, diffNum, pos, false)
				end
			end
			newGoods = goodsInfo
		else
			--已达到该道具的最大重叠数，所以在占另外的背包格子
			local emptyCell = findEmptyCellIndex(bagInfo and bagInfo.goodsList)
			if emptyCell > BagConst.MaxCell then
				--Cat_Todo : handle full cell
				return
			end
			-- print('Cat:BagMgr.lua[81] emptyCell', emptyCell)
			this.id_service = this.id_service or skynet.localname(".id_service")
			local uid = skynet.call(this.id_service, "lua", "gen_uid", "goods")
			local addNum = math.min(overlapNum, diffNum)
			newGoods = {
				uid = uid,
				typeID = goodsTypeID,
				num = addNum,
				pos = pos,
				cell = emptyCell,
				roleID = this.user_info.cur_role_id,
			}
			table.insert(bagInfo.goodsList, emptyCell, newGoods)
			skynet.call(this.gameDBServer, "lua", "insert", "Bag", newGoods)
			diffNum = diffNum-addNum
			if diffNum > 0 then
				changeGoodsNum(goodsTypeID, diffNum, pos, false)
			end
		end
		addNewGoodsToNotifyCache(newGoods, notify)
	else
		--uninit?
		skynet.error("bag:add goods uninit bag info")
	end
end

local getGoodsByUID = function ( uid )
	if not this.bagLists then
		return
	end
	for pos,bagList in pairs(this.bagLists) do
		if bagList.goodsList then
			for i,goodsInfo in ipairs(bagList.goodsList) do
				if goodsInfo.uid == uid then
					return goodsInfo, i
				end
			end
		end
	end
	return nil
end

local clearAllGoods = function()
	if not this.bagLists then return end
	
	this.gameDBServer = this.gameDBServer or skynet.localname(".GameDBServer")
	skynet.call(this.gameDBServer, "lua", "delete", "Bag", "roleID", this.user_info.cur_role_id)
	for pos,bagList in pairs(this.bagLists) do
		bagList.goodsList = {}
		print('Cat:BagMgr.lua[172] pos', pos)
		-- for k,goodsInfo in pairs(bagList.goodsList) do
		-- 	print('Cat:BagMgr.lua[171] goodsInfo.uid', goodsInfo.uid)
		-- end
	end
end

local SprotoHandlers = {}
function SprotoHandlers.Bag_GetInfo( reqData )
	local bagList = this.bagLists[reqData.pos]
	if not bagList then
		bagList = initBagList(reqData.pos)
		this.bagLists[reqData.pos] = bagList
	end
	return bagList
end

function SprotoHandlers.Bag_DropGoods( reqData )
	local goodsInfo, index = getGoodsByUID(reqData.uid)
	-- print('Cat:BagMgr.lua[146] goodsInfo, pos, index', goodsInfo, index, reqData.uid)
	if goodsInfo then
		goodsInfo.num = 0
		addNewGoodsToNotifyCache(goodsInfo, true)
		skynet.call(this.gameDBServer, "lua", "delete", "Bag", "uid", goodsInfo.uid)
		table.remove(this.bagLists[goodsInfo.pos].goodsList, index)
		return {result = ErrorCode.Succeed}
	else
		return {result = ErrorCode.CannotFindGoods}
	end
end

function SprotoHandlers.Bag_GetChangeList( reqData )
	-- print('Cat:BagMgr.lua[131] req get change list')
	if not this.coForGoodsChangeList then
		if not this.cacheChangeList or #this.cacheChangeList <= 0 then
			this.coForGoodsChangeList = coroutine.running()
			-- print('Cat:BagMgr.lua[134] this.coForGoodsChangeList', this.coForGoodsChangeList)
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
function PublicFuncs.ChangeBagGoods( goodsTypeID, diffNum )
	-- print('Cat:BagMgr.lua[137] goodsTypeID, diffNum', goodsTypeID, diffNum)
	changeGoodsNum(goodsTypeID, diffNum, BagConst.Pos.Bag, true)
end
function PublicFuncs.ClearAllGoods()
	print('Cat:BagMgr.lua[222] ClearAllGoods')
	clearAllGoods()
end
SprotoHandlers.PublicClassName = "Bag"
SprotoHandlers.PublicFuncs = PublicFuncs
return SprotoHandlers