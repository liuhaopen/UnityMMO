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
    public class UnityMMOSceneInfoWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityMMO.SceneInfo);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 6, 6);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "LightmapMode", _g_get_LightmapMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LightColorResPath", _g_get_LightColorResPath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LightDirResPath", _g_get_LightDirResPath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Bounds", _g_get_Bounds);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ObjectInfoList", _g_get_ObjectInfoList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BornList", _g_get_BornList);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "LightmapMode", _s_set_LightmapMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LightColorResPath", _s_set_LightColorResPath);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LightDirResPath", _s_set_LightDirResPath);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Bounds", _s_set_Bounds);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ObjectInfoList", _s_set_ObjectInfoList);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BornList", _s_set_BornList);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					UnityMMO.SceneInfo gen_ret = new UnityMMO.SceneInfo();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityMMO.SceneInfo constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LightmapMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneInfo gen_to_be_invoked = (UnityMMO.SceneInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LightmapMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LightColorResPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneInfo gen_to_be_invoked = (UnityMMO.SceneInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LightColorResPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LightDirResPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneInfo gen_to_be_invoked = (UnityMMO.SceneInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LightDirResPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Bounds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneInfo gen_to_be_invoked = (UnityMMO.SceneInfo)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineBounds(L, gen_to_be_invoked.Bounds);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ObjectInfoList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneInfo gen_to_be_invoked = (UnityMMO.SceneInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ObjectInfoList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BornList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneInfo gen_to_be_invoked = (UnityMMO.SceneInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.BornList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LightmapMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneInfo gen_to_be_invoked = (UnityMMO.SceneInfo)translator.FastGetCSObj(L, 1);
                UnityEngine.LightmapsMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.LightmapMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LightColorResPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneInfo gen_to_be_invoked = (UnityMMO.SceneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LightColorResPath = (System.Collections.Generic.List<string>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<string>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LightDirResPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneInfo gen_to_be_invoked = (UnityMMO.SceneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LightDirResPath = (System.Collections.Generic.List<string>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<string>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Bounds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneInfo gen_to_be_invoked = (UnityMMO.SceneInfo)translator.FastGetCSObj(L, 1);
                UnityEngine.Bounds gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.Bounds = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ObjectInfoList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneInfo gen_to_be_invoked = (UnityMMO.SceneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ObjectInfoList = (System.Collections.Generic.List<SceneStaticObject>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<SceneStaticObject>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BornList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneInfo gen_to_be_invoked = (UnityMMO.SceneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BornList = (System.Collections.Generic.List<UnityMMO.BornInfoData>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityMMO.BornInfoData>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
