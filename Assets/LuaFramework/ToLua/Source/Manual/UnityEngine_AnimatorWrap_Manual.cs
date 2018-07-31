using System;
using LuaInterface;

public class UnityEngine_AnimatorWrap_Manual
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UnityEngine.Animator), typeof(UnityEngine.Behaviour));
        L.RegFunction("Play", Play);
		L.RegFunction("CrossFade", CrossFade);
        L.EndClass();
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int CrossFade(IntPtr L)
    {
        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.Animator), typeof(int), typeof(float)))
            {
                UnityEngine.Animator obj = (UnityEngine.Animator)ToLua.ToObject(L, 1);
                if (!obj.gameObject.activeInHierarchy)
                    return 0;
                int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
                float arg1 = (float)LuaDLL.lua_tonumber(L, 3);
                obj.CrossFade(arg0, arg1);
                return 0;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.Animator), typeof(string), typeof(float)))
            {
                UnityEngine.Animator obj = (UnityEngine.Animator)ToLua.ToObject(L, 1);
                if (!obj.gameObject.activeInHierarchy)
                    return 0;
                string arg0 = ToLua.ToString(L, 2);
                float arg1 = (float)LuaDLL.lua_tonumber(L, 3);
                obj.CrossFade(arg0, arg1);
                return 0;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.Animator), typeof(int), typeof(float), typeof(int)))
            {
                UnityEngine.Animator obj = (UnityEngine.Animator)ToLua.ToObject(L, 1);
                if (!obj.gameObject.activeInHierarchy)
                    return 0;
                int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
                float arg1 = (float)LuaDLL.lua_tonumber(L, 3);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 4);
                obj.CrossFade(arg0, arg1, arg2);
                return 0;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.Animator), typeof(string), typeof(float), typeof(int)))
            {
                UnityEngine.Animator obj = (UnityEngine.Animator)ToLua.ToObject(L, 1);
                if (!obj.gameObject.activeInHierarchy)
                    return 0;
                string arg0 = ToLua.ToString(L, 2);
                float arg1 = (float)LuaDLL.lua_tonumber(L, 3);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 4);
                obj.CrossFade(arg0, arg1, arg2);
                return 0;
            }
            else if (count == 5 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.Animator), typeof(string), typeof(float), typeof(int), typeof(float)))
            {
                UnityEngine.Animator obj = (UnityEngine.Animator)ToLua.ToObject(L, 1);
                if (!obj.gameObject.activeInHierarchy)
                    return 0;
                string arg0 = ToLua.ToString(L, 2);
                float arg1 = (float)LuaDLL.lua_tonumber(L, 3);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 4);
                float arg3 = (float)LuaDLL.lua_tonumber(L, 5);
                obj.CrossFade(arg0, arg1, arg2, arg3);
                return 0;
            }
            else if (count == 5 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.Animator), typeof(int), typeof(float), typeof(int), typeof(float)))
            {
                UnityEngine.Animator obj = (UnityEngine.Animator)ToLua.ToObject(L, 1);
                if (!obj.gameObject.activeInHierarchy)
                    return 0;
                int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
                float arg1 = (float)LuaDLL.lua_tonumber(L, 3);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 4);
                float arg3 = (float)LuaDLL.lua_tonumber(L, 5);
                obj.CrossFade(arg0, arg1, arg2, arg3);
                return 0;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.Animator.CrossFade");
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Play(IntPtr L)
    {
        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.Animator), typeof(int)))
            {
                UnityEngine.Animator obj = (UnityEngine.Animator)ToLua.ToObject(L, 1);
                if (!obj.gameObject.activeInHierarchy)
                    return 0;
                int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
                obj.Play(arg0);
                return 0;
            }
            else if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.Animator), typeof(string)))
            {
                UnityEngine.Animator obj = (UnityEngine.Animator)ToLua.ToObject(L, 1);
                if (!obj.gameObject.activeInHierarchy)
                    return 0;
                string arg0 = ToLua.ToString(L, 2);
                obj.Play(arg0);
                return 0;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.Animator), typeof(int), typeof(int)))
            {
                UnityEngine.Animator obj = (UnityEngine.Animator)ToLua.ToObject(L, 1);
                if (!obj.gameObject.activeInHierarchy)
                    return 0;
                int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
                obj.Play(arg0, arg1);
                return 0;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.Animator), typeof(string), typeof(int)))
            {
                UnityEngine.Animator obj = (UnityEngine.Animator)ToLua.ToObject(L, 1);
                if (!obj.gameObject.activeInHierarchy)
                    return 0;
                string arg0 = ToLua.ToString(L, 2);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
                obj.Play(arg0, arg1);
                return 0;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.Animator), typeof(string), typeof(int), typeof(float)))
            {
                UnityEngine.Animator obj = (UnityEngine.Animator)ToLua.ToObject(L, 1);
                if (!obj.gameObject.activeInHierarchy)
                    return 0;
                string arg0 = ToLua.ToString(L, 2);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
                float arg2 = (float)LuaDLL.lua_tonumber(L, 4);
                obj.Play(arg0, arg1, arg2);
                return 0;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.Animator), typeof(int), typeof(int), typeof(float)))
            {
                UnityEngine.Animator obj = (UnityEngine.Animator)ToLua.ToObject(L, 1);
                if (!obj.gameObject.activeInHierarchy)
                    return 0;
                int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
                float arg2 = (float)LuaDLL.lua_tonumber(L, 4);
                obj.Play(arg0, arg1, arg2);
                return 0;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.Animator.Play");
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

}

