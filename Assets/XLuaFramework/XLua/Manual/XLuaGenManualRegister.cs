#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using System;
using System.Collections.Generic;
using System.Reflection;


namespace XLua.CSObjectWrap
{
    public class XLua_Gen_Manual_Register__
	{
        static void wrapInit0(LuaEnv luaenv, ObjectTranslator translator)
        {
            // translator.DelayWrapLoader(typeof(UnityEngine.Transform), TransformWrap_Manual.__Register);
        }
        
        public static void Init(LuaEnv luaenv, ObjectTranslator translator)
        {
            wrapInit0(luaenv, translator);
        }
	}
}
