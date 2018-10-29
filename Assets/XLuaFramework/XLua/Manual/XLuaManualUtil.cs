using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;
using System.Runtime.CompilerServices;

#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

namespace XLua
{
    public class XLuaManualUtil
    {
        public static void BeginObjectRegister(Type type, RealStatePtr L, ObjectTranslator translator, int meta_count, int method_count, int getter_count,
            int setter_count, int type_id = -1)
        {
            if (type == null)
            {
                if (type_id == -1) throw new Exception("Fatal: must provide a type of type_id");
                LuaAPI.xlua_rawgeti(L, LuaIndexes.LUA_REGISTRYINDEX, type_id);
            }
            else
            {
                LuaAPI.luaL_getmetatable(L, type.FullName);
            }
        }

        public static void EndObjectRegister(Type type, RealStatePtr L, ObjectTranslator translator, LuaCSFunction csIndexer,
            LuaCSFunction csNewIndexer, Type base_type, LuaCSFunction arrayIndexer, LuaCSFunction arrayNewIndexer)
        {

        }
        
    }
}