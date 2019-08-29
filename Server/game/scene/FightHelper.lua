local ECS = require "ECS"
local skill_cfg = require "game.config.scene.config_skill"
local math_random = math.random 
local FightHelper = {}
local EntityMgr = nil
function FightHelper.Init(  )
	EntityMgr = ECS.World.Active.EntityManager
end

local randomAddOrMinus = function (  )
	local isAdd = math_random(1,2)==1
	return isAdd and 1 or -1
end

--获取可攻击的坐标	
function FightHelper.GetAssailablePos( curPos, targetPos, minDistance, maxDistance )
	local xAddOrMinus = randomAddOrMinus()
	local xRandomDis = math_random(0, (maxDistance-minDistance))
	local pos_x = targetPos.x + minDistance*xAddOrMinus + xRandomDis*xAddOrMinus
	local pos_y = targetPos.y
	local zAddOrMinus = randomAddOrMinus()
	local zRandomDis = math_random(0, (maxDistance-minDistance))
	local pos_z = targetPos.z + minDistance*zAddOrMinus + zRandomDis*zAddOrMinus
	local newPos = {x=pos_x, y=pos_y, z=pos_z}
	return newPos
end

function FightHelper.IsSkillInCD( entity, skillID )
	local hasCD = EntityMgr:HasComponent(entity, "UMO.CD")
	if hasCD then
		local cdData = EntityMgr:GetComponentData(entity, "UMO.CD")
		local cdEndTime = cdData[skillID]
		return cdEndTime and Time.timeMS <= cdEndTime
	end
	return false
end

function FightHelper.GetSkillCD( skillID, lv )
	lv = lv or 1
	local cfg = skill_cfg[skillID]
	if cfg and cfg.detail[lv] then
		return cfg.detail[lv].cd
	end
	return 0
end

function FightHelper.ApplySkillCD( entity, skillID, lv )
	local hasCD = EntityMgr:HasComponent(entity, "UMO.CD")
	local endTime = 0
	if hasCD then
		local cdData = EntityMgr:GetComponentData(entity, "UMO.CD")
		local cd = FightHelper.GetSkillCD(skillID, lv)
		endTime = Time.timeMS + cd
		cdData[skillID] = endTime
	end
	return endTime
end

function FightHelper.IsLive( entity )
	if entity and EntityMgr:Exists(entity) and EntityMgr:HasComponent(entity, "UMO.HP") then
		local hpData = EntityMgr:GetComponentData(entity, "UMO.HP")
		return hpData.cur > 0
	end
	return false
end

return FightHelper