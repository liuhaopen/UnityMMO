local EntityArray = {}
ECS.EntityArray = EntityArray
function EntityArray.Create( iterator, length )
	assert(iterator~=nil, "iterator should not be nil!")
	assert(length~=nil, "length should not be nil!")
	local array = {
		m_Iterator=iterator, 
		Length=length,
		m_Data = {},
		m_Cache = {
			CachedPtr=nil, CachedBeginIndex=0, CachedEndIndex=0, CachedSizeOf=0, IsWriting=false
		},
	}
	EntityArray.InitMetaTable(array)
	return array
end

local get_fun = function ( t, index )
	if index < 1 or index > t.Length then
		return nil
	end
	if index < t.m_Cache.CachedBeginIndex or index >= t.m_Cache.CachedEndIndex then
        t.m_Iterator:MoveToEntityIndexAndUpdateCache(index, t.m_Cache, false)
    end
    -- return ECS.ChunkDataUtility.ReadComponentFromArray(t.m_Cache.CachedPtr, index, ECS.Entity.Name, t.m_Data)
    local data = ECS.ChunkDataUtility.GetComponentDataWithTypeName(t.m_Cache.CurChunk, ECS.Entity.Name, index-t.m_Cache.CachedBeginIndex)
 	return data
end

local set_fun = function ( t, index, value )
	print("EntityArray setter is useless : ", debug.traceback())
end

local meta_tbl = {
	__index = get_fun,
	__newindex = set_fun,
}
function EntityArray.InitMetaTable( array )
	setmetatable(array, meta_tbl)
end

return EntityArray