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
	},
	Attr = {
		HP = 1,
		MaxHP = 2,
		Attack = 3,
		Def = 4,
		Dodge = 5,
	},
	--SceneConst.ReliveType.SafeArea
	ReliveType = {
		SafeArea = 1,
		Inplace = 2,
	},
}

return SceneConst