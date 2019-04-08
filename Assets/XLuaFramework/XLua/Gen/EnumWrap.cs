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
			
			Utils.BeginClassRegister(typeof(UnityMMO.SceneInfoKey), L, null, 7, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", UnityMMO.SceneInfoKey.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "EnterView", UnityMMO.SceneInfoKey.EnterView);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeaveView", UnityMMO.SceneInfoKey.LeaveView);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PosChange", UnityMMO.SceneInfoKey.PosChange);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TargetPos", UnityMMO.SceneInfoKey.TargetPos);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JumpState", UnityMMO.SceneInfoKey.JumpState);
            
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
				else if (LuaAPI.xlua_is_eq_str(L, 1, "EnterView"))
                {
                    translator.PushUnityMMOSceneInfoKey(L, UnityMMO.SceneInfoKey.EnterView);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LeaveView"))
                {
                    translator.PushUnityMMOSceneInfoKey(L, UnityMMO.SceneInfoKey.LeaveView);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PosChange"))
                {
                    translator.PushUnityMMOSceneInfoKey(L, UnityMMO.SceneInfoKey.PosChange);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TargetPos"))
                {
                    translator.PushUnityMMOSceneInfoKey(L, UnityMMO.SceneInfoKey.TargetPos);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JumpState"))
                {
                    translator.PushUnityMMOSceneInfoKey(L, UnityMMO.SceneInfoKey.JumpState);
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
    
    public class UnityEngineTextAnchorWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.TextAnchor), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.TextAnchor), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.TextAnchor), L, null, 10, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UpperLeft", UnityEngine.TextAnchor.UpperLeft);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UpperCenter", UnityEngine.TextAnchor.UpperCenter);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UpperRight", UnityEngine.TextAnchor.UpperRight);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MiddleLeft", UnityEngine.TextAnchor.MiddleLeft);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MiddleCenter", UnityEngine.TextAnchor.MiddleCenter);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MiddleRight", UnityEngine.TextAnchor.MiddleRight);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LowerLeft", UnityEngine.TextAnchor.LowerLeft);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LowerCenter", UnityEngine.TextAnchor.LowerCenter);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LowerRight", UnityEngine.TextAnchor.LowerRight);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.TextAnchor), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTextAnchor(L, (UnityEngine.TextAnchor)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "UpperLeft"))
                {
                    translator.PushUnityEngineTextAnchor(L, UnityEngine.TextAnchor.UpperLeft);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "UpperCenter"))
                {
                    translator.PushUnityEngineTextAnchor(L, UnityEngine.TextAnchor.UpperCenter);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "UpperRight"))
                {
                    translator.PushUnityEngineTextAnchor(L, UnityEngine.TextAnchor.UpperRight);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "MiddleLeft"))
                {
                    translator.PushUnityEngineTextAnchor(L, UnityEngine.TextAnchor.MiddleLeft);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "MiddleCenter"))
                {
                    translator.PushUnityEngineTextAnchor(L, UnityEngine.TextAnchor.MiddleCenter);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "MiddleRight"))
                {
                    translator.PushUnityEngineTextAnchor(L, UnityEngine.TextAnchor.MiddleRight);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LowerLeft"))
                {
                    translator.PushUnityEngineTextAnchor(L, UnityEngine.TextAnchor.LowerLeft);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LowerCenter"))
                {
                    translator.PushUnityEngineTextAnchor(L, UnityEngine.TextAnchor.LowerCenter);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LowerRight"))
                {
                    translator.PushUnityEngineTextAnchor(L, UnityEngine.TextAnchor.LowerRight);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.TextAnchor!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.TextAnchor! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTouchPhaseWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.TouchPhase), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.TouchPhase), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.TouchPhase), L, null, 6, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Began", UnityEngine.TouchPhase.Began);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Moved", UnityEngine.TouchPhase.Moved);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Stationary", UnityEngine.TouchPhase.Stationary);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Ended", UnityEngine.TouchPhase.Ended);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Canceled", UnityEngine.TouchPhase.Canceled);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.TouchPhase), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTouchPhase(L, (UnityEngine.TouchPhase)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "Began"))
                {
                    translator.PushUnityEngineTouchPhase(L, UnityEngine.TouchPhase.Began);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Moved"))
                {
                    translator.PushUnityEngineTouchPhase(L, UnityEngine.TouchPhase.Moved);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Stationary"))
                {
                    translator.PushUnityEngineTouchPhase(L, UnityEngine.TouchPhase.Stationary);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Ended"))
                {
                    translator.PushUnityEngineTouchPhase(L, UnityEngine.TouchPhase.Ended);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Canceled"))
                {
                    translator.PushUnityEngineTouchPhase(L, UnityEngine.TouchPhase.Canceled);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.TouchPhase!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.TouchPhase! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
}