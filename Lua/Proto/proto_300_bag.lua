return [[

#goods_uid:全宇宙唯一的道具id，由服务器标识和自增数组合而成
#goods_type_id:道具的类型id
#pos：1普通背包 2仓库 3玩家身上装备
#cell:道具在哪个格子
#num:数量
.goods_info {
	goods_uid 0 : integer
	goods_type_id 1 : integer
	pos 2 : integer
	cell 3 : integer
	count 4 : integer
}

.goods_detail {
	
}

#获取背包列表，pos：1普通背包 2仓库 3玩家身上装备
bag_get_info 300 {
	request {
		pos 0 : integer
	}
	response {
		pos 0 : integer
		cell_num 1 : integer
		goods_list 2 : *goods_info
	}
}

#获取背包变更列表
bag_get_change_list 301 {
	request {
	}
	response {
		goods_list 0 : *goods_info
	}
}

#获取道具的详细信息
bag_get_goods_detail 302 {
	request {
		goods_uid 0 : integer
	}
	response {
		detail 0 : goods_detail
	}
}

#使用道具
bag_use_goods 303 {
	request {
		goods_uid 0 : integer
		count 1 : integer
	}
	response {
		result 0 : integer
	}
}

#出售道具
bag_sell_goods 304 {
	request {
		goods_uid 0 : integer
		count 1 : integer
	}
	response {
		result 0 : integer
	}
}

#丢掉道具
bag_drop_goods 305 {
	request {
		goods_uid 0 : integer
	}
	response {
		result 0 : integer
	}
}
]]

