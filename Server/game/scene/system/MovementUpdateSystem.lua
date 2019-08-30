local ECS = require "ECS"

local MovementUpdateSystem = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("UMO.MovementUpdateSystem", MovementUpdateSystem)

function MovementUpdateSystem:Constructor( )
end

function MovementUpdateSystem:OnCreate(  )
	ECS.ComponentSystem.OnCreate(self)

	self.group = self:GetComponentGroup({"UMO.Position", "UMO.TargetPos", "UMO.MoveSpeed", "UMO.AOIHandle"})
end

function MovementUpdateSystem:OnUpdate(  )
	local deltaTime = Time.deltaTime
	local positions = self.group:ToComponentDataArray("UMO.Position")
	local targetPositions = self.group:ToComponentDataArray("UMO.TargetPos")
	local speeds = self.group:ToComponentDataArray("UMO.MoveSpeed")
	local aoi_handles = self.group:ToComponentDataArray("UMO.AOIHandle")
	local dt = Time.deltaTime
	for i=1,positions.Length do
		local startPos = positions[i]
		startPos = Vector3(startPos.x, startPos.y, startPos.z)
		local targetPos = targetPositions[i]
		targetPos = Vector3(targetPos.x, targetPos.y, targetPos.z)
        local speed = speeds[i].curSpeed
        if speed > 0 then
	        local moveDir = Vector3.Sub(targetPos, startPos)
	        local groundDir = moveDir
	        groundDir.y = 0
	        local moveDistance = Vector3.Magnitude(groundDir)
	        groundDir = Vector3.Normalize(groundDir)
	        local isMoveWanted = moveDistance>0.01
	        local newPos
	        -- print('Cat:MovementUpdateSystem.lua[33] moveDistance, dt', moveDistance, dt)
            if (moveDistance < speed*dt) then
                --目标已经离得很近了
                newPos = targetPos
            else
                newPos = startPos+groundDir*speed*dt
            end
            positions[i] = {x=newPos.x, y=newPos.y, z=newPos.z}
            self.sceneMgr.aoi:set_pos(aoi_handles[i].value, newPos.x, newPos.y, newPos.z)
	    end
	end
end

return MovementUpdateSystem