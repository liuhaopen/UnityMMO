MainUIConst = MainUIConst or {
	--MainUIConst.Event.InitMainUIViews
	Event = {
		-- InitMainUIViews = "MainUIConst.Event.InitMainUIViews",
	},
	--MainUIConst.View.Task
	View = {
		Task = 1,
		MainMenu = 2,
		ActIcon = 3,
		SkillBtn = 4,
		RoleHead = 5,
		Joystick = 6,
		SmallChat = 7,
	},
	--MainUIConst.TaskTeamTabID.Task
	TaskTeamTabID = {
		Task = 1, Team = 2,
	},
	--MainUIConst.MaxSkillBtnNum
	MaxSkillBtnNum = 4,
	MainIcons = {
    	{name = "角色", id = 1, open_lv = 0, icon_res = "icon_1", func_id=100001},
    	{name = "技能", id = 2, open_lv = 12, icon_res = "icon_2", func_id=100002},
    	{name = "公会", id = 3, open_lv = 23, icon_res = "icon_3", func_id=100003},
    	{name = "装备", id = 4, open_lv = 44, icon_res = "icon_4", func_id=100004},
	},
	ActIcons = {
    	{name = "副本", id = 111, open_lv = 20, icon_res = "icon_111", func_id=100111},
	},
}
return MainUIConst
