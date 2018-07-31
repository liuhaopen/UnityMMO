using System;
using UnityEngine;
using LuaInterface;

public static class LuaBinderManual
{
    public static void Bind(LuaState L)
    {
        float t = Time.realtimeSinceStartup;
        L.BeginModule(null);
        L.BeginModule("UnityEngine");
        UnityEngine_TransformWrap_Manual.Register(L);
        UnityEngine_RectTransformWrap_Manual.Register(L);
        UnityEngine_AnimatorWrap_Manual.Register(L);
        L.EndModule();

        L.BeginModule("LuaFramework");
        LuaFramework_UtilWrap_Manual.Register(L);
        L.EndModule();
        L.EndModule();
        Debugger.Log("Register manual lua type cost time: {0}", Time.realtimeSinceStartup - t);
    }
}