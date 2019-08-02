#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using System;


namespace XLua
{
    public partial class ObjectTranslator
    {
        
        class IniterAdderUnityEngineVector2
        {
            static IniterAdderUnityEngineVector2()
            {
                LuaEnv.AddIniter(Init);
            }
			
			static void Init(LuaEnv luaenv, ObjectTranslator translator)
			{
			
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Vector2>(translator.PushUnityEngineVector2, translator.Get, translator.UpdateUnityEngineVector2);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Vector3>(translator.PushUnityEngineVector3, translator.Get, translator.UpdateUnityEngineVector3);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Vector4>(translator.PushUnityEngineVector4, translator.Get, translator.UpdateUnityEngineVector4);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Color>(translator.PushUnityEngineColor, translator.Get, translator.UpdateUnityEngineColor);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Quaternion>(translator.PushUnityEngineQuaternion, translator.Get, translator.UpdateUnityEngineQuaternion);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Ray>(translator.PushUnityEngineRay, translator.Get, translator.UpdateUnityEngineRay);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Bounds>(translator.PushUnityEngineBounds, translator.Get, translator.UpdateUnityEngineBounds);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Ray2D>(translator.PushUnityEngineRay2D, translator.Get, translator.UpdateUnityEngineRay2D);
				translator.RegisterPushAndGetAndUpdate<XLuaFramework.NetPackageType>(translator.PushXLuaFrameworkNetPackageType, translator.Get, translator.UpdateXLuaFrameworkNetPackageType);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.UI.InputField.ContentType>(translator.PushUnityEngineUIInputFieldContentType, translator.Get, translator.UpdateUnityEngineUIInputFieldContentType);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.UI.InputField.InputType>(translator.PushUnityEngineUIInputFieldInputType, translator.Get, translator.UpdateUnityEngineUIInputFieldInputType);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.UI.InputField.CharacterValidation>(translator.PushUnityEngineUIInputFieldCharacterValidation, translator.Get, translator.UpdateUnityEngineUIInputFieldCharacterValidation);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.UI.InputField.LineType>(translator.PushUnityEngineUIInputFieldLineType, translator.Get, translator.UpdateUnityEngineUIInputFieldLineType);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.TextAnchor>(translator.PushUnityEngineTextAnchor, translator.Get, translator.UpdateUnityEngineTextAnchor);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.TouchPhase>(translator.PushUnityEngineTouchPhase, translator.Get, translator.UpdateUnityEngineTouchPhase);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.RectTransform.Axis>(translator.PushUnityEngineRectTransformAxis, translator.Get, translator.UpdateUnityEngineRectTransformAxis);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.RectTransform.Edge>(translator.PushUnityEngineRectTransformEdge, translator.Get, translator.UpdateUnityEngineRectTransformEdge);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.UI.ScrollRect.MovementType>(translator.PushUnityEngineUIScrollRectMovementType, translator.Get, translator.UpdateUnityEngineUIScrollRectMovementType);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.UI.ScrollRect.ScrollbarVisibility>(translator.PushUnityEngineUIScrollRectScrollbarVisibility, translator.Get, translator.UpdateUnityEngineUIScrollRectScrollbarVisibility);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.UI.Image.Type>(translator.PushUnityEngineUIImageType, translator.Get, translator.UpdateUnityEngineUIImageType);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.UI.Image.FillMethod>(translator.PushUnityEngineUIImageFillMethod, translator.Get, translator.UpdateUnityEngineUIImageFillMethod);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.UI.Image.OriginHorizontal>(translator.PushUnityEngineUIImageOriginHorizontal, translator.Get, translator.UpdateUnityEngineUIImageOriginHorizontal);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.UI.Image.OriginVertical>(translator.PushUnityEngineUIImageOriginVertical, translator.Get, translator.UpdateUnityEngineUIImageOriginVertical);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.UI.Image.Origin90>(translator.PushUnityEngineUIImageOrigin90, translator.Get, translator.UpdateUnityEngineUIImageOrigin90);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.UI.Image.Origin180>(translator.PushUnityEngineUIImageOrigin180, translator.Get, translator.UpdateUnityEngineUIImageOrigin180);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.UI.Image.Origin360>(translator.PushUnityEngineUIImageOrigin360, translator.Get, translator.UpdateUnityEngineUIImageOrigin360);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Camera.FieldOfViewAxis>(translator.PushUnityEngineCameraFieldOfViewAxis, translator.Get, translator.UpdateUnityEngineCameraFieldOfViewAxis);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Camera.MonoOrStereoscopicEye>(translator.PushUnityEngineCameraMonoOrStereoscopicEye, translator.Get, translator.UpdateUnityEngineCameraMonoOrStereoscopicEye);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Camera.StereoscopicEye>(translator.PushUnityEngineCameraStereoscopicEye, translator.Get, translator.UpdateUnityEngineCameraStereoscopicEye);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Camera.GateFitMode>(translator.PushUnityEngineCameraGateFitMode, translator.Get, translator.UpdateUnityEngineCameraGateFitMode);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.KeyCode>(translator.PushUnityEngineKeyCode, translator.Get, translator.UpdateUnityEngineKeyCode);
				translator.RegisterPushAndGetAndUpdate<XLuaFramework.DisType>(translator.PushXLuaFrameworkDisType, translator.Get, translator.UpdateXLuaFrameworkDisType);
				translator.RegisterPushAndGetAndUpdate<UnityMMO.SceneInfoKey>(translator.PushUnityMMOSceneInfoKey, translator.Get, translator.UpdateUnityMMOSceneInfoKey);
				translator.RegisterPushAndGetAndUpdate<UnityMMO.SceneObjectType>(translator.PushUnityMMOSceneObjectType, translator.Get, translator.UpdateUnityMMOSceneObjectType);
				translator.RegisterPushAndGetAndUpdate<XLuaFramework.Main.State>(translator.PushXLuaFrameworkMainState, translator.Get, translator.UpdateXLuaFrameworkMainState);
				translator.RegisterPushAndGetAndUpdate<XLuaFramework.Main.SubState>(translator.PushXLuaFrameworkMainSubState, translator.Get, translator.UpdateXLuaFrameworkMainSubState);
				translator.RegisterPushAndGetAndUpdate<UnityMMO.TimelineInfo.Event>(translator.PushUnityMMOTimelineInfoEvent, translator.Get, translator.UpdateUnityMMOTimelineInfoEvent);
				translator.RegisterPushAndGetAndUpdate<UnityMMO.Nameboard.ColorStyle>(translator.PushUnityMMONameboardColorStyle, translator.Get, translator.UpdateUnityMMONameboardColorStyle);
			
			}
        }
        
