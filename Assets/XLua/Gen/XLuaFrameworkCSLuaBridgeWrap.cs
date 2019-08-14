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
    public class XLuaFrameworkCSLuaBridgeWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(XLuaFramework.CSLuaBridge);
			Utils.BeginObjectRegister(type, L, translator, 0, 9, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLuaFunc", _m_SetLuaFunc);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CallLuaFunc", _m_CallLuaFunc);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLuaFunc2Num", _m_SetLuaFunc2Num);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CallLuaFunc2Num", _m_CallLuaFunc2Num);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLuaFuncNum", _m_SetLuaFuncNum);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CallLuaFuncNum", _m_CallLuaFuncNum);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLuaFuncStr", _m_SetLuaFuncStr);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CallLuaFuncStr", _m_CallLuaFuncStr);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearDelegate", _m_ClearDelegate);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetInstance", _m_GetInstance_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "XLuaFramework.CSLuaBridge does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInstance_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        XLuaFramework.CSLuaBridge gen_ret = XLuaFramework.CSLuaBridge.GetInstance(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLuaFunc(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.CSLuaBridge gen_to_be_invoked = (XLuaFramework.CSLuaBridge)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _funcID = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Events.UnityAction _func = translator.GetDelegate<UnityEngine.Events.UnityAction>(L, 3);
                    
                    gen_to_be_invoked.SetLuaFunc( _funcID, _func );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CallLuaFunc(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.CSLuaBridge gen_to_be_invoked = (XLuaFramework.CSLuaBridge)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _funcID = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.CallLuaFunc( _funcID );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLuaFunc2Num(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.CSLuaBridge gen_to_be_invoked = (XLuaFramework.CSLuaBridge)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _funcID = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Events.UnityAction<long, long> _func = translator.GetDelegate<UnityEngine.Events.UnityAction<long, long>>(L, 3);
                    
                    gen_to_be_invoked.SetLuaFunc2Num( _funcID, _func );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CallLuaFunc2Num(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.CSLuaBridge gen_to_be_invoked = (XLuaFramework.CSLuaBridge)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _funcID = LuaAPI.xlua_tointeger(L, 2);
                    long _arge1 = LuaAPI.lua_toint64(L, 3);
                    long _arge2 = LuaAPI.lua_toint64(L, 4);
                    
                    gen_to_be_invoked.CallLuaFunc2Num( _funcID, _arge1, _arge2 );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLuaFuncNum(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.CSLuaBridge gen_to_be_invoked = (XLuaFramework.CSLuaBridge)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _funcID = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Events.UnityAction<long> _func = translator.GetDelegate<UnityEngine.Events.UnityAction<long>>(L, 3);
                    
                    gen_to_be_invoked.SetLuaFuncNum( _funcID, _func );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CallLuaFuncNum(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.CSLuaBridge gen_to_be_invoked = (XLuaFramework.CSLuaBridge)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _funcID = LuaAPI.xlua_tointeger(L, 2);
                    long _arge1 = LuaAPI.lua_toint64(L, 3);
                    
                    gen_to_be_invoked.CallLuaFuncNum( _funcID, _arge1 );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLuaFuncStr(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.CSLuaBridge gen_to_be_invoked = (XLuaFramework.CSLuaBridge)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _funcID = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Events.UnityAction<string> _func = translator.GetDelegate<UnityEngine.Events.UnityAction<string>>(L, 3);
                    
                    gen_to_be_invoked.SetLuaFuncStr( _funcID, _func );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CallLuaFuncStr(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.CSLuaBridge gen_to_be_invoked = (XLuaFramework.CSLuaBridge)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _funcID = LuaAPI.xlua_tointeger(L, 2);
                    string _arge1 = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.CallLuaFuncStr( _funcID, _arge1 );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearDelegate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.CSLuaBridge gen_to_be_invoked = (XLuaFramework.CSLuaBridge)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ClearDelegate(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
