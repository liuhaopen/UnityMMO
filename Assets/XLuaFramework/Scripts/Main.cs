using UnityEngine;
using System.Collections;
using XLua;

namespace XLuaFramework {

    //负责整个游戏流程调度,从启动后热更新,渠道sdk接入,各系统的初始化,直到登录才完成使命
    public class Main : MonoBehaviour {
        public enum State{
            None,
            CheckExtractResource,
            UpdateResourceFromNet,
            InitAssetBundle,
            InitSDK,
            InitBaseCode,
            StartLogin,
            StartGame,
            Playing,
        }
        public enum SubState{
            Enter,Update
        }
        State cur_state = State.None;
        SubState cur_sub_state = SubState.Enter;
        public bool IsNeedPrintConcole = true;
        void Start() {
            Debug.Log("Main:Start()");

            AppConfig.Init();

            GameObject.DontDestroyOnLoad(this.gameObject);

            if (IsNeedPrintConcole)
            {
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer ||       Application.platform == RuntimePlatform.Android)
                {
                    this.gameObject.AddComponent<LogHandler>();
                }
            }

            this.gameObject.AddComponent<ThreadManager>();//for download or extract assets
            this.gameObject.AddComponent<ResourceManager>();
            this.gameObject.AddComponent<NetworkManager>();
            this.gameObject.AddComponent<XLuaManager>();
            UnityMMO.NetMsgDispatcher.GetInstance().Init();

            JumpToState(State.CheckExtractResource);
        }

        void JumpToState(State new_state)
        {
            cur_state = new_state;
            cur_sub_state = SubState.Enter;
            Debug.Log("new_state : "+new_state.ToString());
        }

        private void Update() 
        {
            if (cur_state == State.Playing)
                return;
            switch(cur_state)
            {
                case State.CheckExtractResource:
                    if (cur_sub_state == SubState.Enter)
                    {
                        cur_sub_state = SubState.Update;
                        this.gameObject.AddComponent<AssetsHotFixManager>();
                        AssetsHotFixManager.Instance.CheckExtractResource(delegate() {
                            Debug.Log("Main.cs CheckExtractResource OK!!!");
                            JumpToState(State.UpdateResourceFromNet);
                        });
                    }
                    break;  
                case State.UpdateResourceFromNet:
                    if (cur_sub_state == SubState.Enter)
                    {
                        cur_sub_state = SubState.Update;
                        AssetsHotFixManager.Instance.UpdateResource(delegate() {
                            Debug.Log("Main.cs UpdateResourceFromNet OK!!!");
                            JumpToState(State.InitAssetBundle);
                        });
                    }
                    break;
                case State.InitAssetBundle:
                    if (cur_sub_state == SubState.Enter)
                    {
                        cur_sub_state = SubState.Update;
                        ResourceManager.GetInstance().Initialize(AppConfig.AssetDir, delegate() {
                            Debug.Log("Main.cs ResourceManager Initialize OK!!!");
                            JumpToState(State.StartLogin);
                        });
                    }
                    break;
                case State.StartLogin:
                    if (cur_sub_state == SubState.Enter)
                    {
                        cur_sub_state = SubState.Update;
                        XLuaManager.Instance.InitLuaEnv();
                        XLuaManager.Instance.StartLogin(delegate() {
                            Debug.Log("Main.cs XLuaManager StartLogin OK!!!");
                            JumpToState(State.StartGame);
                        });
                    }
                    break;
                case State.StartGame:
                    if (cur_sub_state == SubState.Enter)
                    {
                        cur_sub_state = SubState.Update;
                        this.gameObject.AddComponent<UnityMMO.MainWorld>();
                        UnityMMO.MainWorld.Instance.StartGame();
                        JumpToState(State.Playing);
                    }
                    break;
            }
        }
    }
}