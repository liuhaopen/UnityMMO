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
    public class UnityEngineCameraWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Camera);
			Utils.BeginObjectRegister(type, L, translator, 0, 38, 56, 43);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Reset", _m_Reset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetTransparencySortSettings", _m_ResetTransparencySortSettings);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetAspect", _m_ResetAspect);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetCullingMatrix", _m_ResetCullingMatrix);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetReplacementShader", _m_SetReplacementShader);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetReplacementShader", _m_ResetReplacementShader);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTargetBuffers", _m_SetTargetBuffers);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetWorldToCameraMatrix", _m_ResetWorldToCameraMatrix);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetProjectionMatrix", _m_ResetProjectionMatrix);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CalculateObliqueMatrix", _m_CalculateObliqueMatrix);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WorldToScreenPoint", _m_WorldToScreenPoint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WorldToViewportPoint", _m_WorldToViewportPoint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ViewportToWorldPoint", _m_ViewportToWorldPoint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ScreenToWorldPoint", _m_ScreenToWorldPoint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ScreenToViewportPoint", _m_ScreenToViewportPoint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ViewportToScreenPoint", _m_ViewportToScreenPoint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ViewportPointToRay", _m_ViewportPointToRay);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ScreenPointToRay", _m_ScreenPointToRay);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CalculateFrustumCorners", _m_CalculateFrustumCorners);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetStereoNonJitteredProjectionMatrix", _m_GetStereoNonJitteredProjectionMatrix);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetStereoViewMatrix", _m_GetStereoViewMatrix);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CopyStereoDeviceProjectionMatrixToNonJittered", _m_CopyStereoDeviceProjectionMatrixToNonJittered);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetStereoProjectionMatrix", _m_GetStereoProjectionMatrix);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetStereoProjectionMatrix", _m_SetStereoProjectionMatrix);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetStereoProjectionMatrices", _m_ResetStereoProjectionMatrices);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetStereoViewMatrix", _m_SetStereoViewMatrix);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetStereoViewMatrices", _m_ResetStereoViewMatrices);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RenderToCubemap", _m_RenderToCubemap);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Render", _m_Render);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RenderWithShader", _m_RenderWithShader);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RenderDontRestore", _m_RenderDontRestore);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CopyFrom", _m_CopyFrom);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveCommandBuffers", _m_RemoveCommandBuffers);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveAllCommandBuffers", _m_RemoveAllCommandBuffers);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddCommandBuffer", _m_AddCommandBuffer);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddCommandBufferAsync", _m_AddCommandBufferAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveCommandBuffer", _m_RemoveCommandBuffer);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCommandBuffers", _m_GetCommandBuffers);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "nearClipPlane", _g_get_nearClipPlane);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "farClipPlane", _g_get_farClipPlane);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "fieldOfView", _g_get_fieldOfView);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "renderingPath", _g_get_renderingPath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "actualRenderingPath", _g_get_actualRenderingPath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "allowHDR", _g_get_allowHDR);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "allowMSAA", _g_get_allowMSAA);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "allowDynamicResolution", _g_get_allowDynamicResolution);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "forceIntoRenderTexture", _g_get_forceIntoRenderTexture);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "orthographicSize", _g_get_orthographicSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "orthographic", _g_get_orthographic);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "opaqueSortMode", _g_get_opaqueSortMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "transparencySortMode", _g_get_transparencySortMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "transparencySortAxis", _g_get_transparencySortAxis);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "depth", _g_get_depth);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "aspect", _g_get_aspect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "velocity", _g_get_velocity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cullingMask", _g_get_cullingMask);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "eventMask", _g_get_eventMask);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "layerCullSpherical", _g_get_layerCullSpherical);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cameraType", _g_get_cameraType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "layerCullDistances", _g_get_layerCullDistances);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "useOcclusionCulling", _g_get_useOcclusionCulling);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cullingMatrix", _g_get_cullingMatrix);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "backgroundColor", _g_get_backgroundColor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "clearFlags", _g_get_clearFlags);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "depthTextureMode", _g_get_depthTextureMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "clearStencilAfterLightingPass", _g_get_clearStencilAfterLightingPass);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "usePhysicalProperties", _g_get_usePhysicalProperties);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "sensorSize", _g_get_sensorSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "lensShift", _g_get_lensShift);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "focalLength", _g_get_focalLength);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "gateFit", _g_get_gateFit);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "rect", _g_get_rect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pixelRect", _g_get_pixelRect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pixelWidth", _g_get_pixelWidth);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pixelHeight", _g_get_pixelHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "scaledPixelWidth", _g_get_scaledPixelWidth);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "scaledPixelHeight", _g_get_scaledPixelHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "targetTexture", _g_get_targetTexture);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "activeTexture", _g_get_activeTexture);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "targetDisplay", _g_get_targetDisplay);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cameraToWorldMatrix", _g_get_cameraToWorldMatrix);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "worldToCameraMatrix", _g_get_worldToCameraMatrix);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "projectionMatrix", _g_get_projectionMatrix);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "nonJitteredProjectionMatrix", _g_get_nonJitteredProjectionMatrix);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "useJitteredProjectionMatrixForTransparentRendering", _g_get_useJitteredProjectionMatrixForTransparentRendering);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "previousViewProjectionMatrix", _g_get_previousViewProjectionMatrix);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "scene", _g_get_scene);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "stereoEnabled", _g_get_stereoEnabled);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "stereoSeparation", _g_get_stereoSeparation);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "stereoConvergence", _g_get_stereoConvergence);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "areVRStereoViewMatricesWithinSingleCullTolerance", _g_get_areVRStereoViewMatricesWithinSingleCullTolerance);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "stereoTargetEye", _g_get_stereoTargetEye);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "stereoActiveEye", _g_get_stereoActiveEye);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "commandBufferCount", _g_get_commandBufferCount);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "nearClipPlane", _s_set_nearClipPlane);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "farClipPlane", _s_set_farClipPlane);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "fieldOfView", _s_set_fieldOfView);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "renderingPath", _s_set_renderingPath);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "allowHDR", _s_set_allowHDR);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "allowMSAA", _s_set_allowMSAA);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "allowDynamicResolution", _s_set_allowDynamicResolution);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "forceIntoRenderTexture", _s_set_forceIntoRenderTexture);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "orthographicSize", _s_set_orthographicSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "orthographic", _s_set_orthographic);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "opaqueSortMode", _s_set_opaqueSortMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "transparencySortMode", _s_set_transparencySortMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "transparencySortAxis", _s_set_transparencySortAxis);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "depth", _s_set_depth);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "aspect", _s_set_aspect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cullingMask", _s_set_cullingMask);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "eventMask", _s_set_eventMask);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "layerCullSpherical", _s_set_layerCullSpherical);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cameraType", _s_set_cameraType);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "layerCullDistances", _s_set_layerCullDistances);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "useOcclusionCulling", _s_set_useOcclusionCulling);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cullingMatrix", _s_set_cullingMatrix);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "backgroundColor", _s_set_backgroundColor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "clearFlags", _s_set_clearFlags);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "depthTextureMode", _s_set_depthTextureMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "clearStencilAfterLightingPass", _s_set_clearStencilAfterLightingPass);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "usePhysicalProperties", _s_set_usePhysicalProperties);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "sensorSize", _s_set_sensorSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "lensShift", _s_set_lensShift);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "focalLength", _s_set_focalLength);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "gateFit", _s_set_gateFit);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "rect", _s_set_rect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "pixelRect", _s_set_pixelRect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "targetTexture", _s_set_targetTexture);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "targetDisplay", _s_set_targetDisplay);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "worldToCameraMatrix", _s_set_worldToCameraMatrix);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "projectionMatrix", _s_set_projectionMatrix);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "nonJitteredProjectionMatrix", _s_set_nonJitteredProjectionMatrix);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "useJitteredProjectionMatrixForTransparentRendering", _s_set_useJitteredProjectionMatrixForTransparentRendering);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "scene", _s_set_scene);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "stereoSeparation", _s_set_stereoSeparation);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "stereoConvergence", _s_set_stereoConvergence);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "stereoTargetEye", _s_set_stereoTargetEye);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 6, 7, 3);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "CalculateProjectionMatrixFromPhysicalProperties", _m_CalculateProjectionMatrixFromPhysicalProperties_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "FocalLengthToFOV", _m_FocalLengthToFOV_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "FOVToFocalLength", _m_FOVToFocalLength_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetAllCameras", _m_GetAllCameras_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetupCurrent", _m_SetupCurrent_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "main", _g_get_main);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "current", _g_get_current);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "allCamerasCount", _g_get_allCamerasCount);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "allCameras", _g_get_allCameras);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "onPreCull", _g_get_onPreCull);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "onPreRender", _g_get_onPreRender);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "onPostRender", _g_get_onPostRender);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "onPreCull", _s_set_onPreCull);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "onPreRender", _s_set_onPreRender);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "onPostRender", _s_set_onPostRender);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					UnityEngine.Camera gen_ret = new UnityEngine.Camera();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Camera constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Reset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Reset(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetTransparencySortSettings(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetTransparencySortSettings(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetAspect(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetAspect(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetCullingMatrix(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetCullingMatrix(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetReplacementShader(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Shader _shader = (UnityEngine.Shader)translator.GetObject(L, 2, typeof(UnityEngine.Shader));
                    string _replacementTag = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.SetReplacementShader( _shader, _replacementTag );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetReplacementShader(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetReplacementShader(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTargetBuffers(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.RenderBuffer>(L, 2)&& translator.Assignable<UnityEngine.RenderBuffer>(L, 3)) 
                {
                    UnityEngine.RenderBuffer _colorBuffer;translator.Get(L, 2, out _colorBuffer);
                    UnityEngine.RenderBuffer _depthBuffer;translator.Get(L, 3, out _depthBuffer);
                    
                    gen_to_be_invoked.SetTargetBuffers( _colorBuffer, _depthBuffer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.RenderBuffer[]>(L, 2)&& translator.Assignable<UnityEngine.RenderBuffer>(L, 3)) 
                {
                    UnityEngine.RenderBuffer[] _colorBuffer = (UnityEngine.RenderBuffer[])translator.GetObject(L, 2, typeof(UnityEngine.RenderBuffer[]));
                    UnityEngine.RenderBuffer _depthBuffer;translator.Get(L, 3, out _depthBuffer);
                    
                    gen_to_be_invoked.SetTargetBuffers( _colorBuffer, _depthBuffer );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Camera.SetTargetBuffers!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetWorldToCameraMatrix(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetWorldToCameraMatrix(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetProjectionMatrix(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetProjectionMatrix(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CalculateObliqueMatrix(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector4 _clipPlane;translator.Get(L, 2, out _clipPlane);
                    
                        UnityEngine.Matrix4x4 gen_ret = gen_to_be_invoked.CalculateObliqueMatrix( _clipPlane );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WorldToScreenPoint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 2, out _position);
                    
                        UnityEngine.Vector3 gen_ret = gen_to_be_invoked.WorldToScreenPoint( _position );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Camera.MonoOrStereoscopicEye>(L, 3)) 
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 2, out _position);
                    UnityEngine.Camera.MonoOrStereoscopicEye _eye;translator.Get(L, 3, out _eye);
                    
                        UnityEngine.Vector3 gen_ret = gen_to_be_invoked.WorldToScreenPoint( _position, _eye );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Camera.WorldToScreenPoint!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WorldToViewportPoint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 2, out _position);
                    
                        UnityEngine.Vector3 gen_ret = gen_to_be_invoked.WorldToViewportPoint( _position );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Camera.MonoOrStereoscopicEye>(L, 3)) 
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 2, out _position);
                    UnityEngine.Camera.MonoOrStereoscopicEye _eye;translator.Get(L, 3, out _eye);
                    
                        UnityEngine.Vector3 gen_ret = gen_to_be_invoked.WorldToViewportPoint( _position, _eye );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Camera.WorldToViewportPoint!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ViewportToWorldPoint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 2, out _position);
                    
                        UnityEngine.Vector3 gen_ret = gen_to_be_invoked.ViewportToWorldPoint( _position );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Camera.MonoOrStereoscopicEye>(L, 3)) 
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 2, out _position);
                    UnityEngine.Camera.MonoOrStereoscopicEye _eye;translator.Get(L, 3, out _eye);
                    
                        UnityEngine.Vector3 gen_ret = gen_to_be_invoked.ViewportToWorldPoint( _position, _eye );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Camera.ViewportToWorldPoint!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ScreenToWorldPoint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 2, out _position);
                    
                        UnityEngine.Vector3 gen_ret = gen_to_be_invoked.ScreenToWorldPoint( _position );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Camera.MonoOrStereoscopicEye>(L, 3)) 
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 2, out _position);
                    UnityEngine.Camera.MonoOrStereoscopicEye _eye;translator.Get(L, 3, out _eye);
                    
                        UnityEngine.Vector3 gen_ret = gen_to_be_invoked.ScreenToWorldPoint( _position, _eye );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Camera.ScreenToWorldPoint!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ScreenToViewportPoint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 2, out _position);
                    
                        UnityEngine.Vector3 gen_ret = gen_to_be_invoked.ScreenToViewportPoint( _position );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ViewportToScreenPoint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 2, out _position);
                    
                        UnityEngine.Vector3 gen_ret = gen_to_be_invoked.ViewportToScreenPoint( _position );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ViewportPointToRay(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _pos;translator.Get(L, 2, out _pos);
                    
                        UnityEngine.Ray gen_ret = gen_to_be_invoked.ViewportPointToRay( _pos );
                        translator.PushUnityEngineRay(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Camera.MonoOrStereoscopicEye>(L, 3)) 
                {
                    UnityEngine.Vector3 _pos;translator.Get(L, 2, out _pos);
                    UnityEngine.Camera.MonoOrStereoscopicEye _eye;translator.Get(L, 3, out _eye);
                    
                        UnityEngine.Ray gen_ret = gen_to_be_invoked.ViewportPointToRay( _pos, _eye );
                        translator.PushUnityEngineRay(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Camera.ViewportPointToRay!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ScreenPointToRay(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _pos;translator.Get(L, 2, out _pos);
                    
                        UnityEngine.Ray gen_ret = gen_to_be_invoked.ScreenPointToRay( _pos );
                        translator.PushUnityEngineRay(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Camera.MonoOrStereoscopicEye>(L, 3)) 
                {
                    UnityEngine.Vector3 _pos;translator.Get(L, 2, out _pos);
                    UnityEngine.Camera.MonoOrStereoscopicEye _eye;translator.Get(L, 3, out _eye);
                    
                        UnityEngine.Ray gen_ret = gen_to_be_invoked.ScreenPointToRay( _pos, _eye );
                        translator.PushUnityEngineRay(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Camera.ScreenPointToRay!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CalculateFrustumCorners(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Rect _viewport;translator.Get(L, 2, out _viewport);
                    float _z = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Camera.MonoOrStereoscopicEye _eye;translator.Get(L, 4, out _eye);
                    UnityEngine.Vector3[] _outCorners = (UnityEngine.Vector3[])translator.GetObject(L, 5, typeof(UnityEngine.Vector3[]));
                    
                    gen_to_be_invoked.CalculateFrustumCorners( _viewport, _z, _eye, _outCorners );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CalculateProjectionMatrixFromPhysicalProperties_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 6&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& translator.Assignable<UnityEngine.Vector2>(L, 2)&& translator.Assignable<UnityEngine.Vector2>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<UnityEngine.Camera.GateFitParameters>(L, 6)) 
                {
                    UnityEngine.Matrix4x4 _output;
                    float _focalLength = (float)LuaAPI.lua_tonumber(L, 1);
                    UnityEngine.Vector2 _sensorSize;translator.Get(L, 2, out _sensorSize);
                    UnityEngine.Vector2 _lensShift;translator.Get(L, 3, out _lensShift);
                    float _nearClip = (float)LuaAPI.lua_tonumber(L, 4);
                    float _farClip = (float)LuaAPI.lua_tonumber(L, 5);
                    UnityEngine.Camera.GateFitParameters _gateFitParameters;translator.Get(L, 6, out _gateFitParameters);
                    
                    UnityEngine.Camera.CalculateProjectionMatrixFromPhysicalProperties( out _output, _focalLength, _sensorSize, _lensShift, _nearClip, _farClip, _gateFitParameters );
                    translator.Push(L, _output);
                        
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& translator.Assignable<UnityEngine.Vector2>(L, 2)&& translator.Assignable<UnityEngine.Vector2>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Matrix4x4 _output;
                    float _focalLength = (float)LuaAPI.lua_tonumber(L, 1);
                    UnityEngine.Vector2 _sensorSize;translator.Get(L, 2, out _sensorSize);
                    UnityEngine.Vector2 _lensShift;translator.Get(L, 3, out _lensShift);
                    float _nearClip = (float)LuaAPI.lua_tonumber(L, 4);
                    float _farClip = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    UnityEngine.Camera.CalculateProjectionMatrixFromPhysicalProperties( out _output, _focalLength, _sensorSize, _lensShift, _nearClip, _farClip );
                    translator.Push(L, _output);
                        
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Camera.CalculateProjectionMatrixFromPhysicalProperties!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FocalLengthToFOV_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    float _focalLength = (float)LuaAPI.lua_tonumber(L, 1);
                    float _sensorSize = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        float gen_ret = UnityEngine.Camera.FocalLengthToFOV( _focalLength, _sensorSize );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FOVToFocalLength_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    float _fov = (float)LuaAPI.lua_tonumber(L, 1);
                    float _sensorSize = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        float gen_ret = UnityEngine.Camera.FOVToFocalLength( _fov, _sensorSize );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetStereoNonJitteredProjectionMatrix(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Camera.StereoscopicEye _eye;translator.Get(L, 2, out _eye);
                    
                        UnityEngine.Matrix4x4 gen_ret = gen_to_be_invoked.GetStereoNonJitteredProjectionMatrix( _eye );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetStereoViewMatrix(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Camera.StereoscopicEye _eye;translator.Get(L, 2, out _eye);
                    
                        UnityEngine.Matrix4x4 gen_ret = gen_to_be_invoked.GetStereoViewMatrix( _eye );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CopyStereoDeviceProjectionMatrixToNonJittered(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Camera.StereoscopicEye _eye;translator.Get(L, 2, out _eye);
                    
                    gen_to_be_invoked.CopyStereoDeviceProjectionMatrixToNonJittered( _eye );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetStereoProjectionMatrix(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Camera.StereoscopicEye _eye;translator.Get(L, 2, out _eye);
                    
                        UnityEngine.Matrix4x4 gen_ret = gen_to_be_invoked.GetStereoProjectionMatrix( _eye );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetStereoProjectionMatrix(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Camera.StereoscopicEye _eye;translator.Get(L, 2, out _eye);
                    UnityEngine.Matrix4x4 _matrix;translator.Get(L, 3, out _matrix);
                    
                    gen_to_be_invoked.SetStereoProjectionMatrix( _eye, _matrix );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetStereoProjectionMatrices(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetStereoProjectionMatrices(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetStereoViewMatrix(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Camera.StereoscopicEye _eye;translator.Get(L, 2, out _eye);
                    UnityEngine.Matrix4x4 _matrix;translator.Get(L, 3, out _matrix);
                    
                    gen_to_be_invoked.SetStereoViewMatrix( _eye, _matrix );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetStereoViewMatrices(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetStereoViewMatrices(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAllCameras_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Camera[] _cameras = (UnityEngine.Camera[])translator.GetObject(L, 1, typeof(UnityEngine.Camera[]));
                    
                        int gen_ret = UnityEngine.Camera.GetAllCameras( _cameras );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RenderToCubemap(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Cubemap>(L, 2)) 
                {
                    UnityEngine.Cubemap _cubemap = (UnityEngine.Cubemap)translator.GetObject(L, 2, typeof(UnityEngine.Cubemap));
                    
                        bool gen_ret = gen_to_be_invoked.RenderToCubemap( _cubemap );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.RenderTexture>(L, 2)) 
                {
                    UnityEngine.RenderTexture _cubemap = (UnityEngine.RenderTexture)translator.GetObject(L, 2, typeof(UnityEngine.RenderTexture));
                    
                        bool gen_ret = gen_to_be_invoked.RenderToCubemap( _cubemap );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Cubemap>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Cubemap _cubemap = (UnityEngine.Cubemap)translator.GetObject(L, 2, typeof(UnityEngine.Cubemap));
                    int _faceMask = LuaAPI.xlua_tointeger(L, 3);
                    
                        bool gen_ret = gen_to_be_invoked.RenderToCubemap( _cubemap, _faceMask );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.RenderTexture>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.RenderTexture _cubemap = (UnityEngine.RenderTexture)translator.GetObject(L, 2, typeof(UnityEngine.RenderTexture));
                    int _faceMask = LuaAPI.xlua_tointeger(L, 3);
                    
                        bool gen_ret = gen_to_be_invoked.RenderToCubemap( _cubemap, _faceMask );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.RenderTexture>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Camera.MonoOrStereoscopicEye>(L, 4)) 
                {
                    UnityEngine.RenderTexture _cubemap = (UnityEngine.RenderTexture)translator.GetObject(L, 2, typeof(UnityEngine.RenderTexture));
                    int _faceMask = LuaAPI.xlua_tointeger(L, 3);
                    UnityEngine.Camera.MonoOrStereoscopicEye _stereoEye;translator.Get(L, 4, out _stereoEye);
                    
                        bool gen_ret = gen_to_be_invoked.RenderToCubemap( _cubemap, _faceMask, _stereoEye );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Camera.RenderToCubemap!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Render(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Render(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RenderWithShader(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Shader _shader = (UnityEngine.Shader)translator.GetObject(L, 2, typeof(UnityEngine.Shader));
                    string _replacementTag = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.RenderWithShader( _shader, _replacementTag );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RenderDontRestore(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.RenderDontRestore(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetupCurrent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Camera _cur = (UnityEngine.Camera)translator.GetObject(L, 1, typeof(UnityEngine.Camera));
                    
                    UnityEngine.Camera.SetupCurrent( _cur );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CopyFrom(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Camera _other = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    
                    gen_to_be_invoked.CopyFrom( _other );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveCommandBuffers(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Rendering.CameraEvent _evt;translator.Get(L, 2, out _evt);
                    
                    gen_to_be_invoked.RemoveCommandBuffers( _evt );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveAllCommandBuffers(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.RemoveAllCommandBuffers(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddCommandBuffer(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Rendering.CameraEvent _evt;translator.Get(L, 2, out _evt);
                    UnityEngine.Rendering.CommandBuffer _buffer = (UnityEngine.Rendering.CommandBuffer)translator.GetObject(L, 3, typeof(UnityEngine.Rendering.CommandBuffer));
                    
                    gen_to_be_invoked.AddCommandBuffer( _evt, _buffer );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddCommandBufferAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Rendering.CameraEvent _evt;translator.Get(L, 2, out _evt);
                    UnityEngine.Rendering.CommandBuffer _buffer = (UnityEngine.Rendering.CommandBuffer)translator.GetObject(L, 3, typeof(UnityEngine.Rendering.CommandBuffer));
                    UnityEngine.Rendering.ComputeQueueType _queueType;translator.Get(L, 4, out _queueType);
                    
                    gen_to_be_invoked.AddCommandBufferAsync( _evt, _buffer, _queueType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveCommandBuffer(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Rendering.CameraEvent _evt;translator.Get(L, 2, out _evt);
                    UnityEngine.Rendering.CommandBuffer _buffer = (UnityEngine.Rendering.CommandBuffer)translator.GetObject(L, 3, typeof(UnityEngine.Rendering.CommandBuffer));
                    
                    gen_to_be_invoked.RemoveCommandBuffer( _evt, _buffer );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCommandBuffers(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Rendering.CameraEvent _evt;translator.Get(L, 2, out _evt);
                    
                        UnityEngine.Rendering.CommandBuffer[] gen_ret = gen_to_be_invoked.GetCommandBuffers( _evt );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_nearClipPlane(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.nearClipPlane);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_farClipPlane(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.farClipPlane);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_fieldOfView(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.fieldOfView);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_renderingPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.renderingPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_actualRenderingPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.actualRenderingPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_allowHDR(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.allowHDR);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_allowMSAA(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.allowMSAA);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_allowDynamicResolution(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.allowDynamicResolution);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_forceIntoRenderTexture(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.forceIntoRenderTexture);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_orthographicSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.orthographicSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_orthographic(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.orthographic);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_opaqueSortMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.opaqueSortMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_transparencySortMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.transparencySortMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_transparencySortAxis(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.transparencySortAxis);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_depth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.depth);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_aspect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.aspect);
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
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.velocity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cullingMask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.cullingMask);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_eventMask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.eventMask);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_layerCullSpherical(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.layerCullSpherical);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cameraType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.cameraType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_layerCullDistances(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.layerCullDistances);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_useOcclusionCulling(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.useOcclusionCulling);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cullingMatrix(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.cullingMatrix);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_backgroundColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineColor(L, gen_to_be_invoked.backgroundColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_clearFlags(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.clearFlags);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_depthTextureMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.depthTextureMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_clearStencilAfterLightingPass(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.clearStencilAfterLightingPass);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_usePhysicalProperties(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.usePhysicalProperties);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_sensorSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.sensorSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_lensShift(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.lensShift);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_focalLength(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.focalLength);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_gateFit(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.gateFit);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_rect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.rect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pixelRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.pixelRect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pixelWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.pixelWidth);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pixelHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.pixelHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_scaledPixelWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.scaledPixelWidth);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_scaledPixelHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.scaledPixelHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_targetTexture(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.targetTexture);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_activeTexture(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.activeTexture);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_targetDisplay(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.targetDisplay);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cameraToWorldMatrix(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.cameraToWorldMatrix);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_worldToCameraMatrix(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.worldToCameraMatrix);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_projectionMatrix(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.projectionMatrix);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_nonJitteredProjectionMatrix(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.nonJitteredProjectionMatrix);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_useJitteredProjectionMatrixForTransparentRendering(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.useJitteredProjectionMatrixForTransparentRendering);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_previousViewProjectionMatrix(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.previousViewProjectionMatrix);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_main(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, UnityEngine.Camera.main);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_current(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, UnityEngine.Camera.current);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_scene(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.scene);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_stereoEnabled(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.stereoEnabled);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_stereoSeparation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.stereoSeparation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_stereoConvergence(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.stereoConvergence);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_areVRStereoViewMatricesWithinSingleCullTolerance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.areVRStereoViewMatricesWithinSingleCullTolerance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_stereoTargetEye(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.stereoTargetEye);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_stereoActiveEye(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.stereoActiveEye);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_allCamerasCount(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, UnityEngine.Camera.allCamerasCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_allCameras(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, UnityEngine.Camera.allCameras);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_commandBufferCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.commandBufferCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onPreCull(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, UnityEngine.Camera.onPreCull);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onPreRender(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, UnityEngine.Camera.onPreRender);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onPostRender(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, UnityEngine.Camera.onPostRender);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_nearClipPlane(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.nearClipPlane = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_farClipPlane(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.farClipPlane = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_fieldOfView(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.fieldOfView = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_renderingPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                UnityEngine.RenderingPath gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.renderingPath = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_allowHDR(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.allowHDR = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_allowMSAA(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.allowMSAA = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_allowDynamicResolution(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.allowDynamicResolution = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_forceIntoRenderTexture(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.forceIntoRenderTexture = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_orthographicSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.orthographicSize = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_orthographic(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.orthographic = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_opaqueSortMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                UnityEngine.Rendering.OpaqueSortMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.opaqueSortMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_transparencySortMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                UnityEngine.TransparencySortMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.transparencySortMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_transparencySortAxis(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.transparencySortAxis = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_depth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.depth = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_aspect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.aspect = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cullingMask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.cullingMask = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_eventMask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.eventMask = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_layerCullSpherical(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.layerCullSpherical = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cameraType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                UnityEngine.CameraType gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.cameraType = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_layerCullDistances(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.layerCullDistances = (float[])translator.GetObject(L, 2, typeof(float[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_useOcclusionCulling(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.useOcclusionCulling = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cullingMatrix(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                UnityEngine.Matrix4x4 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.cullingMatrix = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_backgroundColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                UnityEngine.Color gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.backgroundColor = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_clearFlags(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                UnityEngine.CameraClearFlags gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.clearFlags = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_depthTextureMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                UnityEngine.DepthTextureMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.depthTextureMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_clearStencilAfterLightingPass(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.clearStencilAfterLightingPass = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_usePhysicalProperties(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.usePhysicalProperties = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_sensorSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.sensorSize = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_lensShift(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.lensShift = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_focalLength(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.focalLength = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_gateFit(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                UnityEngine.Camera.GateFitMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.gateFit = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_rect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                UnityEngine.Rect gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.rect = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_pixelRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                UnityEngine.Rect gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.pixelRect = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_targetTexture(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.targetTexture = (UnityEngine.RenderTexture)translator.GetObject(L, 2, typeof(UnityEngine.RenderTexture));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_targetDisplay(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.targetDisplay = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_worldToCameraMatrix(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                UnityEngine.Matrix4x4 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.worldToCameraMatrix = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_projectionMatrix(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                UnityEngine.Matrix4x4 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.projectionMatrix = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_nonJitteredProjectionMatrix(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                UnityEngine.Matrix4x4 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.nonJitteredProjectionMatrix = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_useJitteredProjectionMatrixForTransparentRendering(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.useJitteredProjectionMatrixForTransparentRendering = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_scene(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                UnityEngine.SceneManagement.Scene gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.scene = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_stereoSeparation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.stereoSeparation = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_stereoConvergence(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.stereoConvergence = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_stereoTargetEye(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Camera gen_to_be_invoked = (UnityEngine.Camera)translator.FastGetCSObj(L, 1);
                UnityEngine.StereoTargetEyeMask gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.stereoTargetEye = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onPreCull(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    UnityEngine.Camera.onPreCull = translator.GetDelegate<UnityEngine.Camera.CameraCallback>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onPreRender(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    UnityEngine.Camera.onPreRender = translator.GetDelegate<UnityEngine.Camera.CameraCallback>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onPostRender(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    UnityEngine.Camera.onPostRender = translator.GetDelegate<UnityEngine.Camera.CameraCallback>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
