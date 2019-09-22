local SceneConst = {
	--SceneConst.SceneConst.ObjectType.Role
	ObjectType={
		Role=1,Monster=2,NPC=3,
	},
	--SceneConst.InfoKey.EnterView
	InfoKey = {
		EnterView=1,
	    LeaveView=2,
	    PosChange=3,
	    TargetPos=4,
	    JumpState=5,
	    HPChange=6,
        NPCState=7,
        SceneChange=8,
        Buff=9,
        Speed=10,
		Exp=11,
	},
	--有些类型的事件，就算是自己的也要发给前端
	InterestSelfEvent = {
		[3] = true, [6] = true,
	},
	--SceneConst.Attr.Att
	Attr = {
		Att = 1,--攻击
		HP = 2,--血量
		Def = 3,--防御
		Crit = 4,--暴击
		Hit = 5,--命中
		Dodge = 6,--闪避
	},
	--SceneConst.AttrStrMap["att"]
	AttrStrMap = {
		["att"] = 1,
		["hp"] = 2,
		["def"] = 3,
		["crit"] = 4,
		["hit"] = 5,
		["dodge"] = 6,
	},
	--SceneConst.Buff.Attr
	Buff = {
		Attr = 1,--属性buff，改变某属性值，比如攻击，暴击，防御等
		Fire = 2,--火,定时扣血类型
		Poison = 3,--毒,定时扣血类型
		Forzen = 4,--冰冻,动不了加定时扣血类型
		Dizzy = 5,--晕眩，动不了
		HpToMp = 6,--用MP代替HP伤害
		Silence = 7,--沉默，发不出技能
		Speed = 8,--控制速度
		ClearBadBuff = 9,--消除不良buff
		HurtPower = 10,--伤害加成
		Suck = 11,--吸过来
		HPShield = 12,--血量盾牌
		Chaos = 13,--混乱，到处跑
		Sneer = 14,--嘲讽，只能普攻
		Shapeshift = 15,--变身
		Invisible = 16,--隐身
		FilpControl = 17,--反转控制，比如按前即向后走
		SkillTargetMaxNum = 19,--改变发出技能的最大攻击数量
		Exp = 20,--经验加成
		GoodsDrop = 21,--道具掉率
		Rebound = 22,--反弹
		SuckHP = 23,--吸血
	},
	--SceneConst.SkillTargetType.Enemy
	SkillTargetType = {
		Enemy = 1,--敌方
		Me = 2,--自己
		Our = 3,--我方
	},
	--SceneConst.ReliveType.SafeArea
	ReliveType = {
		SafeArea = 1,
		Inplace = 2,
	},
	--SceneConst.ClearBuffType.All
	ClearBuffType = {
		All = 1,
		Bad = 2,
		Good = 3,
	},
}

return SceneConst