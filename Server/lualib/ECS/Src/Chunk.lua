local Chunk = ECS.BaseClass()
ECS.Chunk = Chunk
ECS.Chunk.kChunkSize = 16 * 1024

function Chunk:Constructor(  )
	-- self.Buffer = ECS.Core.CreateChunk(ECS.Chunk.kChunkSize)
	self.Buffer = {}
	self.Count = 0--当前Entity的数量
	self.Capacity = 0--能存放Entity的容量
	self.SharedComponentValueArray = {}
	self.Archetype = nil
	self.ChunkListNode = nil
	self.ChunkListWithEmptySlotsNode = nil
	
end

function Chunk.GetChunkBufferSize( numComponents, numSharedComponents )
	local bufferSize = ECS.Chunk.kChunkSize - (numSharedComponents * 4 + numComponents * 4)
    return bufferSize
end

function Chunk.GetSharedComponentOffset( numSharedComponents )
    return 0
end

function Chunk.GetChangedComponentOffset( numComponents, numSharedComponents )
    return 0
end

return Chunk