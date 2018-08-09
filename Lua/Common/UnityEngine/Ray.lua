--------------------------------------------------------------------------------
--      Copyright (c) 2015 - 2016 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------
-- added by wsh @ 2017-12-28
-- 注意：
-- 1、已经被修改，别从tolua轻易替换来做升级

local rawget = rawget
local setmetatable = setmetatable
local Vector3 = Vector3

local Ray = 
{
	direction = Vector3.zero,
	origin = Vector3.zero,
}

local unity_ray = CS.UnityEngine.Ray

Ray.__index = function(t,k)
	local var = rawget(Ray, k)
	if var ~= nil then
		return var
	end
	
	return rawget(unity_ray, k)
end

Ray.__call = function(t, direction, origin)
	return Ray.New(direction, origin)
end

function Ray.New(direction, origin)
	local ray = {}	
	ray.direction 	= direction:Normalize()
	ray.origin 		= origin
	setmetatable(ray, Ray)	
	return ray
end

function Ray:GetPoint(distance)
	local dir = self.direction * distance
	dir:Add(self.origin)
	return dir
end

function Ray:Get()		
	local o = self.origin
	local d = self.direction
	return o.x, o.y, o.z, d.x, d.y, d.z
end

Ray.__tostring = function(self)
	return string.format("Origin:(%f,%f,%f),Dir:(%f,%f, %f)", self.origin.x, self.origin.y, self.origin.z, self.direction.x, self.direction.y, self.direction.z)
end

Ray.unity_ray = CS.UnityEngine.Ray
CS.UnityEngine.Ray = Ray
setmetatable(Ray, Ray)
return Ray