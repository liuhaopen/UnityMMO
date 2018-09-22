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
    public class UnityMMOSceneMgrWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityMMO.SceneMgr);
			Utils.BeginObjectRegister(type, L, translator, 0, 9, 4, 4);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Awake", _m_Awake);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitArcheType", _m_InitArcheType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDestroy", _m_OnDestroy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadScene", _m_LoadScene);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddMainRole", _m_AddMainRole);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddRole", _m_AddRole);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddSceneObject", _m_AddSceneObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveSceneObject", _m_RemoveSceneObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSceneObject", _m_GetSceneObject);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "EntityManager", _g_get_EntityManager);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RoleArchetype", _g_get_RoleArchetype);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MonsterArchetype", _g_get_MonsterArchetype);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "NPCArchetype", _g_get_NPCArchetype);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "EntityManager", _s_set_EntityManager);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RoleArchetype", _s_set_RoleArchetype);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MonsterArchetype", _s_set_MonsterArchetype);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "NPCArchetype", _s_set_NPCArchetype);
            
			
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
					
					UnityMMO.SceneMgr gen_ret = new UnityMMO.SceneMgr();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityMMO.SceneMgr constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Awake(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Awake(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitArcheType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InitArcheType(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDestroy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnDestroy(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadScene(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _scene_id = LuaAPI.xlua_tointeger(L, 2);
                    float _pos_x = (float)LuaAPI.lua_tonumber(L, 3);
                    float _pos_y = (float)LuaAPI.lua_tonumber(L, 4);
                    float _pos_z = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.LoadScene( _scene_id, _pos_x, _pos_y, _pos_z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddMainRole(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    long _uid = LuaAPI.lua_toint64(L, 2);
                    
                        Unity.Entities.Entity gen_ret = gen_to_be_invoked.AddMainRole( _uid );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddRole(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    long _uid = LuaAPI.lua_toint64(L, 2);
                    
                        Unity.Entities.Entity gen_ret = gen_to_be_invoked.AddRole( _uid );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddSceneObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    long _uid = LuaAPI.lua_toint64(L, 2);
                    UnityMMO.SceneObjectType _type;translator.Get(L, 3, out _type);
                    
                        Unity.Entities.Entity gen_ret = gen_to_be_invoked.AddSceneObject( _uid, _type );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveSceneObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    long _uid = LuaAPI.lua_toint64(L, 2);
                    
                    gen_to_be_invoked.RemoveSceneObject( _uid );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSceneObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    long _uid = LuaAPI.lua_toint64(L, 2);
                    
                        Unity.Entities.Entity gen_ret = gen_to_be_invoked.GetSceneObject( _uid );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EntityManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.EntityManager);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Instance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, UnityMMO.SceneMgr.Instance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RoleArchetype(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.RoleArchetype);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MonsterArchetype(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MonsterArchetype);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_NPCArchetype(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.NPCArchetype);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EntityManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EntityManager = (Unity.Entities.EntityManager)translator.GetObject(L, 2, typeof(Unity.Entities.EntityManager));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Instance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    UnityMMO.SceneMgr.Instance = (UnityMMO.SceneMgr)translator.GetObject(L, 1, typeof(UnityMMO.SceneMgr));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RoleArchetype(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
                Unity.Entities.EntityArchetype gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.RoleArchetype = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MonsterArchetype(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
                Unity.Entities.EntityArchetype gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.MonsterArchetype = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_NPCArchetype(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
                Unity.Entities.EntityArchetype gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.NPCArchetype = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
