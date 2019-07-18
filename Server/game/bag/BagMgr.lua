local skynet = require "skynet"
local BagConst = require "game.bag.BagConst"
local this = {
	bagLists = {},
	user_info = nil,
}

local function initBagList( pos )
	this.gameDBServer = this.gameDBServer or skynet.localname(".GameDBServer")
	local condition = string.format("role_id=%s and pos=%s", this.user_info.cur_role_id, pos)
	local hasBagList, goodsList = skynet.call(this.gameDBServer, "lua", "select_by_condition", "Bag", condition)
	print('Cat:this.lua[15] hasBagList', hasBagList)
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

local generateGoodsUID = function (  )
	
end

local findEmptyCell = function ( bagInfo )
	local cell = 0
	if bagInfo and bagInfo.goodsList then
		for i,v in ipairs(bagInfo.goodsList) do
			
		end
	end
	return cell
end

local notifyBagChange = function (  )
	
end

local function addGoods( goodsTypeID, num, pos )
	local bagInfo = this.bagLists[pos]
	if bagInfo and bagInfo.goodsList then
		local emptyCell, index = findEmptyCell(bagInfo)
		local newGoods = {

		}
		table.insert(bagInfo.goodsList, index, newGoods)
		Task.cacheChangedTaskInfos[roleID] = Task.cacheChangedTaskInfos[roleID] or {}
		table.insert(Task.cacheChangedTaskInfos[roleID], taskInfo)
		-- notifyBagChange(roleID, taskInfo)
	else
		--Cat_Todo : uninit?
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
	if not this.co_change_list then
		local changeList = this.cacheChangeList and this.cacheChangeList
		if not changeList then
			this.co_change_list = coroutine.running()
			skynet.wait(this.co_change_list)
		end
		changeList = this.cacheChangeList and this.cacheChangeList
		if changeList then
			table.remove(this.cacheChangeList, 1)
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
	local id_service = skynet.localname(".id_service")
	print('Cat:BagMgr.lua[99] id_service', id_service)
	for i=1,100 do
		local uid = skynet.call(id_service, "lua", "gen_uid", "goods")
		print('Cat:BagMgr.lua[101] uid', uid)
	end

end
function PublicFuncs.AddBagGoods( goodsTypeID, num )
	addGoods(goodsTypeID, num, BagConst.Pos.Bag)
end

SprotoHandlers.PublicClassName = "Bag"
SprotoHandlers.PublicFuncs = PublicFuncs
return SprotoHandlers