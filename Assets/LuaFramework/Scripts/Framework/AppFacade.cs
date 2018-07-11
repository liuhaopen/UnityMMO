using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AppFacade : Facade
{
    private static AppFacade _instance;

    public AppFacade() : base()
    {
    }

    public static AppFacade Instance
    {
        get{
            if (_instance == null) {
                _instance = new AppFacade();
            }
            return _instance;
        }
    }

    //override protected void InitFramework()
    //{
    //    Debug.Log("AppFacade:InitFramework()");

    //    base.InitFramework();
    //    //RegisterCommand(NotiConst.START_UP, typeof(StartUpCommand));
    //}

    /// <summary>
    /// 启动框架
    /// </summary>
    //public void StartUp() {
    //    //SendMessageCommand(NotiConst.START_UP);
    //    //RemoveMultiCommand(NotiConst.START_UP);
    //}
}

