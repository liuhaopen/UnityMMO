return [[

.goods_info {
	goods_uid 0 : integer
	goods_type_id 1 : integer
	cell 2 : integer
	num 3 : integer
}

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

]]

