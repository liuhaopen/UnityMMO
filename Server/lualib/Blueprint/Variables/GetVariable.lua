local GetVariable = BP.BaseClass(BP.Node)

function GetVariable:Constructor( graph )
    self.is_updatable_bp_node = true
    self.graph = graph
    print('Cat:GetVariable.lua[5] graph', graph)
end

function GetVariable:Update( deltaTime )
    print('Cat:GetVariable.lua[8] update')
end

return GetVariable