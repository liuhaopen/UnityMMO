local OperatorMap = {
	[">"] = function(a, b) return a>b end,
	["<"] = function(a, b) return a<b end,
	[">="] = function(a, b) return a>=b end,
	["<="] = function(a, b) return a<=b end,
	["=="] = function(a, b) return a==b end,
}
local CheckAttr = Ac.OO.Class {
	type 	= "CheckAttr",
	__call = function(self, data)
		local sceneMgr = self.skillData.sceneMgr
		local entityMgr = sceneMgr.entityMgr
		--检查属性的目标有两种，在Buff时用target_uid，在Skill时就是检查施法者caster_uid
		local target_uid = data.target_uid or data.caster_uid
		local targetEntity = sceneMgr:GetEntity(target_uid)
		local curAttrList = entityMgr:GetComponentData(targetEntity, "UMO.BaseAttr")
		local curAttrValue = curAttrList[self[1]]
		local operator = self[2]
		local compareValue = self[3]
		local compareFunc = OperatorMap[operator]
		return compareFunc(curAttrValue, compareValue)
	end
}
return CheckAttr