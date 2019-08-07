local Entity = ECS.BaseClass()
ECS.Entity = Entity
ECS.Entity.Name = "ECS.Entity"
ECS.Entity.Size = nil --Init In CoreHelper
function Entity:Constructor(  )
	self.Index = 0
	self.Version = 0
	setmetatable(self, {__tostring=function(o)
		return "Entity:"..o.Index.." V:"..o.Version
	end})
end

return Entity