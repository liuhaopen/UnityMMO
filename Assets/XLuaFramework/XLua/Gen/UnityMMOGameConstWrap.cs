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
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetRoleResPath", _m_GetRoleResPath);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 7, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRoleCareerResPath", _m_GetRoleCareerResPath_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RealToLogic", UnityMMO.GameConst.RealToLogic);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LogicToReal", UnityMMO.GameConst.LogicToReal);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MinLuaNetSessionID", UnityMMO.GameConst.MinLuaNetSessionID);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MaxLuaNetSessionID", UnityMMO.GameConst.MaxLuaNetSessionID);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NetResultOk", UnityMMO.GameConst.NetResultOk);
            
			
			
			
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
        static int _m_GetRoleResPath(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityMMO.GameConst gen_to_be_invoked = (UnityMMO.GameConst)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        string gen_ret = gen_to_be_invoked.GetRoleResPath(  );
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
        
        
        
        
        
        
		
		
		
		
    }
}
