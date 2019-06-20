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
    public class UnityMMOResPathWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityMMO.ResPath);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 14, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRoleCareerResPath", _m_GetRoleCareerResPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMonsterResPath", _m_GetMonsterResPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetNPCResPath", _m_GetNPCResPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRoleSkillResPath", _m_GetRoleSkillResPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMonsterSkillResPath", _m_GetMonsterSkillResPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRoleJumpResPath", _m_GetRoleJumpResPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetBloodResPath", _m_GetBloodResPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRoleBodyResPath", _m_GetRoleBodyResPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRoleHairResPath", _m_GetRoleHairResPath_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RoleResPath", UnityMMO.ResPath.RoleResPath);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UIResPath", UnityMMO.ResPath.UIResPath);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MonsterResPath", UnityMMO.ResPath.MonsterResPath);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NPCResPath", UnityMMO.ResPath.NPCResPath);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "UnityMMO.ResPath does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRoleCareerResPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _career = LuaAPI.xlua_tointeger(L, 1);
                    
                        string gen_ret = UnityMMO.ResPath.GetRoleCareerResPath( _career );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMonsterResPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    long _typeID = LuaAPI.lua_toint64(L, 1);
                    
                        string gen_ret = UnityMMO.ResPath.GetMonsterResPath( _typeID );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNPCResPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    long _typeID = LuaAPI.lua_toint64(L, 1);
                    
                        string gen_ret = UnityMMO.ResPath.GetNPCResPath( _typeID );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRoleSkillResPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _skillID = LuaAPI.xlua_tointeger(L, 1);
                    
                        string gen_ret = UnityMMO.ResPath.GetRoleSkillResPath( _skillID );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMonsterSkillResPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _skillID = LuaAPI.xlua_tointeger(L, 1);
                    
                        string gen_ret = UnityMMO.ResPath.GetMonsterSkillResPath( _skillID );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRoleJumpResPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _career = LuaAPI.xlua_tointeger(L, 1);
                    int _jumpID = LuaAPI.xlua_tointeger(L, 2);
                    
                        string gen_ret = UnityMMO.ResPath.GetRoleJumpResPath( _career, _jumpID );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBloodResPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityMMO.Nameboard.ColorStyle _style;translator.Get(L, 1, out _style);
                    
                        string gen_ret = UnityMMO.ResPath.GetBloodResPath( _style );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRoleBodyResPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _career = LuaAPI.xlua_tointeger(L, 1);
                    int _bodyID = LuaAPI.xlua_tointeger(L, 2);
                    
                        string gen_ret = UnityMMO.ResPath.GetRoleBodyResPath( _career, _bodyID );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRoleHairResPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _career = LuaAPI.xlua_tointeger(L, 1);
                    int _hairID = LuaAPI.xlua_tointeger(L, 2);
                    
                        string gen_ret = UnityMMO.ResPath.GetRoleHairResPath( _career, _hairID );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
