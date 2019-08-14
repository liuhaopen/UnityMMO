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
    public class UnityEngineAnimatorWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Animator);
			Utils.BeginObjectRegister(type, L, translator, 0, 55, 43, 19);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetFloat", _m_GetFloat);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetFloat", _m_SetFloat);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetBool", _m_GetBool);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetBool", _m_SetBool);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetInteger", _m_GetInteger);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetInteger", _m_SetInteger);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTrigger", _m_SetTrigger);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetTrigger", _m_ResetTrigger);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsParameterControlledByCurve", _m_IsParameterControlledByCurve);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetIKPosition", _m_GetIKPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetIKPosition", _m_SetIKPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetIKRotation", _m_GetIKRotation);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetIKRotation", _m_SetIKRotation);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetIKPositionWeight", _m_GetIKPositionWeight);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetIKPositionWeight", _m_SetIKPositionWeight);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetIKRotationWeight", _m_GetIKRotationWeight);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetIKRotationWeight", _m_SetIKRotationWeight);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetIKHintPosition", _m_GetIKHintPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetIKHintPosition", _m_SetIKHintPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetIKHintPositionWeight", _m_GetIKHintPositionWeight);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetIKHintPositionWeight", _m_SetIKHintPositionWeight);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLookAtPosition", _m_SetLookAtPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLookAtWeight", _m_SetLookAtWeight);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetBoneLocalRotation", _m_SetBoneLocalRotation);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetBehaviours", _m_GetBehaviours);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLayerName", _m_GetLayerName);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLayerIndex", _m_GetLayerIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLayerWeight", _m_GetLayerWeight);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLayerWeight", _m_SetLayerWeight);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCurrentAnimatorStateInfo", _m_GetCurrentAnimatorStateInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetNextAnimatorStateInfo", _m_GetNextAnimatorStateInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAnimatorTransitionInfo", _m_GetAnimatorTransitionInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCurrentAnimatorClipInfoCount", _m_GetCurrentAnimatorClipInfoCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetNextAnimatorClipInfoCount", _m_GetNextAnimatorClipInfoCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCurrentAnimatorClipInfo", _m_GetCurrentAnimatorClipInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetNextAnimatorClipInfo", _m_GetNextAnimatorClipInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsInTransition", _m_IsInTransition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetParameter", _m_GetParameter);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MatchTarget", _m_MatchTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InterruptMatchTarget", _m_InterruptMatchTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CrossFadeInFixedTime", _m_CrossFadeInFixedTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteDefaultValues", _m_WriteDefaultValues);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CrossFade", _m_CrossFade);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayInFixedTime", _m_PlayInFixedTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Play", _m_Play);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTarget", _m_SetTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetBoneTransform", _m_GetBoneTransform);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StartPlayback", _m_StartPlayback);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopPlayback", _m_StopPlayback);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StartRecording", _m_StartRecording);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopRecording", _m_StopRecording);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HasState", _m_HasState);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Rebind", _m_Rebind);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ApplyBuiltinRootMotion", _m_ApplyBuiltinRootMotion);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "isOptimizable", _g_get_isOptimizable);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isHuman", _g_get_isHuman);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "hasRootMotion", _g_get_hasRootMotion);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "humanScale", _g_get_humanScale);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isInitialized", _g_get_isInitialized);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "deltaPosition", _g_get_deltaPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "deltaRotation", _g_get_deltaRotation);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "velocity", _g_get_velocity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "angularVelocity", _g_get_angularVelocity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "rootPosition", _g_get_rootPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "rootRotation", _g_get_rootRotation);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "applyRootMotion", _g_get_applyRootMotion);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "updateMode", _g_get_updateMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "hasTransformHierarchy", _g_get_hasTransformHierarchy);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "gravityWeight", _g_get_gravityWeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "bodyPosition", _g_get_bodyPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "bodyRotation", _g_get_bodyRotation);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "stabilizeFeet", _g_get_stabilizeFeet);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "layerCount", _g_get_layerCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "parameters", _g_get_parameters);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "parameterCount", _g_get_parameterCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "feetPivotActive", _g_get_feetPivotActive);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pivotWeight", _g_get_pivotWeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pivotPosition", _g_get_pivotPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isMatchingTarget", _g_get_isMatchingTarget);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "speed", _g_get_speed);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "targetPosition", _g_get_targetPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "targetRotation", _g_get_targetRotation);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cullingMode", _g_get_cullingMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "playbackTime", _g_get_playbackTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "recorderStartTime", _g_get_recorderStartTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "recorderStopTime", _g_get_recorderStopTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "recorderMode", _g_get_recorderMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "runtimeAnimatorController", _g_get_runtimeAnimatorController);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "hasBoundPlayables", _g_get_hasBoundPlayables);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "avatar", _g_get_avatar);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "playableGraph", _g_get_playableGraph);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "layersAffectMassCenter", _g_get_layersAffectMassCenter);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "leftFeetBottomHeight", _g_get_leftFeetBottomHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "rightFeetBottomHeight", _g_get_rightFeetBottomHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "logWarnings", _g_get_logWarnings);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "fireEvents", _g_get_fireEvents);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "keepAnimatorControllerStateOnDisable", _g_get_keepAnimatorControllerStateOnDisable);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "rootPosition", _s_set_rootPosition);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "rootRotation", _s_set_rootRotation);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "applyRootMotion", _s_set_applyRootMotion);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "updateMode", _s_set_updateMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "bodyPosition", _s_set_bodyPosition);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "bodyRotation", _s_set_bodyRotation);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "stabilizeFeet", _s_set_stabilizeFeet);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "feetPivotActive", _s_set_feetPivotActive);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "speed", _s_set_speed);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cullingMode", _s_set_cullingMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "playbackTime", _s_set_playbackTime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "recorderStartTime", _s_set_recorderStartTime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "recorderStopTime", _s_set_recorderStopTime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "runtimeAnimatorController", _s_set_runtimeAnimatorController);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "avatar", _s_set_avatar);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "layersAffectMassCenter", _s_set_layersAffectMassCenter);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "logWarnings", _s_set_logWarnings);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "fireEvents", _s_set_fireEvents);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "keepAnimatorControllerStateOnDisable", _s_set_keepAnimatorControllerStateOnDisable);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "StringToHash", _m_StringToHash_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					UnityEngine.Animator gen_ret = new UnityEngine.Animator();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Animator constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFloat(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                        float gen_ret = gen_to_be_invoked.GetFloat( _id );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    
                        float gen_ret = gen_to_be_invoked.GetFloat( _name );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Animator.GetFloat!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetFloat(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    float _value = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetFloat( _id, _value );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    float _value = (float)LuaAPI.lua_tonumber(L, 3);
                    float _dampTime = (float)LuaAPI.lua_tonumber(L, 4);
                    float _deltaTime = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.SetFloat( _id, _value, _dampTime, _deltaTime );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    float _value = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetFloat( _name, _value );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    float _value = (float)LuaAPI.lua_tonumber(L, 3);
                    float _dampTime = (float)LuaAPI.lua_tonumber(L, 4);
                    float _deltaTime = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.SetFloat( _name, _value, _dampTime, _deltaTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Animator.SetFloat!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBool(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.GetBool( _id );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.GetBool( _name );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Animator.GetBool!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetBool(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    bool _value = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.SetBool( _id, _value );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    bool _value = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.SetBool( _name, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Animator.SetBool!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInteger(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                        int gen_ret = gen_to_be_invoked.GetInteger( _id );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    
                        int gen_ret = gen_to_be_invoked.GetInteger( _name );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Animator.GetInteger!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetInteger(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    int _value = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.SetInteger( _id, _value );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    int _value = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.SetInteger( _name, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Animator.SetInteger!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTrigger(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.SetTrigger( _id );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.SetTrigger( _name );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Animator.SetTrigger!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetTrigger(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.ResetTrigger( _id );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.ResetTrigger( _name );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Animator.ResetTrigger!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsParameterControlledByCurve(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.IsParameterControlledByCurve( _id );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.IsParameterControlledByCurve( _name );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Animator.IsParameterControlledByCurve!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetIKPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AvatarIKGoal _goal;translator.Get(L, 2, out _goal);
                    
                        UnityEngine.Vector3 gen_ret = gen_to_be_invoked.GetIKPosition( _goal );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetIKPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AvatarIKGoal _goal;translator.Get(L, 2, out _goal);
                    UnityEngine.Vector3 _goalPosition;translator.Get(L, 3, out _goalPosition);
                    
                    gen_to_be_invoked.SetIKPosition( _goal, _goalPosition );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetIKRotation(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AvatarIKGoal _goal;translator.Get(L, 2, out _goal);
                    
                        UnityEngine.Quaternion gen_ret = gen_to_be_invoked.GetIKRotation( _goal );
                        translator.PushUnityEngineQuaternion(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetIKRotation(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AvatarIKGoal _goal;translator.Get(L, 2, out _goal);
                    UnityEngine.Quaternion _goalRotation;translator.Get(L, 3, out _goalRotation);
                    
                    gen_to_be_invoked.SetIKRotation( _goal, _goalRotation );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetIKPositionWeight(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AvatarIKGoal _goal;translator.Get(L, 2, out _goal);
                    
                        float gen_ret = gen_to_be_invoked.GetIKPositionWeight( _goal );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetIKPositionWeight(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AvatarIKGoal _goal;translator.Get(L, 2, out _goal);
                    float _value = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetIKPositionWeight( _goal, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetIKRotationWeight(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AvatarIKGoal _goal;translator.Get(L, 2, out _goal);
                    
                        float gen_ret = gen_to_be_invoked.GetIKRotationWeight( _goal );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetIKRotationWeight(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AvatarIKGoal _goal;translator.Get(L, 2, out _goal);
                    float _value = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetIKRotationWeight( _goal, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetIKHintPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AvatarIKHint _hint;translator.Get(L, 2, out _hint);
                    
                        UnityEngine.Vector3 gen_ret = gen_to_be_invoked.GetIKHintPosition( _hint );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetIKHintPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AvatarIKHint _hint;translator.Get(L, 2, out _hint);
                    UnityEngine.Vector3 _hintPosition;translator.Get(L, 3, out _hintPosition);
                    
                    gen_to_be_invoked.SetIKHintPosition( _hint, _hintPosition );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetIKHintPositionWeight(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AvatarIKHint _hint;translator.Get(L, 2, out _hint);
                    
                        float gen_ret = gen_to_be_invoked.GetIKHintPositionWeight( _hint );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetIKHintPositionWeight(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AvatarIKHint _hint;translator.Get(L, 2, out _hint);
                    float _value = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetIKHintPositionWeight( _hint, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLookAtPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _lookAtPosition;translator.Get(L, 2, out _lookAtPosition);
                    
                    gen_to_be_invoked.SetLookAtPosition( _lookAtPosition );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLookAtWeight(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    float _weight = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.SetLookAtWeight( _weight );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    float _weight = (float)LuaAPI.lua_tonumber(L, 2);
                    float _bodyWeight = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetLookAtWeight( _weight, _bodyWeight );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    float _weight = (float)LuaAPI.lua_tonumber(L, 2);
                    float _bodyWeight = (float)LuaAPI.lua_tonumber(L, 3);
                    float _headWeight = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.SetLookAtWeight( _weight, _bodyWeight, _headWeight );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    float _weight = (float)LuaAPI.lua_tonumber(L, 2);
                    float _bodyWeight = (float)LuaAPI.lua_tonumber(L, 3);
                    float _headWeight = (float)LuaAPI.lua_tonumber(L, 4);
                    float _eyesWeight = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.SetLookAtWeight( _weight, _bodyWeight, _headWeight, _eyesWeight );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    float _weight = (float)LuaAPI.lua_tonumber(L, 2);
                    float _bodyWeight = (float)LuaAPI.lua_tonumber(L, 3);
                    float _headWeight = (float)LuaAPI.lua_tonumber(L, 4);
                    float _eyesWeight = (float)LuaAPI.lua_tonumber(L, 5);
                    float _clampWeight = (float)LuaAPI.lua_tonumber(L, 6);
                    
                    gen_to_be_invoked.SetLookAtWeight( _weight, _bodyWeight, _headWeight, _eyesWeight, _clampWeight );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Animator.SetLookAtWeight!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetBoneLocalRotation(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.HumanBodyBones _humanBoneId;translator.Get(L, 2, out _humanBoneId);
                    UnityEngine.Quaternion _rotation;translator.Get(L, 3, out _rotation);
                    
                    gen_to_be_invoked.SetBoneLocalRotation( _humanBoneId, _rotation );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBehaviours(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _fullPathHash = LuaAPI.xlua_tointeger(L, 2);
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 3);
                    
                        UnityEngine.StateMachineBehaviour[] gen_ret = gen_to_be_invoked.GetBehaviours( _fullPathHash, _layerIndex );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLayerName(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        string gen_ret = gen_to_be_invoked.GetLayerName( _layerIndex );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLayerIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _layerName = LuaAPI.lua_tostring(L, 2);
                    
                        int gen_ret = gen_to_be_invoked.GetLayerIndex( _layerName );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLayerWeight(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        float gen_ret = gen_to_be_invoked.GetLayerWeight( _layerIndex );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLayerWeight(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 2);
                    float _weight = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetLayerWeight( _layerIndex, _weight );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCurrentAnimatorStateInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        UnityEngine.AnimatorStateInfo gen_ret = gen_to_be_invoked.GetCurrentAnimatorStateInfo( _layerIndex );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNextAnimatorStateInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        UnityEngine.AnimatorStateInfo gen_ret = gen_to_be_invoked.GetNextAnimatorStateInfo( _layerIndex );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAnimatorTransitionInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        UnityEngine.AnimatorTransitionInfo gen_ret = gen_to_be_invoked.GetAnimatorTransitionInfo( _layerIndex );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCurrentAnimatorClipInfoCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        int gen_ret = gen_to_be_invoked.GetCurrentAnimatorClipInfoCount( _layerIndex );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNextAnimatorClipInfoCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        int gen_ret = gen_to_be_invoked.GetNextAnimatorClipInfoCount( _layerIndex );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCurrentAnimatorClipInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        UnityEngine.AnimatorClipInfo[] gen_ret = gen_to_be_invoked.GetCurrentAnimatorClipInfo( _layerIndex );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.Collections.Generic.List<UnityEngine.AnimatorClipInfo>>(L, 3)) 
                {
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 2);
                    System.Collections.Generic.List<UnityEngine.AnimatorClipInfo> _clips = (System.Collections.Generic.List<UnityEngine.AnimatorClipInfo>)translator.GetObject(L, 3, typeof(System.Collections.Generic.List<UnityEngine.AnimatorClipInfo>));
                    
                    gen_to_be_invoked.GetCurrentAnimatorClipInfo( _layerIndex, _clips );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Animator.GetCurrentAnimatorClipInfo!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNextAnimatorClipInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        UnityEngine.AnimatorClipInfo[] gen_ret = gen_to_be_invoked.GetNextAnimatorClipInfo( _layerIndex );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.Collections.Generic.List<UnityEngine.AnimatorClipInfo>>(L, 3)) 
                {
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 2);
                    System.Collections.Generic.List<UnityEngine.AnimatorClipInfo> _clips = (System.Collections.Generic.List<UnityEngine.AnimatorClipInfo>)translator.GetObject(L, 3, typeof(System.Collections.Generic.List<UnityEngine.AnimatorClipInfo>));
                    
                    gen_to_be_invoked.GetNextAnimatorClipInfo( _layerIndex, _clips );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Animator.GetNextAnimatorClipInfo!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsInTransition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.IsInTransition( _layerIndex );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetParameter(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        UnityEngine.AnimatorControllerParameter gen_ret = gen_to_be_invoked.GetParameter( _index );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MatchTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Quaternion>(L, 3)&& translator.Assignable<UnityEngine.AvatarTarget>(L, 4)&& translator.Assignable<UnityEngine.MatchTargetWeightMask>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    UnityEngine.Vector3 _matchPosition;translator.Get(L, 2, out _matchPosition);
                    UnityEngine.Quaternion _matchRotation;translator.Get(L, 3, out _matchRotation);
                    UnityEngine.AvatarTarget _targetBodyPart;translator.Get(L, 4, out _targetBodyPart);
                    UnityEngine.MatchTargetWeightMask _weightMask;translator.Get(L, 5, out _weightMask);
                    float _startNormalizedTime = (float)LuaAPI.lua_tonumber(L, 6);
                    
                    gen_to_be_invoked.MatchTarget( _matchPosition, _matchRotation, _targetBodyPart, _weightMask, _startNormalizedTime );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 7&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Quaternion>(L, 3)&& translator.Assignable<UnityEngine.AvatarTarget>(L, 4)&& translator.Assignable<UnityEngine.MatchTargetWeightMask>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)) 
                {
                    UnityEngine.Vector3 _matchPosition;translator.Get(L, 2, out _matchPosition);
                    UnityEngine.Quaternion _matchRotation;translator.Get(L, 3, out _matchRotation);
                    UnityEngine.AvatarTarget _targetBodyPart;translator.Get(L, 4, out _targetBodyPart);
                    UnityEngine.MatchTargetWeightMask _weightMask;translator.Get(L, 5, out _weightMask);
                    float _startNormalizedTime = (float)LuaAPI.lua_tonumber(L, 6);
                    float _targetNormalizedTime = (float)LuaAPI.lua_tonumber(L, 7);
                    
                    gen_to_be_invoked.MatchTarget( _matchPosition, _matchRotation, _targetBodyPart, _weightMask, _startNormalizedTime, _targetNormalizedTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Animator.MatchTarget!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InterruptMatchTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.InterruptMatchTarget(  );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _completeMatch = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.InterruptMatchTarget( _completeMatch );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Animator.InterruptMatchTarget!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CrossFadeInFixedTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _stateHashName = LuaAPI.xlua_tointeger(L, 2);
                    float _fixedTransitionDuration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.CrossFadeInFixedTime( _stateHashName, _fixedTransitionDuration );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    int _stateHashName = LuaAPI.xlua_tointeger(L, 2);
                    float _fixedTransitionDuration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layer = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.CrossFadeInFixedTime( _stateHashName, _fixedTransitionDuration, _layer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    int _stateHashName = LuaAPI.xlua_tointeger(L, 2);
                    float _fixedTransitionDuration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layer = LuaAPI.xlua_tointeger(L, 4);
                    float _fixedTimeOffset = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.CrossFadeInFixedTime( _stateHashName, _fixedTransitionDuration, _layer, _fixedTimeOffset );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    int _stateHashName = LuaAPI.xlua_tointeger(L, 2);
                    float _fixedTransitionDuration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layer = LuaAPI.xlua_tointeger(L, 4);
                    float _fixedTimeOffset = (float)LuaAPI.lua_tonumber(L, 5);
                    float _normalizedTransitionTime = (float)LuaAPI.lua_tonumber(L, 6);
                    
                    gen_to_be_invoked.CrossFadeInFixedTime( _stateHashName, _fixedTransitionDuration, _layer, _fixedTimeOffset, _normalizedTransitionTime );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string _stateName = LuaAPI.lua_tostring(L, 2);
                    float _fixedTransitionDuration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.CrossFadeInFixedTime( _stateName, _fixedTransitionDuration );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    string _stateName = LuaAPI.lua_tostring(L, 2);
                    float _fixedTransitionDuration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layer = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.CrossFadeInFixedTime( _stateName, _fixedTransitionDuration, _layer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    string _stateName = LuaAPI.lua_tostring(L, 2);
                    float _fixedTransitionDuration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layer = LuaAPI.xlua_tointeger(L, 4);
                    float _fixedTimeOffset = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.CrossFadeInFixedTime( _stateName, _fixedTransitionDuration, _layer, _fixedTimeOffset );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    string _stateName = LuaAPI.lua_tostring(L, 2);
                    float _fixedTransitionDuration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layer = LuaAPI.xlua_tointeger(L, 4);
                    float _fixedTimeOffset = (float)LuaAPI.lua_tonumber(L, 5);
                    float _normalizedTransitionTime = (float)LuaAPI.lua_tonumber(L, 6);
                    
                    gen_to_be_invoked.CrossFadeInFixedTime( _stateName, _fixedTransitionDuration, _layer, _fixedTimeOffset, _normalizedTransitionTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Animator.CrossFadeInFixedTime!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteDefaultValues(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.WriteDefaultValues(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CrossFade(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _stateHashName = LuaAPI.xlua_tointeger(L, 2);
                    float _normalizedTransitionDuration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.CrossFade( _stateHashName, _normalizedTransitionDuration );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    int _stateHashName = LuaAPI.xlua_tointeger(L, 2);
                    float _normalizedTransitionDuration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layer = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.CrossFade( _stateHashName, _normalizedTransitionDuration, _layer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    int _stateHashName = LuaAPI.xlua_tointeger(L, 2);
                    float _normalizedTransitionDuration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layer = LuaAPI.xlua_tointeger(L, 4);
                    float _normalizedTimeOffset = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.CrossFade( _stateHashName, _normalizedTransitionDuration, _layer, _normalizedTimeOffset );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    int _stateHashName = LuaAPI.xlua_tointeger(L, 2);
                    float _normalizedTransitionDuration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layer = LuaAPI.xlua_tointeger(L, 4);
                    float _normalizedTimeOffset = (float)LuaAPI.lua_tonumber(L, 5);
                    float _normalizedTransitionTime = (float)LuaAPI.lua_tonumber(L, 6);
                    
                    gen_to_be_invoked.CrossFade( _stateHashName, _normalizedTransitionDuration, _layer, _normalizedTimeOffset, _normalizedTransitionTime );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string _stateName = LuaAPI.lua_tostring(L, 2);
                    float _normalizedTransitionDuration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.CrossFade( _stateName, _normalizedTransitionDuration );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    string _stateName = LuaAPI.lua_tostring(L, 2);
                    float _normalizedTransitionDuration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layer = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.CrossFade( _stateName, _normalizedTransitionDuration, _layer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    string _stateName = LuaAPI.lua_tostring(L, 2);
                    float _normalizedTransitionDuration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layer = LuaAPI.xlua_tointeger(L, 4);
                    float _normalizedTimeOffset = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.CrossFade( _stateName, _normalizedTransitionDuration, _layer, _normalizedTimeOffset );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    string _stateName = LuaAPI.lua_tostring(L, 2);
                    float _normalizedTransitionDuration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _layer = LuaAPI.xlua_tointeger(L, 4);
                    float _normalizedTimeOffset = (float)LuaAPI.lua_tonumber(L, 5);
                    float _normalizedTransitionTime = (float)LuaAPI.lua_tonumber(L, 6);
                    
                    gen_to_be_invoked.CrossFade( _stateName, _normalizedTransitionDuration, _layer, _normalizedTimeOffset, _normalizedTransitionTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Animator.CrossFade!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayInFixedTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _stateNameHash = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.PlayInFixedTime( _stateNameHash );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _stateNameHash = LuaAPI.xlua_tointeger(L, 2);
                    int _layer = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.PlayInFixedTime( _stateNameHash, _layer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    int _stateNameHash = LuaAPI.xlua_tointeger(L, 2);
                    int _layer = LuaAPI.xlua_tointeger(L, 3);
                    float _fixedTime = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.PlayInFixedTime( _stateNameHash, _layer, _fixedTime );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _stateName = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.PlayInFixedTime( _stateName );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string _stateName = LuaAPI.lua_tostring(L, 2);
                    int _layer = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.PlayInFixedTime( _stateName, _layer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    string _stateName = LuaAPI.lua_tostring(L, 2);
                    int _layer = LuaAPI.xlua_tointeger(L, 3);
                    float _fixedTime = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.PlayInFixedTime( _stateName, _layer, _fixedTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Animator.PlayInFixedTime!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Play(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _stateNameHash = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.Play( _stateNameHash );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _stateNameHash = LuaAPI.xlua_tointeger(L, 2);
                    int _layer = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.Play( _stateNameHash, _layer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    int _stateNameHash = LuaAPI.xlua_tointeger(L, 2);
                    int _layer = LuaAPI.xlua_tointeger(L, 3);
                    float _normalizedTime = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.Play( _stateNameHash, _layer, _normalizedTime );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _stateName = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.Play( _stateName );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string _stateName = LuaAPI.lua_tostring(L, 2);
                    int _layer = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.Play( _stateName, _layer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    string _stateName = LuaAPI.lua_tostring(L, 2);
                    int _layer = LuaAPI.xlua_tointeger(L, 3);
                    float _normalizedTime = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.Play( _stateName, _layer, _normalizedTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Animator.Play!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AvatarTarget _targetIndex;translator.Get(L, 2, out _targetIndex);
                    float _targetNormalizedTime = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetTarget( _targetIndex, _targetNormalizedTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBoneTransform(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.HumanBodyBones _humanBoneId;translator.Get(L, 2, out _humanBoneId);
                    
                        UnityEngine.Transform gen_ret = gen_to_be_invoked.GetBoneTransform( _humanBoneId );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartPlayback(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.StartPlayback(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopPlayback(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.StopPlayback(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartRecording(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _frameCount = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.StartRecording( _frameCount );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopRecording(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.StopRecording(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasState(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 2);
                    int _stateID = LuaAPI.xlua_tointeger(L, 3);
                    
                        bool gen_ret = gen_to_be_invoked.HasState( _layerIndex, _stateID );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StringToHash_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    
                        int gen_ret = UnityEngine.Animator.StringToHash( _name );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _deltaTime = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.Update( _deltaTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Rebind(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Rebind(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ApplyBuiltinRootMotion(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ApplyBuiltinRootMotion(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isOptimizable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isOptimizable);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isHuman(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isHuman);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_hasRootMotion(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.hasRootMotion);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_humanScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.humanScale);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isInitialized(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isInitialized);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_deltaPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.deltaPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_deltaRotation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineQuaternion(L, gen_to_be_invoked.deltaRotation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_velocity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.velocity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_angularVelocity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.angularVelocity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_rootPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.rootPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_rootRotation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineQuaternion(L, gen_to_be_invoked.rootRotation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_applyRootMotion(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.applyRootMotion);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_updateMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.updateMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_hasTransformHierarchy(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.hasTransformHierarchy);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_gravityWeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.gravityWeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_bodyPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.bodyPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_bodyRotation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineQuaternion(L, gen_to_be_invoked.bodyRotation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_stabilizeFeet(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.stabilizeFeet);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_layerCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.layerCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_parameters(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.parameters);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_parameterCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.parameterCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_feetPivotActive(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.feetPivotActive);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pivotWeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.pivotWeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pivotPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.pivotPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isMatchingTarget(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isMatchingTarget);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_speed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.speed);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_targetPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.targetPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_targetRotation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineQuaternion(L, gen_to_be_invoked.targetRotation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cullingMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.cullingMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_playbackTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.playbackTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_recorderStartTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.recorderStartTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_recorderStopTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.recorderStopTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_recorderMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.recorderMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_runtimeAnimatorController(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.runtimeAnimatorController);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_hasBoundPlayables(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.hasBoundPlayables);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_avatar(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.avatar);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_playableGraph(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.playableGraph);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_layersAffectMassCenter(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.layersAffectMassCenter);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_leftFeetBottomHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.leftFeetBottomHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_rightFeetBottomHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.rightFeetBottomHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_logWarnings(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.logWarnings);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_fireEvents(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.fireEvents);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_keepAnimatorControllerStateOnDisable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.keepAnimatorControllerStateOnDisable);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_rootPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.rootPosition = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_rootRotation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                UnityEngine.Quaternion gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.rootRotation = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_applyRootMotion(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.applyRootMotion = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_updateMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                UnityEngine.AnimatorUpdateMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.updateMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_bodyPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.bodyPosition = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_bodyRotation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                UnityEngine.Quaternion gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.bodyRotation = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_stabilizeFeet(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.stabilizeFeet = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_feetPivotActive(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.feetPivotActive = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_speed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.speed = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cullingMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                UnityEngine.AnimatorCullingMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.cullingMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_playbackTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.playbackTime = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_recorderStartTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.recorderStartTime = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_recorderStopTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.recorderStopTime = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_runtimeAnimatorController(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.runtimeAnimatorController = (UnityEngine.RuntimeAnimatorController)translator.GetObject(L, 2, typeof(UnityEngine.RuntimeAnimatorController));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_avatar(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.avatar = (UnityEngine.Avatar)translator.GetObject(L, 2, typeof(UnityEngine.Avatar));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_layersAffectMassCenter(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.layersAffectMassCenter = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_logWarnings(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.logWarnings = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_fireEvents(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.fireEvents = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_keepAnimatorControllerStateOnDisable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Animator gen_to_be_invoked = (UnityEngine.Animator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.keepAnimatorControllerStateOnDisable = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
