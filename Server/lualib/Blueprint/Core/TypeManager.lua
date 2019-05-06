local TypeManager = {
	types = {},
}

function TypeManager:RegisterType( typeName, classTbl )
	if not typeName then return end
	
	if self.types[typeName] then
		error("already register type name : "..typeName, 2)
	end
	self.types[typeName] = classTbl
end

function TypeManager:GetType( typeName )
	return self.types[typeName]
end

function TypeManager:InitDefaultTypes(  )
	--以下是默认的一些node
	self:RegisterType("BP.Node", BP.Node)
	
	self:RegisterType("BP.FSM.FSMGraph", BP.FSM.FSMGraph)
	self:RegisterType("BP.FSM.FSMState", BP.FSM.FSMState)


	self:RegisterType("BP.Flow.UpdateEvent", BP.Flow.UpdateEvent)
	-- self:RegisterType("BP.Flow.GetVariable", BP.Flow.GetVariable)
	-- self:RegisterType("BP.Flow.Delay", BP.Flow.Delay)
	
end

return TypeManager