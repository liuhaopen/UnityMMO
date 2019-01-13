//#define LUA_LIB
//#define LUA_BUILD_AS_DLL
//#define LUA_COMPAT_MODULE
#include <memory.h>
extern "C"
{
#include "lua.h"
#include <lualib.h>
#include "lauxlib.h"
}
static int getnumbersize(lua_State *L) {
	lua_pushnumber(L, sizeof(LUA_NUMBER));
	return 1;
}

static int getintegersize(lua_State *L) {
	lua_pushnumber(L, sizeof(LUA_INTEGER));
	return 1;
}

static int getboolsize(lua_State *L) {
	lua_pushnumber(L, sizeof(int));
	return 1;
}

static void *checkchunk(lua_State *L) {
	//void *ud = luaL_checkudata(L, 1, "ECSCore.MetaTable");
	void* ud = lua_touserdata(L, 1);//有可能是lightudata
	luaL_argcheck(L, ud != NULL, 1, "`ECSCore.Chunk' expected");
	return ud;
}

static int newchunk(lua_State *L) {
	int size = luaL_checkint(L, 1);
	void *a = lua_newuserdata(L, size);
	luaL_getmetatable(L, "ECSCore.MetaTable");
	lua_setmetatable(L, -2);
	memset(a, 0, size);
	//printf("new a : %d", a);
	return 1; 
}

static int writenumber(lua_State *L) {
	void *a = checkchunk(L);
	int offset = luaL_checkint(L, 2);
	double value = luaL_checknumber(L, 3);
	*((double*)((char*)a + offset)) = value;
	//printf("newadfe a : %lf", a);
	return 0;
}

static int readnumber(lua_State *L) {
	void *a = checkchunk(L);
	//printf("read a : %d", a);
	int offset = luaL_checkint(L, 2);
	double value = *((double*)((char*)a + offset));
	lua_pushnumber(L, value);
	return 1;
}

static int memclear(lua_State *L) {
	void *a = checkchunk(L);
	int size = luaL_checkint(L, 2);
	memset(a, 1, size);
	return 0;
}

static int memcopy(lua_State *L) {
	void *dest = checkchunk(L);
	void* src = lua_touserdata(L, 2);
	int size = luaL_checkint(L, 3);
	memcpy(dest, src, size);
	return 0;
}

static int writebool(lua_State *L) {
	void *a = checkchunk(L);
	int offset = luaL_checkint(L, 2);
	int value = luaL_checknumber(L, 3);
	*((int*)((char*)a + offset)) = value;
	//printf("newadfe a : %lf", a);
	return 0;
}

static int readbool(lua_State *L) {
	void *a = checkchunk(L);
	int offset = luaL_checkint(L, 2);
	int value = *((int*)((char*)a + offset));
	lua_pushboolean(L, value);
	return 1;
}

static int chunk2string(lua_State *L) {
	void *a = checkchunk(L);
	lua_pushfstring(L, "ECS Chunk Pointer : (%d)", a);
	return 1;
}

static int chunk_add(lua_State *L) {
	void *a = checkchunk(L);
	int offset = luaL_checkint(L, 2);
	lua_pushlightuserdata(L, (char*)a+offset);
	return 1;
}

static const struct luaL_reg meta_info[] = {
	{ "__tostring", chunk2string },
	{ "__add", chunk_add },
};

static const luaL_Reg core_funcs[] = {
	{ "GetNumberSize", getnumbersize },
	{ "GetIntegerSize", getintegersize },
	{ "CreateChunk", newchunk },
	{ "WriteNumber", writenumber },
	{ "ReadNumber", readnumber },
	{ "WriteBool", writebool },
	{ "ReadBool", readbool },
	{ "MemClear", memclear },
	{ "MemCpy", memcopy },
	{ NULL, NULL } 
};

extern "C" _declspec(dllexport) int luaopen_ECSCore(lua_State *L) {
	luaL_newmetatable(L, "ECSCore.MetaTable");
	lua_pushstring(L, "__index");
	lua_pushvalue(L, -2);    /* pushes the metatable */
	lua_settable(L, -3); /* metatable.__index = metatable */
	luaL_openlib(L, NULL, meta_info, 0);

#if LUA_VERSION_NUM < 502
	luaL_openlib(L, "ECSCore", core_funcs, 0);
#else
	luaL_newlib(L, mylib);
#endif
	return 1;
}
