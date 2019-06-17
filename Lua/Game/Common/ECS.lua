local ECS = {}

local EntityManager = CS.Unity.Entities.EntityManager
local EntityManagerExtensions = CS.Unity.Entities.EntityManagerExtensions
function ECS:Init( entityMgr )
	self.func_cache = {}
	setmetatable(self.func_cache, {__mode = "kv"})   
	self:SetEntityManager(entityMgr)
end

function ECS:SetEntityManager( entityMgr )
	self.entityMgr = entityMgr
end

function ECS:GetFunc( funcName, compType, class )
	local name = funcName..tostring(compType)
	local func = self.func_cache[name]
	if func then
		return func
	end
	local generic = xlua.get_generic_method(class, funcName)
	local func = generic(compType)
	self.func_cache[name] = func
	return func
end

--Cat_Todo : no gc
function ECS:GetComponentData( entity, compType )
	local func = self:GetFunc("GetComponentData", compType, EntityManager)
	return func(self.entityMgr, entity, compType)
end

function ECS:SetComponentData( entity, compType, data )
	local func = self:GetFunc("SetComponentData", compType, EntityManager)
	func(self.entityMgr, entity, data, compType)
end

function ECS:HasComponent( entity, compType )
	local func = self:GetFunc("HasComponent", compType, EntityManager)
	return func(self.entityMgr, entity, compType)
end

function ECS:GetComponentObject( entity, compType )
	local func = self:GetFunc("SetComponentData", compType, EntityManagerExtensions)
	return func(self.entityMgr, entity, compType)
end

return ECS