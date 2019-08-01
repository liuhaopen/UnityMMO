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
    public class XLuaFrameworkAssetsHotFixManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(XLuaFramework.AssetsHotFixManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CheckExtractResource", _m_CheckExtractResource);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnExtractResource", _m_OnExtractResource);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UpdateResource", _m_UpdateResource);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 1, 1);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Instance", _g_get_Instance);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "Instance", _s_set_Instance);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					XLuaFramework.AssetsHotFixManager gen_ret = new XLuaFramework.AssetsHotFixManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to XLuaFramework.AssetsHotFixManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckExtractResource(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.AssetsHotFixManager gen_to_be_invoked = (XLuaFramework.AssetsHotFixManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<float> _on_update = translator.GetDelegate<System.Action<float>>(L, 2);
                    System.Action _on_ok = translator.GetDelegate<System.Action>(L, 3);
                    
                    gen_to_be_invoked.CheckExtractResource( _on_update, _on_ok );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnExtractResource(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.AssetsHotFixManager gen_to_be_invoked = (XLuaFramework.AssetsHotFixManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action _on_ok = translator.GetDelegate<System.Action>(L, 2);
                    
                        System.Collections.IEnumerator gen_ret = gen_to_be_invoked.OnExtractResource( _on_ok );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateResource(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.AssetsHotFixManager gen_to_be_invoked = (XLuaFramework.AssetsHotFixManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<float, string> _on_update = translator.GetDelegate<System.Action<float, string>>(L, 2);
                    System.Action<string> _on_ok = translator.GetDelegate<System.Action<string>>(L, 3);
                    
                    gen_to_be_invoked.UpdateResource( _on_update, _on_ok );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Instance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, XLuaFramework.AssetsHotFixManager.Instance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Instance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    XLuaFramework.AssetsHotFixManager.Instance = (XLuaFramework.AssetsHotFixManager)translator.GetObject(L, 1, typeof(XLuaFramework.AssetsHotFixManager));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