        static IniterAdderUnityEngineVector2 s_IniterAdderUnityEngineVector2_dumb_obj = new IniterAdderUnityEngineVector2();
        static IniterAdderUnityEngineVector2 IniterAdderUnityEngineVector2_dumb_obj {get{return s_IniterAdderUnityEngineVector2_dumb_obj;}}
        
        
        int UnityEngineVector2_TypeID = -1;
        public void PushUnityEngineVector2(RealStatePtr L, UnityEngine.Vector2 val)
        {
            if (UnityEngineVector2_TypeID == -1)
            {
			    bool is_first;
                UnityEngineVector2_TypeID = getTypeId(L, typeof(UnityEngine.Vector2), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 8, UnityEngineVector2_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Vector2 ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Vector2 val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineVector2_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Vector2");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Vector2");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Vector2)objectCasters.GetCaster(typeof(UnityEngine.Vector2))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineVector2(RealStatePtr L, int index, UnityEngine.Vector2 val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineVector2_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Vector2");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Vector2 ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineVector3_TypeID = -1;
        public void PushUnityEngineVector3(RealStatePtr L, UnityEngine.Vector3 val)
        {
            if (UnityEngineVector3_TypeID == -1)
            {
			    bool is_first;
                UnityEngineVector3_TypeID = getTypeId(L, typeof(UnityEngine.Vector3), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 12, UnityEngineVector3_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Vector3 ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Vector3 val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineVector3_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Vector3");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Vector3");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Vector3)objectCasters.GetCaster(typeof(UnityEngine.Vector3))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineVector3(RealStatePtr L, int index, UnityEngine.Vector3 val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineVector3_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Vector3");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Vector3 ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineVector4_TypeID = -1;
        public void PushUnityEngineVector4(RealStatePtr L, UnityEngine.Vector4 val)
        {
            if (UnityEngineVector4_TypeID == -1)
            {
			    bool is_first;
                UnityEngineVector4_TypeID = getTypeId(L, typeof(UnityEngine.Vector4), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 16, UnityEngineVector4_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Vector4 ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Vector4 val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineVector4_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Vector4");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Vector4");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Vector4)objectCasters.GetCaster(typeof(UnityEngine.Vector4))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineVector4(RealStatePtr L, int index, UnityEngine.Vector4 val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineVector4_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Vector4");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Vector4 ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineColor_TypeID = -1;
        public void PushUnityEngineColor(RealStatePtr L, UnityEngine.Color val)
        {
            if (UnityEngineColor_TypeID == -1)
            {
			    bool is_first;
                UnityEngineColor_TypeID = getTypeId(L, typeof(UnityEngine.Color), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 16, UnityEngineColor_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Color ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Color val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineColor_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Color");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Color");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Color)objectCasters.GetCaster(typeof(UnityEngine.Color))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineColor(RealStatePtr L, int index, UnityEngine.Color val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineColor_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Color");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Color ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineQuaternion_TypeID = -1;
        public void PushUnityEngineQuaternion(RealStatePtr L, UnityEngine.Quaternion val)
        {
            if (UnityEngineQuaternion_TypeID == -1)
            {
			    bool is_first;
                UnityEngineQuaternion_TypeID = getTypeId(L, typeof(UnityEngine.Quaternion), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 16, UnityEngineQuaternion_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Quaternion ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Quaternion val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineQuaternion_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Quaternion");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Quaternion");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Quaternion)objectCasters.GetCaster(typeof(UnityEngine.Quaternion))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineQuaternion(RealStatePtr L, int index, UnityEngine.Quaternion val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineQuaternion_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Quaternion");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Quaternion ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineRay_TypeID = -1;
        public void PushUnityEngineRay(RealStatePtr L, UnityEngine.Ray val)
        {
            if (UnityEngineRay_TypeID == -1)
            {
			    bool is_first;
                UnityEngineRay_TypeID = getTypeId(L, typeof(UnityEngine.Ray), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 24, UnityEngineRay_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Ray ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Ray val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineRay_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Ray");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Ray");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Ray)objectCasters.GetCaster(typeof(UnityEngine.Ray))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineRay(RealStatePtr L, int index, UnityEngine.Ray val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineRay_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Ray");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Ray ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineBounds_TypeID = -1;
        public void PushUnityEngineBounds(RealStatePtr L, UnityEngine.Bounds val)
        {
            if (UnityEngineBounds_TypeID == -1)
            {
			    bool is_first;
                UnityEngineBounds_TypeID = getTypeId(L, typeof(UnityEngine.Bounds), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 24, UnityEngineBounds_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Bounds ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Bounds val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineBounds_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Bounds");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Bounds");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Bounds)objectCasters.GetCaster(typeof(UnityEngine.Bounds))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineBounds(RealStatePtr L, int index, UnityEngine.Bounds val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineBounds_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Bounds");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Bounds ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineRay2D_TypeID = -1;
        public void PushUnityEngineRay2D(RealStatePtr L, UnityEngine.Ray2D val)
        {
            if (UnityEngineRay2D_TypeID == -1)
            {
			    bool is_first;
                UnityEngineRay2D_TypeID = getTypeId(L, typeof(UnityEngine.Ray2D), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 16, UnityEngineRay2D_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Ray2D ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Ray2D val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineRay2D_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Ray2D");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Ray2D");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Ray2D)objectCasters.GetCaster(typeof(UnityEngine.Ray2D))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineRay2D(RealStatePtr L, int index, UnityEngine.Ray2D val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineRay2D_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Ray2D");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Ray2D ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int XLuaFrameworkNetPackageType_TypeID = -1;
		int XLuaFrameworkNetPackageType_EnumRef = -1;
        
        public void PushXLuaFrameworkNetPackageType(RealStatePtr L, XLuaFramework.NetPackageType val)
        {
            if (XLuaFrameworkNetPackageType_TypeID == -1)
            {
			    bool is_first;
                XLuaFrameworkNetPackageType_TypeID = getTypeId(L, typeof(XLuaFramework.NetPackageType), out is_first);
				
				if (XLuaFrameworkNetPackageType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(XLuaFramework.NetPackageType));
				    XLuaFrameworkNetPackageType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, XLuaFrameworkNetPackageType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, XLuaFrameworkNetPackageType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for XLuaFramework.NetPackageType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, XLuaFrameworkNetPackageType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out XLuaFramework.NetPackageType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != XLuaFrameworkNetPackageType_TypeID)
				{
				    throw new Exception("invalid userdata for XLuaFramework.NetPackageType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for XLuaFramework.NetPackageType");
                }
				val = (XLuaFramework.NetPackageType)e;
                
            }
            else
            {
                val = (XLuaFramework.NetPackageType)objectCasters.GetCaster(typeof(XLuaFramework.NetPackageType))(L, index, null);
            }
        }
		
        public void UpdateXLuaFrameworkNetPackageType(RealStatePtr L, int index, XLuaFramework.NetPackageType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != XLuaFrameworkNetPackageType_TypeID)
				{
				    throw new Exception("invalid userdata for XLuaFramework.NetPackageType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for XLuaFramework.NetPackageType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineUIInputFieldContentType_TypeID = -1;
		int UnityEngineUIInputFieldContentType_EnumRef = -1;
        
        public void PushUnityEngineUIInputFieldContentType(RealStatePtr L, UnityEngine.UI.InputField.ContentType val)
        {
            if (UnityEngineUIInputFieldContentType_TypeID == -1)
            {
			    bool is_first;
                UnityEngineUIInputFieldContentType_TypeID = getTypeId(L, typeof(UnityEngine.UI.InputField.ContentType), out is_first);
				
				if (UnityEngineUIInputFieldContentType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.UI.InputField.ContentType));
				    UnityEngineUIInputFieldContentType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineUIInputFieldContentType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineUIInputFieldContentType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.UI.InputField.ContentType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineUIInputFieldContentType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.UI.InputField.ContentType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIInputFieldContentType_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.InputField.ContentType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.UI.InputField.ContentType");
                }
				val = (UnityEngine.UI.InputField.ContentType)e;
                
            }
            else
            {
                val = (UnityEngine.UI.InputField.ContentType)objectCasters.GetCaster(typeof(UnityEngine.UI.InputField.ContentType))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineUIInputFieldContentType(RealStatePtr L, int index, UnityEngine.UI.InputField.ContentType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIInputFieldContentType_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.InputField.ContentType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.UI.InputField.ContentType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineUIInputFieldInputType_TypeID = -1;
		int UnityEngineUIInputFieldInputType_EnumRef = -1;
        
        public void PushUnityEngineUIInputFieldInputType(RealStatePtr L, UnityEngine.UI.InputField.InputType val)
        {
            if (UnityEngineUIInputFieldInputType_TypeID == -1)
            {
			    bool is_first;
                UnityEngineUIInputFieldInputType_TypeID = getTypeId(L, typeof(UnityEngine.UI.InputField.InputType), out is_first);
				
				if (UnityEngineUIInputFieldInputType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.UI.InputField.InputType));
				    UnityEngineUIInputFieldInputType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineUIInputFieldInputType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineUIInputFieldInputType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.UI.InputField.InputType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineUIInputFieldInputType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.UI.InputField.InputType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIInputFieldInputType_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.InputField.InputType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.UI.InputField.InputType");
                }
				val = (UnityEngine.UI.InputField.InputType)e;
                
            }
            else
            {
                val = (UnityEngine.UI.InputField.InputType)objectCasters.GetCaster(typeof(UnityEngine.UI.InputField.InputType))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineUIInputFieldInputType(RealStatePtr L, int index, UnityEngine.UI.InputField.InputType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIInputFieldInputType_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.InputField.InputType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.UI.InputField.InputType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineUIInputFieldCharacterValidation_TypeID = -1;
		int UnityEngineUIInputFieldCharacterValidation_EnumRef = -1;
        
        public void PushUnityEngineUIInputFieldCharacterValidation(RealStatePtr L, UnityEngine.UI.InputField.CharacterValidation val)
        {
            if (UnityEngineUIInputFieldCharacterValidation_TypeID == -1)
            {
			    bool is_first;
                UnityEngineUIInputFieldCharacterValidation_TypeID = getTypeId(L, typeof(UnityEngine.UI.InputField.CharacterValidation), out is_first);
				
				if (UnityEngineUIInputFieldCharacterValidation_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.UI.InputField.CharacterValidation));
				    UnityEngineUIInputFieldCharacterValidation_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineUIInputFieldCharacterValidation_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineUIInputFieldCharacterValidation_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.UI.InputField.CharacterValidation ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineUIInputFieldCharacterValidation_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.UI.InputField.CharacterValidation val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIInputFieldCharacterValidation_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.InputField.CharacterValidation");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.UI.InputField.CharacterValidation");
                }
				val = (UnityEngine.UI.InputField.CharacterValidation)e;
                
            }
            else
            {
                val = (UnityEngine.UI.InputField.CharacterValidation)objectCasters.GetCaster(typeof(UnityEngine.UI.InputField.CharacterValidation))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineUIInputFieldCharacterValidation(RealStatePtr L, int index, UnityEngine.UI.InputField.CharacterValidation val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIInputFieldCharacterValidation_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.InputField.CharacterValidation");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.UI.InputField.CharacterValidation ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineUIInputFieldLineType_TypeID = -1;
		int UnityEngineUIInputFieldLineType_EnumRef = -1;
        
        public void PushUnityEngineUIInputFieldLineType(RealStatePtr L, UnityEngine.UI.InputField.LineType val)
        {
            if (UnityEngineUIInputFieldLineType_TypeID == -1)
            {
			    bool is_first;
                UnityEngineUIInputFieldLineType_TypeID = getTypeId(L, typeof(UnityEngine.UI.InputField.LineType), out is_first);
				
				if (UnityEngineUIInputFieldLineType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.UI.InputField.LineType));
				    UnityEngineUIInputFieldLineType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineUIInputFieldLineType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineUIInputFieldLineType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.UI.InputField.LineType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineUIInputFieldLineType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.UI.InputField.LineType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIInputFieldLineType_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.InputField.LineType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.UI.InputField.LineType");
                }
				val = (UnityEngine.UI.InputField.LineType)e;
                
            }
            else
            {
                val = (UnityEngine.UI.InputField.LineType)objectCasters.GetCaster(typeof(UnityEngine.UI.InputField.LineType))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineUIInputFieldLineType(RealStatePtr L, int index, UnityEngine.UI.InputField.LineType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIInputFieldLineType_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.InputField.LineType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.UI.InputField.LineType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTextAnchor_TypeID = -1;
		int UnityEngineTextAnchor_EnumRef = -1;
        
        public void PushUnityEngineTextAnchor(RealStatePtr L, UnityEngine.TextAnchor val)
        {
            if (UnityEngineTextAnchor_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTextAnchor_TypeID = getTypeId(L, typeof(UnityEngine.TextAnchor), out is_first);
				
				if (UnityEngineTextAnchor_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.TextAnchor));
				    UnityEngineTextAnchor_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTextAnchor_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTextAnchor_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.TextAnchor ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTextAnchor_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.TextAnchor val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTextAnchor_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.TextAnchor");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.TextAnchor");
                }
				val = (UnityEngine.TextAnchor)e;
                
            }
            else
            {
                val = (UnityEngine.TextAnchor)objectCasters.GetCaster(typeof(UnityEngine.TextAnchor))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTextAnchor(RealStatePtr L, int index, UnityEngine.TextAnchor val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTextAnchor_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.TextAnchor");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.TextAnchor ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTouchPhase_TypeID = -1;
		int UnityEngineTouchPhase_EnumRef = -1;
        
        public void PushUnityEngineTouchPhase(RealStatePtr L, UnityEngine.TouchPhase val)
        {
            if (UnityEngineTouchPhase_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTouchPhase_TypeID = getTypeId(L, typeof(UnityEngine.TouchPhase), out is_first);
				
				if (UnityEngineTouchPhase_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.TouchPhase));
				    UnityEngineTouchPhase_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTouchPhase_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTouchPhase_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.TouchPhase ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTouchPhase_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.TouchPhase val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTouchPhase_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.TouchPhase");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.TouchPhase");
                }
				val = (UnityEngine.TouchPhase)e;
                
            }
            else
            {
                val = (UnityEngine.TouchPhase)objectCasters.GetCaster(typeof(UnityEngine.TouchPhase))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTouchPhase(RealStatePtr L, int index, UnityEngine.TouchPhase val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTouchPhase_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.TouchPhase");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.TouchPhase ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineRectTransformAxis_TypeID = -1;
		int UnityEngineRectTransformAxis_EnumRef = -1;
        
        public void PushUnityEngineRectTransformAxis(RealStatePtr L, UnityEngine.RectTransform.Axis val)
        {
            if (UnityEngineRectTransformAxis_TypeID == -1)
            {
			    bool is_first;
                UnityEngineRectTransformAxis_TypeID = getTypeId(L, typeof(UnityEngine.RectTransform.Axis), out is_first);
				
				if (UnityEngineRectTransformAxis_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.RectTransform.Axis));
				    UnityEngineRectTransformAxis_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineRectTransformAxis_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineRectTransformAxis_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.RectTransform.Axis ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineRectTransformAxis_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.RectTransform.Axis val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineRectTransformAxis_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.RectTransform.Axis");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.RectTransform.Axis");
                }
				val = (UnityEngine.RectTransform.Axis)e;
                
            }
            else
            {
                val = (UnityEngine.RectTransform.Axis)objectCasters.GetCaster(typeof(UnityEngine.RectTransform.Axis))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineRectTransformAxis(RealStatePtr L, int index, UnityEngine.RectTransform.Axis val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineRectTransformAxis_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.RectTransform.Axis");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.RectTransform.Axis ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineRectTransformEdge_TypeID = -1;
		int UnityEngineRectTransformEdge_EnumRef = -1;
        
        public void PushUnityEngineRectTransformEdge(RealStatePtr L, UnityEngine.RectTransform.Edge val)
        {
            if (UnityEngineRectTransformEdge_TypeID == -1)
            {
			    bool is_first;
                UnityEngineRectTransformEdge_TypeID = getTypeId(L, typeof(UnityEngine.RectTransform.Edge), out is_first);
				
				if (UnityEngineRectTransformEdge_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.RectTransform.Edge));
				    UnityEngineRectTransformEdge_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineRectTransformEdge_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineRectTransformEdge_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.RectTransform.Edge ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineRectTransformEdge_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.RectTransform.Edge val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineRectTransformEdge_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.RectTransform.Edge");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.RectTransform.Edge");
                }
				val = (UnityEngine.RectTransform.Edge)e;
                
            }
            else
            {
                val = (UnityEngine.RectTransform.Edge)objectCasters.GetCaster(typeof(UnityEngine.RectTransform.Edge))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineRectTransformEdge(RealStatePtr L, int index, UnityEngine.RectTransform.Edge val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineRectTransformEdge_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.RectTransform.Edge");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.RectTransform.Edge ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineUIScrollRectMovementType_TypeID = -1;
		int UnityEngineUIScrollRectMovementType_EnumRef = -1;
        
        public void PushUnityEngineUIScrollRectMovementType(RealStatePtr L, UnityEngine.UI.ScrollRect.MovementType val)
        {
            if (UnityEngineUIScrollRectMovementType_TypeID == -1)
            {
			    bool is_first;
                UnityEngineUIScrollRectMovementType_TypeID = getTypeId(L, typeof(UnityEngine.UI.ScrollRect.MovementType), out is_first);
				
				if (UnityEngineUIScrollRectMovementType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.UI.ScrollRect.MovementType));
				    UnityEngineUIScrollRectMovementType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineUIScrollRectMovementType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineUIScrollRectMovementType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.UI.ScrollRect.MovementType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineUIScrollRectMovementType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.UI.ScrollRect.MovementType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIScrollRectMovementType_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.ScrollRect.MovementType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.UI.ScrollRect.MovementType");
                }
				val = (UnityEngine.UI.ScrollRect.MovementType)e;
                
            }
            else
            {
                val = (UnityEngine.UI.ScrollRect.MovementType)objectCasters.GetCaster(typeof(UnityEngine.UI.ScrollRect.MovementType))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineUIScrollRectMovementType(RealStatePtr L, int index, UnityEngine.UI.ScrollRect.MovementType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIScrollRectMovementType_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.ScrollRect.MovementType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.UI.ScrollRect.MovementType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineUIScrollRectScrollbarVisibility_TypeID = -1;
		int UnityEngineUIScrollRectScrollbarVisibility_EnumRef = -1;
        
        public void PushUnityEngineUIScrollRectScrollbarVisibility(RealStatePtr L, UnityEngine.UI.ScrollRect.ScrollbarVisibility val)
        {
            if (UnityEngineUIScrollRectScrollbarVisibility_TypeID == -1)
            {
			    bool is_first;
                UnityEngineUIScrollRectScrollbarVisibility_TypeID = getTypeId(L, typeof(UnityEngine.UI.ScrollRect.ScrollbarVisibility), out is_first);
				
				if (UnityEngineUIScrollRectScrollbarVisibility_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.UI.ScrollRect.ScrollbarVisibility));
				    UnityEngineUIScrollRectScrollbarVisibility_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineUIScrollRectScrollbarVisibility_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineUIScrollRectScrollbarVisibility_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.UI.ScrollRect.ScrollbarVisibility ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineUIScrollRectScrollbarVisibility_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.UI.ScrollRect.ScrollbarVisibility val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIScrollRectScrollbarVisibility_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.ScrollRect.ScrollbarVisibility");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.UI.ScrollRect.ScrollbarVisibility");
                }
				val = (UnityEngine.UI.ScrollRect.ScrollbarVisibility)e;
                
            }
            else
            {
                val = (UnityEngine.UI.ScrollRect.ScrollbarVisibility)objectCasters.GetCaster(typeof(UnityEngine.UI.ScrollRect.ScrollbarVisibility))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineUIScrollRectScrollbarVisibility(RealStatePtr L, int index, UnityEngine.UI.ScrollRect.ScrollbarVisibility val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIScrollRectScrollbarVisibility_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.ScrollRect.ScrollbarVisibility");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.UI.ScrollRect.ScrollbarVisibility ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineUIImageType_TypeID = -1;
		int UnityEngineUIImageType_EnumRef = -1;
        
        public void PushUnityEngineUIImageType(RealStatePtr L, UnityEngine.UI.Image.Type val)
        {
            if (UnityEngineUIImageType_TypeID == -1)
            {
			    bool is_first;
                UnityEngineUIImageType_TypeID = getTypeId(L, typeof(UnityEngine.UI.Image.Type), out is_first);
				
				if (UnityEngineUIImageType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.UI.Image.Type));
				    UnityEngineUIImageType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineUIImageType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineUIImageType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.UI.Image.Type ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineUIImageType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.UI.Image.Type val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIImageType_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.Image.Type");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.UI.Image.Type");
                }
				val = (UnityEngine.UI.Image.Type)e;
                
            }
            else
            {
                val = (UnityEngine.UI.Image.Type)objectCasters.GetCaster(typeof(UnityEngine.UI.Image.Type))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineUIImageType(RealStatePtr L, int index, UnityEngine.UI.Image.Type val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIImageType_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.Image.Type");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.UI.Image.Type ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineUIImageFillMethod_TypeID = -1;
		int UnityEngineUIImageFillMethod_EnumRef = -1;
        
        public void PushUnityEngineUIImageFillMethod(RealStatePtr L, UnityEngine.UI.Image.FillMethod val)
        {
            if (UnityEngineUIImageFillMethod_TypeID == -1)
            {
			    bool is_first;
                UnityEngineUIImageFillMethod_TypeID = getTypeId(L, typeof(UnityEngine.UI.Image.FillMethod), out is_first);
				
				if (UnityEngineUIImageFillMethod_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.UI.Image.FillMethod));
				    UnityEngineUIImageFillMethod_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineUIImageFillMethod_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineUIImageFillMethod_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.UI.Image.FillMethod ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineUIImageFillMethod_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.UI.Image.FillMethod val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIImageFillMethod_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.Image.FillMethod");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.UI.Image.FillMethod");
                }
				val = (UnityEngine.UI.Image.FillMethod)e;
                
            }
            else
            {
                val = (UnityEngine.UI.Image.FillMethod)objectCasters.GetCaster(typeof(UnityEngine.UI.Image.FillMethod))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineUIImageFillMethod(RealStatePtr L, int index, UnityEngine.UI.Image.FillMethod val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIImageFillMethod_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.Image.FillMethod");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.UI.Image.FillMethod ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineUIImageOriginHorizontal_TypeID = -1;
		int UnityEngineUIImageOriginHorizontal_EnumRef = -1;
        
        public void PushUnityEngineUIImageOriginHorizontal(RealStatePtr L, UnityEngine.UI.Image.OriginHorizontal val)
        {
            if (UnityEngineUIImageOriginHorizontal_TypeID == -1)
            {
			    bool is_first;
                UnityEngineUIImageOriginHorizontal_TypeID = getTypeId(L, typeof(UnityEngine.UI.Image.OriginHorizontal), out is_first);
				
				if (UnityEngineUIImageOriginHorizontal_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.UI.Image.OriginHorizontal));
				    UnityEngineUIImageOriginHorizontal_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineUIImageOriginHorizontal_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineUIImageOriginHorizontal_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.UI.Image.OriginHorizontal ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineUIImageOriginHorizontal_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.UI.Image.OriginHorizontal val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIImageOriginHorizontal_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.Image.OriginHorizontal");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.UI.Image.OriginHorizontal");
                }
				val = (UnityEngine.UI.Image.OriginHorizontal)e;
                
            }
            else
            {
                val = (UnityEngine.UI.Image.OriginHorizontal)objectCasters.GetCaster(typeof(UnityEngine.UI.Image.OriginHorizontal))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineUIImageOriginHorizontal(RealStatePtr L, int index, UnityEngine.UI.Image.OriginHorizontal val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIImageOriginHorizontal_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.Image.OriginHorizontal");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.UI.Image.OriginHorizontal ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineUIImageOriginVertical_TypeID = -1;
		int UnityEngineUIImageOriginVertical_EnumRef = -1;
        
        public void PushUnityEngineUIImageOriginVertical(RealStatePtr L, UnityEngine.UI.Image.OriginVertical val)
        {
            if (UnityEngineUIImageOriginVertical_TypeID == -1)
            {
			    bool is_first;
                UnityEngineUIImageOriginVertical_TypeID = getTypeId(L, typeof(UnityEngine.UI.Image.OriginVertical), out is_first);
				
				if (UnityEngineUIImageOriginVertical_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.UI.Image.OriginVertical));
				    UnityEngineUIImageOriginVertical_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineUIImageOriginVertical_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineUIImageOriginVertical_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.UI.Image.OriginVertical ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineUIImageOriginVertical_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.UI.Image.OriginVertical val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIImageOriginVertical_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.Image.OriginVertical");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.UI.Image.OriginVertical");
                }
				val = (UnityEngine.UI.Image.OriginVertical)e;
                
            }
            else
            {
                val = (UnityEngine.UI.Image.OriginVertical)objectCasters.GetCaster(typeof(UnityEngine.UI.Image.OriginVertical))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineUIImageOriginVertical(RealStatePtr L, int index, UnityEngine.UI.Image.OriginVertical val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIImageOriginVertical_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.Image.OriginVertical");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.UI.Image.OriginVertical ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineUIImageOrigin90_TypeID = -1;
		int UnityEngineUIImageOrigin90_EnumRef = -1;
        
        public void PushUnityEngineUIImageOrigin90(RealStatePtr L, UnityEngine.UI.Image.Origin90 val)
        {
            if (UnityEngineUIImageOrigin90_TypeID == -1)
            {
			    bool is_first;
                UnityEngineUIImageOrigin90_TypeID = getTypeId(L, typeof(UnityEngine.UI.Image.Origin90), out is_first);
				
				if (UnityEngineUIImageOrigin90_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.UI.Image.Origin90));
				    UnityEngineUIImageOrigin90_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineUIImageOrigin90_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineUIImageOrigin90_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.UI.Image.Origin90 ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineUIImageOrigin90_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.UI.Image.Origin90 val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIImageOrigin90_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.Image.Origin90");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.UI.Image.Origin90");
                }
				val = (UnityEngine.UI.Image.Origin90)e;
                
            }
            else
            {
                val = (UnityEngine.UI.Image.Origin90)objectCasters.GetCaster(typeof(UnityEngine.UI.Image.Origin90))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineUIImageOrigin90(RealStatePtr L, int index, UnityEngine.UI.Image.Origin90 val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIImageOrigin90_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.Image.Origin90");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.UI.Image.Origin90 ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineUIImageOrigin180_TypeID = -1;
		int UnityEngineUIImageOrigin180_EnumRef = -1;
        
        public void PushUnityEngineUIImageOrigin180(RealStatePtr L, UnityEngine.UI.Image.Origin180 val)
        {
            if (UnityEngineUIImageOrigin180_TypeID == -1)
            {
			    bool is_first;
                UnityEngineUIImageOrigin180_TypeID = getTypeId(L, typeof(UnityEngine.UI.Image.Origin180), out is_first);
				
				if (UnityEngineUIImageOrigin180_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.UI.Image.Origin180));
				    UnityEngineUIImageOrigin180_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineUIImageOrigin180_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineUIImageOrigin180_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.UI.Image.Origin180 ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineUIImageOrigin180_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.UI.Image.Origin180 val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIImageOrigin180_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.Image.Origin180");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.UI.Image.Origin180");
                }
				val = (UnityEngine.UI.Image.Origin180)e;
                
            }
            else
            {
                val = (UnityEngine.UI.Image.Origin180)objectCasters.GetCaster(typeof(UnityEngine.UI.Image.Origin180))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineUIImageOrigin180(RealStatePtr L, int index, UnityEngine.UI.Image.Origin180 val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIImageOrigin180_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.Image.Origin180");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.UI.Image.Origin180 ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineUIImageOrigin360_TypeID = -1;
		int UnityEngineUIImageOrigin360_EnumRef = -1;
        
        public void PushUnityEngineUIImageOrigin360(RealStatePtr L, UnityEngine.UI.Image.Origin360 val)
        {
            if (UnityEngineUIImageOrigin360_TypeID == -1)
            {
			    bool is_first;
                UnityEngineUIImageOrigin360_TypeID = getTypeId(L, typeof(UnityEngine.UI.Image.Origin360), out is_first);
				
				if (UnityEngineUIImageOrigin360_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.UI.Image.Origin360));
				    UnityEngineUIImageOrigin360_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineUIImageOrigin360_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineUIImageOrigin360_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.UI.Image.Origin360 ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineUIImageOrigin360_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.UI.Image.Origin360 val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIImageOrigin360_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.Image.Origin360");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.UI.Image.Origin360");
                }
				val = (UnityEngine.UI.Image.Origin360)e;
                
            }
            else
            {
                val = (UnityEngine.UI.Image.Origin360)objectCasters.GetCaster(typeof(UnityEngine.UI.Image.Origin360))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineUIImageOrigin360(RealStatePtr L, int index, UnityEngine.UI.Image.Origin360 val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineUIImageOrigin360_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.UI.Image.Origin360");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.UI.Image.Origin360 ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineCameraFieldOfViewAxis_TypeID = -1;
		int UnityEngineCameraFieldOfViewAxis_EnumRef = -1;
        
        public void PushUnityEngineCameraFieldOfViewAxis(RealStatePtr L, UnityEngine.Camera.FieldOfViewAxis val)
        {
            if (UnityEngineCameraFieldOfViewAxis_TypeID == -1)
            {
			    bool is_first;
                UnityEngineCameraFieldOfViewAxis_TypeID = getTypeId(L, typeof(UnityEngine.Camera.FieldOfViewAxis), out is_first);
				
				if (UnityEngineCameraFieldOfViewAxis_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Camera.FieldOfViewAxis));
				    UnityEngineCameraFieldOfViewAxis_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineCameraFieldOfViewAxis_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineCameraFieldOfViewAxis_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Camera.FieldOfViewAxis ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineCameraFieldOfViewAxis_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Camera.FieldOfViewAxis val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineCameraFieldOfViewAxis_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Camera.FieldOfViewAxis");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Camera.FieldOfViewAxis");
                }
				val = (UnityEngine.Camera.FieldOfViewAxis)e;
                
            }
            else
            {
                val = (UnityEngine.Camera.FieldOfViewAxis)objectCasters.GetCaster(typeof(UnityEngine.Camera.FieldOfViewAxis))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineCameraFieldOfViewAxis(RealStatePtr L, int index, UnityEngine.Camera.FieldOfViewAxis val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineCameraFieldOfViewAxis_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Camera.FieldOfViewAxis");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Camera.FieldOfViewAxis ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineCameraMonoOrStereoscopicEye_TypeID = -1;
		int UnityEngineCameraMonoOrStereoscopicEye_EnumRef = -1;
        
        public void PushUnityEngineCameraMonoOrStereoscopicEye(RealStatePtr L, UnityEngine.Camera.MonoOrStereoscopicEye val)
        {
            if (UnityEngineCameraMonoOrStereoscopicEye_TypeID == -1)
            {
			    bool is_first;
                UnityEngineCameraMonoOrStereoscopicEye_TypeID = getTypeId(L, typeof(UnityEngine.Camera.MonoOrStereoscopicEye), out is_first);
				
				if (UnityEngineCameraMonoOrStereoscopicEye_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Camera.MonoOrStereoscopicEye));
				    UnityEngineCameraMonoOrStereoscopicEye_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineCameraMonoOrStereoscopicEye_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineCameraMonoOrStereoscopicEye_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Camera.MonoOrStereoscopicEye ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineCameraMonoOrStereoscopicEye_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Camera.MonoOrStereoscopicEye val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineCameraMonoOrStereoscopicEye_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Camera.MonoOrStereoscopicEye");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Camera.MonoOrStereoscopicEye");
                }
				val = (UnityEngine.Camera.MonoOrStereoscopicEye)e;
                
            }
            else
            {
                val = (UnityEngine.Camera.MonoOrStereoscopicEye)objectCasters.GetCaster(typeof(UnityEngine.Camera.MonoOrStereoscopicEye))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineCameraMonoOrStereoscopicEye(RealStatePtr L, int index, UnityEngine.Camera.MonoOrStereoscopicEye val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineCameraMonoOrStereoscopicEye_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Camera.MonoOrStereoscopicEye");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Camera.MonoOrStereoscopicEye ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineCameraStereoscopicEye_TypeID = -1;
		int UnityEngineCameraStereoscopicEye_EnumRef = -1;
        
        public void PushUnityEngineCameraStereoscopicEye(RealStatePtr L, UnityEngine.Camera.StereoscopicEye val)
        {
            if (UnityEngineCameraStereoscopicEye_TypeID == -1)
            {
			    bool is_first;
                UnityEngineCameraStereoscopicEye_TypeID = getTypeId(L, typeof(UnityEngine.Camera.StereoscopicEye), out is_first);
				
				if (UnityEngineCameraStereoscopicEye_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Camera.StereoscopicEye));
				    UnityEngineCameraStereoscopicEye_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineCameraStereoscopicEye_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineCameraStereoscopicEye_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Camera.StereoscopicEye ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineCameraStereoscopicEye_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Camera.StereoscopicEye val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineCameraStereoscopicEye_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Camera.StereoscopicEye");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Camera.StereoscopicEye");
                }
				val = (UnityEngine.Camera.StereoscopicEye)e;
                
            }
            else
            {
                val = (UnityEngine.Camera.StereoscopicEye)objectCasters.GetCaster(typeof(UnityEngine.Camera.StereoscopicEye))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineCameraStereoscopicEye(RealStatePtr L, int index, UnityEngine.Camera.StereoscopicEye val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineCameraStereoscopicEye_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Camera.StereoscopicEye");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Camera.StereoscopicEye ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineCameraGateFitMode_TypeID = -1;
		int UnityEngineCameraGateFitMode_EnumRef = -1;
        
        public void PushUnityEngineCameraGateFitMode(RealStatePtr L, UnityEngine.Camera.GateFitMode val)
        {
            if (UnityEngineCameraGateFitMode_TypeID == -1)
            {
			    bool is_first;
                UnityEngineCameraGateFitMode_TypeID = getTypeId(L, typeof(UnityEngine.Camera.GateFitMode), out is_first);
				
				if (UnityEngineCameraGateFitMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Camera.GateFitMode));
				    UnityEngineCameraGateFitMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineCameraGateFitMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineCameraGateFitMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Camera.GateFitMode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineCameraGateFitMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Camera.GateFitMode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineCameraGateFitMode_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Camera.GateFitMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Camera.GateFitMode");
                }
				val = (UnityEngine.Camera.GateFitMode)e;
                
            }
            else
            {
                val = (UnityEngine.Camera.GateFitMode)objectCasters.GetCaster(typeof(UnityEngine.Camera.GateFitMode))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineCameraGateFitMode(RealStatePtr L, int index, UnityEngine.Camera.GateFitMode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineCameraGateFitMode_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Camera.GateFitMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Camera.GateFitMode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineKeyCode_TypeID = -1;
		int UnityEngineKeyCode_EnumRef = -1;
        
        public void PushUnityEngineKeyCode(RealStatePtr L, UnityEngine.KeyCode val)
        {
            if (UnityEngineKeyCode_TypeID == -1)
            {
			    bool is_first;
                UnityEngineKeyCode_TypeID = getTypeId(L, typeof(UnityEngine.KeyCode), out is_first);
				
				if (UnityEngineKeyCode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.KeyCode));
				    UnityEngineKeyCode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineKeyCode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineKeyCode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.KeyCode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineKeyCode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.KeyCode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineKeyCode_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.KeyCode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.KeyCode");
                }
				val = (UnityEngine.KeyCode)e;
                
            }
            else
            {
                val = (UnityEngine.KeyCode)objectCasters.GetCaster(typeof(UnityEngine.KeyCode))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineKeyCode(RealStatePtr L, int index, UnityEngine.KeyCode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineKeyCode_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.KeyCode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.KeyCode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int XLuaFrameworkDisType_TypeID = -1;
		int XLuaFrameworkDisType_EnumRef = -1;
        
        public void PushXLuaFrameworkDisType(RealStatePtr L, XLuaFramework.DisType val)
        {
            if (XLuaFrameworkDisType_TypeID == -1)
            {
			    bool is_first;
                XLuaFrameworkDisType_TypeID = getTypeId(L, typeof(XLuaFramework.DisType), out is_first);
				
				if (XLuaFrameworkDisType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(XLuaFramework.DisType));
				    XLuaFrameworkDisType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, XLuaFrameworkDisType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, XLuaFrameworkDisType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for XLuaFramework.DisType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, XLuaFrameworkDisType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out XLuaFramework.DisType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != XLuaFrameworkDisType_TypeID)
				{
				    throw new Exception("invalid userdata for XLuaFramework.DisType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for XLuaFramework.DisType");
                }
				val = (XLuaFramework.DisType)e;
                
            }
            else
            {
                val = (XLuaFramework.DisType)objectCasters.GetCaster(typeof(XLuaFramework.DisType))(L, index, null);
            }
        }
		
        public void UpdateXLuaFrameworkDisType(RealStatePtr L, int index, XLuaFramework.DisType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != XLuaFrameworkDisType_TypeID)
				{
				    throw new Exception("invalid userdata for XLuaFramework.DisType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for XLuaFramework.DisType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityMMOSceneInfoKey_TypeID = -1;
		int UnityMMOSceneInfoKey_EnumRef = -1;
        
        public void PushUnityMMOSceneInfoKey(RealStatePtr L, UnityMMO.SceneInfoKey val)
        {
            if (UnityMMOSceneInfoKey_TypeID == -1)
            {
			    bool is_first;
                UnityMMOSceneInfoKey_TypeID = getTypeId(L, typeof(UnityMMO.SceneInfoKey), out is_first);
				
				if (UnityMMOSceneInfoKey_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityMMO.SceneInfoKey));
				    UnityMMOSceneInfoKey_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityMMOSceneInfoKey_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityMMOSceneInfoKey_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityMMO.SceneInfoKey ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityMMOSceneInfoKey_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityMMO.SceneInfoKey val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOSceneInfoKey_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.SceneInfoKey");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityMMO.SceneInfoKey");
                }
				val = (UnityMMO.SceneInfoKey)e;
                
            }
            else
            {
                val = (UnityMMO.SceneInfoKey)objectCasters.GetCaster(typeof(UnityMMO.SceneInfoKey))(L, index, null);
            }
        }
		
        public void UpdateUnityMMOSceneInfoKey(RealStatePtr L, int index, UnityMMO.SceneInfoKey val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOSceneInfoKey_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.SceneInfoKey");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityMMO.SceneInfoKey ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityMMOSceneObjectType_TypeID = -1;
		int UnityMMOSceneObjectType_EnumRef = -1;
        
        public void PushUnityMMOSceneObjectType(RealStatePtr L, UnityMMO.SceneObjectType val)
        {
            if (UnityMMOSceneObjectType_TypeID == -1)
            {
			    bool is_first;
                UnityMMOSceneObjectType_TypeID = getTypeId(L, typeof(UnityMMO.SceneObjectType), out is_first);
				
				if (UnityMMOSceneObjectType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityMMO.SceneObjectType));
				    UnityMMOSceneObjectType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityMMOSceneObjectType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityMMOSceneObjectType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityMMO.SceneObjectType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityMMOSceneObjectType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityMMO.SceneObjectType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOSceneObjectType_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.SceneObjectType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityMMO.SceneObjectType");
                }
				val = (UnityMMO.SceneObjectType)e;
                
            }
            else
            {
                val = (UnityMMO.SceneObjectType)objectCasters.GetCaster(typeof(UnityMMO.SceneObjectType))(L, index, null);
            }
        }
		
        public void UpdateUnityMMOSceneObjectType(RealStatePtr L, int index, UnityMMO.SceneObjectType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOSceneObjectType_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.SceneObjectType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityMMO.SceneObjectType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int XLuaFrameworkMainState_TypeID = -1;
		int XLuaFrameworkMainState_EnumRef = -1;
        
        public void PushXLuaFrameworkMainState(RealStatePtr L, XLuaFramework.Main.State val)
        {
            if (XLuaFrameworkMainState_TypeID == -1)
            {
			    bool is_first;
                XLuaFrameworkMainState_TypeID = getTypeId(L, typeof(XLuaFramework.Main.State), out is_first);
				
				if (XLuaFrameworkMainState_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(XLuaFramework.Main.State));
				    XLuaFrameworkMainState_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, XLuaFrameworkMainState_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, XLuaFrameworkMainState_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for XLuaFramework.Main.State ,value="+val);
            }
			
			LuaAPI.lua_getref(L, XLuaFrameworkMainState_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out XLuaFramework.Main.State val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != XLuaFrameworkMainState_TypeID)
				{
				    throw new Exception("invalid userdata for XLuaFramework.Main.State");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for XLuaFramework.Main.State");
                }
				val = (XLuaFramework.Main.State)e;
                
            }
            else
            {
                val = (XLuaFramework.Main.State)objectCasters.GetCaster(typeof(XLuaFramework.Main.State))(L, index, null);
            }
        }
		
        public void UpdateXLuaFrameworkMainState(RealStatePtr L, int index, XLuaFramework.Main.State val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != XLuaFrameworkMainState_TypeID)
				{
				    throw new Exception("invalid userdata for XLuaFramework.Main.State");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for XLuaFramework.Main.State ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int XLuaFrameworkMainSubState_TypeID = -1;
		int XLuaFrameworkMainSubState_EnumRef = -1;
        
        public void PushXLuaFrameworkMainSubState(RealStatePtr L, XLuaFramework.Main.SubState val)
        {
            if (XLuaFrameworkMainSubState_TypeID == -1)
            {
			    bool is_first;
                XLuaFrameworkMainSubState_TypeID = getTypeId(L, typeof(XLuaFramework.Main.SubState), out is_first);
				
				if (XLuaFrameworkMainSubState_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(XLuaFramework.Main.SubState));
				    XLuaFrameworkMainSubState_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, XLuaFrameworkMainSubState_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, XLuaFrameworkMainSubState_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for XLuaFramework.Main.SubState ,value="+val);
            }
			
			LuaAPI.lua_getref(L, XLuaFrameworkMainSubState_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out XLuaFramework.Main.SubState val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != XLuaFrameworkMainSubState_TypeID)
				{
				    throw new Exception("invalid userdata for XLuaFramework.Main.SubState");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for XLuaFramework.Main.SubState");
                }
				val = (XLuaFramework.Main.SubState)e;
                
            }
            else
            {
                val = (XLuaFramework.Main.SubState)objectCasters.GetCaster(typeof(XLuaFramework.Main.SubState))(L, index, null);
            }
        }
		
        public void UpdateXLuaFrameworkMainSubState(RealStatePtr L, int index, XLuaFramework.Main.SubState val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != XLuaFrameworkMainSubState_TypeID)
				{
				    throw new Exception("invalid userdata for XLuaFramework.Main.SubState");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for XLuaFramework.Main.SubState ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityMMOTimelineInfoEvent_TypeID = -1;
		int UnityMMOTimelineInfoEvent_EnumRef = -1;
        
        public void PushUnityMMOTimelineInfoEvent(RealStatePtr L, UnityMMO.TimelineInfo.Event val)
        {
            if (UnityMMOTimelineInfoEvent_TypeID == -1)
            {
			    bool is_first;
                UnityMMOTimelineInfoEvent_TypeID = getTypeId(L, typeof(UnityMMO.TimelineInfo.Event), out is_first);
				
				if (UnityMMOTimelineInfoEvent_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityMMO.TimelineInfo.Event));
				    UnityMMOTimelineInfoEvent_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityMMOTimelineInfoEvent_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityMMOTimelineInfoEvent_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityMMO.TimelineInfo.Event ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityMMOTimelineInfoEvent_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityMMO.TimelineInfo.Event val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOTimelineInfoEvent_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.TimelineInfo.Event");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityMMO.TimelineInfo.Event");
                }
				val = (UnityMMO.TimelineInfo.Event)e;
                
            }
            else
            {
                val = (UnityMMO.TimelineInfo.Event)objectCasters.GetCaster(typeof(UnityMMO.TimelineInfo.Event))(L, index, null);
            }
        }
		
        public void UpdateUnityMMOTimelineInfoEvent(RealStatePtr L, int index, UnityMMO.TimelineInfo.Event val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOTimelineInfoEvent_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.TimelineInfo.Event");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityMMO.TimelineInfo.Event ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityMMONameboardColorStyle_TypeID = -1;
		int UnityMMONameboardColorStyle_EnumRef = -1;
        
        public void PushUnityMMONameboardColorStyle(RealStatePtr L, UnityMMO.Nameboard.ColorStyle val)
        {
            if (UnityMMONameboardColorStyle_TypeID == -1)
            {
			    bool is_first;
                UnityMMONameboardColorStyle_TypeID = getTypeId(L, typeof(UnityMMO.Nameboard.ColorStyle), out is_first);
				
				if (UnityMMONameboardColorStyle_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityMMO.Nameboard.ColorStyle));
				    UnityMMONameboardColorStyle_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityMMONameboardColorStyle_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityMMONameboardColorStyle_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityMMO.Nameboard.ColorStyle ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityMMONameboardColorStyle_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityMMO.Nameboard.ColorStyle val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMONameboardColorStyle_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.Nameboard.ColorStyle");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityMMO.Nameboard.ColorStyle");
                }
				val = (UnityMMO.Nameboard.ColorStyle)e;
                
            }
            else
            {
                val = (UnityMMO.Nameboard.ColorStyle)objectCasters.GetCaster(typeof(UnityMMO.Nameboard.ColorStyle))(L, index, null);
            }
        }
		
        public void UpdateUnityMMONameboardColorStyle(RealStatePtr L, int index, UnityMMO.Nameboard.ColorStyle val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMONameboardColorStyle_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.Nameboard.ColorStyle");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityMMO.Nameboard.ColorStyle ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        
		// table cast optimze
		
        
    }
	
	public partial class StaticLuaCallbacks
    {
	    internal static bool __tryArrayGet(Type type, RealStatePtr L, ObjectTranslator translator, object obj, int index)
		{
		
			if (type == typeof(UnityEngine.Vector2[]))
			{
			    UnityEngine.Vector2[] array = obj as UnityEngine.Vector2[];
				translator.PushUnityEngineVector2(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Vector3[]))
			{
			    UnityEngine.Vector3[] array = obj as UnityEngine.Vector3[];
				translator.PushUnityEngineVector3(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Vector4[]))
			{
			    UnityEngine.Vector4[] array = obj as UnityEngine.Vector4[];
				translator.PushUnityEngineVector4(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Color[]))
			{
			    UnityEngine.Color[] array = obj as UnityEngine.Color[];
				translator.PushUnityEngineColor(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Quaternion[]))
			{
			    UnityEngine.Quaternion[] array = obj as UnityEngine.Quaternion[];
				translator.PushUnityEngineQuaternion(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Ray[]))
			{
			    UnityEngine.Ray[] array = obj as UnityEngine.Ray[];
				translator.PushUnityEngineRay(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Bounds[]))
			{
			    UnityEngine.Bounds[] array = obj as UnityEngine.Bounds[];
				translator.PushUnityEngineBounds(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Ray2D[]))
			{
			    UnityEngine.Ray2D[] array = obj as UnityEngine.Ray2D[];
				translator.PushUnityEngineRay2D(L, array[index]);
				return true;
			}
			else if (type == typeof(XLuaFramework.NetPackageType[]))
			{
			    XLuaFramework.NetPackageType[] array = obj as XLuaFramework.NetPackageType[];
				translator.PushXLuaFrameworkNetPackageType(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.InputField.ContentType[]))
			{
			    UnityEngine.UI.InputField.ContentType[] array = obj as UnityEngine.UI.InputField.ContentType[];
				translator.PushUnityEngineUIInputFieldContentType(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.InputField.InputType[]))
			{
			    UnityEngine.UI.InputField.InputType[] array = obj as UnityEngine.UI.InputField.InputType[];
				translator.PushUnityEngineUIInputFieldInputType(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.InputField.CharacterValidation[]))
			{
			    UnityEngine.UI.InputField.CharacterValidation[] array = obj as UnityEngine.UI.InputField.CharacterValidation[];
				translator.PushUnityEngineUIInputFieldCharacterValidation(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.InputField.LineType[]))
			{
			    UnityEngine.UI.InputField.LineType[] array = obj as UnityEngine.UI.InputField.LineType[];
				translator.PushUnityEngineUIInputFieldLineType(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.TextAnchor[]))
			{
			    UnityEngine.TextAnchor[] array = obj as UnityEngine.TextAnchor[];
				translator.PushUnityEngineTextAnchor(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.TouchPhase[]))
			{
			    UnityEngine.TouchPhase[] array = obj as UnityEngine.TouchPhase[];
				translator.PushUnityEngineTouchPhase(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.RectTransform.Axis[]))
			{
			    UnityEngine.RectTransform.Axis[] array = obj as UnityEngine.RectTransform.Axis[];
				translator.PushUnityEngineRectTransformAxis(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.RectTransform.Edge[]))
			{
			    UnityEngine.RectTransform.Edge[] array = obj as UnityEngine.RectTransform.Edge[];
				translator.PushUnityEngineRectTransformEdge(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.ScrollRect.MovementType[]))
			{
			    UnityEngine.UI.ScrollRect.MovementType[] array = obj as UnityEngine.UI.ScrollRect.MovementType[];
				translator.PushUnityEngineUIScrollRectMovementType(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.ScrollRect.ScrollbarVisibility[]))
			{
			    UnityEngine.UI.ScrollRect.ScrollbarVisibility[] array = obj as UnityEngine.UI.ScrollRect.ScrollbarVisibility[];
				translator.PushUnityEngineUIScrollRectScrollbarVisibility(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.Image.Type[]))
			{
			    UnityEngine.UI.Image.Type[] array = obj as UnityEngine.UI.Image.Type[];
				translator.PushUnityEngineUIImageType(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.Image.FillMethod[]))
			{
			    UnityEngine.UI.Image.FillMethod[] array = obj as UnityEngine.UI.Image.FillMethod[];
				translator.PushUnityEngineUIImageFillMethod(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.Image.OriginHorizontal[]))
			{
			    UnityEngine.UI.Image.OriginHorizontal[] array = obj as UnityEngine.UI.Image.OriginHorizontal[];
				translator.PushUnityEngineUIImageOriginHorizontal(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.Image.OriginVertical[]))
			{
			    UnityEngine.UI.Image.OriginVertical[] array = obj as UnityEngine.UI.Image.OriginVertical[];
				translator.PushUnityEngineUIImageOriginVertical(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.Image.Origin90[]))
			{
			    UnityEngine.UI.Image.Origin90[] array = obj as UnityEngine.UI.Image.Origin90[];
				translator.PushUnityEngineUIImageOrigin90(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.Image.Origin180[]))
			{
			    UnityEngine.UI.Image.Origin180[] array = obj as UnityEngine.UI.Image.Origin180[];
				translator.PushUnityEngineUIImageOrigin180(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.Image.Origin360[]))
			{
			    UnityEngine.UI.Image.Origin360[] array = obj as UnityEngine.UI.Image.Origin360[];
				translator.PushUnityEngineUIImageOrigin360(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Camera.FieldOfViewAxis[]))
			{
			    UnityEngine.Camera.FieldOfViewAxis[] array = obj as UnityEngine.Camera.FieldOfViewAxis[];
				translator.PushUnityEngineCameraFieldOfViewAxis(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Camera.MonoOrStereoscopicEye[]))
			{
			    UnityEngine.Camera.MonoOrStereoscopicEye[] array = obj as UnityEngine.Camera.MonoOrStereoscopicEye[];
				translator.PushUnityEngineCameraMonoOrStereoscopicEye(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Camera.StereoscopicEye[]))
			{
			    UnityEngine.Camera.StereoscopicEye[] array = obj as UnityEngine.Camera.StereoscopicEye[];
				translator.PushUnityEngineCameraStereoscopicEye(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Camera.GateFitMode[]))
			{
			    UnityEngine.Camera.GateFitMode[] array = obj as UnityEngine.Camera.GateFitMode[];
				translator.PushUnityEngineCameraGateFitMode(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.KeyCode[]))
			{
			    UnityEngine.KeyCode[] array = obj as UnityEngine.KeyCode[];
				translator.PushUnityEngineKeyCode(L, array[index]);
				return true;
			}
			else if (type == typeof(XLuaFramework.DisType[]))
			{
			    XLuaFramework.DisType[] array = obj as XLuaFramework.DisType[];
				translator.PushXLuaFrameworkDisType(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityMMO.SceneInfoKey[]))
			{
			    UnityMMO.SceneInfoKey[] array = obj as UnityMMO.SceneInfoKey[];
				translator.PushUnityMMOSceneInfoKey(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityMMO.SceneObjectType[]))
			{
			    UnityMMO.SceneObjectType[] array = obj as UnityMMO.SceneObjectType[];
				translator.PushUnityMMOSceneObjectType(L, array[index]);
				return true;
			}
			else if (type == typeof(XLuaFramework.Main.State[]))
			{
			    XLuaFramework.Main.State[] array = obj as XLuaFramework.Main.State[];
				translator.PushXLuaFrameworkMainState(L, array[index]);
				return true;
			}
			else if (type == typeof(XLuaFramework.Main.SubState[]))
			{
			    XLuaFramework.Main.SubState[] array = obj as XLuaFramework.Main.SubState[];
				translator.PushXLuaFrameworkMainSubState(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityMMO.TimelineInfo.Event[]))
			{
			    UnityMMO.TimelineInfo.Event[] array = obj as UnityMMO.TimelineInfo.Event[];
				translator.PushUnityMMOTimelineInfoEvent(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityMMO.Nameboard.ColorStyle[]))
			{
			    UnityMMO.Nameboard.ColorStyle[] array = obj as UnityMMO.Nameboard.ColorStyle[];
				translator.PushUnityMMONameboardColorStyle(L, array[index]);
				return true;
			}
            return false;
		}
		
		internal static bool __tryArraySet(Type type, RealStatePtr L, ObjectTranslator translator, object obj, int array_idx, int obj_idx)
		{
		
			if (type == typeof(UnityEngine.Vector2[]))
			{
			    UnityEngine.Vector2[] array = obj as UnityEngine.Vector2[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Vector3[]))
			{
			    UnityEngine.Vector3[] array = obj as UnityEngine.Vector3[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Vector4[]))
			{
			    UnityEngine.Vector4[] array = obj as UnityEngine.Vector4[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Color[]))
			{
			    UnityEngine.Color[] array = obj as UnityEngine.Color[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Quaternion[]))
			{
			    UnityEngine.Quaternion[] array = obj as UnityEngine.Quaternion[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Ray[]))
			{
			    UnityEngine.Ray[] array = obj as UnityEngine.Ray[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Bounds[]))
			{
			    UnityEngine.Bounds[] array = obj as UnityEngine.Bounds[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Ray2D[]))
			{
			    UnityEngine.Ray2D[] array = obj as UnityEngine.Ray2D[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(XLuaFramework.NetPackageType[]))
			{
			    XLuaFramework.NetPackageType[] array = obj as XLuaFramework.NetPackageType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.InputField.ContentType[]))
			{
			    UnityEngine.UI.InputField.ContentType[] array = obj as UnityEngine.UI.InputField.ContentType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.InputField.InputType[]))
			{
			    UnityEngine.UI.InputField.InputType[] array = obj as UnityEngine.UI.InputField.InputType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.InputField.CharacterValidation[]))
			{
			    UnityEngine.UI.InputField.CharacterValidation[] array = obj as UnityEngine.UI.InputField.CharacterValidation[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.InputField.LineType[]))
			{
			    UnityEngine.UI.InputField.LineType[] array = obj as UnityEngine.UI.InputField.LineType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.TextAnchor[]))
			{
			    UnityEngine.TextAnchor[] array = obj as UnityEngine.TextAnchor[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.TouchPhase[]))
			{
			    UnityEngine.TouchPhase[] array = obj as UnityEngine.TouchPhase[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.RectTransform.Axis[]))
			{
			    UnityEngine.RectTransform.Axis[] array = obj as UnityEngine.RectTransform.Axis[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.RectTransform.Edge[]))
			{
			    UnityEngine.RectTransform.Edge[] array = obj as UnityEngine.RectTransform.Edge[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.ScrollRect.MovementType[]))
			{
			    UnityEngine.UI.ScrollRect.MovementType[] array = obj as UnityEngine.UI.ScrollRect.MovementType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.ScrollRect.ScrollbarVisibility[]))
			{
			    UnityEngine.UI.ScrollRect.ScrollbarVisibility[] array = obj as UnityEngine.UI.ScrollRect.ScrollbarVisibility[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.Image.Type[]))
			{
			    UnityEngine.UI.Image.Type[] array = obj as UnityEngine.UI.Image.Type[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.Image.FillMethod[]))
			{
			    UnityEngine.UI.Image.FillMethod[] array = obj as UnityEngine.UI.Image.FillMethod[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.Image.OriginHorizontal[]))
			{
			    UnityEngine.UI.Image.OriginHorizontal[] array = obj as UnityEngine.UI.Image.OriginHorizontal[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.Image.OriginVertical[]))
			{
			    UnityEngine.UI.Image.OriginVertical[] array = obj as UnityEngine.UI.Image.OriginVertical[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.Image.Origin90[]))
			{
			    UnityEngine.UI.Image.Origin90[] array = obj as UnityEngine.UI.Image.Origin90[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.Image.Origin180[]))
			{
			    UnityEngine.UI.Image.Origin180[] array = obj as UnityEngine.UI.Image.Origin180[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.UI.Image.Origin360[]))
			{
			    UnityEngine.UI.Image.Origin360[] array = obj as UnityEngine.UI.Image.Origin360[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Camera.FieldOfViewAxis[]))
			{
			    UnityEngine.Camera.FieldOfViewAxis[] array = obj as UnityEngine.Camera.FieldOfViewAxis[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Camera.MonoOrStereoscopicEye[]))
			{
			    UnityEngine.Camera.MonoOrStereoscopicEye[] array = obj as UnityEngine.Camera.MonoOrStereoscopicEye[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Camera.StereoscopicEye[]))
			{
			    UnityEngine.Camera.StereoscopicEye[] array = obj as UnityEngine.Camera.StereoscopicEye[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Camera.GateFitMode[]))
			{
			    UnityEngine.Camera.GateFitMode[] array = obj as UnityEngine.Camera.GateFitMode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.KeyCode[]))
			{
			    UnityEngine.KeyCode[] array = obj as UnityEngine.KeyCode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(XLuaFramework.DisType[]))
			{
			    XLuaFramework.DisType[] array = obj as XLuaFramework.DisType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityMMO.SceneInfoKey[]))
			{
			    UnityMMO.SceneInfoKey[] array = obj as UnityMMO.SceneInfoKey[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityMMO.SceneObjectType[]))
			{
			    UnityMMO.SceneObjectType[] array = obj as UnityMMO.SceneObjectType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(XLuaFramework.Main.State[]))
			{
			    XLuaFramework.Main.State[] array = obj as XLuaFramework.Main.State[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(XLuaFramework.Main.SubState[]))
			{
			    XLuaFramework.Main.SubState[] array = obj as XLuaFramework.Main.SubState[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityMMO.TimelineInfo.Event[]))
			{
			    UnityMMO.TimelineInfo.Event[] array = obj as UnityMMO.TimelineInfo.Event[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityMMO.Nameboard.ColorStyle[]))
			{
			    UnityMMO.Nameboard.ColorStyle[] array = obj as UnityMMO.Nameboard.ColorStyle[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
            return false;
		}
	}
}