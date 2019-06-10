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
			
			Utils.BeginClassRegister(typeof(UnityMMO.SceneInfoKey), L, null, 8, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", UnityMMO.SceneInfoKey.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "EnterView", UnityMMO.SceneInfoKey.EnterView);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeaveView", UnityMMO.SceneInfoKey.LeaveView);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PosChange", UnityMMO.SceneInfoKey.PosChange);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TargetPos", UnityMMO.SceneInfoKey.TargetPos);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JumpState", UnityMMO.SceneInfoKey.JumpState);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "HPChange", UnityMMO.SceneInfoKey.HPChange);
            
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
				else if (LuaAPI.xlua_is_eq_str(L, 1, "HPChange"))
                {
                    translator.PushUnityMMOSceneInfoKey(L, UnityMMO.SceneInfoKey.HPChange);
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
			
			Utils.BeginClassRegister(typeof(UnityMMO.SceneObjectType), L, null, 6, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", UnityMMO.SceneObjectType.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Role", UnityMMO.SceneObjectType.Role);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Monster", UnityMMO.SceneObjectType.Monster);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NPC", UnityMMO.SceneObjectType.NPC);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "DropItem", UnityMMO.SceneObjectType.DropItem);
            
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
				else if (LuaAPI.xlua_is_eq_str(L, 1, "DropItem"))
                {
                    translator.PushUnityMMOSceneObjectType(L, UnityMMO.SceneObjectType.DropItem);
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
    
    public class UnityMMOTimelineStateNewStateWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityMMO.TimelineState.NewState), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityMMO.TimelineState.NewState), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityMMO.TimelineState.NewState), L, null, 3, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Allow", UnityMMO.TimelineState.NewState.Allow);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Forbid", UnityMMO.TimelineState.NewState.Forbid);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityMMO.TimelineState.NewState), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityMMOTimelineStateNewState(L, (UnityMMO.TimelineState.NewState)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "Allow"))
                {
                    translator.PushUnityMMOTimelineStateNewState(L, UnityMMO.TimelineState.NewState.Allow);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Forbid"))
                {
                    translator.PushUnityMMOTimelineStateNewState(L, UnityMMO.TimelineState.NewState.Forbid);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityMMO.TimelineState.NewState!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityMMO.TimelineState.NewState! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityMMOTimelineStateInterruptStateWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityMMO.TimelineState.InterruptState), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityMMO.TimelineState.InterruptState), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityMMO.TimelineState.InterruptState), L, null, 3, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Allow", UnityMMO.TimelineState.InterruptState.Allow);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Forbid", UnityMMO.TimelineState.InterruptState.Forbid);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityMMO.TimelineState.InterruptState), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityMMOTimelineStateInterruptState(L, (UnityMMO.TimelineState.InterruptState)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "Allow"))
                {
                    translator.PushUnityMMOTimelineStateInterruptState(L, UnityMMO.TimelineState.InterruptState.Allow);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Forbid"))
                {
                    translator.PushUnityMMOTimelineStateInterruptState(L, UnityMMO.TimelineState.InterruptState.Forbid);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityMMO.TimelineState.InterruptState!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityMMO.TimelineState.InterruptState! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityMMOLocomotionStateStateWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityMMO.LocomotionState.State), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityMMO.LocomotionState.State), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityMMO.LocomotionState.State), L, null, 11, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Idle", UnityMMO.LocomotionState.State.Idle);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Run", UnityMMO.LocomotionState.State.Run);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Sprint", UnityMMO.LocomotionState.State.Sprint);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Jump", UnityMMO.LocomotionState.State.Jump);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "DoubleJump", UnityMMO.LocomotionState.State.DoubleJump);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TrebleJump", UnityMMO.LocomotionState.State.TrebleJump);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "InAir", UnityMMO.LocomotionState.State.InAir);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BeHit", UnityMMO.LocomotionState.State.BeHit);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Dead", UnityMMO.LocomotionState.State.Dead);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "StateNum", UnityMMO.LocomotionState.State.StateNum);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityMMO.LocomotionState.State), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityMMOLocomotionStateState(L, (UnityMMO.LocomotionState.State)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "Idle"))
                {
                    translator.PushUnityMMOLocomotionStateState(L, UnityMMO.LocomotionState.State.Idle);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Run"))
                {
                    translator.PushUnityMMOLocomotionStateState(L, UnityMMO.LocomotionState.State.Run);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Sprint"))
                {
                    translator.PushUnityMMOLocomotionStateState(L, UnityMMO.LocomotionState.State.Sprint);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Jump"))
                {
                    translator.PushUnityMMOLocomotionStateState(L, UnityMMO.LocomotionState.State.Jump);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "DoubleJump"))
                {
                    translator.PushUnityMMOLocomotionStateState(L, UnityMMO.LocomotionState.State.DoubleJump);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TrebleJump"))
                {
                    translator.PushUnityMMOLocomotionStateState(L, UnityMMO.LocomotionState.State.TrebleJump);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "InAir"))
                {
                    translator.PushUnityMMOLocomotionStateState(L, UnityMMO.LocomotionState.State.InAir);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "BeHit"))
                {
                    translator.PushUnityMMOLocomotionStateState(L, UnityMMO.LocomotionState.State.BeHit);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Dead"))
                {
                    translator.PushUnityMMOLocomotionStateState(L, UnityMMO.LocomotionState.State.Dead);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "StateNum"))
                {
                    translator.PushUnityMMOLocomotionStateState(L, UnityMMO.LocomotionState.State.StateNum);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityMMO.LocomotionState.State!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityMMO.LocomotionState.State! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityMMONameboardDataResStateWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityMMO.NameboardData.ResState), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityMMO.NameboardData.ResState), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityMMO.NameboardData.ResState), L, null, 5, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WaitLoad", UnityMMO.NameboardData.ResState.WaitLoad);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Loading", UnityMMO.NameboardData.ResState.Loading);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Loaded", UnityMMO.NameboardData.ResState.Loaded);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Deleting", UnityMMO.NameboardData.ResState.Deleting);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityMMO.NameboardData.ResState), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityMMONameboardDataResState(L, (UnityMMO.NameboardData.ResState)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "WaitLoad"))
                {
                    translator.PushUnityMMONameboardDataResState(L, UnityMMO.NameboardData.ResState.WaitLoad);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Loading"))
                {
                    translator.PushUnityMMONameboardDataResState(L, UnityMMO.NameboardData.ResState.Loading);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Loaded"))
                {
                    translator.PushUnityMMONameboardDataResState(L, UnityMMO.NameboardData.ResState.Loaded);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Deleting"))
                {
                    translator.PushUnityMMONameboardDataResState(L, UnityMMO.NameboardData.ResState.Deleting);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityMMO.NameboardData.ResState!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityMMO.NameboardData.ResState! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityMMOJumpStateStateWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityMMO.JumpState.State), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityMMO.JumpState.State), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityMMO.JumpState.State), L, null, 5, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", UnityMMO.JumpState.State.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "StartJump", UnityMMO.JumpState.State.StartJump);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "InAir", UnityMMO.JumpState.State.InAir);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "EndJump", UnityMMO.JumpState.State.EndJump);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityMMO.JumpState.State), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityMMOJumpStateState(L, (UnityMMO.JumpState.State)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushUnityMMOJumpStateState(L, UnityMMO.JumpState.State.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "StartJump"))
                {
                    translator.PushUnityMMOJumpStateState(L, UnityMMO.JumpState.State.StartJump);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "InAir"))
                {
                    translator.PushUnityMMOJumpStateState(L, UnityMMO.JumpState.State.InAir);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "EndJump"))
                {
                    translator.PushUnityMMOJumpStateState(L, UnityMMO.JumpState.State.EndJump);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityMMO.JumpState.State!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityMMO.JumpState.State! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityMMOActionInfoTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityMMO.ActionInfo.Type), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityMMO.ActionInfo.Type), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityMMO.ActionInfo.Type), L, null, 6, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", UnityMMO.ActionInfo.Type.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Skill1", UnityMMO.ActionInfo.Type.Skill1);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Skill2", UnityMMO.ActionInfo.Type.Skill2);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Skill3", UnityMMO.ActionInfo.Type.Skill3);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Skill4", UnityMMO.ActionInfo.Type.Skill4);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityMMO.ActionInfo.Type), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityMMOActionInfoType(L, (UnityMMO.ActionInfo.Type)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushUnityMMOActionInfoType(L, UnityMMO.ActionInfo.Type.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Skill1"))
                {
                    translator.PushUnityMMOActionInfoType(L, UnityMMO.ActionInfo.Type.Skill1);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Skill2"))
                {
                    translator.PushUnityMMOActionInfoType(L, UnityMMO.ActionInfo.Type.Skill2);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Skill3"))
                {
                    translator.PushUnityMMOActionInfoType(L, UnityMMO.ActionInfo.Type.Skill3);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Skill4"))
                {
                    translator.PushUnityMMOActionInfoType(L, UnityMMO.ActionInfo.Type.Skill4);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityMMO.ActionInfo.Type!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityMMO.ActionInfo.Type! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityMMOLooksInfoStateWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityMMO.LooksInfo.State), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityMMO.LooksInfo.State), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityMMO.LooksInfo.State), L, null, 4, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", UnityMMO.LooksInfo.State.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Loading", UnityMMO.LooksInfo.State.Loading);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Loaded", UnityMMO.LooksInfo.State.Loaded);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityMMO.LooksInfo.State), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityMMOLooksInfoState(L, (UnityMMO.LooksInfo.State)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushUnityMMOLooksInfoState(L, UnityMMO.LooksInfo.State.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Loading"))
                {
                    translator.PushUnityMMOLooksInfoState(L, UnityMMO.LooksInfo.State.Loading);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Loaded"))
                {
                    translator.PushUnityMMOLooksInfoState(L, UnityMMO.LooksInfo.State.Loaded);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityMMO.LooksInfo.State!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityMMO.LooksInfo.State! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityMMOSceneObjectDataTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityMMO.SceneObjectData.Type), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityMMO.SceneObjectData.Type), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityMMO.SceneObjectData.Type), L, null, 4, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Role", UnityMMO.SceneObjectData.Type.Role);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Monster", UnityMMO.SceneObjectData.Type.Monster);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NPC", UnityMMO.SceneObjectData.Type.NPC);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityMMO.SceneObjectData.Type), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityMMOSceneObjectDataType(L, (UnityMMO.SceneObjectData.Type)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "Role"))
                {
                    translator.PushUnityMMOSceneObjectDataType(L, UnityMMO.SceneObjectData.Type.Role);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Monster"))
                {
                    translator.PushUnityMMOSceneObjectDataType(L, UnityMMO.SceneObjectData.Type.Monster);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "NPC"))
                {
                    translator.PushUnityMMOSceneObjectDataType(L, UnityMMO.SceneObjectData.Type.NPC);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityMMO.SceneObjectData.Type!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityMMO.SceneObjectData.Type! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityMMOTimelineInfoEventWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityMMO.TimelineInfo.Event), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityMMO.TimelineInfo.Event), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityMMO.TimelineInfo.Event), L, null, 3, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AfterAdd", UnityMMO.TimelineInfo.Event.AfterAdd);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "StartPlay", UnityMMO.TimelineInfo.Event.StartPlay);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityMMO.TimelineInfo.Event), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityMMOTimelineInfoEvent(L, (UnityMMO.TimelineInfo.Event)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "AfterAdd"))
                {
                    translator.PushUnityMMOTimelineInfoEvent(L, UnityMMO.TimelineInfo.Event.AfterAdd);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "StartPlay"))
                {
                    translator.PushUnityMMOTimelineInfoEvent(L, UnityMMO.TimelineInfo.Event.StartPlay);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityMMO.TimelineInfo.Event!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityMMO.TimelineInfo.Event! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityMMONameboardColorStyleWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityMMO.Nameboard.ColorStyle), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityMMO.Nameboard.ColorStyle), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityMMO.Nameboard.ColorStyle), L, null, 5, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Green", UnityMMO.Nameboard.ColorStyle.Green);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Red", UnityMMO.Nameboard.ColorStyle.Red);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Blue", UnityMMO.Nameboard.ColorStyle.Blue);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", UnityMMO.Nameboard.ColorStyle.None);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityMMO.Nameboard.ColorStyle), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityMMONameboardColorStyle(L, (UnityMMO.Nameboard.ColorStyle)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "Green"))
                {
                    translator.PushUnityMMONameboardColorStyle(L, UnityMMO.Nameboard.ColorStyle.Green);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Red"))
                {
                    translator.PushUnityMMONameboardColorStyle(L, UnityMMO.Nameboard.ColorStyle.Red);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Blue"))
                {
                    translator.PushUnityMMONameboardColorStyle(L, UnityMMO.Nameboard.ColorStyle.Blue);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushUnityMMONameboardColorStyle(L, UnityMMO.Nameboard.ColorStyle.None);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityMMO.Nameboard.ColorStyle!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityMMO.Nameboard.ColorStyle! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
}