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
    public class UnityEngineTextureWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Texture);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 15, 10);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetNativeTexturePtr", _m_GetNativeTexturePtr);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IncrementUpdateCount", _m_IncrementUpdateCount);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "mipmapCount", _g_get_mipmapCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "graphicsFormat", _g_get_graphicsFormat);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "width", _g_get_width);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "height", _g_get_height);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "dimension", _g_get_dimension);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isReadable", _g_get_isReadable);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "wrapMode", _g_get_wrapMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "wrapModeU", _g_get_wrapModeU);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "wrapModeV", _g_get_wrapModeV);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "wrapModeW", _g_get_wrapModeW);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "filterMode", _g_get_filterMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "anisoLevel", _g_get_anisoLevel);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mipMapBias", _g_get_mipMapBias);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "texelSize", _g_get_texelSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "updateCount", _g_get_updateCount);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "width", _s_set_width);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "height", _s_set_height);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "dimension", _s_set_dimension);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "wrapMode", _s_set_wrapMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "wrapModeU", _s_set_wrapModeU);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "wrapModeV", _s_set_wrapModeV);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "wrapModeW", _s_set_wrapModeW);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "filterMode", _s_set_filterMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "anisoLevel", _s_set_anisoLevel);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mipMapBias", _s_set_mipMapBias);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 4, 16, 5);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "SetGlobalAnisotropicFilteringLimits", _m_SetGlobalAnisotropicFilteringLimits_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetStreamingTextureMaterialDebugProperties", _m_SetStreamingTextureMaterialDebugProperties_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GenerateAllMips", UnityEngine.Texture.GenerateAllMips);
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "masterTextureLimit", _g_get_masterTextureLimit);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "anisotropicFiltering", _g_get_anisotropicFiltering);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "totalTextureMemory", _g_get_totalTextureMemory);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "desiredTextureMemory", _g_get_desiredTextureMemory);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "targetTextureMemory", _g_get_targetTextureMemory);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "currentTextureMemory", _g_get_currentTextureMemory);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "nonStreamingTextureMemory", _g_get_nonStreamingTextureMemory);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "streamingMipmapUploadCount", _g_get_streamingMipmapUploadCount);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "streamingRendererCount", _g_get_streamingRendererCount);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "streamingTextureCount", _g_get_streamingTextureCount);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "nonStreamingTextureCount", _g_get_nonStreamingTextureCount);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "streamingTexturePendingLoadCount", _g_get_streamingTexturePendingLoadCount);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "streamingTextureLoadingCount", _g_get_streamingTextureLoadingCount);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "streamingTextureForceLoadAll", _g_get_streamingTextureForceLoadAll);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "streamingTextureDiscardUnusedMips", _g_get_streamingTextureDiscardUnusedMips);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "allowThreadedTextureCreation", _g_get_allowThreadedTextureCreation);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "masterTextureLimit", _s_set_masterTextureLimit);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "anisotropicFiltering", _s_set_anisotropicFiltering);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "streamingTextureForceLoadAll", _s_set_streamingTextureForceLoadAll);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "streamingTextureDiscardUnusedMips", _s_set_streamingTextureDiscardUnusedMips);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "allowThreadedTextureCreation", _s_set_allowThreadedTextureCreation);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "UnityEngine.Texture does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetGlobalAnisotropicFilteringLimits_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _forcedMin = LuaAPI.xlua_tointeger(L, 1);
                    int _globalMax = LuaAPI.xlua_tointeger(L, 2);
                    
                    UnityEngine.Texture.SetGlobalAnisotropicFilteringLimits( _forcedMin, _globalMax );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNativeTexturePtr(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        System.IntPtr gen_ret = gen_to_be_invoked.GetNativeTexturePtr(  );
                        LuaAPI.lua_pushlightuserdata(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IncrementUpdateCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.IncrementUpdateCount(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetStreamingTextureMaterialDebugProperties_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    UnityEngine.Texture.SetStreamingTextureMaterialDebugProperties(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_masterTextureLimit(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, UnityEngine.Texture.masterTextureLimit);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mipmapCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.mipmapCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_anisotropicFiltering(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, UnityEngine.Texture.anisotropicFiltering);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_graphicsFormat(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.graphicsFormat);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_width(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.width);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_height(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.height);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_dimension(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.dimension);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isReadable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isReadable);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_wrapMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.wrapMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_wrapModeU(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.wrapModeU);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_wrapModeV(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.wrapModeV);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_wrapModeW(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.wrapModeW);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_filterMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.filterMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_anisoLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.anisoLevel);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mipMapBias(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.mipMapBias);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_texelSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.texelSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_updateCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushuint(L, gen_to_be_invoked.updateCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_totalTextureMemory(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushuint64(L, UnityEngine.Texture.totalTextureMemory);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_desiredTextureMemory(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushuint64(L, UnityEngine.Texture.desiredTextureMemory);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_targetTextureMemory(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushuint64(L, UnityEngine.Texture.targetTextureMemory);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_currentTextureMemory(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushuint64(L, UnityEngine.Texture.currentTextureMemory);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_nonStreamingTextureMemory(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushuint64(L, UnityEngine.Texture.nonStreamingTextureMemory);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_streamingMipmapUploadCount(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushuint64(L, UnityEngine.Texture.streamingMipmapUploadCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_streamingRendererCount(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushuint64(L, UnityEngine.Texture.streamingRendererCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_streamingTextureCount(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushuint64(L, UnityEngine.Texture.streamingTextureCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_nonStreamingTextureCount(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushuint64(L, UnityEngine.Texture.nonStreamingTextureCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_streamingTexturePendingLoadCount(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushuint64(L, UnityEngine.Texture.streamingTexturePendingLoadCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_streamingTextureLoadingCount(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushuint64(L, UnityEngine.Texture.streamingTextureLoadingCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_streamingTextureForceLoadAll(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, UnityEngine.Texture.streamingTextureForceLoadAll);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_streamingTextureDiscardUnusedMips(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, UnityEngine.Texture.streamingTextureDiscardUnusedMips);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_allowThreadedTextureCreation(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, UnityEngine.Texture.allowThreadedTextureCreation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_masterTextureLimit(RealStatePtr L)
        {
		    try {
                
			    UnityEngine.Texture.masterTextureLimit = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_anisotropicFiltering(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.AnisotropicFiltering gen_value;translator.Get(L, 1, out gen_value);
				UnityEngine.Texture.anisotropicFiltering = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_width(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.width = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_height(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.height = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_dimension(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                UnityEngine.Rendering.TextureDimension gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.dimension = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_wrapMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                UnityEngine.TextureWrapMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.wrapMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_wrapModeU(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                UnityEngine.TextureWrapMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.wrapModeU = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_wrapModeV(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                UnityEngine.TextureWrapMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.wrapModeV = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_wrapModeW(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                UnityEngine.TextureWrapMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.wrapModeW = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_filterMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                UnityEngine.FilterMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.filterMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_anisoLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.anisoLevel = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mipMapBias(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Texture gen_to_be_invoked = (UnityEngine.Texture)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mipMapBias = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_streamingTextureForceLoadAll(RealStatePtr L)
        {
		    try {
                
			    UnityEngine.Texture.streamingTextureForceLoadAll = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_streamingTextureDiscardUnusedMips(RealStatePtr L)
        {
		    try {
                
			    UnityEngine.Texture.streamingTextureDiscardUnusedMips = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_allowThreadedTextureCreation(RealStatePtr L)
        {
		    try {
                
			    UnityEngine.Texture.allowThreadedTextureCreation = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
