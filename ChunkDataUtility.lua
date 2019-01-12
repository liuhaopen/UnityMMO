local ChunkDataUtility = {}
ECS.ChunkDataUtility = ChunkDataUtility

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

    return chunk.Buffer + (offset + sizeOf * index)
end

function ChunkDataUtility.GetComponentDataWithTypeRW( chunk, index, typeIndex, globalSystemVersion, typeLookupCache )
    local archetype = chunk.Archetype
    ChunkDataUtility.GetIndexInTypeArrayWithCache(archetype, typeIndex, typeLookupCache)
    local indexInTypeArray = typeLookupCache

    local offset = archetype.Offsets[indexInTypeArray]
    local sizeOf = archetype.SizeOfs[indexInTypeArray]

    chunk.ChangeVersion[indexInTypeArray] = globalSystemVersion

    return chunk.Buffer + (offset + sizeOf * index)
end

function ChunkDataUtility.GetComponentDataWithTypeRO( chunk, index, typeIndex )
    local indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(chunk.Archetype, typeIndex)

    local offset = chunk.Archetype.Offsets[indexInTypeArray]
    local sizeOf = chunk.Archetype.SizeOfs[indexInTypeArray]

    return chunk.Buffer + (offset + sizeOf * index)
end

function ChunkDataUtility.GetComponentDataWithTypeRW( chunk, index, typeIndex, globalSystemVersion )
    local indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(chunk.Archetype, typeIndex)
    local offset = chunk.Archetype.Offsets[indexInTypeArray]
    local sizeOf = chunk.Archetype.SizeOfs[indexInTypeArray]
    -- chunk.ChangeVersion[indexInTypeArray] = globalSystemVersion
    return chunk.Buffer + (offset + sizeOf * index)
end

function ChunkDataUtility.ReadComponentFromChunk( chunk_ptr, componentTypeName, out_data )
    local typeInfo = ECS.TypeManager.GetTypeInfoByName(componentTypeName)
    assert(typeInfo~=nil, "cannot find type info with : "..componentTypeName)
    out_data = out_data or {} 
    for k,v in pairs(typeInfo.FieldInfoList) do
        local field_val = ECSCore.ReadNumber(chunk_ptr, v.Offset)
        out_data[v.FieldName] = field_val
    end
    return out_data
end

function ChunkDataUtility.WriteComponentInChunk( chunk_ptr, componentTypeName, componentData )
    local typeInfo = ECS.TypeManager.GetTypeInfoByName(componentTypeName)
    assert(typeInfo~=nil, "cannot find type info with : "..componentTypeName)
    for k,v in pairs(typeInfo.FieldInfoList) do
        local new_field_value = componentData[v.FieldName]
        if v.FieldType ~= "table" then
            ECS.CoreHelper.WriteFieldValueInChunk(chunk_ptr, v.Offset, new_field_value, v.FieldType)
        -- else
            -- ChunkDataUtility.WriteComponentInChunk(chunk_ptr+v.Offset, new_field_value)
        end
    end
    -- ECSCore.WriteNumber(chunk_ptr, 0, 456.789)
end

function ChunkDataUtility.GetComponentDataRO( chunk, index, indexInTypeArray )
    local offset = chunk.Archetype.Offsets[indexInTypeArray]
    local sizeOf = chunk.Archetype.SizeOfs[indexInTypeArray]

    return chunk.Buffer + (offset + sizeOf * index)
end

function ChunkDataUtility.GetComponentDataRW( chunk, index, indexInTypeArray, globalSystemVersion )
    local offset = chunk.Archetype.Offsets[indexInTypeArray]
    local sizeOf = chunk.Archetype.SizeOfs[indexInTypeArray]

    chunk.ChangeVersion[indexInTypeArray] = globalSystemVersion

    return chunk.Buffer + (offset + sizeOf * index)
end

function ChunkDataUtility.Copy( srcChunk, srcIndex, dstChunk, dstIndex, count )
    Assert.IsTrue(srcChunk.Archetype == dstChunk.Archetype)

    local arch = srcChunk.Archetype
    local srcBuffer = srcChunk.Buffer
    local dstBuffer = dstChunk.Buffer
    local offsets = arch.Offsets
    local sizeOfs = arch.SizeOfs
    local typesCount = arch.TypesCount

    for t=1,typesCount do
        local offset = offsets[t]
        local sizeOf = sizeOfs[t]
        local src = srcBuffer + (offset + sizeOf * srcIndex)
        local dst = dstBuffer + (offset + sizeOf * dstIndex)

        dstChunk.ChangeVersion[t] = srcChunk.ChangeVersion[t]
        UnsafeUtility.MemCpy(dst, src, sizeOf * count)
    end
