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
    
    public class UnityEngineUIScrollRectMovementTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.UI.ScrollRect.MovementType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.UI.ScrollRect.MovementType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.UI.ScrollRect.MovementType), L, null, 4, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Unrestricted", UnityEngine.UI.ScrollRect.MovementType.Unrestricted);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Elastic", UnityEngine.UI.ScrollRect.MovementType.Elastic);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Clamped", UnityEngine.UI.ScrollRect.MovementType.Clamped);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.UI.ScrollRect.MovementType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineUIScrollRectMovementType(L, (UnityEngine.UI.ScrollRect.MovementType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "Unrestricted"))
                {
                    translator.PushUnityEngineUIScrollRectMovementType(L, UnityEngine.UI.ScrollRect.MovementType.Unrestricted);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Elastic"))
                {
                    translator.PushUnityEngineUIScrollRectMovementType(L, UnityEngine.UI.ScrollRect.MovementType.Elastic);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Clamped"))
                {
                    translator.PushUnityEngineUIScrollRectMovementType(L, UnityEngine.UI.ScrollRect.MovementType.Clamped);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.UI.ScrollRect.MovementType!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.UI.ScrollRect.MovementType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineUIScrollRectScrollbarVisibilityWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.UI.ScrollRect.ScrollbarVisibility), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.UI.ScrollRect.ScrollbarVisibility), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.UI.ScrollRect.ScrollbarVisibility), L, null, 4, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Permanent", UnityEngine.UI.ScrollRect.ScrollbarVisibility.Permanent);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AutoHide", UnityEngine.UI.ScrollRect.ScrollbarVisibility.AutoHide);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AutoHideAndExpandViewport", UnityEngine.UI.ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.UI.ScrollRect.ScrollbarVisibility), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineUIScrollRectScrollbarVisibility(L, (UnityEngine.UI.ScrollRect.ScrollbarVisibility)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "Permanent"))
                {
                    translator.PushUnityEngineUIScrollRectScrollbarVisibility(L, UnityEngine.UI.ScrollRect.ScrollbarVisibility.Permanent);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "AutoHide"))
                {
                    translator.PushUnityEngineUIScrollRectScrollbarVisibility(L, UnityEngine.UI.ScrollRect.ScrollbarVisibility.AutoHide);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "AutoHideAndExpandViewport"))
                {
                    translator.PushUnityEngineUIScrollRectScrollbarVisibility(L, UnityEngine.UI.ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.UI.ScrollRect.ScrollbarVisibility!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.UI.ScrollRect.ScrollbarVisibility! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineUIImageTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.UI.Image.Type), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.UI.Image.Type), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.UI.Image.Type), L, null, 5, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Simple", UnityEngine.UI.Image.Type.Simple);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Sliced", UnityEngine.UI.Image.Type.Sliced);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Tiled", UnityEngine.UI.Image.Type.Tiled);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Filled", UnityEngine.UI.Image.Type.Filled);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.UI.Image.Type), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineUIImageType(L, (UnityEngine.UI.Image.Type)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "Simple"))
                {
                    translator.PushUnityEngineUIImageType(L, UnityEngine.UI.Image.Type.Simple);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Sliced"))
                {
                    translator.PushUnityEngineUIImageType(L, UnityEngine.UI.Image.Type.Sliced);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Tiled"))
                {
                    translator.PushUnityEngineUIImageType(L, UnityEngine.UI.Image.Type.Tiled);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Filled"))
                {
                    translator.PushUnityEngineUIImageType(L, UnityEngine.UI.Image.Type.Filled);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.UI.Image.Type!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.UI.Image.Type! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineUIImageFillMethodWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.UI.Image.FillMethod), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.UI.Image.FillMethod), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.UI.Image.FillMethod), L, null, 6, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Horizontal", UnityEngine.UI.Image.FillMethod.Horizontal);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Vertical", UnityEngine.UI.Image.FillMethod.Vertical);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Radial90", UnityEngine.UI.Image.FillMethod.Radial90);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Radial180", UnityEngine.UI.Image.FillMethod.Radial180);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Radial360", UnityEngine.UI.Image.FillMethod.Radial360);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.UI.Image.FillMethod), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineUIImageFillMethod(L, (UnityEngine.UI.Image.FillMethod)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "Horizontal"))
                {
                    translator.PushUnityEngineUIImageFillMethod(L, UnityEngine.UI.Image.FillMethod.Horizontal);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Vertical"))
                {
                    translator.PushUnityEngineUIImageFillMethod(L, UnityEngine.UI.Image.FillMethod.Vertical);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Radial90"))
                {
                    translator.PushUnityEngineUIImageFillMethod(L, UnityEngine.UI.Image.FillMethod.Radial90);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Radial180"))
                {
                    translator.PushUnityEngineUIImageFillMethod(L, UnityEngine.UI.Image.FillMethod.Radial180);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Radial360"))
                {
                    translator.PushUnityEngineUIImageFillMethod(L, UnityEngine.UI.Image.FillMethod.Radial360);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.UI.Image.FillMethod!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.UI.Image.FillMethod! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineUIImageOriginHorizontalWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.UI.Image.OriginHorizontal), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.UI.Image.OriginHorizontal), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.UI.Image.OriginHorizontal), L, null, 3, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Left", UnityEngine.UI.Image.OriginHorizontal.Left);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Right", UnityEngine.UI.Image.OriginHorizontal.Right);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.UI.Image.OriginHorizontal), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineUIImageOriginHorizontal(L, (UnityEngine.UI.Image.OriginHorizontal)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "Left"))
                {
                    translator.PushUnityEngineUIImageOriginHorizontal(L, UnityEngine.UI.Image.OriginHorizontal.Left);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Right"))
                {
                    translator.PushUnityEngineUIImageOriginHorizontal(L, UnityEngine.UI.Image.OriginHorizontal.Right);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.UI.Image.OriginHorizontal!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.UI.Image.OriginHorizontal! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineUIImageOriginVerticalWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.UI.Image.OriginVertical), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.UI.Image.OriginVertical), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.UI.Image.OriginVertical), L, null, 3, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Bottom", UnityEngine.UI.Image.OriginVertical.Bottom);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Top", UnityEngine.UI.Image.OriginVertical.Top);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.UI.Image.OriginVertical), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineUIImageOriginVertical(L, (UnityEngine.UI.Image.OriginVertical)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "Bottom"))
                {
                    translator.PushUnityEngineUIImageOriginVertical(L, UnityEngine.UI.Image.OriginVertical.Bottom);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Top"))
                {
                    translator.PushUnityEngineUIImageOriginVertical(L, UnityEngine.UI.Image.OriginVertical.Top);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.UI.Image.OriginVertical!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.UI.Image.OriginVertical! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineUIImageOrigin90Wrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.UI.Image.Origin90), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.UI.Image.Origin90), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.UI.Image.Origin90), L, null, 5, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BottomLeft", UnityEngine.UI.Image.Origin90.BottomLeft);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TopLeft", UnityEngine.UI.Image.Origin90.TopLeft);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TopRight", UnityEngine.UI.Image.Origin90.TopRight);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BottomRight", UnityEngine.UI.Image.Origin90.BottomRight);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.UI.Image.Origin90), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineUIImageOrigin90(L, (UnityEngine.UI.Image.Origin90)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "BottomLeft"))
                {
                    translator.PushUnityEngineUIImageOrigin90(L, UnityEngine.UI.Image.Origin90.BottomLeft);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TopLeft"))
                {
                    translator.PushUnityEngineUIImageOrigin90(L, UnityEngine.UI.Image.Origin90.TopLeft);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TopRight"))
                {
                    translator.PushUnityEngineUIImageOrigin90(L, UnityEngine.UI.Image.Origin90.TopRight);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "BottomRight"))
                {
                    translator.PushUnityEngineUIImageOrigin90(L, UnityEngine.UI.Image.Origin90.BottomRight);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.UI.Image.Origin90!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.UI.Image.Origin90! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineUIImageOrigin180Wrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.UI.Image.Origin180), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.UI.Image.Origin180), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.UI.Image.Origin180), L, null, 5, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Bottom", UnityEngine.UI.Image.Origin180.Bottom);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Left", UnityEngine.UI.Image.Origin180.Left);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Top", UnityEngine.UI.Image.Origin180.Top);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Right", UnityEngine.UI.Image.Origin180.Right);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.UI.Image.Origin180), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineUIImageOrigin180(L, (UnityEngine.UI.Image.Origin180)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "Bottom"))
                {
                    translator.PushUnityEngineUIImageOrigin180(L, UnityEngine.UI.Image.Origin180.Bottom);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Left"))
                {
                    translator.PushUnityEngineUIImageOrigin180(L, UnityEngine.UI.Image.Origin180.Left);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Top"))
                {
                    translator.PushUnityEngineUIImageOrigin180(L, UnityEngine.UI.Image.Origin180.Top);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Right"))
                {
                    translator.PushUnityEngineUIImageOrigin180(L, UnityEngine.UI.Image.Origin180.Right);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.UI.Image.Origin180!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.UI.Image.Origin180! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineUIImageOrigin360Wrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.UI.Image.Origin360), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.UI.Image.Origin360), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.UI.Image.Origin360), L, null, 5, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Bottom", UnityEngine.UI.Image.Origin360.Bottom);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Right", UnityEngine.UI.Image.Origin360.Right);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Top", UnityEngine.UI.Image.Origin360.Top);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Left", UnityEngine.UI.Image.Origin360.Left);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.UI.Image.Origin360), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineUIImageOrigin360(L, (UnityEngine.UI.Image.Origin360)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "Bottom"))
                {
                    translator.PushUnityEngineUIImageOrigin360(L, UnityEngine.UI.Image.Origin360.Bottom);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Right"))
                {
                    translator.PushUnityEngineUIImageOrigin360(L, UnityEngine.UI.Image.Origin360.Right);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Top"))
                {
                    translator.PushUnityEngineUIImageOrigin360(L, UnityEngine.UI.Image.Origin360.Top);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Left"))
                {
                    translator.PushUnityEngineUIImageOrigin360(L, UnityEngine.UI.Image.Origin360.Left);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.UI.Image.Origin360!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.UI.Image.Origin360! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineKeyCodeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.KeyCode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.KeyCode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.KeyCode), L, null, 327, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", UnityEngine.KeyCode.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Backspace", UnityEngine.KeyCode.Backspace);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Delete", UnityEngine.KeyCode.Delete);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Tab", UnityEngine.KeyCode.Tab);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Clear", UnityEngine.KeyCode.Clear);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Return", UnityEngine.KeyCode.Return);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Pause", UnityEngine.KeyCode.Pause);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Escape", UnityEngine.KeyCode.Escape);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Space", UnityEngine.KeyCode.Space);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Keypad0", UnityEngine.KeyCode.Keypad0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Keypad1", UnityEngine.KeyCode.Keypad1);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Keypad2", UnityEngine.KeyCode.Keypad2);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Keypad3", UnityEngine.KeyCode.Keypad3);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Keypad4", UnityEngine.KeyCode.Keypad4);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Keypad5", UnityEngine.KeyCode.Keypad5);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Keypad6", UnityEngine.KeyCode.Keypad6);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Keypad7", UnityEngine.KeyCode.Keypad7);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Keypad8", UnityEngine.KeyCode.Keypad8);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Keypad9", UnityEngine.KeyCode.Keypad9);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "KeypadPeriod", UnityEngine.KeyCode.KeypadPeriod);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "KeypadDivide", UnityEngine.KeyCode.KeypadDivide);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "KeypadMultiply", UnityEngine.KeyCode.KeypadMultiply);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "KeypadMinus", UnityEngine.KeyCode.KeypadMinus);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "KeypadPlus", UnityEngine.KeyCode.KeypadPlus);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "KeypadEnter", UnityEngine.KeyCode.KeypadEnter);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "KeypadEquals", UnityEngine.KeyCode.KeypadEquals);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UpArrow", UnityEngine.KeyCode.UpArrow);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "DownArrow", UnityEngine.KeyCode.DownArrow);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RightArrow", UnityEngine.KeyCode.RightArrow);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeftArrow", UnityEngine.KeyCode.LeftArrow);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Insert", UnityEngine.KeyCode.Insert);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Home", UnityEngine.KeyCode.Home);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "End", UnityEngine.KeyCode.End);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PageUp", UnityEngine.KeyCode.PageUp);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PageDown", UnityEngine.KeyCode.PageDown);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "F1", UnityEngine.KeyCode.F1);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "F2", UnityEngine.KeyCode.F2);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "F3", UnityEngine.KeyCode.F3);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "F4", UnityEngine.KeyCode.F4);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "F5", UnityEngine.KeyCode.F5);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "F6", UnityEngine.KeyCode.F6);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "F7", UnityEngine.KeyCode.F7);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "F8", UnityEngine.KeyCode.F8);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "F9", UnityEngine.KeyCode.F9);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "F10", UnityEngine.KeyCode.F10);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "F11", UnityEngine.KeyCode.F11);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "F12", UnityEngine.KeyCode.F12);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "F13", UnityEngine.KeyCode.F13);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "F14", UnityEngine.KeyCode.F14);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "F15", UnityEngine.KeyCode.F15);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Alpha0", UnityEngine.KeyCode.Alpha0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Alpha1", UnityEngine.KeyCode.Alpha1);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Alpha2", UnityEngine.KeyCode.Alpha2);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Alpha3", UnityEngine.KeyCode.Alpha3);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Alpha4", UnityEngine.KeyCode.Alpha4);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Alpha5", UnityEngine.KeyCode.Alpha5);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Alpha6", UnityEngine.KeyCode.Alpha6);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Alpha7", UnityEngine.KeyCode.Alpha7);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Alpha8", UnityEngine.KeyCode.Alpha8);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Alpha9", UnityEngine.KeyCode.Alpha9);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Exclaim", UnityEngine.KeyCode.Exclaim);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "DoubleQuote", UnityEngine.KeyCode.DoubleQuote);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Hash", UnityEngine.KeyCode.Hash);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Dollar", UnityEngine.KeyCode.Dollar);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Percent", UnityEngine.KeyCode.Percent);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Ampersand", UnityEngine.KeyCode.Ampersand);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Quote", UnityEngine.KeyCode.Quote);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeftParen", UnityEngine.KeyCode.LeftParen);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RightParen", UnityEngine.KeyCode.RightParen);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Asterisk", UnityEngine.KeyCode.Asterisk);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Plus", UnityEngine.KeyCode.Plus);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Comma", UnityEngine.KeyCode.Comma);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Minus", UnityEngine.KeyCode.Minus);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Period", UnityEngine.KeyCode.Period);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Slash", UnityEngine.KeyCode.Slash);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Colon", UnityEngine.KeyCode.Colon);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Semicolon", UnityEngine.KeyCode.Semicolon);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Less", UnityEngine.KeyCode.Less);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Equals", UnityEngine.KeyCode.Equals);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Greater", UnityEngine.KeyCode.Greater);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Question", UnityEngine.KeyCode.Question);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "At", UnityEngine.KeyCode.At);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeftBracket", UnityEngine.KeyCode.LeftBracket);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Backslash", UnityEngine.KeyCode.Backslash);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RightBracket", UnityEngine.KeyCode.RightBracket);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Caret", UnityEngine.KeyCode.Caret);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Underscore", UnityEngine.KeyCode.Underscore);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BackQuote", UnityEngine.KeyCode.BackQuote);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "A", UnityEngine.KeyCode.A);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "B", UnityEngine.KeyCode.B);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "C", UnityEngine.KeyCode.C);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "D", UnityEngine.KeyCode.D);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "E", UnityEngine.KeyCode.E);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "F", UnityEngine.KeyCode.F);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "G", UnityEngine.KeyCode.G);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "H", UnityEngine.KeyCode.H);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "I", UnityEngine.KeyCode.I);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "J", UnityEngine.KeyCode.J);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "K", UnityEngine.KeyCode.K);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "L", UnityEngine.KeyCode.L);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "M", UnityEngine.KeyCode.M);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "N", UnityEngine.KeyCode.N);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "O", UnityEngine.KeyCode.O);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "P", UnityEngine.KeyCode.P);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Q", UnityEngine.KeyCode.Q);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "R", UnityEngine.KeyCode.R);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "S", UnityEngine.KeyCode.S);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "T", UnityEngine.KeyCode.T);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "U", UnityEngine.KeyCode.U);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "V", UnityEngine.KeyCode.V);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "W", UnityEngine.KeyCode.W);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "X", UnityEngine.KeyCode.X);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Y", UnityEngine.KeyCode.Y);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Z", UnityEngine.KeyCode.Z);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeftCurlyBracket", UnityEngine.KeyCode.LeftCurlyBracket);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Pipe", UnityEngine.KeyCode.Pipe);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RightCurlyBracket", UnityEngine.KeyCode.RightCurlyBracket);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Tilde", UnityEngine.KeyCode.Tilde);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Numlock", UnityEngine.KeyCode.Numlock);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CapsLock", UnityEngine.KeyCode.CapsLock);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ScrollLock", UnityEngine.KeyCode.ScrollLock);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RightShift", UnityEngine.KeyCode.RightShift);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeftShift", UnityEngine.KeyCode.LeftShift);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RightControl", UnityEngine.KeyCode.RightControl);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeftControl", UnityEngine.KeyCode.LeftControl);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RightAlt", UnityEngine.KeyCode.RightAlt);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeftAlt", UnityEngine.KeyCode.LeftAlt);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeftCommand", UnityEngine.KeyCode.LeftCommand);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeftApple", UnityEngine.KeyCode.LeftApple);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeftWindows", UnityEngine.KeyCode.LeftWindows);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RightCommand", UnityEngine.KeyCode.RightCommand);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RightApple", UnityEngine.KeyCode.RightApple);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RightWindows", UnityEngine.KeyCode.RightWindows);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AltGr", UnityEngine.KeyCode.AltGr);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Help", UnityEngine.KeyCode.Help);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Print", UnityEngine.KeyCode.Print);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SysReq", UnityEngine.KeyCode.SysReq);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Break", UnityEngine.KeyCode.Break);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Menu", UnityEngine.KeyCode.Menu);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Mouse0", UnityEngine.KeyCode.Mouse0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Mouse1", UnityEngine.KeyCode.Mouse1);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Mouse2", UnityEngine.KeyCode.Mouse2);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Mouse3", UnityEngine.KeyCode.Mouse3);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Mouse4", UnityEngine.KeyCode.Mouse4);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Mouse5", UnityEngine.KeyCode.Mouse5);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Mouse6", UnityEngine.KeyCode.Mouse6);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton0", UnityEngine.KeyCode.JoystickButton0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton1", UnityEngine.KeyCode.JoystickButton1);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton2", UnityEngine.KeyCode.JoystickButton2);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton3", UnityEngine.KeyCode.JoystickButton3);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton4", UnityEngine.KeyCode.JoystickButton4);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton5", UnityEngine.KeyCode.JoystickButton5);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton6", UnityEngine.KeyCode.JoystickButton6);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton7", UnityEngine.KeyCode.JoystickButton7);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton8", UnityEngine.KeyCode.JoystickButton8);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton9", UnityEngine.KeyCode.JoystickButton9);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton10", UnityEngine.KeyCode.JoystickButton10);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton11", UnityEngine.KeyCode.JoystickButton11);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton12", UnityEngine.KeyCode.JoystickButton12);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton13", UnityEngine.KeyCode.JoystickButton13);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton14", UnityEngine.KeyCode.JoystickButton14);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton15", UnityEngine.KeyCode.JoystickButton15);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton16", UnityEngine.KeyCode.JoystickButton16);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton17", UnityEngine.KeyCode.JoystickButton17);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton18", UnityEngine.KeyCode.JoystickButton18);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JoystickButton19", UnityEngine.KeyCode.JoystickButton19);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button0", UnityEngine.KeyCode.Joystick1Button0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button1", UnityEngine.KeyCode.Joystick1Button1);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button2", UnityEngine.KeyCode.Joystick1Button2);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button3", UnityEngine.KeyCode.Joystick1Button3);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button4", UnityEngine.KeyCode.Joystick1Button4);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button5", UnityEngine.KeyCode.Joystick1Button5);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button6", UnityEngine.KeyCode.Joystick1Button6);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button7", UnityEngine.KeyCode.Joystick1Button7);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button8", UnityEngine.KeyCode.Joystick1Button8);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button9", UnityEngine.KeyCode.Joystick1Button9);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button10", UnityEngine.KeyCode.Joystick1Button10);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button11", UnityEngine.KeyCode.Joystick1Button11);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button12", UnityEngine.KeyCode.Joystick1Button12);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button13", UnityEngine.KeyCode.Joystick1Button13);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button14", UnityEngine.KeyCode.Joystick1Button14);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button15", UnityEngine.KeyCode.Joystick1Button15);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button16", UnityEngine.KeyCode.Joystick1Button16);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button17", UnityEngine.KeyCode.Joystick1Button17);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button18", UnityEngine.KeyCode.Joystick1Button18);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick1Button19", UnityEngine.KeyCode.Joystick1Button19);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button0", UnityEngine.KeyCode.Joystick2Button0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button1", UnityEngine.KeyCode.Joystick2Button1);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button2", UnityEngine.KeyCode.Joystick2Button2);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button3", UnityEngine.KeyCode.Joystick2Button3);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button4", UnityEngine.KeyCode.Joystick2Button4);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button5", UnityEngine.KeyCode.Joystick2Button5);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button6", UnityEngine.KeyCode.Joystick2Button6);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button7", UnityEngine.KeyCode.Joystick2Button7);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button8", UnityEngine.KeyCode.Joystick2Button8);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button9", UnityEngine.KeyCode.Joystick2Button9);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button10", UnityEngine.KeyCode.Joystick2Button10);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button11", UnityEngine.KeyCode.Joystick2Button11);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button12", UnityEngine.KeyCode.Joystick2Button12);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button13", UnityEngine.KeyCode.Joystick2Button13);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button14", UnityEngine.KeyCode.Joystick2Button14);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button15", UnityEngine.KeyCode.Joystick2Button15);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button16", UnityEngine.KeyCode.Joystick2Button16);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button17", UnityEngine.KeyCode.Joystick2Button17);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button18", UnityEngine.KeyCode.Joystick2Button18);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick2Button19", UnityEngine.KeyCode.Joystick2Button19);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button0", UnityEngine.KeyCode.Joystick3Button0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button1", UnityEngine.KeyCode.Joystick3Button1);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button2", UnityEngine.KeyCode.Joystick3Button2);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button3", UnityEngine.KeyCode.Joystick3Button3);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button4", UnityEngine.KeyCode.Joystick3Button4);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button5", UnityEngine.KeyCode.Joystick3Button5);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button6", UnityEngine.KeyCode.Joystick3Button6);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button7", UnityEngine.KeyCode.Joystick3Button7);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button8", UnityEngine.KeyCode.Joystick3Button8);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button9", UnityEngine.KeyCode.Joystick3Button9);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button10", UnityEngine.KeyCode.Joystick3Button10);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button11", UnityEngine.KeyCode.Joystick3Button11);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button12", UnityEngine.KeyCode.Joystick3Button12);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button13", UnityEngine.KeyCode.Joystick3Button13);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button14", UnityEngine.KeyCode.Joystick3Button14);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button15", UnityEngine.KeyCode.Joystick3Button15);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button16", UnityEngine.KeyCode.Joystick3Button16);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button17", UnityEngine.KeyCode.Joystick3Button17);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button18", UnityEngine.KeyCode.Joystick3Button18);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick3Button19", UnityEngine.KeyCode.Joystick3Button19);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button0", UnityEngine.KeyCode.Joystick4Button0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button1", UnityEngine.KeyCode.Joystick4Button1);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button2", UnityEngine.KeyCode.Joystick4Button2);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button3", UnityEngine.KeyCode.Joystick4Button3);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button4", UnityEngine.KeyCode.Joystick4Button4);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button5", UnityEngine.KeyCode.Joystick4Button5);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button6", UnityEngine.KeyCode.Joystick4Button6);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button7", UnityEngine.KeyCode.Joystick4Button7);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button8", UnityEngine.KeyCode.Joystick4Button8);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button9", UnityEngine.KeyCode.Joystick4Button9);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button10", UnityEngine.KeyCode.Joystick4Button10);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button11", UnityEngine.KeyCode.Joystick4Button11);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button12", UnityEngine.KeyCode.Joystick4Button12);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button13", UnityEngine.KeyCode.Joystick4Button13);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button14", UnityEngine.KeyCode.Joystick4Button14);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button15", UnityEngine.KeyCode.Joystick4Button15);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button16", UnityEngine.KeyCode.Joystick4Button16);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button17", UnityEngine.KeyCode.Joystick4Button17);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button18", UnityEngine.KeyCode.Joystick4Button18);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick4Button19", UnityEngine.KeyCode.Joystick4Button19);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button0", UnityEngine.KeyCode.Joystick5Button0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button1", UnityEngine.KeyCode.Joystick5Button1);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button2", UnityEngine.KeyCode.Joystick5Button2);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button3", UnityEngine.KeyCode.Joystick5Button3);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button4", UnityEngine.KeyCode.Joystick5Button4);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button5", UnityEngine.KeyCode.Joystick5Button5);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button6", UnityEngine.KeyCode.Joystick5Button6);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button7", UnityEngine.KeyCode.Joystick5Button7);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button8", UnityEngine.KeyCode.Joystick5Button8);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button9", UnityEngine.KeyCode.Joystick5Button9);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button10", UnityEngine.KeyCode.Joystick5Button10);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button11", UnityEngine.KeyCode.Joystick5Button11);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button12", UnityEngine.KeyCode.Joystick5Button12);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button13", UnityEngine.KeyCode.Joystick5Button13);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button14", UnityEngine.KeyCode.Joystick5Button14);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button15", UnityEngine.KeyCode.Joystick5Button15);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button16", UnityEngine.KeyCode.Joystick5Button16);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button17", UnityEngine.KeyCode.Joystick5Button17);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button18", UnityEngine.KeyCode.Joystick5Button18);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick5Button19", UnityEngine.KeyCode.Joystick5Button19);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button0", UnityEngine.KeyCode.Joystick6Button0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button1", UnityEngine.KeyCode.Joystick6Button1);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button2", UnityEngine.KeyCode.Joystick6Button2);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button3", UnityEngine.KeyCode.Joystick6Button3);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button4", UnityEngine.KeyCode.Joystick6Button4);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button5", UnityEngine.KeyCode.Joystick6Button5);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button6", UnityEngine.KeyCode.Joystick6Button6);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button7", UnityEngine.KeyCode.Joystick6Button7);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button8", UnityEngine.KeyCode.Joystick6Button8);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button9", UnityEngine.KeyCode.Joystick6Button9);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button10", UnityEngine.KeyCode.Joystick6Button10);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button11", UnityEngine.KeyCode.Joystick6Button11);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button12", UnityEngine.KeyCode.Joystick6Button12);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button13", UnityEngine.KeyCode.Joystick6Button13);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button14", UnityEngine.KeyCode.Joystick6Button14);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button15", UnityEngine.KeyCode.Joystick6Button15);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button16", UnityEngine.KeyCode.Joystick6Button16);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button17", UnityEngine.KeyCode.Joystick6Button17);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button18", UnityEngine.KeyCode.Joystick6Button18);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick6Button19", UnityEngine.KeyCode.Joystick6Button19);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button0", UnityEngine.KeyCode.Joystick7Button0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button1", UnityEngine.KeyCode.Joystick7Button1);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button2", UnityEngine.KeyCode.Joystick7Button2);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button3", UnityEngine.KeyCode.Joystick7Button3);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button4", UnityEngine.KeyCode.Joystick7Button4);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button5", UnityEngine.KeyCode.Joystick7Button5);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button6", UnityEngine.KeyCode.Joystick7Button6);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button7", UnityEngine.KeyCode.Joystick7Button7);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button8", UnityEngine.KeyCode.Joystick7Button8);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button9", UnityEngine.KeyCode.Joystick7Button9);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button10", UnityEngine.KeyCode.Joystick7Button10);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button11", UnityEngine.KeyCode.Joystick7Button11);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button12", UnityEngine.KeyCode.Joystick7Button12);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button13", UnityEngine.KeyCode.Joystick7Button13);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button14", UnityEngine.KeyCode.Joystick7Button14);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button15", UnityEngine.KeyCode.Joystick7Button15);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button16", UnityEngine.KeyCode.Joystick7Button16);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button17", UnityEngine.KeyCode.Joystick7Button17);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button18", UnityEngine.KeyCode.Joystick7Button18);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick7Button19", UnityEngine.KeyCode.Joystick7Button19);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button0", UnityEngine.KeyCode.Joystick8Button0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button1", UnityEngine.KeyCode.Joystick8Button1);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button2", UnityEngine.KeyCode.Joystick8Button2);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button3", UnityEngine.KeyCode.Joystick8Button3);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button4", UnityEngine.KeyCode.Joystick8Button4);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button5", UnityEngine.KeyCode.Joystick8Button5);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button6", UnityEngine.KeyCode.Joystick8Button6);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button7", UnityEngine.KeyCode.Joystick8Button7);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button8", UnityEngine.KeyCode.Joystick8Button8);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button9", UnityEngine.KeyCode.Joystick8Button9);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button10", UnityEngine.KeyCode.Joystick8Button10);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button11", UnityEngine.KeyCode.Joystick8Button11);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button12", UnityEngine.KeyCode.Joystick8Button12);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button13", UnityEngine.KeyCode.Joystick8Button13);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button14", UnityEngine.KeyCode.Joystick8Button14);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button15", UnityEngine.KeyCode.Joystick8Button15);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button16", UnityEngine.KeyCode.Joystick8Button16);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button17", UnityEngine.KeyCode.Joystick8Button17);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button18", UnityEngine.KeyCode.Joystick8Button18);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Joystick8Button19", UnityEngine.KeyCode.Joystick8Button19);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.KeyCode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineKeyCode(L, (UnityEngine.KeyCode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Backspace"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Backspace);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Delete"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Delete);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Tab"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Tab);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Clear"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Clear);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Return"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Return);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Pause"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Pause);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Escape"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Escape);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Space"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Space);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Keypad0"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Keypad0);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Keypad1"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Keypad1);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Keypad2"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Keypad2);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Keypad3"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Keypad3);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Keypad4"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Keypad4);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Keypad5"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Keypad5);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Keypad6"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Keypad6);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Keypad7"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Keypad7);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Keypad8"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Keypad8);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Keypad9"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Keypad9);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "KeypadPeriod"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.KeypadPeriod);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "KeypadDivide"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.KeypadDivide);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "KeypadMultiply"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.KeypadMultiply);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "KeypadMinus"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.KeypadMinus);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "KeypadPlus"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.KeypadPlus);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "KeypadEnter"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.KeypadEnter);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "KeypadEquals"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.KeypadEquals);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "UpArrow"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.UpArrow);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "DownArrow"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.DownArrow);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RightArrow"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.RightArrow);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LeftArrow"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.LeftArrow);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Insert"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Insert);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Home"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Home);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "End"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.End);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PageUp"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.PageUp);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PageDown"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.PageDown);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "F1"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.F1);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "F2"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.F2);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "F3"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.F3);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "F4"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.F4);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "F5"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.F5);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "F6"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.F6);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "F7"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.F7);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "F8"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.F8);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "F9"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.F9);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "F10"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.F10);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "F11"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.F11);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "F12"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.F12);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "F13"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.F13);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "F14"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.F14);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "F15"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.F15);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Alpha0"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Alpha0);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Alpha1"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Alpha1);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Alpha2"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Alpha2);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Alpha3"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Alpha3);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Alpha4"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Alpha4);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Alpha5"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Alpha5);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Alpha6"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Alpha6);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Alpha7"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Alpha7);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Alpha8"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Alpha8);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Alpha9"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Alpha9);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Exclaim"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Exclaim);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "DoubleQuote"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.DoubleQuote);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Hash"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Hash);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Dollar"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Dollar);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Percent"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Percent);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Ampersand"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Ampersand);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Quote"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Quote);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LeftParen"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.LeftParen);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RightParen"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.RightParen);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Asterisk"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Asterisk);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Plus"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Plus);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Comma"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Comma);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Minus"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Minus);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Period"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Period);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Slash"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Slash);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Colon"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Colon);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Semicolon"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Semicolon);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Less"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Less);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Equals"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Equals);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Greater"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Greater);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Question"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Question);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "At"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.At);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LeftBracket"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.LeftBracket);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Backslash"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Backslash);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RightBracket"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.RightBracket);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Caret"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Caret);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Underscore"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Underscore);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "BackQuote"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.BackQuote);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "A"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.A);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "B"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.B);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "C"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.C);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "D"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.D);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "E"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.E);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "F"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.F);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "G"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.G);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "H"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.H);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "I"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.I);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "J"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.J);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "K"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.K);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "L"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.L);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "M"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.M);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "N"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.N);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "O"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.O);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "P"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.P);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Q"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Q);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "R"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.R);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "S"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.S);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "T"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.T);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "U"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.U);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "V"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.V);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "W"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.W);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "X"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.X);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Y"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Y);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Z"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Z);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LeftCurlyBracket"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.LeftCurlyBracket);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Pipe"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Pipe);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RightCurlyBracket"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.RightCurlyBracket);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Tilde"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Tilde);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Numlock"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Numlock);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "CapsLock"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.CapsLock);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "ScrollLock"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.ScrollLock);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RightShift"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.RightShift);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LeftShift"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.LeftShift);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RightControl"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.RightControl);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LeftControl"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.LeftControl);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RightAlt"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.RightAlt);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LeftAlt"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.LeftAlt);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LeftCommand"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.LeftCommand);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LeftApple"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.LeftApple);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LeftWindows"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.LeftWindows);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RightCommand"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.RightCommand);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RightApple"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.RightApple);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RightWindows"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.RightWindows);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "AltGr"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.AltGr);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Help"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Help);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Print"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Print);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "SysReq"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.SysReq);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Break"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Break);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Menu"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Menu);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Mouse0"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Mouse0);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Mouse1"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Mouse1);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Mouse2"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Mouse2);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Mouse3"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Mouse3);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Mouse4"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Mouse4);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Mouse5"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Mouse5);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Mouse6"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Mouse6);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton0"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton0);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton1"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton1);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton2"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton2);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton3"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton3);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton4"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton4);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton5"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton5);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton6"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton6);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton7"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton7);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton8"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton8);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton9"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton9);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton10"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton10);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton11"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton11);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton12"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton12);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton13"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton13);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton14"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton14);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton15"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton15);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton16"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton16);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton17"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton17);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton18"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton18);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JoystickButton19"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.JoystickButton19);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button0"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button0);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button1"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button1);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button2"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button2);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button3"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button3);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button4"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button4);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button5"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button5);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button6"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button6);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button7"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button7);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button8"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button8);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button9"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button9);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button10"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button10);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button11"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button11);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button12"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button12);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button13"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button13);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button14"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button14);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button15"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button15);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button16"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button16);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button17"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button17);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button18"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button18);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick1Button19"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick1Button19);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button0"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button0);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button1"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button1);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button2"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button2);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button3"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button3);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button4"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button4);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button5"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button5);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button6"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button6);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button7"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button7);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button8"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button8);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button9"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button9);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button10"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button10);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button11"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button11);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button12"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button12);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button13"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button13);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button14"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button14);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button15"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button15);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button16"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button16);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button17"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button17);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button18"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button18);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick2Button19"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick2Button19);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button0"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button0);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button1"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button1);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button2"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button2);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button3"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button3);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button4"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button4);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button5"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button5);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button6"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button6);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button7"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button7);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button8"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button8);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button9"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button9);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button10"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button10);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button11"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button11);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button12"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button12);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button13"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button13);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button14"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button14);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button15"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button15);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button16"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button16);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button17"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button17);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button18"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button18);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick3Button19"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick3Button19);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button0"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button0);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button1"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button1);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button2"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button2);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button3"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button3);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button4"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button4);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button5"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button5);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button6"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button6);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button7"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button7);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button8"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button8);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button9"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button9);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button10"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button10);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button11"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button11);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button12"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button12);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button13"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button13);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button14"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button14);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button15"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button15);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button16"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button16);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button17"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button17);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button18"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button18);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick4Button19"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick4Button19);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button0"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button0);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button1"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button1);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button2"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button2);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button3"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button3);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button4"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button4);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button5"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button5);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button6"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button6);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button7"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button7);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button8"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button8);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button9"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button9);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button10"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button10);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button11"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button11);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button12"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button12);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button13"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button13);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button14"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button14);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button15"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button15);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button16"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button16);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button17"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button17);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button18"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button18);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick5Button19"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick5Button19);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button0"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button0);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button1"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button1);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button2"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button2);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button3"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button3);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button4"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button4);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button5"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button5);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button6"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button6);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button7"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button7);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button8"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button8);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button9"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button9);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button10"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button10);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button11"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button11);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button12"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button12);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button13"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button13);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button14"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button14);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button15"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button15);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button16"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button16);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button17"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button17);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button18"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button18);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick6Button19"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick6Button19);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button0"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button0);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button1"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button1);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button2"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button2);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button3"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button3);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button4"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button4);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button5"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button5);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button6"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button6);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button7"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button7);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button8"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button8);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button9"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button9);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button10"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button10);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button11"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button11);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button12"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button12);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button13"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button13);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button14"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button14);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button15"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button15);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button16"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button16);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button17"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button17);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button18"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button18);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick7Button19"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick7Button19);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button0"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button0);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button1"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button1);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button2"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button2);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button3"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button3);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button4"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button4);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button5"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button5);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button6"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button6);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button7"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button7);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button8"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button8);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button9"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button9);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button10"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button10);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button11"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button11);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button12"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button12);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button13"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button13);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button14"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button14);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button15"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button15);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button16"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button16);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button17"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button17);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button18"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button18);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Joystick8Button19"))
                {
                    translator.PushUnityEngineKeyCode(L, UnityEngine.KeyCode.Joystick8Button19);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.KeyCode!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.KeyCode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class XLuaFrameworkDisTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(XLuaFramework.DisType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(XLuaFramework.DisType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(XLuaFramework.DisType), L, null, 3, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Exception", XLuaFramework.DisType.Exception);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Disconnect", XLuaFramework.DisType.Disconnect);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(XLuaFramework.DisType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushXLuaFrameworkDisType(L, (XLuaFramework.DisType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "Exception"))
                {
                    translator.PushXLuaFrameworkDisType(L, XLuaFramework.DisType.Exception);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Disconnect"))
                {
                    translator.PushXLuaFrameworkDisType(L, XLuaFramework.DisType.Disconnect);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for XLuaFramework.DisType!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for XLuaFramework.DisType! Expect number or string, got + " + lua_type);
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
			
			Utils.BeginClassRegister(typeof(UnityMMO.SceneInfoKey), L, null, 10, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", UnityMMO.SceneInfoKey.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "EnterView", UnityMMO.SceneInfoKey.EnterView);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeaveView", UnityMMO.SceneInfoKey.LeaveView);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PosChange", UnityMMO.SceneInfoKey.PosChange);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TargetPos", UnityMMO.SceneInfoKey.TargetPos);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JumpState", UnityMMO.SceneInfoKey.JumpState);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "HPChange", UnityMMO.SceneInfoKey.HPChange);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NPCState", UnityMMO.SceneInfoKey.NPCState);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SceneChange", UnityMMO.SceneInfoKey.SceneChange);
            
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
				else if (LuaAPI.xlua_is_eq_str(L, 1, "NPCState"))
                {
                    translator.PushUnityMMOSceneInfoKey(L, UnityMMO.SceneInfoKey.NPCState);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "SceneChange"))
                {
                    translator.PushUnityMMOSceneInfoKey(L, UnityMMO.SceneInfoKey.SceneChange);
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
			
			Utils.BeginClassRegister(typeof(UnityMMO.SceneObjectType), L, null, 8, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", UnityMMO.SceneObjectType.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Role", UnityMMO.SceneObjectType.Role);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Monster", UnityMMO.SceneObjectType.Monster);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NPC", UnityMMO.SceneObjectType.NPC);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Collectable", UnityMMO.SceneObjectType.Collectable);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "DropItem", UnityMMO.SceneObjectType.DropItem);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Interactive", UnityMMO.SceneObjectType.Interactive);
            
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
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Collectable"))
                {
                    translator.PushUnityMMOSceneObjectType(L, UnityMMO.SceneObjectType.Collectable);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "DropItem"))
                {
                    translator.PushUnityMMOSceneObjectType(L, UnityMMO.SceneObjectType.DropItem);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Interactive"))
                {
                    translator.PushUnityMMOSceneObjectType(L, UnityMMO.SceneObjectType.Interactive);
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
    
    public class XLuaFrameworkMainStateWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(XLuaFramework.Main.State), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(XLuaFramework.Main.State), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(XLuaFramework.Main.State), L, null, 8, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CheckExtractResource", XLuaFramework.Main.State.CheckExtractResource);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UpdateResourceFromNet", XLuaFramework.Main.State.UpdateResourceFromNet);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "InitAssetBundle", XLuaFramework.Main.State.InitAssetBundle);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "StartLogin", XLuaFramework.Main.State.StartLogin);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "StartGame", XLuaFramework.Main.State.StartGame);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Playing", XLuaFramework.Main.State.Playing);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", XLuaFramework.Main.State.None);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(XLuaFramework.Main.State), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushXLuaFrameworkMainState(L, (XLuaFramework.Main.State)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "CheckExtractResource"))
                {
                    translator.PushXLuaFrameworkMainState(L, XLuaFramework.Main.State.CheckExtractResource);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "UpdateResourceFromNet"))
                {
                    translator.PushXLuaFrameworkMainState(L, XLuaFramework.Main.State.UpdateResourceFromNet);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "InitAssetBundle"))
                {
                    translator.PushXLuaFrameworkMainState(L, XLuaFramework.Main.State.InitAssetBundle);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "StartLogin"))
                {
                    translator.PushXLuaFrameworkMainState(L, XLuaFramework.Main.State.StartLogin);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "StartGame"))
                {
                    translator.PushXLuaFrameworkMainState(L, XLuaFramework.Main.State.StartGame);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Playing"))
                {
                    translator.PushXLuaFrameworkMainState(L, XLuaFramework.Main.State.Playing);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushXLuaFrameworkMainState(L, XLuaFramework.Main.State.None);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for XLuaFramework.Main.State!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for XLuaFramework.Main.State! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class XLuaFrameworkMainSubStateWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(XLuaFramework.Main.SubState), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(XLuaFramework.Main.SubState), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(XLuaFramework.Main.SubState), L, null, 3, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Enter", XLuaFramework.Main.SubState.Enter);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Update", XLuaFramework.Main.SubState.Update);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(XLuaFramework.Main.SubState), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushXLuaFrameworkMainSubState(L, (XLuaFramework.Main.SubState)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "Enter"))
                {
                    translator.PushXLuaFrameworkMainSubState(L, XLuaFramework.Main.SubState.Enter);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Update"))
                {
                    translator.PushXLuaFrameworkMainSubState(L, XLuaFramework.Main.SubState.Update);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for XLuaFramework.Main.SubState!");
                }
            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for XLuaFramework.Main.SubState! Expect number or string, got + " + lua_type);
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