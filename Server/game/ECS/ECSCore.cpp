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

static void *checkchunk(lua_State *L) {
	void *ud = luaL_checkudata(L, 1, "ECSCore.MetaTable");
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
	luaL_argcheck(L, a != NULL, 1, "`chunk' expected");
	*((double*)((char*)a + offset)) = value;
	//printf("newadfe a : %lf", a);
	return 0;
}

static int readnumber(lua_State *L) {
	void *a = checkchunk(L);
	//printf("read a : %d", a);
	int offset = luaL_checkint(L, 2);
	luaL_argcheck(L, a != NULL, 1, "`chunk' expected");
	double value = *((double*)((char*)a + offset));
	lua_pushnumber(L, value);
	return 1;
}

static const luaL_Reg mylib[] = {
	{ "GetNumberSize", getnumbersize },
	{ "GetIntegerSize", getintegersize },
	{ "CreateChunk", newchunk },
	{ "WriteNumber", writenumber },
	{ "ReadNumber", readnumber },
	{ NULL, NULL } 
};

extern "C" _declspec(dllexport) int luaopen_ECSCore(lua_State *L) {
	luaL_newmetatable(L, "ECSCore.MetaTable");
#if LUA_VERSION_NUM < 502
	luaL_openlib(L, "ECSCore", mylib, 0);
#else
	luaL_newlib(L, mylib);
#endif
	return 1;
}
