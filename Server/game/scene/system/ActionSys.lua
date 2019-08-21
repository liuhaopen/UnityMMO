--handle action lifetime
local ECS = require "ECS"
local skynet = require "skynet"
local ActionSys = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("UMO.ActionSys", ActionSys)

function ActionSys:OnCreate(  )
	ECS.ComponentSystem.OnCreate(self)
	self.ecsSystemMgr = self.sceneMgr.ecsSystemMgr
	self.group = self:GetComponentGroup({"UMO.Action"})
end

function ActionSys:OnUpdate(  )
	local actions = self.group:ToComponentDataArray("UMO.Action")
	local entities = self.group:ToEntityArray()
	for i=1,actions.Length do
		self:HandleAction(actions[i], entities[i])
	end
end

function ActionSys:HandleAction( skillData, entity )
	skillData.action:Update(Time.deltaTimeMS)
	local isDone = skillData.action:IsDone()
	if isDone then
		print('Cat:ActionSys.lua[29] isDone', isDone, entity)
		self.ecsSystemMgr:AddDestroyEntity(entity)
	end
end

return ActionSys
