using System;
using LuaInterface;

public class UnityEngine_RectTransformWrap_Manual
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UnityEngine.RectTransform), typeof(UnityEngine.Transform));
        L.RegFunction("GetPivot", get_pivot);
        L.RegFunction("GetDeltaSizeXY", get_size_xy);
        //L.RegFunction("GetAnchoredXY", get_anchored_xy);
        L.RegFunction("SetPivot", set_pivot);
        L.RegFunction("SetAnchoredXY", set_anchored_xy);
        L.EndClass();
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_pivot(IntPtr L)
    {
        object o = null;

        try
        {
            o = ToLua.ToObject(L, 1);
            UnityEngine.RectTransform obj = o as UnityEngine.RectTransform;
            UnityEngine.Vector3 ret = obj.pivot;
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
    static int get_size_xy(IntPtr L)
    {
        object o = null;

        try
        {
            o = ToLua.ToObject(L, 1);
            UnityEngine.RectTransform obj = o as UnityEngine.RectTransform;
            UnityEngine.Vector2 ret = obj.sizeDelta;
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
    static int set_pivot(IntPtr L)
    {
        object o = null;

        try
        {
            int count = LuaDLL.lua_gettop(L);
            if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.Transform), typeof(float), typeof(float)))
            {
                o = ToLua.ToObject(L, 1);
                UnityEngine.RectTransform obj = o as UnityEngine.RectTransform;
                float x = (float)LuaDLL.lua_tonumber(L, 2);
                float y = (float)LuaDLL.lua_tonumber(L, 3);
                obj.pivot = new UnityEngine.Vector2(x, y);
                return 0;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.RectTransform.SetPivot");
            }
            
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int set_anchored_xy(IntPtr L)
    {
        object o = null;

        try
        {
            int count = LuaDLL.lua_gettop(L);
            if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.Transform), typeof(float), typeof(float)))
            {
                o = ToLua.ToObject(L, 1);
                UnityEngine.RectTransform obj = o as UnityEngine.RectTransform;
                float x = (float)LuaDLL.lua_tonumber(L, 2);
                float y = (float)LuaDLL.lua_tonumber(L, 3);
                obj.anchoredPosition = new UnityEngine.Vector2(x, y);
                return 0;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.Transform.SetAnchoredXY");
            }

        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }
    
}

