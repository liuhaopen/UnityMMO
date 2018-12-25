#include "lua.h"

const int kChunkSize = 16 * 1024 - 256;
const int kMaximumEntitiesPerChunk = kChunkSize / 8;
struct Chunk
{
    Chunk* ChunkListNode;
    Chunk* ChunkListWithEmptySlotsNode;
    int* Archetype;
    int* SharedComponentValueArray;
    int Count;
    int Capacity;
    int ManagedArrayIndex;
    int Padding0;
    int* ChangeVersion;
    int* Padding2;
    int* Buffer;
}Chunk;


static int newchunk(lua_State *L) {
    int size = luaL_checkint(L, 1);
    Chunk *a = (Chunk*)lua_newuserdata(L, size);
    a->size = n;
    return 1;
}

static int setchunkdata(lua_State *L) {
    Chunk *a = (Chunk*)lua_touserdata(L, 1);
    int index = luaL_checkint(L, 2);
    double value = luaL_checknumber(L, 3);
    luaL_argcheck(L, a != NULL, 1, "`chunk' expected");
    luaL_argcheck(L, 1 <= index && index <= a->size, 2,
              "index out of range");
    a->values[index-1] = value;
    return 0;
}

static const struct luaL_reg chunklib_f [] = {
    {"new", newchunk},
    {NULL, NULL}
};

static const struct luaL_reg chunklib_m [] = {
    {"set", setchunkdata},
    {"get", getchunkdata},
    {"size", getsize},
    {NULL, NULL}
};

int luaopen_ecschunk(lua_State *L) {
    luaL_newmetatable(L, "ECS.Chunk");
    lua_pushstring(L, "__index");
    lua_pushvalue(L, -2);    /* pushes the metatable */
    lua_settable(L, -3); /* metatable.__index = metatable */
    luaL_openlib(L, NULL, chunklib_m, 0);
    luaL_openlib(L, "chunk", chunklib_f, 0);
    return 1;
}


