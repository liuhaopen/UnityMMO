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
    public class CinemachineCinemachineFreeLookWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Cinemachine.CinemachineFreeLook);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 16, 15);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetRig", _m_GetRig);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsLiveChild", _m_IsLiveChild);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnTargetObjectWarped", _m_OnTargetObjectWarped);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InternalUpdateCameraState", _m_InternalUpdateCameraState);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnTransitionFromCamera", _m_OnTransitionFromCamera);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLocalPositionForCameraFromInput", _m_GetLocalPositionForCameraFromInput);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "State", _g_get_State);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LookAt", _g_get_LookAt);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Follow", _g_get_Follow);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_LookAt", _g_get_m_LookAt);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_Follow", _g_get_m_Follow);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_CommonLens", _g_get_m_CommonLens);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_Lens", _g_get_m_Lens);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_Transitions", _g_get_m_Transitions);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_YAxis", _g_get_m_YAxis);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_YAxisRecentering", _g_get_m_YAxisRecentering);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_XAxis", _g_get_m_XAxis);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_Heading", _g_get_m_Heading);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_RecenterToTargetHeading", _g_get_m_RecenterToTargetHeading);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_BindingMode", _g_get_m_BindingMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_SplineCurvature", _g_get_m_SplineCurvature);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_Orbits", _g_get_m_Orbits);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "LookAt", _s_set_LookAt);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Follow", _s_set_Follow);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_LookAt", _s_set_m_LookAt);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_Follow", _s_set_m_Follow);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_CommonLens", _s_set_m_CommonLens);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_Lens", _s_set_m_Lens);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_Transitions", _s_set_m_Transitions);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_YAxis", _s_set_m_YAxis);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_YAxisRecentering", _s_set_m_YAxisRecentering);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_XAxis", _s_set_m_XAxis);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_Heading", _s_set_m_Heading);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_RecenterToTargetHeading", _s_set_m_RecenterToTargetHeading);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_BindingMode", _s_set_m_BindingMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_SplineCurvature", _s_set_m_SplineCurvature);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_Orbits", _s_set_m_Orbits);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 3, 2);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "RigNames", _g_get_RigNames);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "CreateRigOverride", _g_get_CreateRigOverride);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "DestroyRigOverride", _g_get_DestroyRigOverride);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "CreateRigOverride", _s_set_CreateRigOverride);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "DestroyRigOverride", _s_set_DestroyRigOverride);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Cinemachine.CinemachineFreeLook gen_ret = new Cinemachine.CinemachineFreeLook();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Cinemachine.CinemachineFreeLook constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRig(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _i = LuaAPI.xlua_tointeger(L, 2);
                    
                        Cinemachine.CinemachineVirtualCamera gen_ret = gen_to_be_invoked.GetRig( _i );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsLiveChild(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<Cinemachine.ICinemachineCamera>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    Cinemachine.ICinemachineCamera _vcam = (Cinemachine.ICinemachineCamera)translator.GetObject(L, 2, typeof(Cinemachine.ICinemachineCamera));
                    bool _dominantChildOnly = LuaAPI.lua_toboolean(L, 3);
                    
                        bool gen_ret = gen_to_be_invoked.IsLiveChild( _vcam, _dominantChildOnly );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<Cinemachine.ICinemachineCamera>(L, 2)) 
                {
                    Cinemachine.ICinemachineCamera _vcam = (Cinemachine.ICinemachineCamera)translator.GetObject(L, 2, typeof(Cinemachine.ICinemachineCamera));
                    
                        bool gen_ret = gen_to_be_invoked.IsLiveChild( _vcam );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Cinemachine.CinemachineFreeLook.IsLiveChild!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnTargetObjectWarped(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3 _positionDelta;translator.Get(L, 3, out _positionDelta);
                    
                    gen_to_be_invoked.OnTargetObjectWarped( _target, _positionDelta );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InternalUpdateCameraState(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _worldUp;translator.Get(L, 2, out _worldUp);
                    float _deltaTime = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.InternalUpdateCameraState( _worldUp, _deltaTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnTransitionFromCamera(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Cinemachine.ICinemachineCamera _fromCam = (Cinemachine.ICinemachineCamera)translator.GetObject(L, 2, typeof(Cinemachine.ICinemachineCamera));
                    UnityEngine.Vector3 _worldUp;translator.Get(L, 3, out _worldUp);
                    float _deltaTime = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.OnTransitionFromCamera( _fromCam, _worldUp, _deltaTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLocalPositionForCameraFromInput(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _t = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        UnityEngine.Vector3 gen_ret = gen_to_be_invoked.GetLocalPositionForCameraFromInput( _t );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RigNames(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Cinemachine.CinemachineFreeLook.RigNames);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_State(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.State);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LookAt(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LookAt);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Follow(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Follow);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_LookAt(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_LookAt);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_Follow(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_Follow);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_CommonLens(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.m_CommonLens);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_Lens(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_Lens);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_Transitions(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_Transitions);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_YAxis(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_YAxis);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_YAxisRecentering(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_YAxisRecentering);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_XAxis(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_XAxis);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_Heading(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_Heading);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_RecenterToTargetHeading(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_RecenterToTargetHeading);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_BindingMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_BindingMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_SplineCurvature(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_SplineCurvature);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_Orbits(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_Orbits);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CreateRigOverride(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Cinemachine.CinemachineFreeLook.CreateRigOverride);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DestroyRigOverride(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Cinemachine.CinemachineFreeLook.DestroyRigOverride);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LookAt(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LookAt = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Follow(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Follow = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_LookAt(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_LookAt = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_Follow(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_Follow = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_CommonLens(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_CommonLens = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_Lens(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                Cinemachine.LensSettings gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_Lens = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_Transitions(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                Cinemachine.CinemachineVirtualCameraBase.TransitionParams gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_Transitions = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_YAxis(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                Cinemachine.AxisState gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_YAxis = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_YAxisRecentering(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                Cinemachine.AxisState.Recentering gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_YAxisRecentering = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_XAxis(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                Cinemachine.AxisState gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_XAxis = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_Heading(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                Cinemachine.CinemachineOrbitalTransposer.Heading gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_Heading = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_RecenterToTargetHeading(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                Cinemachine.AxisState.Recentering gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_RecenterToTargetHeading = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_BindingMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                Cinemachine.CinemachineTransposer.BindingMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_BindingMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_SplineCurvature(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_SplineCurvature = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_Orbits(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineFreeLook gen_to_be_invoked = (Cinemachine.CinemachineFreeLook)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_Orbits = (Cinemachine.CinemachineFreeLook.Orbit[])translator.GetObject(L, 2, typeof(Cinemachine.CinemachineFreeLook.Orbit[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CreateRigOverride(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Cinemachine.CinemachineFreeLook.CreateRigOverride = translator.GetDelegate<Cinemachine.CinemachineFreeLook.CreateRigDelegate>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DestroyRigOverride(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Cinemachine.CinemachineFreeLook.DestroyRigOverride = translator.GetDelegate<Cinemachine.CinemachineFreeLook.DestroyRigDelegate>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
