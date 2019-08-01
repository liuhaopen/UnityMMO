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
    public class XLuaFrameworkByteBufferWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(XLuaFramework.ByteBuffer);
			Utils.BeginObjectRegister(type, L, translator, 0, 26, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Close", _m_Close);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteByte", _m_WriteByte);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteShort", _m_WriteShort);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteUshort", _m_WriteUshort);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteInt", _m_WriteInt);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteUint", _m_WriteUint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteLong", _m_WriteLong);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteUlong", _m_WriteUlong);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteFloat", _m_WriteFloat);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteDouble", _m_WriteDouble);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteString", _m_WriteString);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteText", _m_WriteText);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteBytes", _m_WriteBytes);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadByte", _m_ReadByte);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadShort", _m_ReadShort);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadUshort", _m_ReadUshort);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadInt", _m_ReadInt);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadUint", _m_ReadUint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadLong", _m_ReadLong);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadUlong", _m_ReadUlong);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadFloat", _m_ReadFloat);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadDouble", _m_ReadDouble);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadString", _m_ReadString);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadBytes", _m_ReadBytes);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToBytes", _m_ToBytes);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Flush", _m_Flush);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					XLuaFramework.ByteBuffer gen_ret = new XLuaFramework.ByteBuffer();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 2 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
				{
					byte[] _data = LuaAPI.lua_tobytes(L, 2);
					
					XLuaFramework.ByteBuffer gen_ret = new XLuaFramework.ByteBuffer(_data);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to XLuaFramework.ByteBuffer constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Close(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Close(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteByte(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    byte _v = (byte)LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.WriteByte( _v );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteShort(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    short _v = (short)LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.WriteShort( _v );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteUshort(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    ushort _v = (ushort)LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.WriteUshort( _v );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteInt(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _v = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.WriteInt( _v );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteUint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    uint _v = LuaAPI.xlua_touint(L, 2);
                    
                    gen_to_be_invoked.WriteUint( _v );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteLong(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    long _v = LuaAPI.lua_toint64(L, 2);
                    
                    gen_to_be_invoked.WriteLong( _v );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteUlong(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    ulong _v = LuaAPI.lua_touint64(L, 2);
                    
                    gen_to_be_invoked.WriteUlong( _v );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteFloat(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _v = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.WriteFloat( _v );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteDouble(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    double _v = LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.WriteDouble( _v );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _v = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.WriteString( _v );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteText(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _v = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.WriteText( _v );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteBytes(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    byte[] _v = LuaAPI.lua_tobytes(L, 2);
                    
                    gen_to_be_invoked.WriteBytes( _v );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadByte(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        byte gen_ret = gen_to_be_invoked.ReadByte(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadShort(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        short gen_ret = gen_to_be_invoked.ReadShort(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadUshort(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        ushort gen_ret = gen_to_be_invoked.ReadUshort(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadInt(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        int gen_ret = gen_to_be_invoked.ReadInt(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadUint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        uint gen_ret = gen_to_be_invoked.ReadUint(  );
                        LuaAPI.xlua_pushuint(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadLong(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        long gen_ret = gen_to_be_invoked.ReadLong(  );
                        LuaAPI.lua_pushint64(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadUlong(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        ulong gen_ret = gen_to_be_invoked.ReadUlong(  );
                        LuaAPI.lua_pushuint64(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadFloat(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        float gen_ret = gen_to_be_invoked.ReadFloat(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadDouble(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        double gen_ret = gen_to_be_invoked.ReadDouble(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        string gen_ret = gen_to_be_invoked.ReadString(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadBytes(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        byte[] gen_ret = gen_to_be_invoked.ReadBytes(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToBytes(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        byte[] gen_ret = gen_to_be_invoked.ToBytes(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Flush(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaFramework.ByteBuffer gen_to_be_invoked = (XLuaFramework.ByteBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Flush(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
