local ChunkDataUtility = {}

function ChunkDataUtility.GetIndexInTypeArray( archetype, typeIndex )
	local types = archetype.Types
    local typeCount = archetype.TypesCount
    for i=1,typeCount do
        if typeIndex == types[i].TypeIndex then
            return i
        end
    end
    return -1
end

function ChunkDataUtility.IsTypeNameInArchetype( archetype, typeName )
    local result = archetype and archetype.TypesMap and archetype.TypesMap[typeName]
    return result ~= nil
end

function ChunkDataUtility.GetIndexInTypeArrayWithCache( archetype, typeIndex, typeLookupCache )
    local types = archetype.Types
    local typeCount = archetype.TypesCount

    if typeLookupCache < typeCount and types[typeLookupCache].TypeIndex == typeIndex then
        return typeLookupCache
    end

    for i=1,typeCount do
        if (typeIndex == types[i].TypeIndex) then
	        return i
	    end
        break
    end
    return typeLookupCache
end

function ChunkDataUtility.GetComponentDataWithTypeROWithCache( chunk, index, typeIndex, typeLookupCache )
    local archetype = chunk.Archetype
    ChunkDataUtility.GetIndexInTypeArrayWithCache(archetype, typeIndex, typeLookupCache)
    local indexInTypeArray = typeLookupCache

    local offset = archetype.Offsets[indexInTypeArray]
    local sizeOf = archetype.SizeOfs[indexInTypeArray]

    return chunk.Buffer + (offset + sizeOf * (index-1))
end

function ChunkDataUtility.GetComponentDataWithTypeRW( chunk, index, typeIndex, globalSystemVersion, typeLookupCache )
    local archetype = chunk.Archetype
    ChunkDataUtility.GetIndexInTypeArrayWithCache(archetype, typeIndex, typeLookupCache)
    local indexInTypeArray = typeLookupCache

    local offset = archetype.Offsets[indexInTypeArray]
    local sizeOf = archetype.SizeOfs[indexInTypeArray]

    chunk.ChangeVersion[indexInTypeArray] = globalSystemVersion

    return chunk.Buffer + (offset + sizeOf * (index-1))
end

function ChunkDataUtility.GetComponentDataWithTypeRO( chunk, index, typeIndex )
    local indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(chunk.Archetype, typeIndex)
    local offset = chunk.Archetype.Offsets[indexInTypeArray]
    local sizeOf = chunk.Archetype.SizeOfs[indexInTypeArray]
    return chunk.Buffer + (offset + sizeOf * (index-1))
end

function ChunkDataUtility.GetComponentDataWithTypeRW( chunk, index, typeIndex, globalSystemVersion )
    local indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(chunk.Archetype, typeIndex)
    local offset = chunk.Archetype.Offsets[indexInTypeArray]
    local sizeOf = chunk.Archetype.SizeOfs[indexInTypeArray]
    -- chunk.ChangeVersion[indexInTypeArray] = globalSystemVersion
    return chunk.Buffer + (offset + sizeOf * (index-1))
end

function ChunkDataUtility.ReadComponentFromChunk( chunk_ptr, componentTypeName, out_data, offset )
    local typeInfo = ECS.TypeManager.GetTypeInfoByName(componentTypeName)
    assert(typeInfo~=nil, "cannot find type info with : "..componentTypeName)
    out_data = out_data or {} 
    offset = offset or 0
    for k,v in pairs(typeInfo.FieldInfoList) do
        local field_val = ECS.CoreHelper.ReadFieldValueInChunk(chunk_ptr, offset+v.Offset, v.FieldType)
        out_data[v.FieldName] = field_val
    end
    return out_data
end

function ChunkDataUtility.WriteComponentInChunk_old( chunk_ptr, componentTypeName, componentData, offset )
    local typeInfo = ECS.TypeManager.GetTypeInfoByName(componentTypeName)
    assert(typeInfo~=nil, "cannot find type info with : "..componentTypeName)
    offset = offset or 0
    for k,v in pairs(typeInfo.FieldInfoList) do
        local new_field_value = componentData[v.FieldName]
        if v.FieldType ~= "table" then
            ECS.CoreHelper.WriteFieldValueInChunk(chunk_ptr, offset+v.Offset, new_field_value, v.FieldType)
        -- else
            -- ChunkDataUtility.WriteComponentInChunk(chunk_ptr+v.Offset, new_field_value)
        end
    end
