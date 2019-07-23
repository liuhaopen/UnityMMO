--[[
命名约定：
goodsUID：全宇宙唯一的道具id，由平台，服务器id和自增数组成，方便合服时直接合并数据库
goodsTypeID:道具的类型id，如10000代表钻石，11111代表某具体的刀
goodsKind:道具的种类：比如装备，外观碎片等
pos:背包有多个，见BagConst.Pos结构，有普通背包，仓库，装备等
cell:道具在背包里的哪个格子
--]]
local BagConst = {
	--BagConst.Event.BagChange
	Event = {
		BagChange = "BagConst.Event.BagChange",
	},
	--BagConst.TabID.Bag
	TabID = {
		Bag = 1,
		Warehouse = 2,
	},
	--BagConst.Pos.Bag
	Pos = {
		Bag = 1,
		Warehouse = 2,
		Equip = 3,
	},
	--BagConst.GoodsType.Money
	GoodsType = {
		Money = 1,--货币如钻石、铜币等
		Exp = 2,--经验类如经验丹等
		Weapon = 10,--武器
		Helmet = 11,--头盔
		Necklace = 12,--项链
		Clothing = 13,--衣服
		Scroll = 14,--卷轴
		Pants = 15,--裤子
		Bracelet = 16,--手镯
		Handguard = 17,--护手
		Ring = 18,--戒指
		Shoe = 19,--鞋子
		Guard = 20,--守护
		NameCard = 30,--更名卡

	},
	--BagConst.MaxCell
	MaxCell = 50,
}
return BagConst