--[[
skill_id：技能id
shape：攻击范围形状--1圆形，2直线，3扇形
duration：施放时间（毫秒）
detail：每级的具体属性
detail.condition：学习条件--{lv,1}角色等级，{money,100}货币
detail.cd：冷却时间（毫秒）
detail.attack_max_num：攻击最大数量
detail.damage_rate：伤害比率
detail.area：攻击范围--shape为圆形时即半径，直线时即为距离
]]--
local config = {
	--男角色技能
	[110000] = {
		skill_id = 110000, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[110001] = {
		skill_id = 110001, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[110002] = {
		skill_id = 110002, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[110003] = {
		skill_id = 110003, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[110010] = {
		skill_id = 110010, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 5000, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[110011] = {
		skill_id = 110011, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 6000, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[110012] = {
		skill_id = 120012, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 7000, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[110013] = {
		skill_id = 110013, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 15000, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	--女角色技能
	[120000] = {
		skill_id = 120000, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[120001] = {
		skill_id = 120001, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[120002] = {
		skill_id = 120002, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[120003] = {
		skill_id = 120003, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[120010] = {
		skill_id = 120010, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 5000, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[120011] = {
		skill_id = 120011, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 6000, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[120012] = {
		skill_id = 120012, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 7000, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[120013] = {
		skill_id = 120013, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 15000, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	--怪物技能
	[200000] = {
		skill_id = 200000, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200001] = {
		skill_id = 200001, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200100] = {
		skill_id = 200100, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200101] = {
		skill_id = 200101, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200200] = {
		skill_id = 200200, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200201] = {
		skill_id = 200201, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200300] = {
		skill_id = 200300, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200301] = {
		skill_id = 200301, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200400] = {
		skill_id = 200400, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200401] = {
		skill_id = 200401, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200500] = {
		skill_id = 200500, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200501] = {
		skill_id = 200501, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
}

return config