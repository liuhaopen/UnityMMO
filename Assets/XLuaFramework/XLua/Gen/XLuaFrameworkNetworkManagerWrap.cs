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
    public class XLuaFrameworkNetworkManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(XLuaFramework.NetworkManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 4, 4);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendConnect", _m_SendConnect);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendBytes", _m_SendBytes);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendBytesWithTag", _m_SendBytesWithTag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Close", _m_Close);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnConnectCallBack", _g_get_OnConnectCallBack);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnDisConnectCallBack", _g_get_OnDisConnectCallBack);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnReceiveLineCallBack", _g_get_OnReceiveLineCallBack);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnReceiveMsgCallBack", _g_get_OnReceiveMsgCallBack);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnConnectCallBack", _s_set_OnConnectCallBack);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnDisConnectCallBack", _s_set_OnDisConnectCallBack);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnReceiveLineCallBack", _s_set_OnReceiveLineCallBack);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnReceiveMsgCallBack", _s_set_OnReceiveMsgCallBack);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 6, 1, 1);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetInstance", _m_GetInstance_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddEvent", _m_AddEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SwapUInt16", _m_SwapUInt16_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SwapInt16", _m_SwapInt16_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SwapInt32", _m_SwapInt32_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "loggedIn", _g_get_loggedIn);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "loggedIn", _s_set_loggedIn);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					XLuaFramework.NetworkManager gen_ret = new XLuaFramework.NetworkManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to XLuaFramework.NetworkManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInstance_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        XLuaFramework.NetworkManager gen_ret = XLuaFramework.NetworkManager.GetInstance(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action<byte[]> __event = translator.GetDelegate<System.Action<byte[]>>(L, 1);
                    byte[] _data = LuaAPI.lua_tobytes(L, 2);
                    
                    XLuaFramework.NetworkManager.AddEvent( __event, _data );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendConnect(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.NetworkManager gen_to_be_invoked = (XLuaFramework.NetworkManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _host = LuaAPI.lua_tostring(L, 2);
                    int _port = LuaAPI.xlua_tointeger(L, 3);
                    XLuaFramework.NetPackageType _type;translator.Get(L, 4, out _type);
                    
                    gen_to_be_invoked.SendConnect( _host, _port, _type );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SwapUInt16_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    ushort _n = (ushort)LuaAPI.xlua_tointeger(L, 1);
                    
                        ushort gen_ret = XLuaFramework.NetworkManager.SwapUInt16( _n );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SwapInt16_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    short _n = (short)LuaAPI.xlua_tointeger(L, 1);
                    
                        short gen_ret = XLuaFramework.NetworkManager.SwapInt16( _n );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SwapInt32_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _n = LuaAPI.xlua_tointeger(L, 1);
                    
                        int gen_ret = XLuaFramework.NetworkManager.SwapInt32( _n );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendBytes(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.NetworkManager gen_to_be_invoked = (XLuaFramework.NetworkManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    byte[] _message = LuaAPI.lua_tobytes(L, 2);
                    
                    gen_to_be_invoked.SendBytes( _message );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendBytesWithTag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.NetworkManager gen_to_be_invoked = (XLuaFramework.NetworkManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _tag = LuaAPI.xlua_tointeger(L, 2);
                    byte[] _message = LuaAPI.lua_tobytes(L, 3);
                    
                    gen_to_be_invoked.SendBytesWithTag( _tag, _message );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Close(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.NetworkManager gen_to_be_invoked = (XLuaFramework.NetworkManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Close(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnConnectCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLuaFramework.NetworkManager gen_to_be_invoked = (XLuaFramework.NetworkManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnConnectCallBack);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnDisConnectCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLuaFramework.NetworkManager gen_to_be_invoked = (XLuaFramework.NetworkManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnDisConnectCallBack);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnReceiveLineCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLuaFramework.NetworkManager gen_to_be_invoked = (XLuaFramework.NetworkManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnReceiveLineCallBack);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnReceiveMsgCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLuaFramework.NetworkManager gen_to_be_invoked = (XLuaFramework.NetworkManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnReceiveMsgCallBack);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_loggedIn(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, XLuaFramework.NetworkManager.loggedIn);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnConnectCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLuaFramework.NetworkManager gen_to_be_invoked = (XLuaFramework.NetworkManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnConnectCallBack = translator.GetDelegate<System.Action<byte[]>>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnDisConnectCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLuaFramework.NetworkManager gen_to_be_invoked = (XLuaFramework.NetworkManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnDisConnectCallBack = translator.GetDelegate<System.Action<byte[]>>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnReceiveLineCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLuaFramework.NetworkManager gen_to_be_invoked = (XLuaFramework.NetworkManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnReceiveLineCallBack = translator.GetDelegate<System.Action<byte[]>>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnReceiveMsgCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLuaFramework.NetworkManager gen_to_be_invoked = (XLuaFramework.NetworkManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnReceiveMsgCallBack = translator.GetDelegate<System.Action<byte[]>>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_loggedIn(RealStatePtr L)
        {
		    try {
                
			    XLuaFramework.NetworkManager.loggedIn = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
