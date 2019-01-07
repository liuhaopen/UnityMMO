local Chunk = {}
ECS.Chunk = Chunk

ECS.Chunk.kChunkSize = 16 * 1024

function Chunk.GetChunkBufferSize( numComponents, numSharedComponents )
	-- local bufferSize = ECS.Chunk.kChunkSize - (numSharedComponents * sizeof(int) + numComponents * sizeof(uint))
	local bufferSize = ECS.Chunk.kChunkSize - (numSharedComponents * 4 + numComponents * 4)
    return bufferSize
end

return Chunk