end

function ChunkDataUtility:WriteComponentInChunk( buffer, componentTypeName, index, componentData )
    buffer[componentTypeName][index] = componentData
end

function ChunkDataUtility.ReadComponentFromArray( chunk_ptr, index, componentTypeName, out_data )
    local typeInfo = ECS.TypeManager.GetTypeInfoByName(componentTypeName)
    assert(typeInfo~=nil, "cannot find type info with : "..componentTypeName)
    return ChunkDataUtility.ReadComponentFromChunk(chunk_ptr, componentTypeName, out_data, (index-1)*typeInfo.SizeInChunk)
end

function ChunkDataUtility.WriteComponentFromArray( chunk_ptr, index, componentTypeName, componentData )
    local typeInfo = ECS.TypeManager.GetTypeInfoByName(componentTypeName)
    assert(typeInfo~=nil, "cannot find type info with : "..componentTypeName)
    return ChunkDataUtility.WriteComponentInChunk(chunk_ptr, componentTypeName, componentData, (index-1)*typeInfo.SizeInChunk)
end

function ChunkDataUtility.GetComponentDataRO( chunk, index, indexInTypeArray )
    local offset = chunk.Archetype.Offsets[indexInTypeArray]
    local sizeOf = chunk.Archetype.SizeOfs[indexInTypeArray]
    return chunk.Buffer + (offset + sizeOf * (index-1))
end

function ChunkDataUtility.GetComponentDataWithTypeName( chunk, componentName, index )
    local data = chunk.Buffer[componentName][index]
    if data ~= nil then
        return data
    else
        --lazy init
        local typeInfo = ECS.TypeManager.GetTypeInfoByName(componentName) 
        data = ECS.ChunkDataUtility.DeepCopy(typeInfo.Prototype)
        chunk.Buffer[componentName][index] = data
        return data
    end
end

function ChunkDataUtility.GetComponentDataRW( chunk, index, indexInTypeArray, globalSystemVersion )
    local offset = chunk.Archetype.Offsets[indexInTypeArray]
    local sizeOf = chunk.Archetype.SizeOfs[indexInTypeArray]

    chunk.ChangeVersion[indexInTypeArray] = globalSystemVersion

    return chunk.Buffer + (offset + sizeOf * (index-1))
end

function ChunkDataUtility.Copy( srcChunk, srcIndex, dstChunk, dstIndex, count )
    assert(srcChunk.Archetype == dstChunk.Archetype)

    local arch = srcChunk.Archetype
    local srcBuffer = srcChunk.Buffer
    local dstBuffer = dstChunk.Buffer
    local offsets = arch.Offsets
    local sizeOfs = arch.SizeOfs
    local typesCount = arch.TypesCount
    for k,v in pairs(srcChunk.Buffer) do
        for iii=1,count do
            if dstBuffer[k] and dstBuffer[k][dstIndex+iii] then
                dstBuffer[k][dstIndex+iii] = v[srcIndex+iii]
            end
        end
    end
end

function ChunkDataUtility.DeepCopy( object )
    local lookup_table = {}
    local function _copy(object)
        if type(object) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end

        local new_table = {}
        lookup_table[object] = new_table
        for index, value in pairs(object) do
            new_table[_copy(index)] = _copy(value)
        end

        return setmetatable(new_table, getmetatable(object))
    end

    return _copy(object)
end

function ChunkDataUtility.InitializeComponents( dstChunk, dstIndex, count )
end

