#define LUA_LIB
#if LUA_VERSION_NUM < 502
#define ECSCORE_API	LUALIB_API
#else 
#define ECSCORE_API	LUAMOD_API
#endif
#include <memory.h>
#include "lua.h"
#include <lualib.h>
#include "lauxlib.h"

static int getnumbersize(lua_State *L) {
	lua_pushnumber(L, sizeof(LUA_NUMBER));
	return 1;
}

static int getintegersize(lua_State *L) {
	lua_pushnumber(L, sizeof(LUA_INTEGER));
	return 1;
}

static int getbooleansize(lua_State *L) {
	lua_pushnumber(L, sizeof(int));
	return 1;
}

static void *checkchunk(lua_State *L) {
	//void *ud = luaL_checkudata(L, 1, "ECSCore.MetaTable");
	void* ud = lua_touserdata(L, 1);//may be a lightudata
	luaL_argcheck(L, ud != NULL, 1, "`ECSCore.Chunk' expected");
	return ud;
}

void HexDump(char *buf, int len, int addr) {
	int i, j, k;
	char binstr[80];

	for (i = 0; i<len; i++) {
		if (0 == (i % 16)) {
			sprintf(binstr, "%08x -", i + addr);
			sprintf(binstr, "%s %02x", binstr, (unsigned char)buf[i]);
		}
		else if (15 == (i % 16)) {
			sprintf(binstr, "%s %02x", binstr, (unsigned char)buf[i]);
			sprintf(binstr, "%s  ", binstr);
			for (j = i - 15; j <= i; j++) {
				sprintf(binstr, "%s%c", binstr, ('!'<buf[j] && buf[j] <= '~') ? buf[j] : '.');
			}
			printf("%s\n", binstr);
		}
		else {
			sprintf(binstr, "%s %02x", binstr, (unsigned char)buf[i]);
		}
	}
	if (0 != (i % 16)) {
		k = 16 - (i % 16);
		for (j = 0; j<k; j++) {
			sprintf(binstr, "%s   ", binstr);
		}
		sprintf(binstr, "%s  ", binstr);
		k = 16 - k;
		for (j = i - k; j<i; j++) {
			sprintf(binstr, "%s%c", binstr, ('!'<buf[j] && buf[j] <= '~') ? buf[j] : '.');
		}
		printf("%s\n", binstr);
	}
}

static int printchunk(lua_State *L) {
	void* ud = lua_touserdata(L, 1);//may be a lightudata
	int size = luaL_checkinteger(L, 2);
	HexDump((char*)ud, size, 0);
	return 0;
}

static int newchunk(lua_State *L) {
	int size = luaL_checkinteger(L, 1);
	void *a = lua_newuserdata(L, size);
	luaL_getmetatable(L, "ECSCore.MetaTable");
	lua_setmetatable(L, -2);
	memset(a, 0, size);
	//printf("new a : %d", a);
	return 1; 
}

static int writenumber(lua_State *L) {
	void *a = checkchunk(L);
	int offset = luaL_checkinteger(L, 2);
	lua_Number value = luaL_checknumber(L, 3);
	*((lua_Number*)((char*)a + offset)) = value;
	return 0;
}

static int readnumber(lua_State *L) {
	void *a = checkchunk(L);
	int offset = luaL_checkinteger(L, 2);
	lua_Number value = *((lua_Number*)((char*)a + offset));
	lua_pushnumber(L, value);
	return 1;
}

static int writeinteger(lua_State *L) {
	void *a = checkchunk(L);
	int offset = luaL_checkinteger(L, 2);
	lua_Integer value = luaL_checknumber(L, 3);
	*((lua_Integer*)((char*)a + offset)) = value;
	return 0;
}

static int readinteger(lua_State *L) {
	void *a = checkchunk(L);
	int offset = luaL_checkinteger(L, 2);
	lua_Integer value = *((lua_Integer*)((char*)a + offset));
	lua_pushinteger(L, value);
	return 1;
}

static int memclear(lua_State *L) {
	void *a = checkchunk(L);
	int size = luaL_checkinteger(L, 2);
	memset(a, 0, size);
	return 0;
}

static int memcopy(lua_State *L) {
	void *dest = checkchunk(L);
	void* src = lua_touserdata(L, 2);
	int size = luaL_checkinteger(L, 3);
	memcpy(dest, src, size);
	return 0;
}

static int writebool(lua_State *L) {
	void *a = checkchunk(L);
	int offset = luaL_checkinteger(L, 2);
	int value = luaL_checknumber(L, 3);
	*((int*)((char*)a + offset)) = value;
	//printf("newadfe a : %lf", a);
	return 0;
}

static int readbool(lua_State *L) {
	void *a = checkchunk(L);
	int offset = luaL_checkinteger(L, 2);
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
	int offset = luaL_checkinteger(L, 2);
	lua_pushlightuserdata(L, (char*)a+offset);
	return 1;
}

static const struct luaL_Reg meta_info[] = {
	{ "__tostring", chunk2string },
	{ "__add", chunk_add },
};

static const luaL_Reg core_funcs[] = {
	{ "GetNumberSize", getnumbersize },
	{ "GetIntegerSize", getintegersize },
	{ "GetBooleanSize", getbooleansize },
	{ "CreateChunk", newchunk },
	{ "PrintChunk", printchunk },
	{ "WriteNumber", writenumber },
	{ "ReadNumber", readnumber },
	{ "WriteInteger", writeinteger },
	{ "ReadInteger", readinteger },
	{ "WriteBoolean", writebool },
	{ "ReadBoolean", readbool },
	{ "MemClear", memclear },
	{ "MemCpy", memcopy },
	{ NULL, NULL } 
};

ECSCORE_API int luaopen_ECSCore(lua_State *L) {
	luaL_newmetatable(L, "ECSCore.MetaTable");
	lua_pushstring(L, "__index");
	lua_pushvalue(L, -2);    /* pushes the metatable */
	lua_settable(L, -3); /* metatable.__index = metatable */

#if LUA_VERSION_NUM < 502
	luaL_openlib(L, NULL, meta_info, 0);
	luaL_openlib(L, "ECSCore", core_funcs, 0);
#else
	luaL_setfuncs(L, meta_info, 0);
	luaL_newlib(L, core_funcs);
#endif
	return 1;
}
