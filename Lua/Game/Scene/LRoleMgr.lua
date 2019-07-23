local LRoleMgr = {}
local RoleMgr = CS.UnityMMO.RoleMgr

function LRoleMgr:GetMainRoleLooksInfo()
	--Cat_Todo : 主角信息变更时此值也要更新
	if not self.mainRoleLooksInfo then
		self.mainRoleLooksInfo = RoleMgr:GetInstance():GetMainRoleLooksInfo()
	end
	return self.mainRoleLooksInfo
end

return LRoleMgr