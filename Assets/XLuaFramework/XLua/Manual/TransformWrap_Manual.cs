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
using UnityEngine;

namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;

    public static class TransformWrap_Manual
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Transform);
			XLuaManualUtil.BeginObjectRegister(type, L, translator, 0, 1, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLocalPosXYZ", get_xyz);
			
			
			XLuaManualUtil.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    // Utils.BeginClassRegister(type, L, __CreateInstance, 2, 1, 1);
			// Utils.RegisterFunc(L, Utils.CLS_IDX, "CustomLoader", _m_CustomLoader_xlua_st_);
			// Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int get_xyz(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
                Transform gen_to_be_invoked = (Transform)translator.FastGetCSObj(L, 1);
                {
                    Vector3 pos = gen_to_be_invoked.localPosition;
                    translator.Push(L, pos.x);
                    translator.Push(L, pos.y);
                    translator.Push(L, pos.z);
                    return 3;
                }
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
        }
        
		// public TransformWrap_Manual()
        // {}

        // public static void SetTransformPos(this Transform self, float x, float y, float z)
        // {
        //     self.localPosition = new Vector3(x, y, z);
        // }
		
		
		
    }
}
