local Entity = BaseClass()
ECS.Entity = Entity

ECS.Entity.Size = 8*2
function Entity:Constructor(  )
	self.Index = 0
	self.Version = 0
end

return Entity