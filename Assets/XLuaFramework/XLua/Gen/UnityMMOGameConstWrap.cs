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
    public class UnityMMOGameConstWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityMMO.GameConst);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 15, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRoleResPath", _m_GetRoleResPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUIResPath", _m_GetUIResPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRoleCareerResPath", _m_GetRoleCareerResPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMonsterResPath", _m_GetMonsterResPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRoleSkillResPath", _m_GetRoleSkillResPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMonsterSkillResPath", _m_GetMonsterSkillResPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRoleJumpResPath", _m_GetRoleJumpResPath_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RealToLogic", UnityMMO.GameConst.RealToLogic);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SpeedFactor", UnityMMO.GameConst.SpeedFactor);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MinLuaNetSessionID", UnityMMO.GameConst.MinLuaNetSessionID);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MaxLuaNetSessionID", UnityMMO.GameConst.MaxLuaNetSessionID);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NetResultOk", UnityMMO.GameConst.NetResultOk);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Gravity", UnityMMO.GameConst.Gravity);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MaxJumpCount", UnityMMO.GameConst.MaxJumpCount);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					UnityMMO.GameConst gen_ret = new UnityMMO.GameConst();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityMMO.GameConst constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRoleResPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        string gen_ret = UnityMMO.GameConst.GetRoleResPath(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUIResPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        string gen_ret = UnityMMO.GameConst.GetUIResPath(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRoleCareerResPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _career = LuaAPI.xlua_tointeger(L, 1);
                    
                        string gen_ret = UnityMMO.GameConst.GetRoleCareerResPath( _career );
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
                    
                        string gen_ret = UnityMMO.GameConst.GetMonsterResPath( _typeID );
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
                    int _career = LuaAPI.xlua_tointeger(L, 1);
                    int _skillID = LuaAPI.xlua_tointeger(L, 2);
                    
                        string gen_ret = UnityMMO.GameConst.GetRoleSkillResPath( _career, _skillID );
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
                    
                        string gen_ret = UnityMMO.GameConst.GetMonsterSkillResPath( _skillID );
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
                    
                        string gen_ret = UnityMMO.GameConst.GetRoleJumpResPath( _career, _jumpID );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
