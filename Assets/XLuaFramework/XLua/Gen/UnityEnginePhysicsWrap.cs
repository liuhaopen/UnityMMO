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
    public class UnityEnginePhysicsWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Physics);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 35, 17, 16);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "IgnoreCollision", _m_IgnoreCollision_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IgnoreLayerCollision", _m_IgnoreLayerCollision_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetIgnoreLayerCollision", _m_GetIgnoreLayerCollision_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetIgnoreCollision", _m_GetIgnoreCollision_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Raycast", _m_Raycast_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Linecast", _m_Linecast_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CapsuleCast", _m_CapsuleCast_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SphereCast", _m_SphereCast_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BoxCast", _m_BoxCast_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RaycastAll", _m_RaycastAll_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RaycastNonAlloc", _m_RaycastNonAlloc_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CapsuleCastAll", _m_CapsuleCastAll_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SphereCastAll", _m_SphereCastAll_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OverlapCapsule", _m_OverlapCapsule_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OverlapSphere", _m_OverlapSphere_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Simulate", _m_Simulate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SyncTransforms", _m_SyncTransforms_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ComputePenetration", _m_ComputePenetration_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ClosestPoint", _m_ClosestPoint_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OverlapSphereNonAlloc", _m_OverlapSphereNonAlloc_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CheckSphere", _m_CheckSphere_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CapsuleCastNonAlloc", _m_CapsuleCastNonAlloc_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SphereCastNonAlloc", _m_SphereCastNonAlloc_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CheckCapsule", _m_CheckCapsule_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CheckBox", _m_CheckBox_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OverlapBox", _m_OverlapBox_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OverlapBoxNonAlloc", _m_OverlapBoxNonAlloc_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BoxCastNonAlloc", _m_BoxCastNonAlloc_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BoxCastAll", _m_BoxCastAll_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OverlapCapsuleNonAlloc", _m_OverlapCapsuleNonAlloc_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RebuildBroadphaseRegions", _m_RebuildBroadphaseRegions_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "IgnoreRaycastLayer", UnityEngine.Physics.IgnoreRaycastLayer);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "DefaultRaycastLayers", UnityEngine.Physics.DefaultRaycastLayers);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AllLayers", UnityEngine.Physics.AllLayers);
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "gravity", _g_get_gravity);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "defaultContactOffset", _g_get_defaultContactOffset);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "sleepThreshold", _g_get_sleepThreshold);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "queriesHitTriggers", _g_get_queriesHitTriggers);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "queriesHitBackfaces", _g_get_queriesHitBackfaces);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "bounceThreshold", _g_get_bounceThreshold);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "defaultSolverIterations", _g_get_defaultSolverIterations);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "defaultSolverVelocityIterations", _g_get_defaultSolverVelocityIterations);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "defaultMaxAngularSpeed", _g_get_defaultMaxAngularSpeed);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "defaultPhysicsScene", _g_get_defaultPhysicsScene);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "autoSimulation", _g_get_autoSimulation);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "autoSyncTransforms", _g_get_autoSyncTransforms);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "reuseCollisionCallbacks", _g_get_reuseCollisionCallbacks);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "interCollisionDistance", _g_get_interCollisionDistance);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "interCollisionStiffness", _g_get_interCollisionStiffness);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "interCollisionSettingsToggle", _g_get_interCollisionSettingsToggle);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "clothGravity", _g_get_clothGravity);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "gravity", _s_set_gravity);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "defaultContactOffset", _s_set_defaultContactOffset);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "sleepThreshold", _s_set_sleepThreshold);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "queriesHitTriggers", _s_set_queriesHitTriggers);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "queriesHitBackfaces", _s_set_queriesHitBackfaces);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "bounceThreshold", _s_set_bounceThreshold);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "defaultSolverIterations", _s_set_defaultSolverIterations);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "defaultSolverVelocityIterations", _s_set_defaultSolverVelocityIterations);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "defaultMaxAngularSpeed", _s_set_defaultMaxAngularSpeed);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "autoSimulation", _s_set_autoSimulation);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "autoSyncTransforms", _s_set_autoSyncTransforms);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "reuseCollisionCallbacks", _s_set_reuseCollisionCallbacks);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "interCollisionDistance", _s_set_interCollisionDistance);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "interCollisionStiffness", _s_set_interCollisionStiffness);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "interCollisionSettingsToggle", _s_set_interCollisionSettingsToggle);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "clothGravity", _s_set_clothGravity);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					UnityEngine.Physics gen_ret = new UnityEngine.Physics();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IgnoreCollision_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Collider>(L, 1)&& translator.Assignable<UnityEngine.Collider>(L, 2)) 
                {
                    UnityEngine.Collider _collider1 = (UnityEngine.Collider)translator.GetObject(L, 1, typeof(UnityEngine.Collider));
                    UnityEngine.Collider _collider2 = (UnityEngine.Collider)translator.GetObject(L, 2, typeof(UnityEngine.Collider));
                    
                    UnityEngine.Physics.IgnoreCollision( _collider1, _collider2 );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Collider>(L, 1)&& translator.Assignable<UnityEngine.Collider>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Collider _collider1 = (UnityEngine.Collider)translator.GetObject(L, 1, typeof(UnityEngine.Collider));
                    UnityEngine.Collider _collider2 = (UnityEngine.Collider)translator.GetObject(L, 2, typeof(UnityEngine.Collider));
                    bool _ignore = LuaAPI.lua_toboolean(L, 3);
                    
                    UnityEngine.Physics.IgnoreCollision( _collider1, _collider2, _ignore );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.IgnoreCollision!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IgnoreLayerCollision_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _layer1 = LuaAPI.xlua_tointeger(L, 1);
                    int _layer2 = LuaAPI.xlua_tointeger(L, 2);
                    
                    UnityEngine.Physics.IgnoreLayerCollision( _layer1, _layer2 );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    int _layer1 = LuaAPI.xlua_tointeger(L, 1);
                    int _layer2 = LuaAPI.xlua_tointeger(L, 2);
                    bool _ignore = LuaAPI.lua_toboolean(L, 3);
                    
                    UnityEngine.Physics.IgnoreLayerCollision( _layer1, _layer2, _ignore );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.IgnoreLayerCollision!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetIgnoreLayerCollision_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _layer1 = LuaAPI.xlua_tointeger(L, 1);
                    int _layer2 = LuaAPI.xlua_tointeger(L, 2);
                    
                        bool gen_ret = UnityEngine.Physics.GetIgnoreLayerCollision( _layer1, _layer2 );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetIgnoreCollision_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Collider _collider1 = (UnityEngine.Collider)translator.GetObject(L, 1, typeof(UnityEngine.Collider));
                    UnityEngine.Collider _collider2 = (UnityEngine.Collider)translator.GetObject(L, 2, typeof(UnityEngine.Collider));
                    
                        bool gen_ret = UnityEngine.Physics.GetIgnoreCollision( _collider1, _collider2 );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Raycast_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& translator.Assignable<UnityEngine.Ray>(L, 1)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    
                        bool gen_ret = UnityEngine.Physics.Raycast( _ray );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        bool gen_ret = UnityEngine.Physics.Raycast( _ray, _maxDistance );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1&& translator.Assignable<UnityEngine.Ray>(L, 1)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    UnityEngine.RaycastHit _hitInfo;
                    
                        bool gen_ret = UnityEngine.Physics.Raycast( _ray, out _hitInfo );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 2);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 3);
                    
                        bool gen_ret = UnityEngine.Physics.Raycast( _ray, _maxDistance, _layerMask );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    UnityEngine.RaycastHit _hitInfo;
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        bool gen_ret = UnityEngine.Physics.Raycast( _ray, out _hitInfo, _maxDistance );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    UnityEngine.RaycastHit _hitInfo;
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 2);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 3);
                    
                        bool gen_ret = UnityEngine.Physics.Raycast( _ray, out _hitInfo, _maxDistance, _layerMask );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    UnityEngine.Vector3 _direction;translator.Get(L, 2, out _direction);
                    
                        bool gen_ret = UnityEngine.Physics.Raycast( _origin, _direction );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    UnityEngine.Vector3 _direction;translator.Get(L, 2, out _direction);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        bool gen_ret = UnityEngine.Physics.Raycast( _origin, _direction, _maxDistance );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    UnityEngine.Vector3 _direction;translator.Get(L, 2, out _direction);
                    UnityEngine.RaycastHit _hitInfo;
                    
                        bool gen_ret = UnityEngine.Physics.Raycast( _origin, _direction, out _hitInfo );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    UnityEngine.Vector3 _direction;translator.Get(L, 2, out _direction);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    
                        bool gen_ret = UnityEngine.Physics.Raycast( _origin, _direction, _maxDistance, _layerMask );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    UnityEngine.Vector3 _direction;translator.Get(L, 2, out _direction);
                    UnityEngine.RaycastHit _hitInfo;
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        bool gen_ret = UnityEngine.Physics.Raycast( _origin, _direction, out _hitInfo, _maxDistance );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 4)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 2);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 3);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 4, out _queryTriggerInteraction);
                    
                        bool gen_ret = UnityEngine.Physics.Raycast( _ray, _maxDistance, _layerMask, _queryTriggerInteraction );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    UnityEngine.Vector3 _direction;translator.Get(L, 2, out _direction);
                    UnityEngine.RaycastHit _hitInfo;
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    
                        bool gen_ret = UnityEngine.Physics.Raycast( _origin, _direction, out _hitInfo, _maxDistance, _layerMask );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 4)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    UnityEngine.RaycastHit _hitInfo;
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 2);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 3);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 4, out _queryTriggerInteraction);
                    
                        bool gen_ret = UnityEngine.Physics.Raycast( _ray, out _hitInfo, _maxDistance, _layerMask, _queryTriggerInteraction );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 5)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    UnityEngine.Vector3 _direction;translator.Get(L, 2, out _direction);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 5, out _queryTriggerInteraction);
                    
                        bool gen_ret = UnityEngine.Physics.Raycast( _origin, _direction, _maxDistance, _layerMask, _queryTriggerInteraction );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 5)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    UnityEngine.Vector3 _direction;translator.Get(L, 2, out _direction);
                    UnityEngine.RaycastHit _hitInfo;
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 5, out _queryTriggerInteraction);
                    
                        bool gen_ret = UnityEngine.Physics.Raycast( _origin, _direction, out _hitInfo, _maxDistance, _layerMask, _queryTriggerInteraction );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.Raycast!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Linecast_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _start;translator.Get(L, 1, out _start);
                    UnityEngine.Vector3 _end;translator.Get(L, 2, out _end);
                    
                        bool gen_ret = UnityEngine.Physics.Linecast( _start, _end );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector3 _start;translator.Get(L, 1, out _start);
                    UnityEngine.Vector3 _end;translator.Get(L, 2, out _end);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 3);
                    
                        bool gen_ret = UnityEngine.Physics.Linecast( _start, _end, _layerMask );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _start;translator.Get(L, 1, out _start);
                    UnityEngine.Vector3 _end;translator.Get(L, 2, out _end);
                    UnityEngine.RaycastHit _hitInfo;
                    
                        bool gen_ret = UnityEngine.Physics.Linecast( _start, _end, out _hitInfo );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector3 _start;translator.Get(L, 1, out _start);
                    UnityEngine.Vector3 _end;translator.Get(L, 2, out _end);
                    UnityEngine.RaycastHit _hitInfo;
                    int _layerMask = LuaAPI.xlua_tointeger(L, 3);
                    
                        bool gen_ret = UnityEngine.Physics.Linecast( _start, _end, out _hitInfo, _layerMask );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 4)) 
                {
                    UnityEngine.Vector3 _start;translator.Get(L, 1, out _start);
                    UnityEngine.Vector3 _end;translator.Get(L, 2, out _end);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 3);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 4, out _queryTriggerInteraction);
                    
                        bool gen_ret = UnityEngine.Physics.Linecast( _start, _end, _layerMask, _queryTriggerInteraction );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 4)) 
                {
                    UnityEngine.Vector3 _start;translator.Get(L, 1, out _start);
                    UnityEngine.Vector3 _end;translator.Get(L, 2, out _end);
                    UnityEngine.RaycastHit _hitInfo;
                    int _layerMask = LuaAPI.xlua_tointeger(L, 3);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 4, out _queryTriggerInteraction);
                    
                        bool gen_ret = UnityEngine.Physics.Linecast( _start, _end, out _hitInfo, _layerMask, _queryTriggerInteraction );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.Linecast!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CapsuleCast_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)) 
                {
                    UnityEngine.Vector3 _point1;translator.Get(L, 1, out _point1);
                    UnityEngine.Vector3 _point2;translator.Get(L, 2, out _point2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector3 _direction;translator.Get(L, 4, out _direction);
                    
                        bool gen_ret = UnityEngine.Physics.CapsuleCast( _point1, _point2, _radius, _direction );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Vector3 _point1;translator.Get(L, 1, out _point1);
                    UnityEngine.Vector3 _point2;translator.Get(L, 2, out _point2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector3 _direction;translator.Get(L, 4, out _direction);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    
                        bool gen_ret = UnityEngine.Physics.CapsuleCast( _point1, _point2, _radius, _direction, _maxDistance );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)) 
                {
                    UnityEngine.Vector3 _point1;translator.Get(L, 1, out _point1);
                    UnityEngine.Vector3 _point2;translator.Get(L, 2, out _point2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector3 _direction;translator.Get(L, 4, out _direction);
                    UnityEngine.RaycastHit _hitInfo;
                    
                        bool gen_ret = UnityEngine.Physics.CapsuleCast( _point1, _point2, _radius, _direction, out _hitInfo );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    UnityEngine.Vector3 _point1;translator.Get(L, 1, out _point1);
                    UnityEngine.Vector3 _point2;translator.Get(L, 2, out _point2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector3 _direction;translator.Get(L, 4, out _direction);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 6);
                    
                        bool gen_ret = UnityEngine.Physics.CapsuleCast( _point1, _point2, _radius, _direction, _maxDistance, _layerMask );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Vector3 _point1;translator.Get(L, 1, out _point1);
                    UnityEngine.Vector3 _point2;translator.Get(L, 2, out _point2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector3 _direction;translator.Get(L, 4, out _direction);
                    UnityEngine.RaycastHit _hitInfo;
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    
                        bool gen_ret = UnityEngine.Physics.CapsuleCast( _point1, _point2, _radius, _direction, out _hitInfo, _maxDistance );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    UnityEngine.Vector3 _point1;translator.Get(L, 1, out _point1);
                    UnityEngine.Vector3 _point2;translator.Get(L, 2, out _point2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector3 _direction;translator.Get(L, 4, out _direction);
                    UnityEngine.RaycastHit _hitInfo;
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 6);
                    
                        bool gen_ret = UnityEngine.Physics.CapsuleCast( _point1, _point2, _radius, _direction, out _hitInfo, _maxDistance, _layerMask );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 7&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 7)) 
                {
                    UnityEngine.Vector3 _point1;translator.Get(L, 1, out _point1);
                    UnityEngine.Vector3 _point2;translator.Get(L, 2, out _point2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector3 _direction;translator.Get(L, 4, out _direction);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 6);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 7, out _queryTriggerInteraction);
                    
                        bool gen_ret = UnityEngine.Physics.CapsuleCast( _point1, _point2, _radius, _direction, _maxDistance, _layerMask, _queryTriggerInteraction );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 7&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 7)) 
                {
                    UnityEngine.Vector3 _point1;translator.Get(L, 1, out _point1);
                    UnityEngine.Vector3 _point2;translator.Get(L, 2, out _point2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector3 _direction;translator.Get(L, 4, out _direction);
                    UnityEngine.RaycastHit _hitInfo;
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 6);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 7, out _queryTriggerInteraction);
                    
                        bool gen_ret = UnityEngine.Physics.CapsuleCast( _point1, _point2, _radius, _direction, out _hitInfo, _maxDistance, _layerMask, _queryTriggerInteraction );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.CapsuleCast!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SphereCast_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        bool gen_ret = UnityEngine.Physics.SphereCast( _ray, _radius );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        bool gen_ret = UnityEngine.Physics.SphereCast( _ray, _radius, _maxDistance );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.RaycastHit _hitInfo;
                    
                        bool gen_ret = UnityEngine.Physics.SphereCast( _ray, _radius, out _hitInfo );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    
                        bool gen_ret = UnityEngine.Physics.SphereCast( _ray, _radius, _maxDistance, _layerMask );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.RaycastHit _hitInfo;
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        bool gen_ret = UnityEngine.Physics.SphereCast( _ray, _radius, out _hitInfo, _maxDistance );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.RaycastHit _hitInfo;
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    
                        bool gen_ret = UnityEngine.Physics.SphereCast( _ray, _radius, out _hitInfo, _maxDistance, _layerMask );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.RaycastHit _hitInfo;
                    
                        bool gen_ret = UnityEngine.Physics.SphereCast( _origin, _radius, _direction, out _hitInfo );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.RaycastHit _hitInfo;
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        bool gen_ret = UnityEngine.Physics.SphereCast( _origin, _radius, _direction, out _hitInfo, _maxDistance );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 5)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 5, out _queryTriggerInteraction);
                    
                        bool gen_ret = UnityEngine.Physics.SphereCast( _ray, _radius, _maxDistance, _layerMask, _queryTriggerInteraction );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.RaycastHit _hitInfo;
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 4);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 5);
                    
                        bool gen_ret = UnityEngine.Physics.SphereCast( _origin, _radius, _direction, out _hitInfo, _maxDistance, _layerMask );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 5)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.RaycastHit _hitInfo;
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 5, out _queryTriggerInteraction);
                    
                        bool gen_ret = UnityEngine.Physics.SphereCast( _ray, _radius, out _hitInfo, _maxDistance, _layerMask, _queryTriggerInteraction );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 6)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.RaycastHit _hitInfo;
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 4);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 5);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 6, out _queryTriggerInteraction);
                    
                        bool gen_ret = UnityEngine.Physics.SphereCast( _origin, _radius, _direction, out _hitInfo, _maxDistance, _layerMask, _queryTriggerInteraction );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.SphereCast!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BoxCast_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    
                        bool gen_ret = UnityEngine.Physics.BoxCast( _center, _halfExtents, _direction );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.RaycastHit _hitInfo;
                    
                        bool gen_ret = UnityEngine.Physics.BoxCast( _center, _halfExtents, _direction, out _hitInfo );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.Quaternion _orientation;translator.Get(L, 4, out _orientation);
                    
                        bool gen_ret = UnityEngine.Physics.BoxCast( _center, _halfExtents, _direction, _orientation );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.Quaternion _orientation;translator.Get(L, 4, out _orientation);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    
                        bool gen_ret = UnityEngine.Physics.BoxCast( _center, _halfExtents, _direction, _orientation, _maxDistance );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.RaycastHit _hitInfo;
                    UnityEngine.Quaternion _orientation;translator.Get(L, 4, out _orientation);
                    
                        bool gen_ret = UnityEngine.Physics.BoxCast( _center, _halfExtents, _direction, out _hitInfo, _orientation );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.Quaternion _orientation;translator.Get(L, 4, out _orientation);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 6);
                    
                        bool gen_ret = UnityEngine.Physics.BoxCast( _center, _halfExtents, _direction, _orientation, _maxDistance, _layerMask );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.RaycastHit _hitInfo;
                    UnityEngine.Quaternion _orientation;translator.Get(L, 4, out _orientation);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    
                        bool gen_ret = UnityEngine.Physics.BoxCast( _center, _halfExtents, _direction, out _hitInfo, _orientation, _maxDistance );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.RaycastHit _hitInfo;
                    UnityEngine.Quaternion _orientation;translator.Get(L, 4, out _orientation);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 6);
                    
                        bool gen_ret = UnityEngine.Physics.BoxCast( _center, _halfExtents, _direction, out _hitInfo, _orientation, _maxDistance, _layerMask );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 7&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 7)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.Quaternion _orientation;translator.Get(L, 4, out _orientation);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 6);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 7, out _queryTriggerInteraction);
                    
                        bool gen_ret = UnityEngine.Physics.BoxCast( _center, _halfExtents, _direction, _orientation, _maxDistance, _layerMask, _queryTriggerInteraction );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 7&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 7)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.RaycastHit _hitInfo;
                    UnityEngine.Quaternion _orientation;translator.Get(L, 4, out _orientation);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 6);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 7, out _queryTriggerInteraction);
                    
                        bool gen_ret = UnityEngine.Physics.BoxCast( _center, _halfExtents, _direction, out _hitInfo, _orientation, _maxDistance, _layerMask, _queryTriggerInteraction );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hitInfo);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.BoxCast!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RaycastAll_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& translator.Assignable<UnityEngine.Ray>(L, 1)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.RaycastAll( _ray );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.RaycastAll( _ray, _maxDistance );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 2);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 3);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.RaycastAll( _ray, _maxDistance, _layerMask );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    UnityEngine.Vector3 _direction;translator.Get(L, 2, out _direction);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.RaycastAll( _origin, _direction );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    UnityEngine.Vector3 _direction;translator.Get(L, 2, out _direction);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.RaycastAll( _origin, _direction, _maxDistance );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    UnityEngine.Vector3 _direction;translator.Get(L, 2, out _direction);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.RaycastAll( _origin, _direction, _maxDistance, _layerMask );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 4)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 2);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 3);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 4, out _queryTriggerInteraction);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.RaycastAll( _ray, _maxDistance, _layerMask, _queryTriggerInteraction );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 5)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    UnityEngine.Vector3 _direction;translator.Get(L, 2, out _direction);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 5, out _queryTriggerInteraction);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.RaycastAll( _origin, _direction, _maxDistance, _layerMask, _queryTriggerInteraction );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.RaycastAll!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RaycastNonAlloc_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Ray>(L, 1)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 2)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 2, typeof(UnityEngine.RaycastHit[]));
                    
                        int gen_ret = UnityEngine.Physics.RaycastNonAlloc( _ray, _results );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Ray>(L, 1)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 2, typeof(UnityEngine.RaycastHit[]));
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        int gen_ret = UnityEngine.Physics.RaycastNonAlloc( _ray, _results, _maxDistance );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Ray>(L, 1)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 2, typeof(UnityEngine.RaycastHit[]));
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    
                        int gen_ret = UnityEngine.Physics.RaycastNonAlloc( _ray, _results, _maxDistance, _layerMask );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 3)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    UnityEngine.Vector3 _direction;translator.Get(L, 2, out _direction);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 3, typeof(UnityEngine.RaycastHit[]));
                    
                        int gen_ret = UnityEngine.Physics.RaycastNonAlloc( _origin, _direction, _results );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    UnityEngine.Vector3 _direction;translator.Get(L, 2, out _direction);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 3, typeof(UnityEngine.RaycastHit[]));
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        int gen_ret = UnityEngine.Physics.RaycastNonAlloc( _origin, _direction, _results, _maxDistance );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Ray>(L, 1)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 5)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 2, typeof(UnityEngine.RaycastHit[]));
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 5, out _queryTriggerInteraction);
                    
                        int gen_ret = UnityEngine.Physics.RaycastNonAlloc( _ray, _results, _maxDistance, _layerMask, _queryTriggerInteraction );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    UnityEngine.Vector3 _direction;translator.Get(L, 2, out _direction);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 3, typeof(UnityEngine.RaycastHit[]));
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 4);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 5);
                    
                        int gen_ret = UnityEngine.Physics.RaycastNonAlloc( _origin, _direction, _results, _maxDistance, _layerMask );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 6)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    UnityEngine.Vector3 _direction;translator.Get(L, 2, out _direction);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 3, typeof(UnityEngine.RaycastHit[]));
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 4);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 5);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 6, out _queryTriggerInteraction);
                    
                        int gen_ret = UnityEngine.Physics.RaycastNonAlloc( _origin, _direction, _results, _maxDistance, _layerMask, _queryTriggerInteraction );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.RaycastNonAlloc!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CapsuleCastAll_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)) 
                {
                    UnityEngine.Vector3 _point1;translator.Get(L, 1, out _point1);
                    UnityEngine.Vector3 _point2;translator.Get(L, 2, out _point2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector3 _direction;translator.Get(L, 4, out _direction);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.CapsuleCastAll( _point1, _point2, _radius, _direction );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Vector3 _point1;translator.Get(L, 1, out _point1);
                    UnityEngine.Vector3 _point2;translator.Get(L, 2, out _point2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector3 _direction;translator.Get(L, 4, out _direction);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.CapsuleCastAll( _point1, _point2, _radius, _direction, _maxDistance );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    UnityEngine.Vector3 _point1;translator.Get(L, 1, out _point1);
                    UnityEngine.Vector3 _point2;translator.Get(L, 2, out _point2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector3 _direction;translator.Get(L, 4, out _direction);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 6);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.CapsuleCastAll( _point1, _point2, _radius, _direction, _maxDistance, _layerMask );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 7&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 7)) 
                {
                    UnityEngine.Vector3 _point1;translator.Get(L, 1, out _point1);
                    UnityEngine.Vector3 _point2;translator.Get(L, 2, out _point2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector3 _direction;translator.Get(L, 4, out _direction);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 6);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 7, out _queryTriggerInteraction);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.CapsuleCastAll( _point1, _point2, _radius, _direction, _maxDistance, _layerMask, _queryTriggerInteraction );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.CapsuleCastAll!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SphereCastAll_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.SphereCastAll( _ray, _radius );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.SphereCastAll( _ray, _radius, _maxDistance );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.SphereCastAll( _ray, _radius, _maxDistance, _layerMask );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.SphereCastAll( _origin, _radius, _direction );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.SphereCastAll( _origin, _radius, _direction, _maxDistance );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 4);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 5);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.SphereCastAll( _origin, _radius, _direction, _maxDistance, _layerMask );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 5)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 5, out _queryTriggerInteraction);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.SphereCastAll( _ray, _radius, _maxDistance, _layerMask, _queryTriggerInteraction );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 6)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 4);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 5);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 6, out _queryTriggerInteraction);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.SphereCastAll( _origin, _radius, _direction, _maxDistance, _layerMask, _queryTriggerInteraction );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.SphereCastAll!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OverlapCapsule_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector3 _point0;translator.Get(L, 1, out _point0);
                    UnityEngine.Vector3 _point1;translator.Get(L, 2, out _point1);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        UnityEngine.Collider[] gen_ret = UnityEngine.Physics.OverlapCapsule( _point0, _point1, _radius );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 _point0;translator.Get(L, 1, out _point0);
                    UnityEngine.Vector3 _point1;translator.Get(L, 2, out _point1);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    
                        UnityEngine.Collider[] gen_ret = UnityEngine.Physics.OverlapCapsule( _point0, _point1, _radius, _layerMask );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 5)) 
                {
                    UnityEngine.Vector3 _point0;translator.Get(L, 1, out _point0);
                    UnityEngine.Vector3 _point1;translator.Get(L, 2, out _point1);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 5, out _queryTriggerInteraction);
                    
                        UnityEngine.Collider[] gen_ret = UnityEngine.Physics.OverlapCapsule( _point0, _point1, _radius, _layerMask, _queryTriggerInteraction );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.OverlapCapsule!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OverlapSphere_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 1, out _position);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        UnityEngine.Collider[] gen_ret = UnityEngine.Physics.OverlapSphere( _position, _radius );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 1, out _position);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 3);
                    
                        UnityEngine.Collider[] gen_ret = UnityEngine.Physics.OverlapSphere( _position, _radius, _layerMask );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 4)) 
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 1, out _position);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 3);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 4, out _queryTriggerInteraction);
                    
                        UnityEngine.Collider[] gen_ret = UnityEngine.Physics.OverlapSphere( _position, _radius, _layerMask, _queryTriggerInteraction );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.OverlapSphere!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Simulate_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    float _step = (float)LuaAPI.lua_tonumber(L, 1);
                    
                    UnityEngine.Physics.Simulate( _step );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SyncTransforms_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    UnityEngine.Physics.SyncTransforms(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ComputePenetration_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Collider _colliderA = (UnityEngine.Collider)translator.GetObject(L, 1, typeof(UnityEngine.Collider));
                    UnityEngine.Vector3 _positionA;translator.Get(L, 2, out _positionA);
                    UnityEngine.Quaternion _rotationA;translator.Get(L, 3, out _rotationA);
                    UnityEngine.Collider _colliderB = (UnityEngine.Collider)translator.GetObject(L, 4, typeof(UnityEngine.Collider));
                    UnityEngine.Vector3 _positionB;translator.Get(L, 5, out _positionB);
                    UnityEngine.Quaternion _rotationB;translator.Get(L, 6, out _rotationB);
                    UnityEngine.Vector3 _direction;
                    float _distance;
                    
                        bool gen_ret = UnityEngine.Physics.ComputePenetration( _colliderA, _positionA, _rotationA, _colliderB, _positionB, _rotationB, out _direction, out _distance );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.PushUnityEngineVector3(L, _direction);
                        
                    LuaAPI.lua_pushnumber(L, _distance);
                        
                    
                    
                    
                    return 3;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClosestPoint_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 _point;translator.Get(L, 1, out _point);
                    UnityEngine.Collider _collider = (UnityEngine.Collider)translator.GetObject(L, 2, typeof(UnityEngine.Collider));
                    UnityEngine.Vector3 _position;translator.Get(L, 3, out _position);
                    UnityEngine.Quaternion _rotation;translator.Get(L, 4, out _rotation);
                    
                        UnityEngine.Vector3 gen_ret = UnityEngine.Physics.ClosestPoint( _point, _collider, _position, _rotation );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OverlapSphereNonAlloc_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Collider[]>(L, 3)) 
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 1, out _position);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Collider[] _results = (UnityEngine.Collider[])translator.GetObject(L, 3, typeof(UnityEngine.Collider[]));
                    
                        int gen_ret = UnityEngine.Physics.OverlapSphereNonAlloc( _position, _radius, _results );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Collider[]>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 1, out _position);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Collider[] _results = (UnityEngine.Collider[])translator.GetObject(L, 3, typeof(UnityEngine.Collider[]));
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    
                        int gen_ret = UnityEngine.Physics.OverlapSphereNonAlloc( _position, _radius, _results, _layerMask );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Collider[]>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 5)) 
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 1, out _position);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Collider[] _results = (UnityEngine.Collider[])translator.GetObject(L, 3, typeof(UnityEngine.Collider[]));
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 5, out _queryTriggerInteraction);
                    
                        int gen_ret = UnityEngine.Physics.OverlapSphereNonAlloc( _position, _radius, _results, _layerMask, _queryTriggerInteraction );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.OverlapSphereNonAlloc!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckSphere_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 1, out _position);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        bool gen_ret = UnityEngine.Physics.CheckSphere( _position, _radius );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 1, out _position);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 3);
                    
                        bool gen_ret = UnityEngine.Physics.CheckSphere( _position, _radius, _layerMask );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 4)) 
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 1, out _position);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 3);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 4, out _queryTriggerInteraction);
                    
                        bool gen_ret = UnityEngine.Physics.CheckSphere( _position, _radius, _layerMask, _queryTriggerInteraction );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.CheckSphere!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CapsuleCastNonAlloc_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 5)) 
                {
                    UnityEngine.Vector3 _point1;translator.Get(L, 1, out _point1);
                    UnityEngine.Vector3 _point2;translator.Get(L, 2, out _point2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector3 _direction;translator.Get(L, 4, out _direction);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 5, typeof(UnityEngine.RaycastHit[]));
                    
                        int gen_ret = UnityEngine.Physics.CapsuleCastNonAlloc( _point1, _point2, _radius, _direction, _results );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    UnityEngine.Vector3 _point1;translator.Get(L, 1, out _point1);
                    UnityEngine.Vector3 _point2;translator.Get(L, 2, out _point2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector3 _direction;translator.Get(L, 4, out _direction);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 5, typeof(UnityEngine.RaycastHit[]));
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 6);
                    
                        int gen_ret = UnityEngine.Physics.CapsuleCastNonAlloc( _point1, _point2, _radius, _direction, _results, _maxDistance );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 7&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)) 
                {
                    UnityEngine.Vector3 _point1;translator.Get(L, 1, out _point1);
                    UnityEngine.Vector3 _point2;translator.Get(L, 2, out _point2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector3 _direction;translator.Get(L, 4, out _direction);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 5, typeof(UnityEngine.RaycastHit[]));
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 6);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 7);
                    
                        int gen_ret = UnityEngine.Physics.CapsuleCastNonAlloc( _point1, _point2, _radius, _direction, _results, _maxDistance, _layerMask );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 8&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 8)) 
                {
                    UnityEngine.Vector3 _point1;translator.Get(L, 1, out _point1);
                    UnityEngine.Vector3 _point2;translator.Get(L, 2, out _point2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector3 _direction;translator.Get(L, 4, out _direction);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 5, typeof(UnityEngine.RaycastHit[]));
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 6);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 7);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 8, out _queryTriggerInteraction);
                    
                        int gen_ret = UnityEngine.Physics.CapsuleCastNonAlloc( _point1, _point2, _radius, _direction, _results, _maxDistance, _layerMask, _queryTriggerInteraction );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.CapsuleCastNonAlloc!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SphereCastNonAlloc_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 3)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 3, typeof(UnityEngine.RaycastHit[]));
                    
                        int gen_ret = UnityEngine.Physics.SphereCastNonAlloc( _ray, _radius, _results );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 3, typeof(UnityEngine.RaycastHit[]));
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        int gen_ret = UnityEngine.Physics.SphereCastNonAlloc( _ray, _radius, _results, _maxDistance );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 3, typeof(UnityEngine.RaycastHit[]));
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 4);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 5);
                    
                        int gen_ret = UnityEngine.Physics.SphereCastNonAlloc( _ray, _radius, _results, _maxDistance, _layerMask );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 4)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 4, typeof(UnityEngine.RaycastHit[]));
                    
                        int gen_ret = UnityEngine.Physics.SphereCastNonAlloc( _origin, _radius, _direction, _results );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 4, typeof(UnityEngine.RaycastHit[]));
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    
                        int gen_ret = UnityEngine.Physics.SphereCastNonAlloc( _origin, _radius, _direction, _results, _maxDistance );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 4, typeof(UnityEngine.RaycastHit[]));
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 6);
                    
                        int gen_ret = UnityEngine.Physics.SphereCastNonAlloc( _origin, _radius, _direction, _results, _maxDistance, _layerMask );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 6)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 3, typeof(UnityEngine.RaycastHit[]));
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 4);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 5);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 6, out _queryTriggerInteraction);
                    
                        int gen_ret = UnityEngine.Physics.SphereCastNonAlloc( _ray, _radius, _results, _maxDistance, _layerMask, _queryTriggerInteraction );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 7&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 7)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 4, typeof(UnityEngine.RaycastHit[]));
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 6);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 7, out _queryTriggerInteraction);
                    
                        int gen_ret = UnityEngine.Physics.SphereCastNonAlloc( _origin, _radius, _direction, _results, _maxDistance, _layerMask, _queryTriggerInteraction );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.SphereCastNonAlloc!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckCapsule_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector3 _start;translator.Get(L, 1, out _start);
                    UnityEngine.Vector3 _end;translator.Get(L, 2, out _end);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        bool gen_ret = UnityEngine.Physics.CheckCapsule( _start, _end, _radius );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 _start;translator.Get(L, 1, out _start);
                    UnityEngine.Vector3 _end;translator.Get(L, 2, out _end);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    
                        bool gen_ret = UnityEngine.Physics.CheckCapsule( _start, _end, _radius, _layerMask );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 5)) 
                {
                    UnityEngine.Vector3 _start;translator.Get(L, 1, out _start);
                    UnityEngine.Vector3 _end;translator.Get(L, 2, out _end);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 5, out _queryTriggerInteraction);
                    
                        bool gen_ret = UnityEngine.Physics.CheckCapsule( _start, _end, _radius, _layerMask, _queryTriggerInteraction );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.CheckCapsule!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckBox_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    
                        bool gen_ret = UnityEngine.Physics.CheckBox( _center, _halfExtents );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Quaternion>(L, 3)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Quaternion _orientation;translator.Get(L, 3, out _orientation);
                    
                        bool gen_ret = UnityEngine.Physics.CheckBox( _center, _halfExtents, _orientation );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Quaternion>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Quaternion _orientation;translator.Get(L, 3, out _orientation);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    
                        bool gen_ret = UnityEngine.Physics.CheckBox( _center, _halfExtents, _orientation, _layerMask );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Quaternion>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 5)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Quaternion _orientation;translator.Get(L, 3, out _orientation);
                    int _layermask = LuaAPI.xlua_tointeger(L, 4);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 5, out _queryTriggerInteraction);
                    
                        bool gen_ret = UnityEngine.Physics.CheckBox( _center, _halfExtents, _orientation, _layermask, _queryTriggerInteraction );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.CheckBox!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OverlapBox_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    
                        UnityEngine.Collider[] gen_ret = UnityEngine.Physics.OverlapBox( _center, _halfExtents );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Quaternion>(L, 3)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Quaternion _orientation;translator.Get(L, 3, out _orientation);
                    
                        UnityEngine.Collider[] gen_ret = UnityEngine.Physics.OverlapBox( _center, _halfExtents, _orientation );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Quaternion>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Quaternion _orientation;translator.Get(L, 3, out _orientation);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    
                        UnityEngine.Collider[] gen_ret = UnityEngine.Physics.OverlapBox( _center, _halfExtents, _orientation, _layerMask );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Quaternion>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 5)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Quaternion _orientation;translator.Get(L, 3, out _orientation);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 4);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 5, out _queryTriggerInteraction);
                    
                        UnityEngine.Collider[] gen_ret = UnityEngine.Physics.OverlapBox( _center, _halfExtents, _orientation, _layerMask, _queryTriggerInteraction );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.OverlapBox!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OverlapBoxNonAlloc_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Collider[]>(L, 3)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Collider[] _results = (UnityEngine.Collider[])translator.GetObject(L, 3, typeof(UnityEngine.Collider[]));
                    
                        int gen_ret = UnityEngine.Physics.OverlapBoxNonAlloc( _center, _halfExtents, _results );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Collider[]>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Collider[] _results = (UnityEngine.Collider[])translator.GetObject(L, 3, typeof(UnityEngine.Collider[]));
                    UnityEngine.Quaternion _orientation;translator.Get(L, 4, out _orientation);
                    
                        int gen_ret = UnityEngine.Physics.OverlapBoxNonAlloc( _center, _halfExtents, _results, _orientation );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Collider[]>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Collider[] _results = (UnityEngine.Collider[])translator.GetObject(L, 3, typeof(UnityEngine.Collider[]));
                    UnityEngine.Quaternion _orientation;translator.Get(L, 4, out _orientation);
                    int _mask = LuaAPI.xlua_tointeger(L, 5);
                    
                        int gen_ret = UnityEngine.Physics.OverlapBoxNonAlloc( _center, _halfExtents, _results, _orientation, _mask );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Collider[]>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 6)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Collider[] _results = (UnityEngine.Collider[])translator.GetObject(L, 3, typeof(UnityEngine.Collider[]));
                    UnityEngine.Quaternion _orientation;translator.Get(L, 4, out _orientation);
                    int _mask = LuaAPI.xlua_tointeger(L, 5);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 6, out _queryTriggerInteraction);
                    
                        int gen_ret = UnityEngine.Physics.OverlapBoxNonAlloc( _center, _halfExtents, _results, _orientation, _mask, _queryTriggerInteraction );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.OverlapBoxNonAlloc!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BoxCastNonAlloc_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 4)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 4, typeof(UnityEngine.RaycastHit[]));
                    
                        int gen_ret = UnityEngine.Physics.BoxCastNonAlloc( _center, _halfExtents, _direction, _results );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 4)&& translator.Assignable<UnityEngine.Quaternion>(L, 5)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 4, typeof(UnityEngine.RaycastHit[]));
                    UnityEngine.Quaternion _orientation;translator.Get(L, 5, out _orientation);
                    
                        int gen_ret = UnityEngine.Physics.BoxCastNonAlloc( _center, _halfExtents, _direction, _results, _orientation );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 4)&& translator.Assignable<UnityEngine.Quaternion>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 4, typeof(UnityEngine.RaycastHit[]));
                    UnityEngine.Quaternion _orientation;translator.Get(L, 5, out _orientation);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 6);
                    
                        int gen_ret = UnityEngine.Physics.BoxCastNonAlloc( _center, _halfExtents, _direction, _results, _orientation, _maxDistance );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 7&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 4)&& translator.Assignable<UnityEngine.Quaternion>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 4, typeof(UnityEngine.RaycastHit[]));
                    UnityEngine.Quaternion _orientation;translator.Get(L, 5, out _orientation);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 6);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 7);
                    
                        int gen_ret = UnityEngine.Physics.BoxCastNonAlloc( _center, _halfExtents, _direction, _results, _orientation, _maxDistance, _layerMask );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 8&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.RaycastHit[]>(L, 4)&& translator.Assignable<UnityEngine.Quaternion>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 8)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.RaycastHit[] _results = (UnityEngine.RaycastHit[])translator.GetObject(L, 4, typeof(UnityEngine.RaycastHit[]));
                    UnityEngine.Quaternion _orientation;translator.Get(L, 5, out _orientation);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 6);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 7);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 8, out _queryTriggerInteraction);
                    
                        int gen_ret = UnityEngine.Physics.BoxCastNonAlloc( _center, _halfExtents, _direction, _results, _orientation, _maxDistance, _layerMask, _queryTriggerInteraction );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.BoxCastNonAlloc!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BoxCastAll_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.BoxCastAll( _center, _halfExtents, _direction );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.Quaternion _orientation;translator.Get(L, 4, out _orientation);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.BoxCastAll( _center, _halfExtents, _direction, _orientation );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.Quaternion _orientation;translator.Get(L, 4, out _orientation);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.BoxCastAll( _center, _halfExtents, _direction, _orientation, _maxDistance );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.Quaternion _orientation;translator.Get(L, 4, out _orientation);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 6);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.BoxCastAll( _center, _halfExtents, _direction, _orientation, _maxDistance, _layerMask );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 7&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 7)) 
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 1, out _center);
                    UnityEngine.Vector3 _halfExtents;translator.Get(L, 2, out _halfExtents);
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    UnityEngine.Quaternion _orientation;translator.Get(L, 4, out _orientation);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 5);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 6);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 7, out _queryTriggerInteraction);
                    
                        UnityEngine.RaycastHit[] gen_ret = UnityEngine.Physics.BoxCastAll( _center, _halfExtents, _direction, _orientation, _maxDistance, _layerMask, _queryTriggerInteraction );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.BoxCastAll!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OverlapCapsuleNonAlloc_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Collider[]>(L, 4)) 
                {
                    UnityEngine.Vector3 _point0;translator.Get(L, 1, out _point0);
                    UnityEngine.Vector3 _point1;translator.Get(L, 2, out _point1);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Collider[] _results = (UnityEngine.Collider[])translator.GetObject(L, 4, typeof(UnityEngine.Collider[]));
                    
                        int gen_ret = UnityEngine.Physics.OverlapCapsuleNonAlloc( _point0, _point1, _radius, _results );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Collider[]>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Vector3 _point0;translator.Get(L, 1, out _point0);
                    UnityEngine.Vector3 _point1;translator.Get(L, 2, out _point1);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Collider[] _results = (UnityEngine.Collider[])translator.GetObject(L, 4, typeof(UnityEngine.Collider[]));
                    int _layerMask = LuaAPI.xlua_tointeger(L, 5);
                    
                        int gen_ret = UnityEngine.Physics.OverlapCapsuleNonAlloc( _point0, _point1, _radius, _results, _layerMask );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Collider[]>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<UnityEngine.QueryTriggerInteraction>(L, 6)) 
                {
                    UnityEngine.Vector3 _point0;translator.Get(L, 1, out _point0);
                    UnityEngine.Vector3 _point1;translator.Get(L, 2, out _point1);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Collider[] _results = (UnityEngine.Collider[])translator.GetObject(L, 4, typeof(UnityEngine.Collider[]));
                    int _layerMask = LuaAPI.xlua_tointeger(L, 5);
                    UnityEngine.QueryTriggerInteraction _queryTriggerInteraction;translator.Get(L, 6, out _queryTriggerInteraction);
                    
                        int gen_ret = UnityEngine.Physics.OverlapCapsuleNonAlloc( _point0, _point1, _radius, _results, _layerMask, _queryTriggerInteraction );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Physics.OverlapCapsuleNonAlloc!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RebuildBroadphaseRegions_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Bounds _worldBounds;translator.Get(L, 1, out _worldBounds);
                    int _subdivisions = LuaAPI.xlua_tointeger(L, 2);
                    
                    UnityEngine.Physics.RebuildBroadphaseRegions( _worldBounds, _subdivisions );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_gravity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushUnityEngineVector3(L, UnityEngine.Physics.gravity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_defaultContactOffset(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, UnityEngine.Physics.defaultContactOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_sleepThreshold(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, UnityEngine.Physics.sleepThreshold);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_queriesHitTriggers(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, UnityEngine.Physics.queriesHitTriggers);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_queriesHitBackfaces(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, UnityEngine.Physics.queriesHitBackfaces);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_bounceThreshold(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, UnityEngine.Physics.bounceThreshold);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_defaultSolverIterations(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, UnityEngine.Physics.defaultSolverIterations);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_defaultSolverVelocityIterations(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, UnityEngine.Physics.defaultSolverVelocityIterations);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_defaultMaxAngularSpeed(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, UnityEngine.Physics.defaultMaxAngularSpeed);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_defaultPhysicsScene(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, UnityEngine.Physics.defaultPhysicsScene);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_autoSimulation(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, UnityEngine.Physics.autoSimulation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_autoSyncTransforms(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, UnityEngine.Physics.autoSyncTransforms);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_reuseCollisionCallbacks(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, UnityEngine.Physics.reuseCollisionCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_interCollisionDistance(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, UnityEngine.Physics.interCollisionDistance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_interCollisionStiffness(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, UnityEngine.Physics.interCollisionStiffness);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_interCollisionSettingsToggle(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, UnityEngine.Physics.interCollisionSettingsToggle);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_clothGravity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushUnityEngineVector3(L, UnityEngine.Physics.clothGravity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_gravity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Vector3 gen_value;translator.Get(L, 1, out gen_value);
				UnityEngine.Physics.gravity = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_defaultContactOffset(RealStatePtr L)
        {
		    try {
                
			    UnityEngine.Physics.defaultContactOffset = (float)LuaAPI.lua_tonumber(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_sleepThreshold(RealStatePtr L)
        {
		    try {
                
			    UnityEngine.Physics.sleepThreshold = (float)LuaAPI.lua_tonumber(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_queriesHitTriggers(RealStatePtr L)
        {
		    try {
                
			    UnityEngine.Physics.queriesHitTriggers = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_queriesHitBackfaces(RealStatePtr L)
        {
		    try {
                
			    UnityEngine.Physics.queriesHitBackfaces = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_bounceThreshold(RealStatePtr L)
        {
		    try {
                
			    UnityEngine.Physics.bounceThreshold = (float)LuaAPI.lua_tonumber(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_defaultSolverIterations(RealStatePtr L)
        {
		    try {
                
			    UnityEngine.Physics.defaultSolverIterations = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_defaultSolverVelocityIterations(RealStatePtr L)
        {
		    try {
                
			    UnityEngine.Physics.defaultSolverVelocityIterations = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_defaultMaxAngularSpeed(RealStatePtr L)
        {
		    try {
                
			    UnityEngine.Physics.defaultMaxAngularSpeed = (float)LuaAPI.lua_tonumber(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_autoSimulation(RealStatePtr L)
        {
		    try {
                
			    UnityEngine.Physics.autoSimulation = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_autoSyncTransforms(RealStatePtr L)
        {
		    try {
                
			    UnityEngine.Physics.autoSyncTransforms = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_reuseCollisionCallbacks(RealStatePtr L)
        {
		    try {
                
			    UnityEngine.Physics.reuseCollisionCallbacks = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_interCollisionDistance(RealStatePtr L)
        {
		    try {
                
			    UnityEngine.Physics.interCollisionDistance = (float)LuaAPI.lua_tonumber(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_interCollisionStiffness(RealStatePtr L)
        {
		    try {
                
			    UnityEngine.Physics.interCollisionStiffness = (float)LuaAPI.lua_tonumber(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_interCollisionSettingsToggle(RealStatePtr L)
        {
		    try {
                
			    UnityEngine.Physics.interCollisionSettingsToggle = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_clothGravity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Vector3 gen_value;translator.Get(L, 1, out gen_value);
				UnityEngine.Physics.clothGravity = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
