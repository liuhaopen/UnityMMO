#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    
    public class XLuaFrameworkNetPackageTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(XLuaFramework.NetPackageType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(XLuaFramework.NetPackageType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(XLuaFramework.NetPackageType), L, null, 3, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BaseLine", XLuaFramework.NetPackageType.BaseLine);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BaseHead", XLuaFramework.NetPackageType.BaseHead);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(XLuaFramework.NetPackageType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushXLuaFrameworkNetPackageType(L, (XLuaFramework.NetPackageType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "BaseLine"))
                {
                    translator.PushXLuaFrameworkNetPackageType(L, XLuaFramework.NetPackageType.BaseLine);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "BaseHead"))
                {
                    translator.PushXLuaFrameworkNetPackageType(L, XLuaFramework.NetPackageType.BaseHead);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for XLuaFramework.NetPackageType!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for XLuaFramework.NetPackageType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityMMOSceneInfoKeyWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityMMO.SceneInfoKey), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityMMO.SceneInfoKey), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityMMO.SceneInfoKey), L, null, 5, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", UnityMMO.SceneInfoKey.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "EnterScene", UnityMMO.SceneInfoKey.EnterScene);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeaveScene", UnityMMO.SceneInfoKey.LeaveScene);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PosChange", UnityMMO.SceneInfoKey.PosChange);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityMMO.SceneInfoKey), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityMMOSceneInfoKey(L, (UnityMMO.SceneInfoKey)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushUnityMMOSceneInfoKey(L, UnityMMO.SceneInfoKey.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "EnterScene"))
                {
                    translator.PushUnityMMOSceneInfoKey(L, UnityMMO.SceneInfoKey.EnterScene);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LeaveScene"))
                {
                    translator.PushUnityMMOSceneInfoKey(L, UnityMMO.SceneInfoKey.LeaveScene);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PosChange"))
                {
                    translator.PushUnityMMOSceneInfoKey(L, UnityMMO.SceneInfoKey.PosChange);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityMMO.SceneInfoKey!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityMMO.SceneInfoKey! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityMMOSceneObjectTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityMMO.SceneObjectType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityMMO.SceneObjectType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityMMO.SceneObjectType), L, null, 5, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", UnityMMO.SceneObjectType.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Role", UnityMMO.SceneObjectType.Role);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Monster", UnityMMO.SceneObjectType.Monster);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NPC", UnityMMO.SceneObjectType.NPC);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityMMO.SceneObjectType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityMMOSceneObjectType(L, (UnityMMO.SceneObjectType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushUnityMMOSceneObjectType(L, UnityMMO.SceneObjectType.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Role"))
                {
                    translator.PushUnityMMOSceneObjectType(L, UnityMMO.SceneObjectType.Role);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Monster"))
                {
                    translator.PushUnityMMOSceneObjectType(L, UnityMMO.SceneObjectType.Monster);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "NPC"))
                {
                    translator.PushUnityMMOSceneObjectType(L, UnityMMO.SceneObjectType.NPC);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityMMO.SceneObjectType!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityMMO.SceneObjectType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
}