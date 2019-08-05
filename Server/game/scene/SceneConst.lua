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
	},
	Attr = {
		HP = 1,
		MaxHP = 2,
		Attack = 3,
		Def = 4,
		Dodge = 5,
	},
	--SceneConst.BuffType.Attr
	BuffID = {
		Attr = 1,--属性buff，改变某属性值，比如血量，暴击，防御等
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
		Invisible = 16,--隐身
		FilpControl = 17,--反转控制，比如按前即向后走
		AddDodgeAfterHPLow = 18,--血量低于某值时增加闪避
		SkillTargetMaxNum = 19,--改变发出技能的最大攻击数量
		Exp = 20,--经验加成
		GoodsDrop = 21,--道具掉率
		Rebound = 22,--反弹
		
	},
	--SceneConst.SkillTargetType.Enemy
	SkillTargetType = {
		Enemy = 1,--敌方
		Me = 2,--自己
		Our = 3,--我方
	},
}

return SceneConst