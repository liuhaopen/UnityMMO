--错误码：前3位是模块号，后三位自增数
local ErrorCode = {
	Succeed = 0,
	Unknow = 1,
	FullCreateRoleNum = 2,
	WrongRoleIDForEnterGame = 3,
	SkillCastFail = 200000,
	SkillCfgNotFind = 200001,
	SkillInCD = 200002,
	UIDErrorOnCastSkill = 200003,
	CannotFindGoods = 300000,
	GMArgeWrong = 400000,
	GMUnknow = 400001,
}

return ErrorCode