function ChunkDataUtility.ReplicateComponents( srcChunk, srcIndex, dstChunk, dstBaseIndex, count )
    local srcArchetype  = srcChunk.Archetype
    local dstArchetype  = dstChunk.Archetype
    local srcBuffer     = srcChunk.Buffer
    local dstBuffer     = dstChunk.Buffer
    local srcOffsets    = srcArchetype.Offsets
    local srcSizeOfs    = srcArchetype.SizeOfs
    local srcTypesCount = srcArchetype.TypesCount
    local srcTypes      = srcArchetype.Types
    local dstTypeIndex  = 1
    -- type[0] is always Entity, and will be patched up later, so just skip
    for srcTypeIndex=2,srcTypesCount do
        local srcType   = srcTypes[srcTypeIndex]
        local dstType   = srcTypes[dstTypeIndex]
        
        -- Type does not exist in destination. Skip it.
        if srcType.TypeIndex == dstType.TypeIndex then
        
            local srcOffset = srcOffsets[srcTypeIndex]
            local srcSizeOf = srcSizeOfs[srcTypeIndex]
            
            local src = srcBuffer + (srcOffset + srcSizeOf * srcIndex)
            local dst = dstBuffer + (srcOffset + srcSizeOf * dstBaseIndex)

            if not srcType.IsBuffer then
                UnsafeUtility.MemCpyReplicate(dst, src, srcSizeOf, count)
            else
                local alignment = 8 -- TODO: Need a way to compute proper alignment for arbitrary non-generic types in TypeManager
                for i=1,count do
                    local srcHdr = src
                    local dstHdr = dst
                    BufferHeader.Initialize(dstHdr, srcType.BufferCapacity)
                    BufferHeader.Assign(dstHdr, BufferHeader.GetElementPointer(srcHdr), srcHdr.Length, srcSizeOf, alignment)

                    src = src + srcSizeOf
                    dst = dst + srcSizeOf
                end
            end

            dstTypeIndex = dstTypeIndex + 1
        end
    end
end

function ChunkDataUtility.Convert( srcChunk, srcIndex, dstChunk, dstIndex )
    local srcArch = srcChunk.Archetype
    local dstArch = dstChunk.Archetype
    for k,v in pairs(srcChunk.Buffer) do
        v[dstIndex] = srcChunk.Buffer[k][srcIndex]
    end
end

function ChunkDataUtility.PoisonUnusedChunkData( chunk, value )
    local arch = chunk.Archetype
    local bufferSize = Chunk.GetChunkBufferSize(arch.TypesCount, arch.NumSharedComponents)
    local buffer = chunk.Buffer
    local count = chunk.Count

    for i=1,arch.TypesCount-1 do
        local index = arch.TypeMemoryOrder[i]
        local nextIndex = arch.TypeMemoryOrder[i + 1]
        local startOffset = arch.Offsets[index] + count * arch.SizeOfs[index]
        local endOffset = arch.Offsets[nextIndex]
        UnsafeUtilityEx.MemSet(buffer + startOffset, value, endOffset - startOffset)
    end
    local lastIndex = arch.TypeMemoryOrder[arch.TypesCount - 1]
    local lastStartOffset = arch.Offsets[lastIndex] + count * arch.SizeOfs[lastIndex]
    UnsafeUtilityEx.MemSet(buffer + lastStartOffset, value, bufferSize - lastStartOffset)
end

function ChunkDataUtility.CopyManagedObjects( typeMan, srcChunk, srcStartIndex, dstChunk, dstStartIndex, count )
    local srcArch = srcChunk.Archetype
    local dstArch = dstChunk.Archetype

    local srcI = 0
    local dstI = 0
    while (srcI < srcArch.TypesCount and dstI < dstArch.TypesCount) do
        if (srcArch.Types[srcI] < dstArch.Types[dstI]) then
            srcI = srcI + 1
        elseif (srcArch.Types[srcI] > dstArch.Types[dstI]) then
            dstI = dstI + 1
        else
            if (srcArch.ManagedArrayOffset[srcI] >= 0) then
                for i=1,count do
                    local obj = typeMan.GetManagedObject(srcChunk, srcI, srcStartIndex + i)
                    typeMan.SetManagedObject(dstChunk, dstI, dstStartIndex + i, obj)
                end
            end
            srcI = srcI + 1
            dstI = dstI + 1
        end
    end
end

function ChunkDataUtility.ClearManagedObjects( typeMan, chunk, index, count )
    local arch = chunk.Archetype

    for type=1,arch.TypesCount do
        if (arch.ManagedArrayOffset[type] >= 0) then
            for i=1,count do
                typeMan.SetManagedObject(chunk, type, index + i, null)
            end
        end
    end
end

return ChunkDataUtility