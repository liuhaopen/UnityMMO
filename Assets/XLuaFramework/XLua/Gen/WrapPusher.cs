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
				translator.RegisterPushAndGetAndUpdate<UnityMMO.SceneInfoKey>(translator.PushUnityMMOSceneInfoKey, translator.Get, translator.UpdateUnityMMOSceneInfoKey);
				translator.RegisterPushAndGetAndUpdate<UnityMMO.SceneObjectType>(translator.PushUnityMMOSceneObjectType, translator.Get, translator.UpdateUnityMMOSceneObjectType);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.TextAnchor>(translator.PushUnityEngineTextAnchor, translator.Get, translator.UpdateUnityEngineTextAnchor);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.TouchPhase>(translator.PushUnityEngineTouchPhase, translator.Get, translator.UpdateUnityEngineTouchPhase);
				translator.RegisterPushAndGetAndUpdate<UnityMMO.TimelineState.NewState>(translator.PushUnityMMOTimelineStateNewState, translator.Get, translator.UpdateUnityMMOTimelineStateNewState);
				translator.RegisterPushAndGetAndUpdate<UnityMMO.TimelineState.InterruptState>(translator.PushUnityMMOTimelineStateInterruptState, translator.Get, translator.UpdateUnityMMOTimelineStateInterruptState);
				translator.RegisterPushAndGetAndUpdate<UnityMMO.LocomotionState.State>(translator.PushUnityMMOLocomotionStateState, translator.Get, translator.UpdateUnityMMOLocomotionStateState);
				translator.RegisterPushAndGetAndUpdate<UnityMMO.NameboardData.ResState>(translator.PushUnityMMONameboardDataResState, translator.Get, translator.UpdateUnityMMONameboardDataResState);
				translator.RegisterPushAndGetAndUpdate<UnityMMO.JumpState.State>(translator.PushUnityMMOJumpStateState, translator.Get, translator.UpdateUnityMMOJumpStateState);
				translator.RegisterPushAndGetAndUpdate<UnityMMO.ActionInfo.Type>(translator.PushUnityMMOActionInfoType, translator.Get, translator.UpdateUnityMMOActionInfoType);
				translator.RegisterPushAndGetAndUpdate<UnityMMO.LooksInfo.State>(translator.PushUnityMMOLooksInfoState, translator.Get, translator.UpdateUnityMMOLooksInfoState);
				translator.RegisterPushAndGetAndUpdate<UnityMMO.SceneObjectData.Type>(translator.PushUnityMMOSceneObjectDataType, translator.Get, translator.UpdateUnityMMOSceneObjectDataType);
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
        
        int UnityMMOTimelineStateNewState_TypeID = -1;
		int UnityMMOTimelineStateNewState_EnumRef = -1;
        
        public void PushUnityMMOTimelineStateNewState(RealStatePtr L, UnityMMO.TimelineState.NewState val)
        {
            if (UnityMMOTimelineStateNewState_TypeID == -1)
            {
			    bool is_first;
                UnityMMOTimelineStateNewState_TypeID = getTypeId(L, typeof(UnityMMO.TimelineState.NewState), out is_first);
				
				if (UnityMMOTimelineStateNewState_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityMMO.TimelineState.NewState));
				    UnityMMOTimelineStateNewState_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityMMOTimelineStateNewState_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityMMOTimelineStateNewState_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityMMO.TimelineState.NewState ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityMMOTimelineStateNewState_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityMMO.TimelineState.NewState val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOTimelineStateNewState_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.TimelineState.NewState");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityMMO.TimelineState.NewState");
                }
				val = (UnityMMO.TimelineState.NewState)e;
                
            }
            else
            {
                val = (UnityMMO.TimelineState.NewState)objectCasters.GetCaster(typeof(UnityMMO.TimelineState.NewState))(L, index, null);
            }
        }
		
        public void UpdateUnityMMOTimelineStateNewState(RealStatePtr L, int index, UnityMMO.TimelineState.NewState val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOTimelineStateNewState_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.TimelineState.NewState");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityMMO.TimelineState.NewState ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityMMOTimelineStateInterruptState_TypeID = -1;
		int UnityMMOTimelineStateInterruptState_EnumRef = -1;
        
        public void PushUnityMMOTimelineStateInterruptState(RealStatePtr L, UnityMMO.TimelineState.InterruptState val)
        {
            if (UnityMMOTimelineStateInterruptState_TypeID == -1)
            {
			    bool is_first;
                UnityMMOTimelineStateInterruptState_TypeID = getTypeId(L, typeof(UnityMMO.TimelineState.InterruptState), out is_first);
				
				if (UnityMMOTimelineStateInterruptState_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityMMO.TimelineState.InterruptState));
				    UnityMMOTimelineStateInterruptState_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityMMOTimelineStateInterruptState_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityMMOTimelineStateInterruptState_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityMMO.TimelineState.InterruptState ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityMMOTimelineStateInterruptState_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityMMO.TimelineState.InterruptState val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOTimelineStateInterruptState_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.TimelineState.InterruptState");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityMMO.TimelineState.InterruptState");
                }
				val = (UnityMMO.TimelineState.InterruptState)e;
                
            }
            else
            {
                val = (UnityMMO.TimelineState.InterruptState)objectCasters.GetCaster(typeof(UnityMMO.TimelineState.InterruptState))(L, index, null);
            }
        }
		
        public void UpdateUnityMMOTimelineStateInterruptState(RealStatePtr L, int index, UnityMMO.TimelineState.InterruptState val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOTimelineStateInterruptState_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.TimelineState.InterruptState");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityMMO.TimelineState.InterruptState ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityMMOLocomotionStateState_TypeID = -1;
		int UnityMMOLocomotionStateState_EnumRef = -1;
        
        public void PushUnityMMOLocomotionStateState(RealStatePtr L, UnityMMO.LocomotionState.State val)
        {
            if (UnityMMOLocomotionStateState_TypeID == -1)
            {
			    bool is_first;
                UnityMMOLocomotionStateState_TypeID = getTypeId(L, typeof(UnityMMO.LocomotionState.State), out is_first);
				
				if (UnityMMOLocomotionStateState_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityMMO.LocomotionState.State));
				    UnityMMOLocomotionStateState_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityMMOLocomotionStateState_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityMMOLocomotionStateState_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityMMO.LocomotionState.State ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityMMOLocomotionStateState_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityMMO.LocomotionState.State val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOLocomotionStateState_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.LocomotionState.State");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityMMO.LocomotionState.State");
                }
				val = (UnityMMO.LocomotionState.State)e;
                
            }
            else
            {
                val = (UnityMMO.LocomotionState.State)objectCasters.GetCaster(typeof(UnityMMO.LocomotionState.State))(L, index, null);
            }
        }
		
        public void UpdateUnityMMOLocomotionStateState(RealStatePtr L, int index, UnityMMO.LocomotionState.State val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOLocomotionStateState_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.LocomotionState.State");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityMMO.LocomotionState.State ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityMMONameboardDataResState_TypeID = -1;
		int UnityMMONameboardDataResState_EnumRef = -1;
        
        public void PushUnityMMONameboardDataResState(RealStatePtr L, UnityMMO.NameboardData.ResState val)
        {
            if (UnityMMONameboardDataResState_TypeID == -1)
            {
			    bool is_first;
                UnityMMONameboardDataResState_TypeID = getTypeId(L, typeof(UnityMMO.NameboardData.ResState), out is_first);
				
				if (UnityMMONameboardDataResState_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityMMO.NameboardData.ResState));
				    UnityMMONameboardDataResState_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityMMONameboardDataResState_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityMMONameboardDataResState_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityMMO.NameboardData.ResState ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityMMONameboardDataResState_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityMMO.NameboardData.ResState val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMONameboardDataResState_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.NameboardData.ResState");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityMMO.NameboardData.ResState");
                }
				val = (UnityMMO.NameboardData.ResState)e;
                
            }
            else
            {
                val = (UnityMMO.NameboardData.ResState)objectCasters.GetCaster(typeof(UnityMMO.NameboardData.ResState))(L, index, null);
            }
        }
		
        public void UpdateUnityMMONameboardDataResState(RealStatePtr L, int index, UnityMMO.NameboardData.ResState val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMONameboardDataResState_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.NameboardData.ResState");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityMMO.NameboardData.ResState ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityMMOJumpStateState_TypeID = -1;
		int UnityMMOJumpStateState_EnumRef = -1;
        
        public void PushUnityMMOJumpStateState(RealStatePtr L, UnityMMO.JumpState.State val)
        {
            if (UnityMMOJumpStateState_TypeID == -1)
            {
			    bool is_first;
                UnityMMOJumpStateState_TypeID = getTypeId(L, typeof(UnityMMO.JumpState.State), out is_first);
				
				if (UnityMMOJumpStateState_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityMMO.JumpState.State));
				    UnityMMOJumpStateState_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityMMOJumpStateState_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityMMOJumpStateState_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityMMO.JumpState.State ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityMMOJumpStateState_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityMMO.JumpState.State val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOJumpStateState_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.JumpState.State");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityMMO.JumpState.State");
                }
				val = (UnityMMO.JumpState.State)e;
                
            }
            else
            {
                val = (UnityMMO.JumpState.State)objectCasters.GetCaster(typeof(UnityMMO.JumpState.State))(L, index, null);
            }
        }
		
        public void UpdateUnityMMOJumpStateState(RealStatePtr L, int index, UnityMMO.JumpState.State val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOJumpStateState_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.JumpState.State");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityMMO.JumpState.State ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityMMOActionInfoType_TypeID = -1;
		int UnityMMOActionInfoType_EnumRef = -1;
        
        public void PushUnityMMOActionInfoType(RealStatePtr L, UnityMMO.ActionInfo.Type val)
        {
            if (UnityMMOActionInfoType_TypeID == -1)
            {
			    bool is_first;
                UnityMMOActionInfoType_TypeID = getTypeId(L, typeof(UnityMMO.ActionInfo.Type), out is_first);
				
				if (UnityMMOActionInfoType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityMMO.ActionInfo.Type));
				    UnityMMOActionInfoType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityMMOActionInfoType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityMMOActionInfoType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityMMO.ActionInfo.Type ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityMMOActionInfoType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityMMO.ActionInfo.Type val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOActionInfoType_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.ActionInfo.Type");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityMMO.ActionInfo.Type");
                }
				val = (UnityMMO.ActionInfo.Type)e;
                
            }
            else
            {
                val = (UnityMMO.ActionInfo.Type)objectCasters.GetCaster(typeof(UnityMMO.ActionInfo.Type))(L, index, null);
            }
        }
		
        public void UpdateUnityMMOActionInfoType(RealStatePtr L, int index, UnityMMO.ActionInfo.Type val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOActionInfoType_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.ActionInfo.Type");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityMMO.ActionInfo.Type ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityMMOLooksInfoState_TypeID = -1;
		int UnityMMOLooksInfoState_EnumRef = -1;
        
        public void PushUnityMMOLooksInfoState(RealStatePtr L, UnityMMO.LooksInfo.State val)
        {
            if (UnityMMOLooksInfoState_TypeID == -1)
            {
			    bool is_first;
                UnityMMOLooksInfoState_TypeID = getTypeId(L, typeof(UnityMMO.LooksInfo.State), out is_first);
				
				if (UnityMMOLooksInfoState_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityMMO.LooksInfo.State));
				    UnityMMOLooksInfoState_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityMMOLooksInfoState_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityMMOLooksInfoState_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityMMO.LooksInfo.State ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityMMOLooksInfoState_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityMMO.LooksInfo.State val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOLooksInfoState_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.LooksInfo.State");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityMMO.LooksInfo.State");
                }
				val = (UnityMMO.LooksInfo.State)e;
                
            }
            else
            {
                val = (UnityMMO.LooksInfo.State)objectCasters.GetCaster(typeof(UnityMMO.LooksInfo.State))(L, index, null);
            }
        }
		
        public void UpdateUnityMMOLooksInfoState(RealStatePtr L, int index, UnityMMO.LooksInfo.State val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOLooksInfoState_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.LooksInfo.State");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityMMO.LooksInfo.State ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityMMOSceneObjectDataType_TypeID = -1;
		int UnityMMOSceneObjectDataType_EnumRef = -1;
        
        public void PushUnityMMOSceneObjectDataType(RealStatePtr L, UnityMMO.SceneObjectData.Type val)
        {
            if (UnityMMOSceneObjectDataType_TypeID == -1)
            {
			    bool is_first;
                UnityMMOSceneObjectDataType_TypeID = getTypeId(L, typeof(UnityMMO.SceneObjectData.Type), out is_first);
				
				if (UnityMMOSceneObjectDataType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityMMO.SceneObjectData.Type));
				    UnityMMOSceneObjectDataType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityMMOSceneObjectDataType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityMMOSceneObjectDataType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityMMO.SceneObjectData.Type ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityMMOSceneObjectDataType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityMMO.SceneObjectData.Type val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOSceneObjectDataType_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.SceneObjectData.Type");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityMMO.SceneObjectData.Type");
                }
				val = (UnityMMO.SceneObjectData.Type)e;
                
            }
            else
            {
                val = (UnityMMO.SceneObjectData.Type)objectCasters.GetCaster(typeof(UnityMMO.SceneObjectData.Type))(L, index, null);
            }
        }
		
        public void UpdateUnityMMOSceneObjectDataType(RealStatePtr L, int index, UnityMMO.SceneObjectData.Type val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityMMOSceneObjectDataType_TypeID)
				{
				    throw new Exception("invalid userdata for UnityMMO.SceneObjectData.Type");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityMMO.SceneObjectData.Type ,value="+val);
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
			else if (type == typeof(UnityMMO.TimelineState.NewState[]))
			{
			    UnityMMO.TimelineState.NewState[] array = obj as UnityMMO.TimelineState.NewState[];
				translator.PushUnityMMOTimelineStateNewState(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityMMO.TimelineState.InterruptState[]))
			{
			    UnityMMO.TimelineState.InterruptState[] array = obj as UnityMMO.TimelineState.InterruptState[];
				translator.PushUnityMMOTimelineStateInterruptState(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityMMO.LocomotionState.State[]))
			{
			    UnityMMO.LocomotionState.State[] array = obj as UnityMMO.LocomotionState.State[];
				translator.PushUnityMMOLocomotionStateState(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityMMO.NameboardData.ResState[]))
			{
			    UnityMMO.NameboardData.ResState[] array = obj as UnityMMO.NameboardData.ResState[];
				translator.PushUnityMMONameboardDataResState(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityMMO.JumpState.State[]))
			{
			    UnityMMO.JumpState.State[] array = obj as UnityMMO.JumpState.State[];
				translator.PushUnityMMOJumpStateState(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityMMO.ActionInfo.Type[]))
			{
			    UnityMMO.ActionInfo.Type[] array = obj as UnityMMO.ActionInfo.Type[];
				translator.PushUnityMMOActionInfoType(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityMMO.LooksInfo.State[]))
			{
			    UnityMMO.LooksInfo.State[] array = obj as UnityMMO.LooksInfo.State[];
				translator.PushUnityMMOLooksInfoState(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityMMO.SceneObjectData.Type[]))
			{
			    UnityMMO.SceneObjectData.Type[] array = obj as UnityMMO.SceneObjectData.Type[];
				translator.PushUnityMMOSceneObjectDataType(L, array[index]);
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
			else if (type == typeof(UnityMMO.TimelineState.NewState[]))
			{
			    UnityMMO.TimelineState.NewState[] array = obj as UnityMMO.TimelineState.NewState[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityMMO.TimelineState.InterruptState[]))
			{
			    UnityMMO.TimelineState.InterruptState[] array = obj as UnityMMO.TimelineState.InterruptState[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityMMO.LocomotionState.State[]))
			{
			    UnityMMO.LocomotionState.State[] array = obj as UnityMMO.LocomotionState.State[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityMMO.NameboardData.ResState[]))
			{
			    UnityMMO.NameboardData.ResState[] array = obj as UnityMMO.NameboardData.ResState[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityMMO.JumpState.State[]))
			{
			    UnityMMO.JumpState.State[] array = obj as UnityMMO.JumpState.State[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityMMO.ActionInfo.Type[]))
			{
			    UnityMMO.ActionInfo.Type[] array = obj as UnityMMO.ActionInfo.Type[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityMMO.LooksInfo.State[]))
			{
			    UnityMMO.LooksInfo.State[] array = obj as UnityMMO.LooksInfo.State[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityMMO.SceneObjectData.Type[]))
			{
			    UnityMMO.SceneObjectData.Type[] array = obj as UnityMMO.SceneObjectData.Type[];
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