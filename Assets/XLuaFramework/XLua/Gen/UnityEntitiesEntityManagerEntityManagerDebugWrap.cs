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
    public class UnityEntitiesEntityManagerEntityManagerDebugWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Unity.Entities.EntityManager.EntityManagerDebug);
			Utils.BeginObjectRegister(type, L, translator, 0, 7, 1, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PoisonUnusedDataInAllChunks", _m_PoisonUnusedDataInAllChunks);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetGlobalSystemVersion", _m_SetGlobalSystemVersion);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsSharedComponentManagerEmpty", _m_IsSharedComponentManagerEmpty);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LogEntityInfo", _m_LogEntityInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetEntityInfo", _m_GetEntityInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetComponentBoxed", _m_GetComponentBoxed);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CheckInternalConsistency", _m_CheckInternalConsistency);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "EntityCount", _g_get_EntityCount);
            
			
			
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
				if(LuaAPI.lua_gettop(L) == 2 && translator.Assignable<Unity.Entities.EntityManager>(L, 2))
				{
					Unity.Entities.EntityManager _entityManager = (Unity.Entities.EntityManager)translator.GetObject(L, 2, typeof(Unity.Entities.EntityManager));
					
					Unity.Entities.EntityManager.EntityManagerDebug gen_ret = new Unity.Entities.EntityManager.EntityManagerDebug(_entityManager);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Unity.Entities.EntityManager.EntityManagerDebug constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PoisonUnusedDataInAllChunks(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager.EntityManagerDebug gen_to_be_invoked = (Unity.Entities.EntityManager.EntityManagerDebug)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Unity.Entities.EntityArchetype _archetype;translator.Get(L, 2, out _archetype);
                    byte _value = (byte)LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.PoisonUnusedDataInAllChunks( _archetype, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetGlobalSystemVersion(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager.EntityManagerDebug gen_to_be_invoked = (Unity.Entities.EntityManager.EntityManagerDebug)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    uint _version = LuaAPI.xlua_touint(L, 2);
                    
                    gen_to_be_invoked.SetGlobalSystemVersion( _version );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsSharedComponentManagerEmpty(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager.EntityManagerDebug gen_to_be_invoked = (Unity.Entities.EntityManager.EntityManagerDebug)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.IsSharedComponentManagerEmpty(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LogEntityInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager.EntityManagerDebug gen_to_be_invoked = (Unity.Entities.EntityManager.EntityManagerDebug)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Unity.Entities.Entity _entity;translator.Get(L, 2, out _entity);
                    
                    gen_to_be_invoked.LogEntityInfo( _entity );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetEntityInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager.EntityManagerDebug gen_to_be_invoked = (Unity.Entities.EntityManager.EntityManagerDebug)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Unity.Entities.Entity _entity;translator.Get(L, 2, out _entity);
                    
                        string gen_ret = gen_to_be_invoked.GetEntityInfo( _entity );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetComponentBoxed(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager.EntityManagerDebug gen_to_be_invoked = (Unity.Entities.EntityManager.EntityManagerDebug)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<Unity.Entities.Entity>(L, 2)&& translator.Assignable<Unity.Entities.ComponentType>(L, 3)) 
                {
                    Unity.Entities.Entity _entity;translator.Get(L, 2, out _entity);
                    Unity.Entities.ComponentType _type;translator.Get(L, 3, out _type);
                    
                        object gen_ret = gen_to_be_invoked.GetComponentBoxed( _entity, _type );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<Unity.Entities.Entity>(L, 2)&& translator.Assignable<System.Type>(L, 3)) 
                {
                    Unity.Entities.Entity _entity;translator.Get(L, 2, out _entity);
                    System.Type _type = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    
                        object gen_ret = gen_to_be_invoked.GetComponentBoxed( _entity, _type );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Unity.Entities.EntityManager.EntityManagerDebug.GetComponentBoxed!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckInternalConsistency(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager.EntityManagerDebug gen_to_be_invoked = (Unity.Entities.EntityManager.EntityManagerDebug)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.CheckInternalConsistency(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EntityCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Unity.Entities.EntityManager.EntityManagerDebug gen_to_be_invoked = (Unity.Entities.EntityManager.EntityManagerDebug)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.EntityCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
