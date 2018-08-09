--------------------------------------------------------------------------------
--      Copyright (c) 2015 - 2016 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------
-- added by wsh @ 2017-12-28
-- 注意：
-- 1、已经被修改，别从tolua轻易替换来做升级

local setmetatable = setmetatable
local Mathf = Mathf
local Vector3 = Vector3

local Plane = {}
local unity_plane = CS.UnityEngine.Plane

Plane.__index = function(t,k)
	return rawget(Plane, k)	
end

Plane.__call = function(t,v)
	return Plane.New(v)
end

function Plane.New(normal, d)
	return setmetatable({normal = normal:Normalize(), distance = d}, Plane)	
end

function Plane:Get()
	return self.normal, self.distance
end

function Plane:Raycast(ray)
	local a = Vector3.Dot(ray.direction, self.normal)
    local num2 = -Vector3.Dot(ray.origin, self.normal) - self.distance
	
    if Mathf.Approximately(a, 0) then                   
		return false, 0        
	end
	
    local enter = num2 / a    
	return enter > 0, enter
end

function Plane:SetNormalAndPosition(inNormal, inPoint)    
    self.normal = inNormal:Normalize()
    self.distance = -Vector3.Dot(inNormal, inPoint)
end    

function Plane:Set3Points(a, b, c)    
    self.normal = Vector3.Normalize(Vector3.Cross(b - a, c - a))
    self.distance = -Vector3.Dot(self.normal, a)
end		    

function Plane:GetDistanceToPoint(inPt)    
	return Vector3.Dot(self.normal, inPt) + self.distance
end    

function Plane:GetSide(inPt)    
	return (Vector3.Dot(self.normal, inPt) + self.distance) > 0
end    

function Plane:SameSide(inPt0, inPt1)    
	local distanceToPoint = self:GetDistanceToPoint(inPt0)
	local num2 = self:GetDistanceToPoint(inPt1)
	return (distanceToPoint > 0 and num2 > 0) or (distanceToPoint <= 0 and num2 <= 0)
end    

Plane.unity_plane = CS.UnityEngine.Plane
CS.UnityEngine.Plane = Plane
setmetatable(Plane, Plane)
return Plane