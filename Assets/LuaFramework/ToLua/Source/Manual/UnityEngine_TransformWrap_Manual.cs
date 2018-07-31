using System;
using LuaInterface;

public class UnityEngine_TransformWrap_Manual
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UnityEngine.Transform), typeof(UnityEngine.Component));
        //L.RegVar("xyz", get_xyz, set_xyz);
        L.RegFunction("GetLocalPosXYZ", get_xyz);
        L.RegFunction("GetLocalPosXY", get_xy);
        L.RegFunction("GetLocalPosX", get_x);
        L.RegFunction("GetLocalPosY", get_y);
        L.RegFunction("GetLocalPosZ", get_z);
        //L.RegFunction("GetAnchoredXYZ", get_xyz);
        L.RegFunction("GetLocalPosXY", get_xy);
        L.RegFunction("GetLocalPosX", get_x);
        L.RegFunction("GetLocalPosY", get_y);
        L.RegFunction("GetLocalPosZ", get_z);
        L.RegFunction("SetLocalPosXYZ", set_xyz);
        L.RegFunction("SetLocalPosXY", set_xy);
        L.RegFunction("SetLocalPosX", set_x);
        L.RegFunction("SetLocalPosY", set_y);
        L.RegFunction("SetLocalPosZ", set_z);
        //L.RegFunction("SetPivot", set_pivot);
        L.EndClass();
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_xyz(IntPtr L)
    {
        try
        {
            //UnityEngine.Transform obj = (UnityEngine.Transform)ToLua.CheckUnityObject(L, 1, typeof(UnityEngine.Transform));
            object o = ToLua.ToObject(L, 1);
            UnityEngine.Transform obj = o as UnityEngine.Transform;
            UnityEngine.Vector3 ret = obj.localPosition;
            LuaDLL.lua_pushnumber(L, ret.x);
            LuaDLL.lua_pushnumber(L, ret.y);
            LuaDLL.lua_pushnumber(L, ret.z);
            return 3;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_xy(IntPtr L)
    {
        try
        {
            object o = ToLua.ToObject(L, 1);
            UnityEngine.Transform obj = o as UnityEngine.Transform;
            UnityEngine.Vector3 ret = obj.localPosition;
            LuaDLL.lua_pushnumber(L, ret.x);
            LuaDLL.lua_pushnumber(L, ret.y);
            return 2;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_x(IntPtr L)
    {
        try
        {
            object o = ToLua.ToObject(L, 1);
            UnityEngine.Transform obj = o as UnityEngine.Transform;
            LuaDLL.lua_pushnumber(L, obj.localPosition.x);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_y(IntPtr L)
    {
        try
        {
            object o = ToLua.ToObject(L, 1);
            UnityEngine.Transform obj = o as UnityEngine.Transform;
            LuaDLL.lua_pushnumber(L, obj.localPosition.y);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_z(IntPtr L)
    {
        try
        {
            object o = ToLua.ToObject(L, 1);
            UnityEngine.Transform obj = o as UnityEngine.Transform;
            LuaDLL.lua_pushnumber(L, obj.localPosition.z);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int set_xyz(IntPtr L)
    {
        try
        {
            object o = ToLua.ToObject(L, 1);
            UnityEngine.Transform obj = o as UnityEngine.Transform;
            float x = (float)LuaDLL.lua_tonumber(L, 2);
            float y = (float)LuaDLL.lua_tonumber(L, 3);
            float z = (float)LuaDLL.lua_tonumber(L, 4);
            //Debugger.Log("set_xyz : " + x.ToString() + "  " + y.ToString() + "  " + z.ToString());
            obj.localPosition = new UnityEngine.Vector3(x, y, z);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int set_xy(IntPtr L)
    {
        try
        {
            object o = ToLua.ToObject(L, 1);
            UnityEngine.Transform obj = o as UnityEngine.Transform;
            float x = (float)LuaDLL.lua_tonumber(L, 2);
            float y = (float)LuaDLL.lua_tonumber(L, 3);
            obj.localPosition = new UnityEngine.Vector3(x, y, obj.localPosition.z);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int set_x(IntPtr L)
    {
        try
        {
            object o = ToLua.ToObject(L, 1);
            UnityEngine.Transform obj = o as UnityEngine.Transform;
            float x = (float)LuaDLL.lua_tonumber(L, 2);
            obj.localPosition = new UnityEngine.Vector3(x, obj.localPosition.y, obj.localPosition.z);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int set_y(IntPtr L)
    {
        try
        {
            object o = ToLua.ToObject(L, 1);
            UnityEngine.Transform obj = o as UnityEngine.Transform;
            float y = (float)LuaDLL.lua_tonumber(L, 2);
            obj.localPosition = new UnityEngine.Vector3(obj.localPosition.x, y, obj.localPosition.z);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int set_z(IntPtr L)
    {
        try
        {
            object o = ToLua.ToObject(L, 1);
            UnityEngine.Transform obj = o as UnityEngine.Transform;
            float z = (float)LuaDLL.lua_tonumber(L, 2);
            obj.localPosition = new UnityEngine.Vector3(obj.localPosition.x, obj.localPosition.y, z);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }
    
}

