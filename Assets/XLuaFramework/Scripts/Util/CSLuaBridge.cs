using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XLuaFramework
{
public class CSLuaBridge
{
    private static CSLuaBridge Instance = null;
    Dictionary<int, UnityAction> funcs;
    Dictionary<int, UnityAction<long>> funcsNum;
    Dictionary<int, UnityAction<long, long>> funcs2Num;
    Dictionary<int, UnityAction<string>> funcsStr;
    public static CSLuaBridge GetInstance()
    {
        if (Instance != null)
            return Instance;
        Instance = new CSLuaBridge();
        return Instance;
    }

    public void SetLuaFunc(int funcID, UnityAction func)
    {
        if (funcs.ContainsKey(funcID))
        {
            throw new Exception("already bind func for funcID :"+funcID);
        }
        funcs.Add(funcID, func);
    }

    public void CallLuaFunc(int funcID)
    {
        if (funcs.ContainsKey(funcID))
        {
            funcs[funcID]();
            return;
        }
        Debug.LogError("has no bind lua func : "+funcID);
    }

    public void SetLuaFunc2Num(int funcID, UnityAction<long, long> func)
    {
        // Debug.Log("SetLuaFunc2Num funcID : "+funcID+" func:"+(func!=null));
        if (funcs2Num.ContainsKey(funcID))
        {
            throw new Exception("already bind func2num for funcID :"+funcID);
        }
        funcs2Num.Add(funcID, func);
    }

    public void CallLuaFunc2Num(int funcID, long arge1, long arge2)
    {
        // Debug.Log("CallLuaFunc2Num funcID : "+funcID+" arge1:"+arge1);
        if (funcs2Num.ContainsKey(funcID))
        {
            funcs2Num[funcID](arge1, arge2);
            return;
        }
        Debug.LogError("has no bind lua func2num : "+funcID);
    }

    public void SetLuaFuncNum(int funcID, UnityAction<long> func)
    {
        if (funcsNum.ContainsKey(funcID))
        {
            throw new Exception("already bind funcnum for funcID :"+funcID);
        }
        funcsNum.Add(funcID, func);
    }

    public void CallLuaFuncNum(int funcID, long arge1)
    {
        if (funcsNum.ContainsKey(funcID))
        {
            funcsNum[funcID](arge1);
            return;
        }
        Debug.LogError("has no bind lua funcnum : "+funcID);
    }
    
    public void SetLuaFuncStr(int funcID, UnityAction<string> func)
    {
        // Debug.Log("SetLuaFuncStr funcID : "+funcID+" func:"+(func!=null));
        if (funcsStr.ContainsKey(funcID))
        {
            throw new Exception("already bind funcStr for funcID :"+funcID);
        }
        funcsStr.Add(funcID, func);
    }

    public void CallLuaFuncStr(int funcID, string arge1)
    {
        // Debug.Log("CallLuaFuncStr funcID : "+funcID+" arge1:"+arge1);
        if (funcsStr.ContainsKey(funcID))
        {
            funcsStr[funcID](arge1);
            return;
        }
        Debug.LogError("has no bind lua funcStr : "+funcID);
    }

    private CSLuaBridge()
    {
        funcs = new Dictionary<int, UnityAction>();
        funcs2Num = new Dictionary<int, UnityAction<long, long>>();
        funcsNum = new Dictionary<int, UnityAction<long>>();
        funcsStr = new Dictionary<int, UnityAction<string>>();
    }

    public void ClearDelegate()
    {
        funcs = null;
        funcs2Num = null;
        funcsNum = null;
        funcsStr = null;
    }
}

}
