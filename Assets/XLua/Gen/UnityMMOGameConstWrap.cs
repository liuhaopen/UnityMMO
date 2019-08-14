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

		    Utils.BeginClassRegister(type, L, __CreateInstance, 11, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RealToLogic", UnityMMO.GameConst.RealToLogic);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SpeedFactor", UnityMMO.GameConst.SpeedFactor);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MinLuaNetSessionID", UnityMMO.GameConst.MinLuaNetSessionID);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MaxLuaNetSessionID", UnityMMO.GameConst.MaxLuaNetSessionID);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NetResultOk", UnityMMO.GameConst.NetResultOk);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Gravity", UnityMMO.GameConst.Gravity);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MaxJumpCount", UnityMMO.GameConst.MaxJumpCount);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JumpAscentDuration", UnityMMO.GameConst.JumpAscentDuration);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JumpAscentHeight", UnityMMO.GameConst.JumpAscentHeight);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MaxFallVelocity", UnityMMO.GameConst.MaxFallVelocity);
            
			
			
			
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
        
		
        
		
        
        
        
        
        
        
        
        
        
		
		
		
		
    }
}
