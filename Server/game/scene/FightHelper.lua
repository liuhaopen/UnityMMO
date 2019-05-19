local FightHelper = {}

--获取可攻击的坐标	
function FightHelper.GetAssailablePos( curPos, targetPos, minDistance, maxDistance )
	local randomDis = math.random(0, (maxDistance-minDistance))--返回0~1
	-- local distancePercent = math.max(0, randomPercent-0.2)
	local bestDistance = minDistance+randomDis
	local pos_x = targetPos.x + math.random(-bestDistance, bestDistance)
	local pos_y = targetPos.y + math.random(-bestDistance, bestDistance)
	local pos_z = targetPos.z + math.random(-bestDistance, bestDistance)
	local newPos = {x=pos_x, y=pos_y, z=pos_z}
	-- local dir = Vector3.Sub(curPos, targetPos):Normalize()
	--Cat_Todo : 优化-最好有个弧度
	-- local newPos = Vector3.Add(targetPos, dir*bestDistance)
	-- print('Cat:FightHelper.lua[10] curPos.x, curPos.z, targetPos.x, targetPos.y, newPos.x, newPos.z', curPos.x, curPos.z, targetPos.x, targetPos.y, newPos.x, newPos.z)
	return newPos
end

return FightHelper