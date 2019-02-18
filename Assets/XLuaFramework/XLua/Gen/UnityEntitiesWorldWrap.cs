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
    public class UnityEntitiesWorldWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Unity.Entities.World);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 4, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToString", _m_ToString);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Dispose", _m_Dispose);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateManager", _m_CreateManager);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetOrCreateManager", _m_GetOrCreateManager);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetExistingManager", _m_GetExistingManager);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DestroyManager", _m_DestroyManager);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "BehaviourManagers", _g_get_BehaviourManagers);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Name", _g_get_Name);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Version", _g_get_Version);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsCreated", _g_get_IsCreated);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 2, 1);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "DisposeAllWorlds", _m_DisposeAllWorlds_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Active", _g_get_Active);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "AllWorlds", _g_get_AllWorlds);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "Active", _s_set_Active);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 2 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
				{
					string _name = LuaAPI.lua_tostring(L, 2);
					
					Unity.Entities.World gen_ret = new Unity.Entities.World(_name);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Unity.Entities.World constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.World gen_to_be_invoked = (Unity.Entities.World)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        string gen_ret = gen_to_be_invoked.ToString(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Dispose(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.World gen_to_be_invoked = (Unity.Entities.World)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Dispose(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DisposeAllWorlds_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Unity.Entities.World.DisposeAllWorlds(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateManager(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.World gen_to_be_invoked = (Unity.Entities.World)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Type _type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    object[] _constructorArgumnents = translator.GetParams<object>(L, 3);
                    
                        Unity.Entities.ScriptBehaviourManager gen_ret = gen_to_be_invoked.CreateManager( _type, _constructorArgumnents );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetOrCreateManager(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.World gen_to_be_invoked = (Unity.Entities.World)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Type _type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        Unity.Entities.ScriptBehaviourManager gen_ret = gen_to_be_invoked.GetOrCreateManager( _type );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetExistingManager(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.World gen_to_be_invoked = (Unity.Entities.World)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Type _type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        Unity.Entities.ScriptBehaviourManager gen_ret = gen_to_be_invoked.GetExistingManager( _type );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DestroyManager(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.World gen_to_be_invoked = (Unity.Entities.World)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Unity.Entities.ScriptBehaviourManager _manager = (Unity.Entities.ScriptBehaviourManager)translator.GetObject(L, 2, typeof(Unity.Entities.ScriptBehaviourManager));
                    
                    gen_to_be_invoked.DestroyManager( _manager );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BehaviourManagers(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Unity.Entities.World gen_to_be_invoked = (Unity.Entities.World)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.BehaviourManagers);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Name(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Unity.Entities.World gen_to_be_invoked = (Unity.Entities.World)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Name);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Version(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Unity.Entities.World gen_to_be_invoked = (Unity.Entities.World)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Version);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Active(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Unity.Entities.World.Active);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AllWorlds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Unity.Entities.World.AllWorlds);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsCreated(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Unity.Entities.World gen_to_be_invoked = (Unity.Entities.World)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsCreated);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Active(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Unity.Entities.World.Active = (Unity.Entities.World)translator.GetObject(L, 1, typeof(Unity.Entities.World));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
