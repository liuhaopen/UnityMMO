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
    return ECS.ChunkDataUtility.ReadComponentFromArray(t.m_Cache.CachedPtr, index, t.m_ComponentTypeName, t.m_Data)
end

local set_fun = function ( t, index, value )
	if index < t.m_Cache.CachedBeginIndex or index >= t.m_Cache.CachedEndIndex then
        t.m_Iterator:MoveToEntityIndexAndUpdateCache(index, t.m_Cache, true)
    -- elseif not t.m_Cache.IsWriting then
    --     t.m_Cache.IsWriting = true;
    --     t.m_Iterator:UpdateChangeVersion()
    end
    ECS.ChunkDataUtility.WriteComponentFromArray(t.m_Cache.CachedPtr, index, t.m_ComponentTypeName, value)
end

-- local get_len = function ( t )
-- 	return t.Length
-- end

local meta_tbl = {
	__index = get_fun,
	__newindex = set_fun,
	-- __len = get_len
}
function ComponentDataArray.InitMetaTable( array )
	setmetatable(array, meta_tbl)
end

-- function ComponentDataArray:GetChunkArray( startIndex, maxCount )
--     local count
--     local ptr = GetUnsafeChunkPtr(startIndex, maxCount, count, true)
--     local arr = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray(ptr, count, Allocator.Invalid)
--     return arr
-- end

return ComponentDataArray