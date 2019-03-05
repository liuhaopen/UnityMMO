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
    public class UnityEntitiesEntityManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Unity.Entities.EntityManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 33, 7, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateComponentGroup", _m_CreateComponentGroup);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateArchetype", _m_CreateArchetype);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateEntity", _m_CreateEntity);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LockChunk", _m_LockChunk);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnlockChunk", _m_UnlockChunk);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateChunk", _m_CreateChunk);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetChunk", _m_GetChunk);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DestroyEntity", _m_DestroyEntity);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetName", _m_GetName);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetName", _m_SetName);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Exists", _m_Exists);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HasComponent", _m_HasComponent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Instantiate", _m_Instantiate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddComponent", _m_AddComponent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddComponents", _m_AddComponents);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveComponent", _m_RemoveComponent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSharedComponentCount", _m_GetSharedComponentCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAllEntities", _m_GetAllEntities);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAllChunks", _m_GetAllChunks);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetComponentTypes", _m_GetComponentTypes);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetComponentCount", _m_GetComponentCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BeginExclusiveEntityTransaction", _m_BeginExclusiveEntityTransaction);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EndExclusiveEntityTransaction", _m_EndExclusiveEntityTransaction);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CompleteAllJobs", _m_CompleteAllJobs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MoveEntitiesFrom", _m_MoveEntitiesFrom);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateEntityRemapArray", _m_CreateEntityRemapArray);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAssignableComponentTypes", _m_GetAssignableComponentTypes);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddMatchingArchetypes", _m_AddMatchingArchetypes);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAllArchetypes", _m_GetAllArchetypes);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateArchetypeChunkArray", _m_CreateArchetypeChunkArray);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetArchetypeChunkEntityType", _m_GetArchetypeChunkEntityType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SwapComponents", _m_SwapComponents);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PrepareForDeserialize", _m_PrepareForDeserialize);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Version", _g_get_Version);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "GlobalSystemVersion", _g_get_GlobalSystemVersion);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsCreated", _g_get_IsCreated);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EntityCapacity", _g_get_EntityCapacity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ExclusiveEntityTransactionDependency", _g_get_ExclusiveEntityTransactionDependency);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Debug", _g_get_Debug);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "World", _g_get_World);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "EntityCapacity", _s_set_EntityCapacity);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ExclusiveEntityTransactionDependency", _s_set_ExclusiveEntityTransactionDependency);
            
			
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
					
					Unity.Entities.EntityManager gen_ret = new Unity.Entities.EntityManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Unity.Entities.EntityManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateComponentGroup(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Unity.Entities.ComponentType[] _requiredComponents = translator.GetParams<Unity.Entities.ComponentType>(L, 2);
                    
                        Unity.Entities.ComponentGroup gen_ret = gen_to_be_invoked.CreateComponentGroup( _requiredComponents );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateArchetype(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Unity.Entities.ComponentType[] _types = translator.GetParams<Unity.Entities.ComponentType>(L, 2);
                    
                        Unity.Entities.EntityArchetype gen_ret = gen_to_be_invoked.CreateArchetype( _types );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateEntity(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<Unity.Entities.EntityArchetype>(L, 2)) 
                {
                    Unity.Entities.EntityArchetype _archetype;translator.Get(L, 2, out _archetype);
                    
                        Unity.Entities.Entity gen_ret = gen_to_be_invoked.CreateEntity( _archetype );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count >= 1&& (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 2) || translator.Assignable<Unity.Entities.ComponentType>(L, 2))) 
                {
                    Unity.Entities.ComponentType[] _types = translator.GetParams<Unity.Entities.ComponentType>(L, 2);
                    
                        Unity.Entities.Entity gen_ret = gen_to_be_invoked.CreateEntity( _types );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<Unity.Entities.EntityArchetype>(L, 2)&& translator.Assignable<Unity.Collections.NativeArray<Unity.Entities.Entity>>(L, 3)) 
                {
                    Unity.Entities.EntityArchetype _archetype;translator.Get(L, 2, out _archetype);
                    Unity.Collections.NativeArray<Unity.Entities.Entity> _entities;translator.Get(L, 3, out _entities);
                    
                    gen_to_be_invoked.CreateEntity( _archetype, _entities );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Unity.Entities.EntityManager.CreateEntity!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LockChunk(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<Unity.Entities.ArchetypeChunk>(L, 2)) 
                {
                    Unity.Entities.ArchetypeChunk _chunk;translator.Get(L, 2, out _chunk);
                    
                    gen_to_be_invoked.LockChunk( _chunk );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Unity.Collections.NativeArray<Unity.Entities.ArchetypeChunk>>(L, 2)) 
                {
                    Unity.Collections.NativeArray<Unity.Entities.ArchetypeChunk> _chunks;translator.Get(L, 2, out _chunks);
                    
                    gen_to_be_invoked.LockChunk( _chunks );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Unity.Entities.EntityManager.LockChunk!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnlockChunk(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<Unity.Entities.ArchetypeChunk>(L, 2)) 
                {
                    Unity.Entities.ArchetypeChunk _chunk;translator.Get(L, 2, out _chunk);
                    
                    gen_to_be_invoked.UnlockChunk( _chunk );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Unity.Collections.NativeArray<Unity.Entities.ArchetypeChunk>>(L, 2)) 
                {
                    Unity.Collections.NativeArray<Unity.Entities.ArchetypeChunk> _chunks;translator.Get(L, 2, out _chunks);
                    
                    gen_to_be_invoked.UnlockChunk( _chunks );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Unity.Entities.EntityManager.UnlockChunk!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateChunk(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Unity.Entities.EntityArchetype _archetype;translator.Get(L, 2, out _archetype);
                    Unity.Collections.NativeArray<Unity.Entities.ArchetypeChunk> _chunks;translator.Get(L, 3, out _chunks);
                    int _entityCount = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.CreateChunk( _archetype, _chunks, _entityCount );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetChunk(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Unity.Entities.Entity _entity;translator.Get(L, 2, out _entity);
                    
                        Unity.Entities.ArchetypeChunk gen_ret = gen_to_be_invoked.GetChunk( _entity );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DestroyEntity(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<Unity.Entities.ComponentGroup>(L, 2)) 
                {
                    Unity.Entities.ComponentGroup _componentGroupFilter = (Unity.Entities.ComponentGroup)translator.GetObject(L, 2, typeof(Unity.Entities.ComponentGroup));
                    
                    gen_to_be_invoked.DestroyEntity( _componentGroupFilter );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Unity.Collections.NativeArray<Unity.Entities.Entity>>(L, 2)) 
                {
                    Unity.Collections.NativeArray<Unity.Entities.Entity> _entities;translator.Get(L, 2, out _entities);
                    
                    gen_to_be_invoked.DestroyEntity( _entities );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Unity.Collections.NativeSlice<Unity.Entities.Entity>>(L, 2)) 
                {
                    Unity.Collections.NativeSlice<Unity.Entities.Entity> _entities;translator.Get(L, 2, out _entities);
                    
                    gen_to_be_invoked.DestroyEntity( _entities );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Unity.Entities.Entity>(L, 2)) 
                {
                    Unity.Entities.Entity _entity;translator.Get(L, 2, out _entity);
                    
                    gen_to_be_invoked.DestroyEntity( _entity );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Unity.Entities.EntityManager.DestroyEntity!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetName(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Unity.Entities.Entity _entity;translator.Get(L, 2, out _entity);
                    
                        string gen_ret = gen_to_be_invoked.GetName( _entity );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetName(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Unity.Entities.Entity _entity;translator.Get(L, 2, out _entity);
                    string _name = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.SetName( _entity, _name );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Exists(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Unity.Entities.Entity _entity;translator.Get(L, 2, out _entity);
                    
                        bool gen_ret = gen_to_be_invoked.Exists( _entity );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasComponent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Unity.Entities.Entity _entity;translator.Get(L, 2, out _entity);
                    Unity.Entities.ComponentType _type;translator.Get(L, 3, out _type);
                    
                        bool gen_ret = gen_to_be_invoked.HasComponent( _entity, _type );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Instantiate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<Unity.Entities.Entity>(L, 2)) 
                {
                    Unity.Entities.Entity _srcEntity;translator.Get(L, 2, out _srcEntity);
                    
                        Unity.Entities.Entity gen_ret = gen_to_be_invoked.Instantiate( _srcEntity );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<Unity.Entities.Entity>(L, 2)&& translator.Assignable<Unity.Collections.NativeArray<Unity.Entities.Entity>>(L, 3)) 
                {
                    Unity.Entities.Entity _srcEntity;translator.Get(L, 2, out _srcEntity);
                    Unity.Collections.NativeArray<Unity.Entities.Entity> _outputEntities;translator.Get(L, 3, out _outputEntities);
                    
                    gen_to_be_invoked.Instantiate( _srcEntity, _outputEntities );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Unity.Entities.EntityManager.Instantiate!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddComponent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<Unity.Entities.Entity>(L, 2)&& translator.Assignable<Unity.Entities.ComponentType>(L, 3)) 
                {
                    Unity.Entities.Entity _entity;translator.Get(L, 2, out _entity);
                    Unity.Entities.ComponentType _type;translator.Get(L, 3, out _type);
                    
                    gen_to_be_invoked.AddComponent( _entity, _type );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<Unity.Entities.ComponentGroup>(L, 2)&& translator.Assignable<Unity.Entities.ComponentType>(L, 3)) 
                {
                    Unity.Entities.ComponentGroup _componentGroupFilter = (Unity.Entities.ComponentGroup)translator.GetObject(L, 2, typeof(Unity.Entities.ComponentGroup));
                    Unity.Entities.ComponentType _type;translator.Get(L, 3, out _type);
                    
                    gen_to_be_invoked.AddComponent( _componentGroupFilter, _type );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<Unity.Collections.NativeArray<Unity.Entities.Entity>>(L, 2)&& translator.Assignable<Unity.Entities.ComponentType>(L, 3)) 
                {
                    Unity.Collections.NativeArray<Unity.Entities.Entity> _entities;translator.Get(L, 2, out _entities);
                    Unity.Entities.ComponentType _type;translator.Get(L, 3, out _type);
                    
                    gen_to_be_invoked.AddComponent( _entities, _type );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Unity.Entities.EntityManager.AddComponent!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddComponents(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Unity.Entities.Entity _entity;translator.Get(L, 2, out _entity);
                    Unity.Entities.ComponentTypes _types;translator.Get(L, 3, out _types);
                    
                    gen_to_be_invoked.AddComponents( _entity, _types );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveComponent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<Unity.Entities.Entity>(L, 2)&& translator.Assignable<Unity.Entities.ComponentType>(L, 3)) 
                {
                    Unity.Entities.Entity _entity;translator.Get(L, 2, out _entity);
                    Unity.Entities.ComponentType _type;translator.Get(L, 3, out _type);
                    
                    gen_to_be_invoked.RemoveComponent( _entity, _type );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<Unity.Entities.ComponentGroup>(L, 2)&& translator.Assignable<Unity.Entities.ComponentType>(L, 3)) 
                {
                    Unity.Entities.ComponentGroup _componentGroupFilter = (Unity.Entities.ComponentGroup)translator.GetObject(L, 2, typeof(Unity.Entities.ComponentGroup));
                    Unity.Entities.ComponentType _type;translator.Get(L, 3, out _type);
                    
                    gen_to_be_invoked.RemoveComponent( _componentGroupFilter, _type );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<Unity.Collections.NativeArray<Unity.Entities.Entity>>(L, 2)&& translator.Assignable<Unity.Entities.ComponentType>(L, 3)) 
                {
                    Unity.Collections.NativeArray<Unity.Entities.Entity> _entities;translator.Get(L, 2, out _entities);
                    Unity.Entities.ComponentType _type;translator.Get(L, 3, out _type);
                    
                    gen_to_be_invoked.RemoveComponent( _entities, _type );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Unity.Entities.EntityManager.RemoveComponent!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSharedComponentCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        int gen_ret = gen_to_be_invoked.GetSharedComponentCount(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAllEntities(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<Unity.Collections.Allocator>(L, 2)) 
                {
                    Unity.Collections.Allocator _allocator;translator.Get(L, 2, out _allocator);
                    
                        Unity.Collections.NativeArray<Unity.Entities.Entity> gen_ret = gen_to_be_invoked.GetAllEntities( _allocator );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1) 
                {
                    
                        Unity.Collections.NativeArray<Unity.Entities.Entity> gen_ret = gen_to_be_invoked.GetAllEntities(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Unity.Entities.EntityManager.GetAllEntities!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAllChunks(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<Unity.Collections.Allocator>(L, 2)) 
                {
                    Unity.Collections.Allocator _allocator;translator.Get(L, 2, out _allocator);
                    
                        Unity.Collections.NativeArray<Unity.Entities.ArchetypeChunk> gen_ret = gen_to_be_invoked.GetAllChunks( _allocator );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1) 
                {
                    
                        Unity.Collections.NativeArray<Unity.Entities.ArchetypeChunk> gen_ret = gen_to_be_invoked.GetAllChunks(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Unity.Entities.EntityManager.GetAllChunks!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetComponentTypes(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<Unity.Entities.Entity>(L, 2)&& translator.Assignable<Unity.Collections.Allocator>(L, 3)) 
                {
                    Unity.Entities.Entity _entity;translator.Get(L, 2, out _entity);
                    Unity.Collections.Allocator _allocator;translator.Get(L, 3, out _allocator);
                    
                        Unity.Collections.NativeArray<Unity.Entities.ComponentType> gen_ret = gen_to_be_invoked.GetComponentTypes( _entity, _allocator );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<Unity.Entities.Entity>(L, 2)) 
                {
                    Unity.Entities.Entity _entity;translator.Get(L, 2, out _entity);
                    
                        Unity.Collections.NativeArray<Unity.Entities.ComponentType> gen_ret = gen_to_be_invoked.GetComponentTypes( _entity );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Unity.Entities.EntityManager.GetComponentTypes!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetComponentCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Unity.Entities.Entity _entity;translator.Get(L, 2, out _entity);
                    
                        int gen_ret = gen_to_be_invoked.GetComponentCount( _entity );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BeginExclusiveEntityTransaction(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        Unity.Entities.ExclusiveEntityTransaction gen_ret = gen_to_be_invoked.BeginExclusiveEntityTransaction(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EndExclusiveEntityTransaction(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.EndExclusiveEntityTransaction(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CompleteAllJobs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.CompleteAllJobs(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MoveEntitiesFrom(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<Unity.Entities.EntityManager>(L, 2)) 
                {
                    Unity.Entities.EntityManager _srcEntities = (Unity.Entities.EntityManager)translator.GetObject(L, 2, typeof(Unity.Entities.EntityManager));
                    
                    gen_to_be_invoked.MoveEntitiesFrom( _srcEntities );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Unity.Entities.EntityManager>(L, 2)) 
                {
                    Unity.Collections.NativeArray<Unity.Entities.Entity> _output;
                    Unity.Entities.EntityManager _srcEntities = (Unity.Entities.EntityManager)translator.GetObject(L, 2, typeof(Unity.Entities.EntityManager));
                    
                    gen_to_be_invoked.MoveEntitiesFrom( out _output, _srcEntities );
                    translator.Push(L, _output);
                        
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<Unity.Entities.EntityManager>(L, 2)&& translator.Assignable<Unity.Collections.NativeArray<Unity.Entities.EntityRemapUtility.EntityRemapInfo>>(L, 3)) 
                {
                    Unity.Entities.EntityManager _srcEntities = (Unity.Entities.EntityManager)translator.GetObject(L, 2, typeof(Unity.Entities.EntityManager));
                    Unity.Collections.NativeArray<Unity.Entities.EntityRemapUtility.EntityRemapInfo> _entityRemapping;translator.Get(L, 3, out _entityRemapping);
                    
                    gen_to_be_invoked.MoveEntitiesFrom( _srcEntities, _entityRemapping );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<Unity.Entities.EntityManager>(L, 2)&& translator.Assignable<Unity.Collections.NativeArray<Unity.Entities.EntityRemapUtility.EntityRemapInfo>>(L, 3)) 
                {
                    Unity.Collections.NativeArray<Unity.Entities.Entity> _output;
                    Unity.Entities.EntityManager _srcEntities = (Unity.Entities.EntityManager)translator.GetObject(L, 2, typeof(Unity.Entities.EntityManager));
                    Unity.Collections.NativeArray<Unity.Entities.EntityRemapUtility.EntityRemapInfo> _entityRemapping;translator.Get(L, 3, out _entityRemapping);
                    
                    gen_to_be_invoked.MoveEntitiesFrom( out _output, _srcEntities, _entityRemapping );
                    translator.Push(L, _output);
                        
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<Unity.Entities.EntityManager>(L, 2)&& translator.Assignable<Unity.Entities.ComponentGroup>(L, 3)&& translator.Assignable<Unity.Collections.NativeArray<Unity.Entities.EntityRemapUtility.EntityRemapInfo>>(L, 4)) 
                {
                    Unity.Entities.EntityManager _srcEntities = (Unity.Entities.EntityManager)translator.GetObject(L, 2, typeof(Unity.Entities.EntityManager));
                    Unity.Entities.ComponentGroup _filter = (Unity.Entities.ComponentGroup)translator.GetObject(L, 3, typeof(Unity.Entities.ComponentGroup));
                    Unity.Collections.NativeArray<Unity.Entities.EntityRemapUtility.EntityRemapInfo> _entityRemapping;translator.Get(L, 4, out _entityRemapping);
                    
                    gen_to_be_invoked.MoveEntitiesFrom( _srcEntities, _filter, _entityRemapping );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<Unity.Entities.EntityManager>(L, 2)&& translator.Assignable<Unity.Entities.ComponentGroup>(L, 3)&& translator.Assignable<Unity.Collections.NativeArray<Unity.Entities.EntityRemapUtility.EntityRemapInfo>>(L, 4)) 
                {
                    Unity.Collections.NativeArray<Unity.Entities.Entity> _output;
                    Unity.Entities.EntityManager _srcEntities = (Unity.Entities.EntityManager)translator.GetObject(L, 2, typeof(Unity.Entities.EntityManager));
                    Unity.Entities.ComponentGroup _filter = (Unity.Entities.ComponentGroup)translator.GetObject(L, 3, typeof(Unity.Entities.ComponentGroup));
                    Unity.Collections.NativeArray<Unity.Entities.EntityRemapUtility.EntityRemapInfo> _entityRemapping;translator.Get(L, 4, out _entityRemapping);
                    
                    gen_to_be_invoked.MoveEntitiesFrom( out _output, _srcEntities, _filter, _entityRemapping );
                    translator.Push(L, _output);
                        
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Unity.Entities.EntityManager.MoveEntitiesFrom!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateEntityRemapArray(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Unity.Collections.Allocator _allocator;translator.Get(L, 2, out _allocator);
                    
                        Unity.Collections.NativeArray<Unity.Entities.EntityRemapUtility.EntityRemapInfo> gen_ret = gen_to_be_invoked.CreateEntityRemapArray( _allocator );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAssignableComponentTypes(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Type _interfaceType = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        System.Collections.Generic.List<System.Type> gen_ret = gen_to_be_invoked.GetAssignableComponentTypes( _interfaceType );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddMatchingArchetypes(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Unity.Entities.EntityArchetypeQuery _query = (Unity.Entities.EntityArchetypeQuery)translator.GetObject(L, 2, typeof(Unity.Entities.EntityArchetypeQuery));
                    Unity.Collections.NativeList<Unity.Entities.EntityArchetype> _foundArchetypes;translator.Get(L, 3, out _foundArchetypes);
                    
                    gen_to_be_invoked.AddMatchingArchetypes( _query, _foundArchetypes );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAllArchetypes(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Unity.Collections.NativeList<Unity.Entities.EntityArchetype> _allArchetypes;translator.Get(L, 2, out _allArchetypes);
                    
                    gen_to_be_invoked.GetAllArchetypes( _allArchetypes );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateArchetypeChunkArray(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<Unity.Collections.NativeList<Unity.Entities.EntityArchetype>>(L, 2)&& translator.Assignable<Unity.Collections.Allocator>(L, 3)) 
                {
                    Unity.Collections.NativeList<Unity.Entities.EntityArchetype> _archetypes;translator.Get(L, 2, out _archetypes);
                    Unity.Collections.Allocator _allocator;translator.Get(L, 3, out _allocator);
                    
                        Unity.Collections.NativeArray<Unity.Entities.ArchetypeChunk> gen_ret = gen_to_be_invoked.CreateArchetypeChunkArray( _archetypes, _allocator );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<Unity.Entities.EntityArchetypeQuery>(L, 2)&& translator.Assignable<Unity.Collections.Allocator>(L, 3)) 
                {
                    Unity.Entities.EntityArchetypeQuery _query = (Unity.Entities.EntityArchetypeQuery)translator.GetObject(L, 2, typeof(Unity.Entities.EntityArchetypeQuery));
                    Unity.Collections.Allocator _allocator;translator.Get(L, 3, out _allocator);
                    
                        Unity.Collections.NativeArray<Unity.Entities.ArchetypeChunk> gen_ret = gen_to_be_invoked.CreateArchetypeChunkArray( _query, _allocator );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Unity.Entities.EntityManager.CreateArchetypeChunkArray!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetArchetypeChunkEntityType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        Unity.Entities.ArchetypeChunkEntityType gen_ret = gen_to_be_invoked.GetArchetypeChunkEntityType(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SwapComponents(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Unity.Entities.ArchetypeChunk _leftChunk;translator.Get(L, 2, out _leftChunk);
                    int _leftIndex = LuaAPI.xlua_tointeger(L, 3);
                    Unity.Entities.ArchetypeChunk _rightChunk;translator.Get(L, 4, out _rightChunk);
                    int _rightIndex = LuaAPI.xlua_tointeger(L, 5);
                    
                    gen_to_be_invoked.SwapComponents( _leftChunk, _leftIndex, _rightChunk, _rightIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PrepareForDeserialize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.PrepareForDeserialize(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Version(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Version);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GlobalSystemVersion(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushuint(L, gen_to_be_invoked.GlobalSystemVersion);
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
			
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsCreated);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EntityCapacity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.EntityCapacity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ExclusiveEntityTransactionDependency(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ExclusiveEntityTransactionDependency);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Debug(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Debug);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_World(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.World);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EntityCapacity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EntityCapacity = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ExclusiveEntityTransactionDependency(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Unity.Entities.EntityManager gen_to_be_invoked = (Unity.Entities.EntityManager)translator.FastGetCSObj(L, 1);
                Unity.Jobs.JobHandle gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.ExclusiveEntityTransactionDependency = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
