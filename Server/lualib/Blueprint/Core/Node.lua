local Node = BP.BaseClass()

BP.Status = {
	Failure = 0,
	Success = 1,
	Running = 2,
	Resting = 3,
	Error = 4,
	Optional = 5,
}

function Node:Constructor(  )
	self.id = 0
	self.name = "node"
	self.typeName = "Node"
	self.graph = nil
	self.inSlots = {}
	self.outSlots = {}
	self.status = BP.Status.Resting
end

function Node.Create( targetGraph, nodeType )
	if targetGraph == nil then
        error("Can't Create a Node without providing a Target Graph", 2)
        return nil
    end

	local classTbl = nodeType and BP.TypeManager:GetType(nodeType)
	local newNode = nil
	if classTbl then 
		newNode = classTbl.New()
	else
		error("try to get an unexist type name : "..nodeType, 2)
		return nil
	end

    newNode.graph = targetGraph
    newNode:OnValidate(targetGraph)
    newNode:OnCreate(targetGraph)
    return newNode
end

function Node:OnValidate( graph )
	--override me
end

function Node:OnCreate( graph )
	--override me
end

function Node:SetInSlot( slotName, node )
	self.inSlots[slotName] = node
end

function Node:SetOutSlot( slotName, node )
	self.outSlots[slotName] = node
end

function Node:OnGraphStarted(  )
	--override me
end

function Node:OnGraphUnpaused(  )
	--override me
end

function Node:OnGraphPaused(  )
	--override me
end

function Node:OnGraphStoped(  )
	--override me
end

function Node:Execute( owner )
	self.status = self:OnExecute(owner)
	return self.status
end

function Node:OnExecute( owner )
	--override me
	return self.status
end

function Node:OnReset(  )
	--override me
end

function Node:Reset( recursively )
	-- if recursively == nil then
	-- 	recursively = true
	-- end
	if self.status == BP.Status.Resting or self.isChecked then
		return
	end
	self:OnReset()
	self.status = BP.Status.Resting
	self.isChecked = true
	--handle outConnections 
	self.isChecked = false
end

return Node