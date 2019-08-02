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
    public class UnityEngineSpriteWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Sprite);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 16, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPhysicsShapeCount", _m_GetPhysicsShapeCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPhysicsShapePointCount", _m_GetPhysicsShapePointCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPhysicsShape", _m_GetPhysicsShape);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OverridePhysicsShape", _m_OverridePhysicsShape);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OverrideGeometry", _m_OverrideGeometry);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "bounds", _g_get_bounds);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "rect", _g_get_rect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "border", _g_get_border);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "texture", _g_get_texture);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pixelsPerUnit", _g_get_pixelsPerUnit);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "spriteAtlasTextureScale", _g_get_spriteAtlasTextureScale);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "associatedAlphaSplitTexture", _g_get_associatedAlphaSplitTexture);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pivot", _g_get_pivot);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "packed", _g_get_packed);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "packingMode", _g_get_packingMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "packingRotation", _g_get_packingRotation);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "textureRect", _g_get_textureRect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "textureRectOffset", _g_get_textureRectOffset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "vertices", _g_get_vertices);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "triangles", _g_get_triangles);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "uv", _g_get_uv);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Create", _m_Create_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "UnityEngine.Sprite does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPhysicsShapeCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        int gen_ret = gen_to_be_invoked.GetPhysicsShapeCount(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPhysicsShapePointCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _shapeIdx = LuaAPI.xlua_tointeger(L, 2);
                    
                        int gen_ret = gen_to_be_invoked.GetPhysicsShapePointCount( _shapeIdx );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPhysicsShape(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _shapeIdx = LuaAPI.xlua_tointeger(L, 2);
                    System.Collections.Generic.List<UnityEngine.Vector2> _physicsShape = (System.Collections.Generic.List<UnityEngine.Vector2>)translator.GetObject(L, 3, typeof(System.Collections.Generic.List<UnityEngine.Vector2>));
                    
                        int gen_ret = gen_to_be_invoked.GetPhysicsShape( _shapeIdx, _physicsShape );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OverridePhysicsShape(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.Generic.IList<UnityEngine.Vector2[]> _physicsShapes = (System.Collections.Generic.IList<UnityEngine.Vector2[]>)translator.GetObject(L, 2, typeof(System.Collections.Generic.IList<UnityEngine.Vector2[]>));
                    
                    gen_to_be_invoked.OverridePhysicsShape( _physicsShapes );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OverrideGeometry(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2[] _vertices = (UnityEngine.Vector2[])translator.GetObject(L, 2, typeof(UnityEngine.Vector2[]));
                    ushort[] _triangles = (ushort[])translator.GetObject(L, 3, typeof(ushort[]));
                    
                    gen_to_be_invoked.OverrideGeometry( _vertices, _triangles );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Create_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Texture2D>(L, 1)&& translator.Assignable<UnityEngine.Rect>(L, 2)&& translator.Assignable<UnityEngine.Vector2>(L, 3)) 
                {
                    UnityEngine.Texture2D _texture = (UnityEngine.Texture2D)translator.GetObject(L, 1, typeof(UnityEngine.Texture2D));
                    UnityEngine.Rect _rect;translator.Get(L, 2, out _rect);
                    UnityEngine.Vector2 _pivot;translator.Get(L, 3, out _pivot);
                    
                        UnityEngine.Sprite gen_ret = UnityEngine.Sprite.Create( _texture, _rect, _pivot );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Texture2D>(L, 1)&& translator.Assignable<UnityEngine.Rect>(L, 2)&& translator.Assignable<UnityEngine.Vector2>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Texture2D _texture = (UnityEngine.Texture2D)translator.GetObject(L, 1, typeof(UnityEngine.Texture2D));
                    UnityEngine.Rect _rect;translator.Get(L, 2, out _rect);
                    UnityEngine.Vector2 _pivot;translator.Get(L, 3, out _pivot);
                    float _pixelsPerUnit = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        UnityEngine.Sprite gen_ret = UnityEngine.Sprite.Create( _texture, _rect, _pivot, _pixelsPerUnit );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Texture2D>(L, 1)&& translator.Assignable<UnityEngine.Rect>(L, 2)&& translator.Assignable<UnityEngine.Vector2>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Texture2D _texture = (UnityEngine.Texture2D)translator.GetObject(L, 1, typeof(UnityEngine.Texture2D));
                    UnityEngine.Rect _rect;translator.Get(L, 2, out _rect);
                    UnityEngine.Vector2 _pivot;translator.Get(L, 3, out _pivot);
                    float _pixelsPerUnit = (float)LuaAPI.lua_tonumber(L, 4);
                    uint _extrude = LuaAPI.xlua_touint(L, 5);
                    
                        UnityEngine.Sprite gen_ret = UnityEngine.Sprite.Create( _texture, _rect, _pivot, _pixelsPerUnit, _extrude );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Texture2D>(L, 1)&& translator.Assignable<UnityEngine.Rect>(L, 2)&& translator.Assignable<UnityEngine.Vector2>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<UnityEngine.SpriteMeshType>(L, 6)) 
                {
                    UnityEngine.Texture2D _texture = (UnityEngine.Texture2D)translator.GetObject(L, 1, typeof(UnityEngine.Texture2D));
                    UnityEngine.Rect _rect;translator.Get(L, 2, out _rect);
                    UnityEngine.Vector2 _pivot;translator.Get(L, 3, out _pivot);
                    float _pixelsPerUnit = (float)LuaAPI.lua_tonumber(L, 4);
                    uint _extrude = LuaAPI.xlua_touint(L, 5);
                    UnityEngine.SpriteMeshType _meshType;translator.Get(L, 6, out _meshType);
                    
                        UnityEngine.Sprite gen_ret = UnityEngine.Sprite.Create( _texture, _rect, _pivot, _pixelsPerUnit, _extrude, _meshType );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 7&& translator.Assignable<UnityEngine.Texture2D>(L, 1)&& translator.Assignable<UnityEngine.Rect>(L, 2)&& translator.Assignable<UnityEngine.Vector2>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<UnityEngine.SpriteMeshType>(L, 6)&& translator.Assignable<UnityEngine.Vector4>(L, 7)) 
                {
                    UnityEngine.Texture2D _texture = (UnityEngine.Texture2D)translator.GetObject(L, 1, typeof(UnityEngine.Texture2D));
                    UnityEngine.Rect _rect;translator.Get(L, 2, out _rect);
                    UnityEngine.Vector2 _pivot;translator.Get(L, 3, out _pivot);
                    float _pixelsPerUnit = (float)LuaAPI.lua_tonumber(L, 4);
                    uint _extrude = LuaAPI.xlua_touint(L, 5);
                    UnityEngine.SpriteMeshType _meshType;translator.Get(L, 6, out _meshType);
                    UnityEngine.Vector4 _border;translator.Get(L, 7, out _border);
                    
                        UnityEngine.Sprite gen_ret = UnityEngine.Sprite.Create( _texture, _rect, _pivot, _pixelsPerUnit, _extrude, _meshType, _border );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 8&& translator.Assignable<UnityEngine.Texture2D>(L, 1)&& translator.Assignable<UnityEngine.Rect>(L, 2)&& translator.Assignable<UnityEngine.Vector2>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<UnityEngine.SpriteMeshType>(L, 6)&& translator.Assignable<UnityEngine.Vector4>(L, 7)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 8)) 
                {
                    UnityEngine.Texture2D _texture = (UnityEngine.Texture2D)translator.GetObject(L, 1, typeof(UnityEngine.Texture2D));
                    UnityEngine.Rect _rect;translator.Get(L, 2, out _rect);
                    UnityEngine.Vector2 _pivot;translator.Get(L, 3, out _pivot);
                    float _pixelsPerUnit = (float)LuaAPI.lua_tonumber(L, 4);
                    uint _extrude = LuaAPI.xlua_touint(L, 5);
                    UnityEngine.SpriteMeshType _meshType;translator.Get(L, 6, out _meshType);
                    UnityEngine.Vector4 _border;translator.Get(L, 7, out _border);
                    bool _generateFallbackPhysicsShape = LuaAPI.lua_toboolean(L, 8);
                    
                        UnityEngine.Sprite gen_ret = UnityEngine.Sprite.Create( _texture, _rect, _pivot, _pixelsPerUnit, _extrude, _meshType, _border, _generateFallbackPhysicsShape );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Sprite.Create!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_bounds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineBounds(L, gen_to_be_invoked.bounds);
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
			
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.rect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_border(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector4(L, gen_to_be_invoked.border);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_texture(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.texture);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pixelsPerUnit(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.pixelsPerUnit);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_spriteAtlasTextureScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.spriteAtlasTextureScale);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_associatedAlphaSplitTexture(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.associatedAlphaSplitTexture);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pivot(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.pivot);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_packed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.packed);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_packingMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.packingMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_packingRotation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.packingRotation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_textureRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.textureRect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_textureRectOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.textureRectOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_vertices(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.vertices);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_triangles(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.triangles);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_uv(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Sprite gen_to_be_invoked = (UnityEngine.Sprite)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.uv);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
