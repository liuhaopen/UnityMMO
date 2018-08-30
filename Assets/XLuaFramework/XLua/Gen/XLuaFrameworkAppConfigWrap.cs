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
    public class XLuaFrameworkAppConfigWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(XLuaFramework.AppConfig);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 9, 10, 4);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Init", _m_Init_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRelativePath", _m_GetRelativePath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetStreamingAssetsTargetPathByPlatform", _m_GetStreamingAssetsTargetPathByPlatform_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LuaByteMode", XLuaFramework.AppConfig.LuaByteMode);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AppName", XLuaFramework.AppConfig.AppName);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LuaTempDir", XLuaFramework.AppConfig.LuaTempDir);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AppPrefix", XLuaFramework.AppConfig.AppPrefix);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AssetDir", XLuaFramework.AppConfig.AssetDir);
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "DataPath", _g_get_DataPath);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "LuaAssetsDir", _g_get_LuaAssetsDir);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "FrameworkRoot", _g_get_FrameworkRoot);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "StreamingAssetsTargetPath", _g_get_StreamingAssetsTargetPath);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "WebUrl", _g_get_WebUrl);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "AppDataPath", _g_get_AppDataPath);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "DebugMode", _g_get_DebugMode);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "UpdateMode", _g_get_UpdateMode);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "LuaBundleMode", _g_get_LuaBundleMode);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "SprotoBinMode", _g_get_SprotoBinMode);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "DebugMode", _s_set_DebugMode);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "UpdateMode", _s_set_UpdateMode);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "LuaBundleMode", _s_set_LuaBundleMode);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "SprotoBinMode", _s_set_SprotoBinMode);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					XLuaFramework.AppConfig gen_ret = new XLuaFramework.AppConfig();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to XLuaFramework.AppConfig constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    XLuaFramework.AppConfig.Init(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRelativePath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        string gen_ret = XLuaFramework.AppConfig.GetRelativePath(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetStreamingAssetsTargetPathByPlatform_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.RuntimePlatform _platform;translator.Get(L, 1, out _platform);
                    
                        string gen_ret = XLuaFramework.AppConfig.GetStreamingAssetsTargetPathByPlatform( _platform );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DataPath(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, XLuaFramework.AppConfig.DataPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaAssetsDir(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, XLuaFramework.AppConfig.LuaAssetsDir);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FrameworkRoot(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, XLuaFramework.AppConfig.FrameworkRoot);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_StreamingAssetsTargetPath(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, XLuaFramework.AppConfig.StreamingAssetsTargetPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WebUrl(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, XLuaFramework.AppConfig.WebUrl);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AppDataPath(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, XLuaFramework.AppConfig.AppDataPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DebugMode(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, XLuaFramework.AppConfig.DebugMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UpdateMode(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, XLuaFramework.AppConfig.UpdateMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaBundleMode(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, XLuaFramework.AppConfig.LuaBundleMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SprotoBinMode(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, XLuaFramework.AppConfig.SprotoBinMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DebugMode(RealStatePtr L)
        {
		    try {
                
			    XLuaFramework.AppConfig.DebugMode = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UpdateMode(RealStatePtr L)
        {
		    try {
                
			    XLuaFramework.AppConfig.UpdateMode = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LuaBundleMode(RealStatePtr L)
        {
		    try {
                
			    XLuaFramework.AppConfig.LuaBundleMode = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SprotoBinMode(RealStatePtr L)
        {
		    try {
                
			    XLuaFramework.AppConfig.SprotoBinMode = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
