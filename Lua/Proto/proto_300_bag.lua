return [[

#goods_uid:全宇宙唯一的道具id，由服务器标识和自增数组合而成
#goods_type_id:道具的类型id
#pos：1普通背包 2仓库 3玩家身上装备
#cell:道具在哪个格子
#num:数量
.goodsInfo {
	uid 0 : integer
	typeID 1 : integer
	pos 2 : integer
	cell 3 : integer
	num 4 : integer
}

.goodsDetail {
	
}

#获取背包列表，pos：1普通背包 2仓库 3玩家身上装备
Bag_GetInfo 300 {
	request {
		pos 0 : integer
	}
	response {
		pos 0 : integer
		cellNum 1 : integer
		goodsList 2 : *goodsInfo
	}
}

#获取背包变更列表
Bag_GetChangeList 301 {
	response {
		goodsList 0 : *goodsInfo
	}
}

#获取道具的详细信息
Bag_GetGoodsDetail 302 {
	request {
		uid 0 : integer
	}
	response {
		detail 0 : goodsDetail
	}
}

#使用道具
Bag_UseGoods 303 {
	request {
		uid 0 : integer
		num 1 : integer
	}
	response {
		result 0 : integer
	}
}

#出售道具
Bag_SellGoods 304 {
	request {
		uid 0 : integer
		num 1 : integer
	}
	response {
		result 0 : integer
	}
}

#丢掉道具
Bag_DropGoods 305 {
	request {
		uid 0 : integer
	}
	response {
		result 0 : integer
	}
}

#整理背包
Bag_Sort 306 {
	request {
		pos 0 : integer
	}
	response {
		result 0 : integer
	}
}
]]

