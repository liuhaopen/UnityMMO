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
			Utils.BeginObjectRegister(type, L, translator, 0, 14, 8, 4);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CheckMainRolePos", _m_CheckMainRolePos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDestroy", _m_OnDestroy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadScene", _m_LoadScene);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ApplyDetector", _m_ApplyDetector);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CorrectMainRolePos", _m_CorrectMainRolePos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCorrectPos", _m_GetCorrectPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ApplyMainRole", _m_ApplyMainRole);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddMainRole", _m_AddMainRole);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddSceneObject", _m_AddSceneObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetNameByUID", _m_GetNameByUID);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSceneObjTypeByUID", _m_GetSceneObjTypeByUID);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveSceneObject", _m_RemoveSceneObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSceneObject", _m_GetSceneObject);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "EntityManager", _g_get_EntityManager);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsLoadingScene", _g_get_IsLoadingScene);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FreeLookCamera", _g_get_FreeLookCamera);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MainCameraTrans", _g_get_MainCameraTrans);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FreeLookCameraTrans", _g_get_FreeLookCameraTrans);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurSceneInfo", _g_get_CurSceneInfo);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "detector", _g_get_detector);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "groundLayer", _g_get_groundLayer);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsLoadingScene", _s_set_IsLoadingScene);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FreeLookCamera", _s_set_FreeLookCamera);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "detector", _s_set_detector);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "groundLayer", _s_set_groundLayer);
            
			
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
        static int _m_CheckMainRolePos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.CheckMainRolePos(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    GameWorld _world = (GameWorld)translator.GetObject(L, 2, typeof(GameWorld));
                    
                    gen_to_be_invoked.Init( _world );
                    
                    
                    
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
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    int _scene_id = LuaAPI.xlua_tointeger(L, 2);
                    float _pos_x = (float)LuaAPI.lua_tonumber(L, 3);
                    float _pos_y = (float)LuaAPI.lua_tonumber(L, 4);
                    float _pos_z = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.LoadScene( _scene_id, _pos_x, _pos_y, _pos_z );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    int _scene_id = LuaAPI.xlua_tointeger(L, 2);
                    float _pos_x = (float)LuaAPI.lua_tonumber(L, 3);
                    float _pos_y = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.LoadScene( _scene_id, _pos_x, _pos_y );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _scene_id = LuaAPI.xlua_tointeger(L, 2);
                    float _pos_x = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.LoadScene( _scene_id, _pos_x );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _scene_id = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.LoadScene( _scene_id );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityMMO.SceneMgr.LoadScene!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ApplyDetector(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    SceneDetectorBase _detector = (SceneDetectorBase)translator.GetObject(L, 2, typeof(SceneDetectorBase));
                    
                    gen_to_be_invoked.ApplyDetector( _detector );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CorrectMainRolePos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.CorrectMainRolePos(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCorrectPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _originPos;translator.Get(L, 2, out _originPos);
                    
                        UnityEngine.Vector3 gen_ret = gen_to_be_invoked.GetCorrectPos( _originPos );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ApplyMainRole(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Unity.Entities.GameObjectEntity _mainRole = (Unity.Entities.GameObjectEntity)translator.GetObject(L, 2, typeof(Unity.Entities.GameObjectEntity));
                    
                    gen_to_be_invoked.ApplyMainRole( _mainRole );
                    
                    
                    
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
                    long _typeID = LuaAPI.lua_toint64(L, 3);
                    string _name = LuaAPI.lua_tostring(L, 4);
                    int _career = LuaAPI.xlua_tointeger(L, 5);
                    UnityEngine.Vector3 _pos;translator.Get(L, 6, out _pos);
                    
                        Unity.Entities.Entity gen_ret = gen_to_be_invoked.AddMainRole( _uid, _typeID, _name, _career, _pos );
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
                    string _content = LuaAPI.lua_tostring(L, 3);
                    
                        Unity.Entities.Entity gen_ret = gen_to_be_invoked.AddSceneObject( _uid, _content );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNameByUID(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    long _uid = LuaAPI.lua_toint64(L, 2);
                    
                        string gen_ret = gen_to_be_invoked.GetNameByUID( _uid );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSceneObjTypeByUID(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    long _uid = LuaAPI.lua_toint64(L, 2);
                    
                        UnityMMO.SceneObjectType gen_ret = gen_to_be_invoked.GetSceneObjTypeByUID( _uid );
                        translator.PushUnityMMOSceneObjectType(L, gen_ret);
                    
                    
                    
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
        static int _g_get_IsLoadingScene(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsLoadingScene);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FreeLookCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.FreeLookCamera);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MainCameraTrans(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MainCameraTrans);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FreeLookCameraTrans(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.FreeLookCameraTrans);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurSceneInfo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.CurSceneInfo);
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
        static int _g_get_detector(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.detector);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_groundLayer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.groundLayer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsLoadingScene(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsLoadingScene = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FreeLookCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FreeLookCamera = (Cinemachine.CinemachineFreeLook)translator.GetObject(L, 2, typeof(Cinemachine.CinemachineFreeLook));
            
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
        static int _s_set_detector(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.detector = (SceneDetectorBase)translator.GetObject(L, 2, typeof(SceneDetectorBase));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_groundLayer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityMMO.SceneMgr gen_to_be_invoked = (UnityMMO.SceneMgr)translator.FastGetCSObj(L, 1);
                UnityEngine.LayerMask gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.groundLayer = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
