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
    public class UnityMMOSkillManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityMMO.SkillManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 15, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCurAttackID", _m_GetCurAttackID);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetCombo", _m_ResetCombo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IncreaseCombo", _m_IncreaseCombo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSkillIDByIndex", _m_GetSkillIDByIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetSkillCD", _m_SetSkillCD);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSkillCD", _m_GetSkillCD);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSkillResPath", _m_GetSkillResPath);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCareerBySkillID", _m_GetCareerBySkillID);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSceneObjTypeBySkillID", _m_GetSceneObjTypeBySkillID);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsSkillInCD", _m_IsSkillInCD);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsNormalAttack", _m_IsNormalAttack);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CastSkillByIndex", _m_CastSkillByIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CastRandomSkill", _m_CastRandomSkill);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CastSkill", _m_CastSkill);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetInstance", _m_GetInstance_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "UnityMMO.SkillManager does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInstance_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        UnityMMO.SkillManager gen_ret = UnityMMO.SkillManager.GetInstance(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
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
            
            
                UnityMMO.SkillManager gen_to_be_invoked = (UnityMMO.SkillManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _career = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.Init( _career );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCurAttackID(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SkillManager gen_to_be_invoked = (UnityMMO.SkillManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        int gen_ret = gen_to_be_invoked.GetCurAttackID(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetCombo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SkillManager gen_to_be_invoked = (UnityMMO.SkillManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetCombo(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IncreaseCombo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SkillManager gen_to_be_invoked = (UnityMMO.SkillManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.IncreaseCombo(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSkillIDByIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SkillManager gen_to_be_invoked = (UnityMMO.SkillManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _skillIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        int gen_ret = gen_to_be_invoked.GetSkillIDByIndex( _skillIndex );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetSkillCD(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SkillManager gen_to_be_invoked = (UnityMMO.SkillManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _skillID = LuaAPI.xlua_tointeger(L, 2);
                    long _endTime = LuaAPI.lua_toint64(L, 3);
                    
                    gen_to_be_invoked.SetSkillCD( _skillID, _endTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSkillCD(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SkillManager gen_to_be_invoked = (UnityMMO.SkillManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _skillID = LuaAPI.xlua_tointeger(L, 2);
                    
                        long gen_ret = gen_to_be_invoked.GetSkillCD( _skillID );
                        LuaAPI.lua_pushint64(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSkillResPath(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SkillManager gen_to_be_invoked = (UnityMMO.SkillManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _skillID = LuaAPI.xlua_tointeger(L, 2);
                    
                        string gen_ret = gen_to_be_invoked.GetSkillResPath( _skillID );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCareerBySkillID(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SkillManager gen_to_be_invoked = (UnityMMO.SkillManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _skillID = LuaAPI.xlua_tointeger(L, 2);
                    
                        int gen_ret = gen_to_be_invoked.GetCareerBySkillID( _skillID );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSceneObjTypeBySkillID(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SkillManager gen_to_be_invoked = (UnityMMO.SkillManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _skillID = LuaAPI.xlua_tointeger(L, 2);
                    
                        int gen_ret = gen_to_be_invoked.GetSceneObjTypeBySkillID( _skillID );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsSkillInCD(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SkillManager gen_to_be_invoked = (UnityMMO.SkillManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _skillID = LuaAPI.xlua_tointeger(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.IsSkillInCD( _skillID );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsNormalAttack(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SkillManager gen_to_be_invoked = (UnityMMO.SkillManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _skillID = LuaAPI.xlua_tointeger(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.IsNormalAttack( _skillID );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CastSkillByIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SkillManager gen_to_be_invoked = (UnityMMO.SkillManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _skillIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.CastSkillByIndex( _skillIndex );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.CastSkillByIndex(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityMMO.SkillManager.CastSkillByIndex!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CastRandomSkill(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SkillManager gen_to_be_invoked = (UnityMMO.SkillManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.CastRandomSkill(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CastSkill(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.SkillManager gen_to_be_invoked = (UnityMMO.SkillManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _skillID = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.CastSkill( _skillID );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
