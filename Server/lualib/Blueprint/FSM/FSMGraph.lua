local FSMGraph = BP.BaseClass(BP.Graph)

function FSMGraph:Constructor( )
	
end

function FSMGraph.Create( luaData )
	local graph = FSMGraph.New()
	local isOk = graph:LoadFromLuaData(luaData)
	if isOk then
		return graph
	end
	return nil
end

return FSMGraph