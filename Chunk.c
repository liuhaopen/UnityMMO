#include "Chunk.h"

static int GetChunkBufferSize(int numComponents, int numSharedComponents)
{
    int bufferSize = kChunkSize -
                     (sizeof(Chunk) - 4 + numSharedComponents * sizeof(int) + numComponents * sizeof(uint));
    return bufferSize;
}

static int GetSharedComponentOffset(int numSharedComponents)
{
    return kChunkSize - numSharedComponents * sizeof(int);
}

static int GetChangedComponentOffset(int numComponents, int numSharedComponents)
{
    return GetSharedComponentOffset(numSharedComponents) - numComponents * sizeof(uint);
}