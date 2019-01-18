local ComponentDataArray = {}
ECS.ComponentDataArray = ComponentDataArray

function ComponentDataArray.Create( iterator, length, componentName )
	assert(iterator~=nil, "iterator should not be nil!")
	assert(length~=nil, "length should not be nil!")
	assert(componentName~=nil, "componentName should not be nil!")
	local array = {
		m_Iterator=iterator, 
		m_Length=length,
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
	if index < 1 or index > t.m_Length then
		return nil
	end
	if index < t.m_Cache.CachedBeginIndex or index >= t.m_Cache.CachedEndIndex then
        t.m_Iterator:MoveToEntityIndexAndUpdateCache(index, t.m_Cache, false)
    end
    return ECS.ChunkDataUtility.ReadComponentFromArray(t.m_Cache.CachedPtr, index, t.m_ComponentTypeName, t.m_Data)
end

local set_fun = function ( t, k )
end

local get_len = function ( t )
	print('Cat:ComponentDataArray.lua[39] t.m_Length', t.m_Length)
	return t.m_Length
end

function ComponentDataArray.InitMetaTable( array )
	local meta_tbl = {}
	meta_tbl.__index = get_fun
	meta_tbl.__len = get_len
	setmetatable(array, meta_tbl)
end

-- function ComponentDataArray:GetChunkArray( startIndex, maxCount )
--     local count
--     local ptr = GetUnsafeChunkPtr(startIndex, maxCount, count, true)
--     local arr = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray(ptr, count, Allocator.Invalid)
--     return arr
-- end

return ComponentDataArray