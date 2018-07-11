using UnityEngine;
using System.Collections;

namespace LuaFramework {

    /// <summary>
    /// </summary>
    public class Main : MonoBehaviour {
        public bool IsNeedPrintConcole = true;
        void Start() {
            Debug.Log("Main:Start()");
            if (Application.isMobilePlatform)
            {
                AppConst.DebugMode = false;
                AppConst.UpdateMode = true;
            }

            //先检查是否有打包资源和导出Lua接口
            if (!Util.CheckEnvironment()) return;

            //加个控制台打印输出
            if (IsNeedPrintConcole)
            {
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.Android)
                    GameObject.Find("GameManager").AddComponent<LogHandler>();
            }

            GameObject gameMgr = GameObject.Find("GlobalGenerator");
            if (gameMgr != null)
            {
                AppView appView = gameMgr.AddComponent<AppView>();
            }
            //-----------------关联命令-----------------------
            Debug.Log("Start Add Managers");
            AppFacade.Instance.RegisterCommand(NotiConst.DISPATCH_MESSAGE, typeof(SocketCommand));

            //-----------------初始化管理器-----------------------
            AppFacade.Instance.AddManager<LuaManager>(ManagerName.Lua);
            AppFacade.Instance.AddManager<PanelManager>(ManagerName.Panel);
            AppFacade.Instance.AddManager<SoundManager>(ManagerName.Sound);
            AppFacade.Instance.AddManager<TimerManager>(ManagerName.Timer);
            AppFacade.Instance.AddManager<NetworkManager>(ManagerName.Network);
            AppFacade.Instance.AddManager<ResourceManager>(ManagerName.Resource);
            AppFacade.Instance.AddManager<ThreadManager>(ManagerName.Thread);
            AppFacade.Instance.AddManager<ObjectPoolManager>(ManagerName.ObjectPool);
            AppFacade.Instance.AddManager<GameManager>(ManagerName.Game);
        }
    }
}