end

function ChunkDataUtility.InitializeComponents( dstChunk, dstIndex, count )
    local arch = dstChunk.Archetype

    local offsets = arch.Offsets
    local sizeOfs = arch.SizeOfs
    local dstBuffer = dstChunk.Buffer
    local typesCount = arch.TypesCount
    local types = arch.Types

    for t=2,typesCount do
        local offset = offsets[t]
        local sizeOf = sizeOfs[t]
        local dst = dstBuffer + (offset + sizeOf * dstIndex)

        if types[t].IsBuffer then
            for i=1,count do
                BufferHeader.Initialize(dst, types[t].BufferCapacity)
                dst = dst + sizeOf
            end
        else
            -- UnsafeUtility.MemClear(dst, sizeOf * count)
        end
    end
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

    local srcI = 0
    local dstI = 0
    while (srcI < srcArch.TypesCount and dstI < dstArch.TypesCount) do
        local src = srcChunk.Buffer + srcArch.Offsets[srcI] + srcIndex * srcArch.SizeOfs[srcI]
        local dst = dstChunk.Buffer + dstArch.Offsets[dstI] + dstIndex * dstArch.SizeOfs[dstI]

        if (srcArch.Types[srcI] < dstArch.Types[dstI]) then
            -- Clear any buffers we're not going to keep.
            if (srcArch.Types[srcI].IsBuffer) then
                BufferHeader.Destroy(src)
            end

            srcI = srcI + 1
        elseif (srcArch.Types[srcI] > dstArch.Types[dstI]) then
            -- Clear components in the destination that aren't copied
            if dstArch.Types[dstI].IsBuffer then
                BufferHeader.Initialize(dst, dstArch.Types[dstI].BufferCapacity)
            else
                UnsafeUtility.MemClear(dst, dstArch.SizeOfs[dstI])
            end
            dstI = dstI+1
        else
            UnsafeUtility.MemCpy(dst, src, srcArch.SizeOfs[srcI])
            -- Poison source buffer to make sure there is no aliasing.
            if (srcArch.Types[srcI].IsBuffer) then
                BufferHeader.Initialize(src, srcArch.Types[srcI].BufferCapacity)
            end
            srcI = srcI+1
            dstI = dstI+1
        end
    end

    -- Handle remaining components in the source that aren't copied
    for srcI=1,srcArch.TypesCount do
        local src = srcChunk.Buffer + srcArch.Offsets[srcI] + srcIndex * srcArch.SizeOfs[srcI]
        if (srcArch.Types[srcI].IsBuffer) then
            BufferHeader.Destroy(src)
        end
    end

    -- Clear remaining components in the destination that aren't copied
    for dstI=1,dstArch.TypesCount do
        local dst = dstChunk.Buffer + dstArch.Offsets[dstI] + dstIndex * dstArch.SizeOfs[dstI]
        if (dstArch.Types[dstI].IsBuffer) then
            BufferHeader.Initialize(dst, dstArch.Types[dstI].BufferCapacity)
        else
            UnsafeUtility.MemClear(dst, dstArch.SizeOfs[dstI])
        end
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

    local srcI = 0;
    local dstI = 0;
    while (srcI < srcArch.TypesCount and dstI < dstArch.TypesCount) do
        if (srcArch.Types[srcI] < dstArch.Types[dstI]) then
            srcI = srcI + 1
        else if (srcArch.Types[srcI] > dstArch.Types[dstI]) then
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

-- function ChunkDataUtility.ClearManagedObjects( typeMan, chunk, index, count )
--     local arch = chunk.Archetype;

--     for (local type = 0; type < arch.TypesCount; ++type) do
--         if (arch.ManagedArrayOffset[type] < 0)
--             continue;

--         for (local i = 0; i < count; ++i)
--             typeMan.SetManagedObject(chunk, type, index + i, null);
--         end
--     end
-- end