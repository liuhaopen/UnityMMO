local ComponentDataArray = {}
ECS.ComponentDataArray = ComponentDataArray

function ComponentDataArray.Create( iterator, length, componentName )
	assert(iterator~=nil, "iterator should not be nil!")
	assert(length~=nil, "length should not be nil!")
	assert(componentName~=nil, "componentName should not be nil!")
	local array = {
		m_Iterator=iterator, 
		Length=length,
		m_ComponentTypeName=componentName,
		m_Data = {},
		m_Cache = {
			CachedPtr=nil, CachedBeginIndex=0, CachedEndIndex=0, CachedSizeOf=0, IsWriting=false
		},
	}
	ComponentDataArray.InitMetaTable(array)
	return array
end

local get_fun = function ( t, index )
	if index < 1 or index > t.Length then
		return nil
	end
	if index < t.m_Cache.CachedBeginIndex or index >= t.m_Cache.CachedEndIndex then
        t.m_Iterator:MoveToEntityIndexAndUpdateCache(index, t.m_Cache, false)
    end
 	local data = ECS.ChunkDataUtility.GetComponentDataWithTypeName(t.m_Cache.CurChunk, t.m_ComponentTypeName, index-t.m_Cache.CachedBeginIndex)
 	return data
end

local set_fun = function ( t, index, value )
	if index < t.m_Cache.CachedBeginIndex or index >= t.m_Cache.CachedEndIndex then
        t.m_Iterator:MoveToEntityIndexAndUpdateCache(index, t.m_Cache, true)
    -- elseif not t.m_Cache.IsWriting then
    --     t.m_Cache.IsWriting = true;
    --     t.m_Iterator:UpdateChangeVersion()
    end
    t.m_Cache.CurChunk.Buffer[t.m_ComponentTypeName][index-t.m_Cache.CachedBeginIndex] = value
end

local meta_tbl = {
	__index = get_fun,
	__newindex = set_fun,
}
function ComponentDataArray.InitMetaTable( array )
	setmetatable(array, meta_tbl)
end

return ComponentDataArray